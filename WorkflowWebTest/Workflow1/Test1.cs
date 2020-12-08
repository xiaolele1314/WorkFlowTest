using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace WorkflowWebTest.Workflow1
{
    public class Test1 : IWorkflow
    {
        public string Id => "test1";

        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder.StartWith<Test1Step1>()
                .Then<Test1Step2>();
        }
    }
}
