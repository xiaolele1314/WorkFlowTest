using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkFlowTest.MutipleOutcomes;

namespace WorkFlowTest.ParallelTask
{
    public class ParallelWorkflow : IWorkflow<MyData>
    {
        public string Id => "parallel-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder
                .StartWith<User2>()
                .Parallel()
                    .Do(then =>
                        then.StartWith<PrintMessage>()
                                .Input(step => step.Message, data => "Item 1.1")
                            .Then<PrintMessage>()
                                .Input(step => step.Message, data => "Item 1.2"))
                    .Do(then =>
                        then.StartWith<PrintMessage>()
                                .Input(step => step.Message, data => "Item 2.1")
                            .Then<PrintMessage>()
                                .Input(step => step.Message, data => "Item 2.2")
                            .Then<PrintMessage>()
                                .Input(step => step.Message, data => "Item 2.3"))
                    .Do(then =>
                        then.StartWith<PrintMessage>()
                                .Input(step => step.Message, data => "Item 3.1")
                            .Then<PrintMessage>()
                                .Input(step => step.Message, data => "Item 3.2"))
                .Join()
                .Then<User3>();
        }
    }

    public class MyData
    {
        public int Counter { get; set; }
    }
}
