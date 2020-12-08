using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowWebTest.Step
{
    public class Step1 : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            context.Workflow.Status = WorkflowStatus.Suspended;
            Console.WriteLine("step1 start");
            return ExecutionResult.Next();
        }
    }
}
