using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore;
using WorkflowCore.Interface;
using WorkFlowTest;

namespace WorkFlowTest
{
    public class UserWorkflow1 : IWorkflow<MyData>
    {
        public string Id => "workflow1";

        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder
                .StartWith<User1>()
                .Then<User2>()
                .Then<User3>();
        }
    }

    public class MyData
    {
        public string Message { get; set; }
    }
}
