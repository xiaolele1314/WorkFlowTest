﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Persistence.EntityFramework.Interfaces;
using WorkflowCore.Persistence.EntityFramework.Models;
using WorkflowCore.Persistence.EntityFramework.Services;
using WorkflowWebTest.MyPersistence;

namespace WorkflowWebTest.Test
{
    public class MyEntityFrameworkPersistenceProvider: EntityFrameworkPersistenceProvider
    {
        private readonly bool _canCreateDB;
        private readonly bool _canMigrateDB;
        private readonly IWorkflowDbContextFactory _contextFactory;

        public MyEntityFrameworkPersistenceProvider(IWorkflowDbContextFactory contextFactory, bool canCreateDB, bool canMigrateDB)
            :base(contextFactory,canCreateDB,canMigrateDB)
        {

            _contextFactory = contextFactory;
            _canCreateDB = canCreateDB;
            _canMigrateDB = canMigrateDB;
        }

        private WorkflowDbContext ConstructDbContext()
        {
            return this._contextFactory.Build();
        }

        public async Task<string> CreateWorkflowDefination(MyWorkflowDefination defination)
        {
            using(var db = ConstructDbContext())
            {
                defination.InstanceId = Guid.NewGuid().ToString();
                var result = db.Set<MyWorkflowDefination>().Add(defination);
                await db.SaveChangesAsync();
                return defination.InstanceId;
            }
        }

        public async Task<MyWorkflowDefination> GetWorkflowDefination(string id)
        {
            using (var db = ConstructDbContext())
            {
                var raw = await db.Set<MyWorkflowDefination>()
                    .Where(x => x.InstanceId == id).FirstOrDefaultAsync();

                if (raw == null)
                    return null;

                return raw;
            }
        }
        public async Task<MyWorkflowDefination> GetWorkflowDefination(string workflowId,int version)
        {
            using (var db = ConstructDbContext())
            {
                var raw = await db.Set<MyWorkflowDefination>()
                    .Where(x => x.Id == workflowId && x.Version == version).FirstOrDefaultAsync();

                if (raw == null)
                    return null;

                return raw;
            }
        }

        public async Task<PersistedExecutionPointer> GetWorkflowStep(string id)
        {
            using (var db = ConstructDbContext())
            {
                var raw = await db.Set<PersistedExecutionPointer>()
                    .Where(x => x.Id == id).FirstOrDefaultAsync();

                if (raw == null)
                    return null;

                return raw;
            }
        }

        public async Task<string> CreateWorkflowDefinationRelation(MyWorkflowDefinationRelation relation)
        {
            using (var db = ConstructDbContext())
            {
                relation.Id = Guid.NewGuid().ToString();
                var result = db.Set<MyWorkflowDefinationRelation>().Add(relation);
                await db.SaveChangesAsync();
                return relation.Id;
            }
        }
    }
}
