using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace WorkflowWebTest.Workflow2
{
    public class Test2 : IWorkflow
    {
        public string Id => "test2";

        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder.StartWith<Test2Step1>()
                .Then<Test2Step2>();
        }
    }
}
