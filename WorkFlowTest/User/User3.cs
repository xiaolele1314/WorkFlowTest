using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest
{
    public class User3 : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("user3");
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
            return ExecutionResult.Next();
        }
    }
}
