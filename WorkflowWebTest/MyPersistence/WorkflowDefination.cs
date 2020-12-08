using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Models;

namespace WorkflowWebTest.Test
{
    public class MyWorkflowDefination
    {
        [Key]
        public long PersistenceId { get; set; }
        public string Id { get; set; }
        public string WorkflowId { get; set; }
        public string DataType { get; set; }
        public int? WorkflowVersion { get; set; }
        public string Steps { get; set; }
        public WorkflowErrorHandling DefaultErrorBehavior { get; set; } = WorkflowErrorHandling.Retry;
        public string DefaultErrorRetryInterval { get; set; }
        public string OnPostMiddlewareError { get; set; }
    }
}
