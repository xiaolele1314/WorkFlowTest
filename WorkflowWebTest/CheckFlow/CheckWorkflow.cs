using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace WorkflowWebTest.CheckFlow
{
    public class CheckWorkflow : IWorkflow<CheckData>
    {
        public string Id => "check";

        public int Version => 1;

        public void Build(IWorkflowBuilder<CheckData> builder)
        {
            builder
                .If(data => data.IsAgree)
                    .Do(then =>
                        then.StartWith<Agree>())
                .If(data => !data.IsAgree)
                    .Do(then =>
                        then.StartWith<Disagree>());
        }
    }
}
