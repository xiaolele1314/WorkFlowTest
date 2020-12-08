using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.Check
{
    public class Review : StepBody
    {
        public bool Approved { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        { 
            this.Approved = true;
            return OutcomeResult(this.Approved);
        }
    }
}
