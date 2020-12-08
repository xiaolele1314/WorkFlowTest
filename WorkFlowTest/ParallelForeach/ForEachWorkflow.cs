using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkFlowTest.RoleWorkflow;

namespace WorkFlowTest.ParallelForeach
{
    public class ForEachWorkflow : IWorkflow<User>
    {
        public string Id => "Foreach";
        public int Version => 1;

        public void Build(IWorkflowBuilder<User> builder)
        {
            builder
                .StartWith<User1>()
                .ForEach(data => data.Nums)
                    .Do(x => x
                        .StartWith<DisplayContext>()
                            //.Input(step => step.Item, (data, context) =>  context.Item)
                            .Input((step, data, context) => step.Item = context.Item)
                        .Then<DoSomething>())
                .Then<User3>();
        }
    }

    public class User
    {
        public List<int> Nums { get; set; }
        public List<Role> Roles { get; set; }
    }
}
