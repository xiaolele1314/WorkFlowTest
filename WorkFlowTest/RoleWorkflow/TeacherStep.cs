using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.RoleWorkflow
{
    public class TeacherStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("I'm a teacher");
            return ExecutionResult.Next();
        }
    }
}
