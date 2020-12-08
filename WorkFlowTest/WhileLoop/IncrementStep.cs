using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.WhileLoop
{
    public class IncrementStep : StepBody
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Value2 = Value1 + 1;
            Console.WriteLine($"OutValue is {Value2}");
            return ExecutionResult.Next();
        }
    }
}
