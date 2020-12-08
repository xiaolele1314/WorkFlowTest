using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.MutipleOutcomes
{
    public class OutcomeWorkflow : IWorkflow<MyData>
    {
        public string Id => "outcome-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {
            var branch1 = builder.CreateBranch()
                .StartWith<PrintMessage>()
                    .Input(step => step.Message, data => "hi from 1")
                    .Input(step => step.id, data => data.id)
                .Then<PrintMessage>()
                    .Input(step => step.Message, data => "bye from 1");

            var branch2 = builder.CreateBranch()
                .StartWith<PrintMessage>()
                    .Input((step, data) => { step.id = data.id; step.Message = "hi from 2"; });
                //.Then<PrintMessage>()
                //    .Input(step => step.Message, data => "bye from 2");


            builder
                .StartWith(x => { Console.WriteLine("start w2"); return ExecutionResult.Next(); })
                .Decide(data => 2)
                    .Branch(1, branch1)
                    .Branch(2, branch2);
        }
    }

    public class MyData
    {
        public int Value { get; set; }
        public string id { get; set; }
    }
}
