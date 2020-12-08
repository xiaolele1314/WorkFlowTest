using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest
{
    public class User2 : StepBody
    {
        public string Message { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("user2");
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
            return ExecutionResult.Next();
            //return ExecutionResult.Next();
        }
    }
}
