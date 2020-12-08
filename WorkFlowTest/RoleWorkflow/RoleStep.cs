using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.RoleWorkflow
{
    public class RoleStep : StepBody
    {
        public Role Role { get; set; }
        public IRoleSrvice roleSrvice;

        //public RoleStep()
        //{

        //}
        public RoleStep(IRoleSrvice service)
        {
            this.roleSrvice = service;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            this.Role = this.roleSrvice.GetRole();

            return ExecutionResult.Next();
        }
    }

    
}
