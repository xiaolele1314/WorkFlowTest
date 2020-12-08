using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.DependencyInjection
{
    public class DoSomething : StepBody
    {
        private IMyService _myService;

        public DoSomething(IMyService myService)
        {
            _myService = myService;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _myService.DoTheThings();
            return ExecutionResult.Next();
        }
    }
}
