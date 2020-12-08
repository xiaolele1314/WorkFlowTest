using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Persistence.EntityFramework.Services;
using WorkflowCore.Persistence.MySQL;

namespace WorkFlowTest.ParallelForeach
{
    public class DoSomething : StepBody
    {
        private readonly MysqlContext _context;

        //public DoSomething()
        //{

        //}
        public DoSomething(WorkflowDbContext context)
        {
            this._context = (MysqlContext)context;
        }
        public string Message { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var a = this._context;
            Console.WriteLine(Message);
            return ExecutionResult.Next();
        }
    }
}
