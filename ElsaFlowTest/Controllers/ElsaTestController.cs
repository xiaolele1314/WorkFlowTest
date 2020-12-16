using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Activities.UserTask.Activities;
using Elsa.Extensions;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.EntityFrameworkCore.DbContexts;
using Elsa.Persistence.EntityFrameworkCore.Entities;
using Elsa.Serialization;
using Elsa.Serialization.Formatters;
using Elsa.Services;
using Elsa.Services.Extensions;
using Elsa.Services.Models;
using ElsaFlowTest.Flow;
using ElsaFlowTest.Model;
using Microsoft.AspNetCore.Mvc;

namespace ElsaFlowTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElsaTestController : ControllerBase
    {
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowRegistry _registry;
        private readonly Func<IWorkflowBuilder> _workflowBuidler;
        private readonly ElsaContext _context;
        private readonly IWorkflowFactory _workflowFactory;
        private readonly IWorkflowInvoker _workflowInvoker;
        private readonly IMapper _mapper;
        private readonly IWorkflowSerializer _serializer;

        public ElsaTestController(
            IWorkflowDefinitionStore workflowDefinitionStore,
            IWorkflowInstanceStore workflowInstanceStore,
            IWorkflowRegistry registry,
            Func<IWorkflowBuilder> workflowBuilder,
            ElsaContext context,
            IWorkflowFactory workflowFactory,
            IWorkflowInvoker workflowInvoker,
            IMapper mapper,
            IWorkflowSerializer serializer
            )
        {
            this._workflowDefinitionStore = workflowDefinitionStore;
            this._workflowInstanceStore = workflowInstanceStore;
            this._registry = registry;
            this._workflowBuidler = workflowBuilder;
            this._context = context;
            this._workflowFactory = workflowFactory;
            this._workflowInvoker = workflowInvoker;
            this._mapper = mapper;
            this._serializer = serializer;
        }


        [HttpPost("defination")]
        public async Task<IActionResult> RegisterWorkflow()
        {
            var workflow = new UserWorkflow();
            var builder = _workflowBuidler();
            builder.WithVersion(1);
            builder.WithName("userTask");
            workflow.Build(builder);
            var workflowDefination = builder.Build();
            var entity = await this._registry.GetWorkflowDefinitionAsync(workflowDefination.DefinitionId, VersionOptions.Latest, CancellationToken.None);
            if(entity != null)
            {
                return new JsonResult("已注冊");
            }
            await this._workflowDefinitionStore.SaveAsync(workflowDefination, CancellationToken.None);
            await this._context.SaveChangesAsync(CancellationToken.None);
            return new JsonResult(workflowDefination.DefinitionId);
        }

        [HttpPost("{defiantionKey}/workflow")]
        public async Task<IActionResult> StartWorkflow(string defiantionKey)
        {
            var defination = await this._registry.GetWorkflowDefinitionAsync(defiantionKey, VersionOptions.Latest, CancellationToken.None);
            if(defination == null)
            {
                return new JsonResult("未注册");
            }

            var correlationId = Guid.NewGuid().ToString("N");
            var workflow = this._workflowFactory.CreateWorkflow(defination,correlationId:correlationId);
           
            var workflowInstance = workflow.ToInstance();
            workflowInstance = await this._workflowInstanceStore.SaveAsync(workflowInstance, CancellationToken.None);
            //var result = this._serializer.Serialize<WorkflowInstance>(workflowInstance, JsonTokenFormatter.FormatName);
            var result = this._mapper.Map<WorkflowInstanceEntity>(workflowInstance);
            return new JsonResult(result);
        }

        [HttpPost("{definationKey}/workflow/{worflowKey}/execute")]
        public async Task<IActionResult> ExecuteWorkflow(string definationKey, string worflowKey, [FromBody]ExecuteWorkflowPostData data)
        {
            var defination = await this._workflowDefinitionStore.GetByIdAsync(definationKey, VersionOptions.Latest, CancellationToken.None);
            if (defination == null)
            {
                return new JsonResult("未注册");
            }

            var instance = await this._workflowInstanceStore.GetByIdAsync(worflowKey, CancellationToken.None);
            if(instance == null)
            {
                return new JsonResult("没有此工作流实例");
            }
            var workflow = this._workflowFactory.CreateWorkflow(defination, data.Input, instance, data.CorrelationId);
            var startActivities = workflow.Activities.Find(data.StartIds);
            var executionContext = await this._workflowInvoker.StartAsync(workflow, startActivities, CancellationToken.None);
            var result = executionContext.Workflow.Connections.Where(x => x.Source.Activity.Id == executionContext.CurrentActivity.Id).ToList();
            return new JsonResult(result);

        }

        [HttpPost("{definationKey}/workflow/{worflowKey}/execute/{correlationKey}/action")]
        public async Task<IActionResult> ActivityAction(string definationKey, string worflowKey, string correlationKey, [FromBody] ActionPostData data)
        {
            var defination = await this._workflowDefinitionStore.GetByIdAsync(definationKey, VersionOptions.Latest, CancellationToken.None);
            if (defination == null)
            {
                return new JsonResult($"工作流未注册:{definationKey}");
            }

            var instance = await this._workflowInstanceStore.GetByIdAsync(worflowKey, CancellationToken.None);
            if (instance == null)
            {
                return new JsonResult("没有此工作流实例");
            }

            if(instance.Status != WorkflowStatus.Executing)
            {
                return new JsonResult("此工作流已不再运行");
            }

            var triggeredExecutionContexts = await this._workflowInvoker.TriggerAsync(nameof(UserTask), new Variables { ["UserAction"] = new Variable(data.OutCome) }, correlationKey);
            var executionContext = triggeredExecutionContexts.First();
            var result = executionContext.Workflow.Connections.Where(x => x.Source.Activity.Id == executionContext.CurrentActivity.Id).ToList();
            if(result.Count == 0)
            {
                return new JsonResult("工作流运行完成");
            }
            return new JsonResult(result);
        }

        [HttpGet("defination/{definationKey}")]
        public async Task<IActionResult> Get(string definationKey)
        {
            var defination = await this._workflowDefinitionStore.GetByIdAsync(definationKey, VersionOptions.Latest, CancellationToken.None);
            if(defination == null)
            {
                return new JsonResult($"工作流未注册:{definationKey}");
            }
            var result = this._mapper.Map<WorkflowDefinitionVersionEntity>(defination);
            return new JsonResult(result);
        }

        [HttpGet("defination/{definationKey}/workflow/{workflowKey}")]

        public async Task<IActionResult> Get(string definationKey, string workflowKey)
        {
            var defination = await this._workflowDefinitionStore.GetByIdAsync(definationKey, VersionOptions.Latest, CancellationToken.None);
            if (defination == null)
            {
                return new JsonResult($"工作流未注册:{definationKey}");
            }
            var instance = await this._workflowInstanceStore.GetByIdAsync(workflowKey, CancellationToken.None);
            if (defination == null)
            {
                return new JsonResult($"工作流不存在:{workflowKey}");
            }
            var result = this._mapper.Map<WorkflowInstanceEntity>(instance);
            return new JsonResult(result);
        }
    }
}
