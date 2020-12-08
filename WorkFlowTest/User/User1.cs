using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest
{
    public class User1 : StepBody
    {

        public IPersistenceProvider _persistenceProvider;
        public IWorkflowController _workflowController;

        //public User1()
        //{

        //}
    public User1(IWorkflowController workflowController, IPersistenceProvider persistenceProvider)
    {
        this._workflowController = workflowController;
        this._persistenceProvider = persistenceProvider;
    }

    public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("start");
            Console.WriteLine("user1");
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
           
            context.Workflow.Status = WorkflowStatus.Suspended;
            

            return ExecutionResult.Next();
        }
    }
}
