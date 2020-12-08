using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkFlowTest.ParallelForeach;

namespace WorkFlowTest.WhileLoop
{
    public class WhileWorkflow : IWorkflow<MyData>
    {
        public string Id => "While";
        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder
                .StartWith<User1>()
                .While(data => data.Counter < 3)
                    .Do(x => x
                        .StartWith<DoSomething>()
                        .Then<IncrementStep>()
                            .Input(step => step.Value1, data => data.Counter)
                            .Output(data => data.Counter, step => step.Value2))
                .Then<User3>();
        }
    }

    public class MyData
    {
        public int Counter { get; set; }
    }
}
