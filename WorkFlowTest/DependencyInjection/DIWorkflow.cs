using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace WorkFlowTest.DependencyInjection
{
    public class DIWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<User2>()
                .Then<DoSomething>();
        }

        public string Id => "DIWorkflow";

        public int Version => 1;

    }
}
