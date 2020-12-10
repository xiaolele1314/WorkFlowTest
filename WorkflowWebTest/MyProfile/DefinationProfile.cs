using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Models;
using WorkflowWebTest.Test;

namespace WorkflowWebTest.MyProfile
{
    public class DefinationProfile : Profile
    {
        public DefinationProfile()
        {
            this.CreateMap<WorkflowDefinition, MyWorkflowDefination>();
            this.CreateMap<MyWorkflowDefination, WorkflowDefinition>();
        }
    }
}
