using Elsa.Activities.Console.Activities;
using Elsa.Activities.UserTask.Activities;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaFlowTest.Flow
{
    public class UserWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .StartWith<WriteLine>(activity => activity.TextExpression = new LiteralExpression("Workflow started. Waiting for user action."))
                .Then<UserTask>(
                    activity => activity.Actions = new[] { "agree", "disagree", "Undo" },
                    userTask =>
                    {
                        userTask
                            .When("agree")
                            .Then<UserTask>(
                                activity => activity.Actions = new[] { "agree1", "disagree1", "Undo1" },
                                agreeUserTask =>
                                {
                                    agreeUserTask
                                        .When("agree1")
                                        .Then<WriteLine>(activity => activity.TextExpression = new LiteralExpression("Greate! Your work has been passed."))
                                        .Then("Exit");

                                    agreeUserTask
                                        .When("disagree1")
                                        .Then<WriteLine>(activity => activity.TextExpression = new LiteralExpression("Sorry! Your work has been rejected."))
                                        .Then("Exit");

                                    agreeUserTask
                                        .When("undo1")
                                        .Then<WriteLine>(activity => activity.TextExpression = new LiteralExpression("So close! Your work needs a little bit more work."))
                                        .Then("WaitAgree");
                                },
                                "WaitAgree")
                            .Then("Exit");

                        userTask.When("disagree")
                            .Then<WriteLine>(activity => activity.TextExpression = new LiteralExpression("Sorry! Your work has been rejected."))
                            .Then("Exit");

                        userTask.When("undo")
                            .Then<WriteLine>(activity => activity.TextExpression = new LiteralExpression("So close! Your work needs a little bit more work."))
                            .Then("WaitUser");
                    },
                    "WaitUser"
                )
                .Add<WriteLine>(activity => activity.TextExpression = new LiteralExpression("Workflow finished."), "Exit");
        }
    }
}
