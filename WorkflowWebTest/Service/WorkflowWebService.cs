using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Exceptions;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Services.DefinitionStorage;
using WorkflowWebTest.CheckFlow;
using WorkflowWebTest.Interface;
using WorkflowWebTest.MyPersistence;
using WorkflowWebTest.Test;
using WorkflowWebTest.Workflow1;

namespace WorkflowWebTest.Service
{
    public class WorkflowWebService : IWorkflowWeb
    {

        private readonly IWorkflowHost _host;
        private readonly MyEntityFrameworkPersistenceProvider _persistence;
        private readonly IWorkflowRegistry _registry;
        private readonly IExecutionPointerFactory _pointerFactory;
        private readonly IWorkflowMiddlewareRunner _middlewareRunner;
        private readonly IQueueProvider _queueProvider;
        private readonly ILifeCycleEventHub _eventHub;
        private readonly IWorkflowExecutor _executor;
        private readonly IMapper _mapper;
        private readonly IDefinitionLoader _loader;
        public WorkflowWebService(
            IWorkflowHost host,
            IExecutionPointerFactory pointerFactory,
            IWorkflowMiddlewareRunner middlewareRunner,
            ILifeCycleEventHub eventHub,
            IWorkflowExecutor executor,
            IMapper mapper,
            IDefinitionLoader loader)
        {
            this._host = host;
            this._persistence = host.PersistenceStore as MyEntityFrameworkPersistenceProvider;
            this._registry = host.Registry;
            this._queueProvider = host.QueueProvider;

            this._pointerFactory = pointerFactory;
            this._middlewareRunner = middlewareRunner;
            this._eventHub = eventHub;
            this._executor = executor;
            this._mapper = mapper;
            this._loader = loader;
        }

        public async Task<IActionResult> AgreeStep(WorkflowStepPostData postData)
        {
            // TODO:從數據庫根據id和version獲取定義，加載到内存，保存到當前工作流實例中
           
            var instance = this._persistence.GetWorkflowInstance(postData.WorkflowId).Result;
            if (!await MyRegisterDefinationAsync(instance.WorkflowDefinitionId, instance.Version))
            {
                return new JsonResult("工作流沒有注冊");
            }

            CheckData data = new CheckData { IsAgree = true };
            instance.Data = data;
            var result = await RunStep(instance);
            return result;
        }

        public async Task<IActionResult> DisagreeStep(WorkflowStepPostData postData)
        {
            var instance = this._persistence.GetWorkflowInstance(postData.WorkflowId).Result;
            if (!await MyRegisterDefinationAsync(instance.WorkflowDefinitionId, instance.Version))
            {
                return new JsonResult("工作流沒有注冊");
            }

            CheckData data = new CheckData { IsAgree = false };
            instance.Data = data;
            var result = await RunStep(instance);
            return result;
        }

        public IActionResult RegisterDefinationAsync()
        {
            //if (this._registry.IsRegistered(data.WorkflowId, data.Version ?? 1))
            //{
            //    return new JsonResult("已經注冊");
            //}

            string jsonString = File.ReadAllText("C:\\Users\\zhangle\\source\\repos\\WorkFlowTest\\WorkflowWebTest\\CheckFlow\\CheckWorkFlow.json", Encoding.Default);
            var def = this._loader.LoadDefinition(jsonString, Deserializers.Json);

            var myDef = this._mapper.Map<MyWorkflowDefination>(def);
            myDef.WorkflowData = jsonString;

            var result = this._persistence.CreateWorkflowDefination(myDef).Result;
            return new JsonResult($"defination Id is {result}");
        }

        public async Task<IActionResult> StartWorkFlow(WorkflowPostData data)
        {
            // TODO:從數據庫根據id和version獲取定義，加載到内存，保存到當前工作流實例中
            if (!await MyRegisterDefinationAsync(data.WorkflowId, data.Version ?? 1))
            {
                return new JsonResult("工作流沒有注冊");
            }
            //this._host.RegisterWorkflow<CheckWorkflow, CheckData>();
            var result = MystartWorkflow(data.WorkflowId, data.Version).Result;

            return new JsonResult($"workflow Id is {result}");
        }

        public IActionResult UndoClaim(string workflowId)
        {
            var result = this._host.TerminateWorkflow(workflowId).Result;
            if (result)
            {
                return new JsonResult("撤銷成功");
            }
            return new JsonResult("撤銷失敗");
        }

        private Task<string> MystartWorkflow(string workflowId, int? version, object data = null, string reference = null)
        {
            return MystartWorkflow<object>(workflowId, version, data, reference);
        }

        public async Task<string> MystartWorkflow<TData>(string workflowId, int? version, TData data = null, string reference = null)
            where TData : class, new()
        {
            var def = this._registry.GetDefinition(workflowId, version);
            if (def == null)
            {
                throw new WorkflowNotRegisteredException(workflowId, version);
            }

            var wf = new WorkflowInstance
            {
                WorkflowDefinitionId = workflowId,
                Version = def.Version,
                Data = data,
                Description = def.Description,
                NextExecution = 0,
                CreateTime = DateTime.Now.ToUniversalTime(),
                Status = WorkflowStatus.Runnable,
                Reference = reference
            };

            if ((def.DataType != null) && (data == null))
            {
                if (typeof(TData) == def.DataType)
                    wf.Data = new TData();
                else
                    wf.Data = def.DataType.GetConstructor(new Type[0]).Invoke(new object[0]);
            }

            wf.ExecutionPointers.Add(this._pointerFactory.BuildGenesisPointer(def));

            //await this._middlewareRunner.RunPreMiddleware(wf, def);

            string id = await this._host.PersistenceStore.CreateNewWorkflow(wf);

            return id;
        }

        private async Task<IActionResult> RunStep(WorkflowInstance instance)
        {
            var result = new WorkflowStepResult();
            result.InstanceId = instance.ExecutionPointers.Last().Id;
            result.WorkflowStepId = instance.ExecutionPointers.Last().StepId + 1;
            result.WorkflowName = instance.ExecutionPointers.Last().StepName;
            await this._executor.Execute(instance);
            await this._persistence.PersistWorkflow(instance);
            if (instance.Status == WorkflowStatus.Runnable)
            {
                return new JsonResult(result);
            }
            if (instance.Status == WorkflowStatus.Suspended)
            {
                return new JsonResult("suspend steps");
            }
            if (instance.Status == WorkflowStatus.Complete)
            {
                return new JsonResult("complete steps");
            }
            if (instance.Status == WorkflowStatus.Terminated)
            {
                return new JsonResult("teminate step");
            }
            return new JsonResult("workflow status Error");
        }

        private async Task<bool> MyRegisterDefinationAsync(string workflowId, int version)
        {
            if (this._registry.IsRegistered(workflowId, version))
            {
                return true;
            }

            var mydef = await this._persistence.GetWorkflowDefination(workflowId, version);
            if (mydef == null)
            {
                return false;
            }

            try
            {
                var def = this._loader.LoadDefinition(mydef.WorkflowData, Deserializers.Json);

            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
