using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.Event
{
    public class EventSampleWorkflow : IWorkflow<EventFlowData>
    {
        public string Id => "EventSampleWorkflow";

        public int Version => 1;

        public void Build(IWorkflowBuilder<EventFlowData> builder)
        {
            builder
                .StartWith(context => ExecutionResult.Next())
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
                    .Output(data => data.Value1, step => step.EventData)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => "The data from the event is " + data.Value1)
                .Then(context => Console.WriteLine("workflow complete"));
        }
    }
}
