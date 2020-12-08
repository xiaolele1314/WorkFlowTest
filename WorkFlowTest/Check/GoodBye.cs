using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.Check
{
    public class Goodbye : StepBody
    {

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Check over");
            return ExecutionResult.Next();
        }
    }
}
