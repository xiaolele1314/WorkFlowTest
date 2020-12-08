using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.Event
{
    public class CustomMessage : StepBody
    {
        public string Message { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine(Message);
            return ExecutionResult.Next();
        }
    }

    public class EventFlowData
    {
        public string Value1 { get; set; }
    }
}
