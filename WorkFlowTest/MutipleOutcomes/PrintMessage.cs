using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowTest.MutipleOutcomes
{
    public class PrintMessage : StepBody
    {
        public string Message { get; set; }
        public string id { get; set; }

        public IPersistenceProvider _persistenceProvider;
        public IWorkflowController _workflowController;
        public PrintMessage(IWorkflowController workflowController, IPersistenceProvider persistenceProvider)
        {
            this._workflowController = workflowController;
            this._persistenceProvider = persistenceProvider;
        }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var workflow = this._persistenceProvider.GetWorkflowInstance(id).Result;
            var flag = this._workflowController.ResumeWorkflow(id).Result;
            Console.WriteLine(this.Message);
            return ExecutionResult.Next();
        }
    }
}
