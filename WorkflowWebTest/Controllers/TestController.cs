using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WorkflowCore.Interface;
using WorkflowWebTest.Workflow1;
using WorkflowCore.Models;
using WorkflowWebTest.Test;
using Newtonsoft.Json;
using WorkflowCore.Exceptions;
using WorkflowCore.Models.LifeCycleEvents;
using AutoMapper;

namespace WorkflowWebTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
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
        public TestController(
            IWorkflowHost host,
            IExecutionPointerFactory pointerFactory,
            IWorkflowMiddlewareRunner middlewareRunner,
            ILifeCycleEventHub eventHub,
            IWorkflowExecutor executor,
            IMapper mapper)
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
        }

        [HttpPost("startworkflow")]
        public IActionResult StartWorkflow([FromBody] WorkflowPostData startData)
        {
            if (!this._registry.IsRegistered(startData.WorkflowId, startData.Version ?? 1))
            {
                this._host.RegisterWorkflow<Test1>();
            }


            var result = new WorkflowResult();
            //result.WorkflowId = this._host.StartWorkflow(startData.WorkflowId, startData.Version).Result;
            result.WorkflowId = MystartWorkflow(startData.WorkflowId, startData.Version).Result;
            result.WorkflowDefinationId = PersistWorkflowDefinetion(startData.WorkflowId, startData.Version);

            //await StartStep(result.WorkflowId, startData);
            return new JsonResult(result);
        }


        [HttpPost("startstep")]
        public async Task<IActionResult> StartStep([FromBody] WorkflowStepPostData data)
        {

            var instance = this._persistence.GetWorkflowInstance(data.WorkflowId).Result;

            if (instance.Status == WorkflowStatus.Runnable)
            {
                var result = new WorkflowStepResult();
                result.InstanceId = instance.ExecutionPointers.Last().Id;
                result.WorkflowStepId = instance.ExecutionPointers.Last().StepId + 1;
                result.WorkflowName = instance.ExecutionPointers.Last().StepName;
                await this._executor.Execute(instance);
                await this._persistence.PersistWorkflow(instance);

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

        [HttpGet("/step/{key}")]
        public async Task<IActionResult> GetStep(string key)
        {
            var result = await this._persistence.GetWorkflowStep(key);
            return new JsonResult(result);
        }

        [HttpGet("/worflow/{key}")]
        public async Task<IActionResult> GetWorkflow(string key)
        {
            var result = await this._persistence.GetWorkflowInstance(key);
            return new JsonResult(result);
        }

        [HttpGet("/defination/{key}")]
        public async Task<IActionResult> GetDefination(string key)
        {
            var result = await this._persistence.GetWorkflowDefination(key);
            return new JsonResult(result);
        }

        //[HttpGet("starthost")]
        //public IActionResult StartHost()
        //{
        //    this._host.Start();
        //    return new JsonResult("start backgroundTask");
        //}

        private string PersistWorkflowDefinetion(string workflowId, int? version = null)
        {
            var workflowDefination = this._registry.GetDefinition(workflowId, version);
            //var defination = new MyWorkflowDefination();
            var defination = this._mapper.Map<MyWorkflowDefination>(workflowDefination);
            defination.WorkflowId = workflowId;
            defination.WorkflowVersion = version;
            //defination.DataType = JsonConvert.SerializeObject(workflowDefination.DataType);
            //defination.Steps = JsonConvert.SerializeObject(workflowDefination.Steps);
            //defination.DefaultErrorBehavior = workflowDefination.DefaultErrorBehavior;
            //defination.DefaultErrorRetryInterval = JsonConvert.SerializeObject(workflowDefination.DefaultErrorRetryInterval);
            //defination.OnPostMiddlewareError = JsonConvert.SerializeObject(workflowDefination.OnPostMiddlewareError);

            var definationId = this._persistence.CreateWorkflowDefination(defination).Result;
            return definationId;
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

        //private async Task StartStep(string id, WorkflowPostData data)
        //{
        //    await this._queueProvider.QueueWork(id, QueueType.Workflow);
        //    await this._queueProvider.QueueWork(id, QueueType.Index);
        //    await this._eventHub.PublishNotification(new WorkflowStarted()
        //    {
        //        EventTimeUtc = DateTime.UtcNow,
        //        Reference = data.Reference,
        //        WorkflowInstanceId = id,
        //        WorkflowDefinitionId = data.WorkflowId,
        //        Version = data.Version ?? 1
        //    });
        //}
    }
}
