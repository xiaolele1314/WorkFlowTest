using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.ParallelForeach
{
    public class DisplayContext : StepBody
    {
        public object Item { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var role = Item;
            Console.WriteLine($"Working on role ");
            return ExecutionResult.Next();
        }
    }
}
