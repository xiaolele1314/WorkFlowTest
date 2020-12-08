using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowWebTest.Workflow1
{ 
    public class Test1Step2 : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //context.Workflow.Status = WorkflowStatus.Suspended;
            Console.WriteLine("Test1Step2 start");
            return ExecutionResult.Next();
        }
    }
}
