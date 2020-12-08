using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace WorkFlowTest.Looping
{
    public class LoopingWorkflow : IWorkflow
    {
        public string Id => "LoopingWorkflow";

        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<User2>()
                .Then<RandomOutput>(randomOutput =>
                {
                    randomOutput
                        .When(0)
                            .Then<CustomMessage>(cm =>
                            {
                                cm.Name("Print custom message");
                                cm.Input(step => step.Message, data => "Looping back....");
                            })
                            .Then(randomOutput);  //loop back to randomOutput

                    randomOutput
                        .When(1)
                            .Then<User3>();
                });
        }
    }
}
