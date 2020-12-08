using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.RoleWorkflow
{
    public class StudentStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("I'm a student");
            return ExecutionResult.Next();
        }
    }
}
