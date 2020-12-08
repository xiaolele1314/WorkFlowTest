using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowWebTest.Step
{
    public class Step2 : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            context.Workflow.Status = WorkflowStatus.Suspended;
            Console.WriteLine("step2 start");
            return ExecutionResult.Next();
        }
    }
}
