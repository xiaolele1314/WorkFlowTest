using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowWebTest.Step;

namespace WorkflowWebTest.Workflow
{
    public class Test1 : IWorkflow
    {
        public string Id => "test1";

        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder.StartWith<Step1>()
                .Then<Step2>();
        }
    }
}
