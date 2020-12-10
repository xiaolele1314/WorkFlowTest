using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowWebTest.Test;

namespace WorkflowWebTest.Interface
{
    public interface IWorkflowWeb
    {
        public IActionResult RegisterDefinationAsync();
        public Task<IActionResult> StartWorkFlow(WorkflowPostData data);
        public IActionResult UndoClaim(string workflowId);
        public  Task<IActionResult> AgreeStep(WorkflowStepPostData data);
        public  Task<IActionResult> DisagreeStep(WorkflowStepPostData data);
    }
}
