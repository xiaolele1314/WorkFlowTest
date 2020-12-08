using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkFlowTest.Check;
using WorkFlowTest.MutipleOutcomes;

namespace WorkFlowTest.Check
{
    public class CheckWorkflow : IWorkflow<CheckData>
    {
        public string Id => "check";
        public int Version => 1;

        public void Build(IWorkflowBuilder<CheckData> builder)
        {
            //builder
            //    .StartWith<User2>()
            //    .If(data => data.Counter < 3).Do(then => then
            //        .StartWith<PrintMessage>()
            //            .Input(step => step.Message, data => "Value is greater than 3")
            //        .EndWorkflow()
            //    )
            //    .If(data => data.Counter < 5).Do(then => then
            //        .StartWith<PrintMessage>()
            //            .Input(step => step.Message, data => "Value is less than 5")
            //    )
            //    .Then<User3>();

            builder
                .StartWith<PrintMessage>(x => x.Name("start"))
                    .Input(step => step.Message, data => "Starting")
                    .Id("Review")
                .Then<PrintMessage>()
                    .Input(step => step.Message, data => "Reviewing")
                .Then<Review>()
                    .Output(data => data.Approved, step => step.Approved)
                .When(x => true, "Approved")
                    .Do(then => then.StartWith<PrintMessage>()
                        .Input(step => step.Message, data => "Approved")
                    .Then<Goodbye>()
                    .Id("Goodbye")
                    )
                .When(x => false, "NotApproved")
                .Do(then => then.StartWith<PrintMessage>()
                    .Id("NotApproved")
                    .Input(step => step.Message, data => "Not Approved")
                    .Attach("Review")
                );
        }
    }

    public class CheckData
    {
        public int Counter { get; set; }
        public bool Approved { get; set; }
    }
}
