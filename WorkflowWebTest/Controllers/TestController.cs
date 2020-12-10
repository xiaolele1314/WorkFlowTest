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
using WorkflowWebTest.Interface;

namespace WorkflowWebTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IWorkflowWeb _service;

        public TestController(IWorkflowWeb service)
        {
            this._service = service;
        }

        [HttpPost("startworkflow")]
        public IActionResult StartWorkflow([FromBody] WorkflowPostData startData)
        {
            var result = this._service.StartWorkFlow(startData).Result;
            return new JsonResult(result);
        }

        [HttpPost("registerworkflow")]
        public IActionResult RegisterWorkflow([FromBody] WorkflowPostData startData)
        {
            var result = this._service.RegisterDefinationAsync();
            return new JsonResult(result);
        }


        [HttpPost("agree")]
        public async Task<IActionResult> AgreeStep([FromBody] WorkflowStepPostData data)
        {

            var result = await this._service.AgreeStep(data);
            return new JsonResult(result);
        }

        [HttpPost("disagree")]
        public async Task<IActionResult> DisagreeStep([FromBody] WorkflowStepPostData data)
        {

            var result = await this._service.DisagreeStep(data);
            return new JsonResult(result);
        }

        [HttpDelete("/undo/{key}")]
        public IActionResult UndoWorkflow(string key)
        {
            var result =  this._service.UndoClaim(key);
            return new JsonResult(result);
        }
    }
}
