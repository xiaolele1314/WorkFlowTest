using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowWebTest.Workflow2
{
    public class Test2Step1 : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //context.Workflow.Status = WorkflowStatus.Suspended;
            Console.WriteLine("Test2Step1 start");
            return ExecutionResult.Next();
        }
    }
}
