using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkFlowTest.ParallelForeach;

namespace WorkFlowTest.RoleWorkflow
{
    public class RoleSwitchWorkflow : IWorkflow<RoleData>
    {
        public string Id => "role workflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<RoleData> builder)
        {
            var branch1 = builder
                .CreateBranch()
                .StartWith<DoSomething>()
                    .Input(step => step.Message, data => "Starting 1")
                .Then<RoleStep>()
                    .Input(step => step.Role, data => data.Role)
                    .Output(data => data.Role, step => step.Role)
                .If(data => data.Role == Role.Student)
                    .Do(then => then
                        .StartWith<StudentStep>())
                .If(data => data.Role == Role.Teacher)
                    .Do(then => then
                        .StartWith<TeacherStep>());

            var branch2 = builder
                .CreateBranch()
                .StartWith<DoSomething>()
                    .Input(step => step.Message, data => "Starting 2")
                .If(data => data.Role == Role.Student)
                    .Do(then => then
                        .StartWith<StudentStep>())
                .If(data => data.Role == Role.Teacher)
                    .Do(then => then
                        .StartWith<TeacherStep>());

            builder
                .StartWith<DoSomething>()
                    .Input(step => step.Message, data => "branch")
                .Decide(data => data.branchId)
                    .Branch(1, branch1)
                    .Branch(2, branch2);




        }
    }

    public class RoleData
    {
        public int branchId { get; set; }
        public Role Role { get; set; }
    }
}
