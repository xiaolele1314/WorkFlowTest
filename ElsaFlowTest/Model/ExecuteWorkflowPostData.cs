using Elsa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaFlowTest.Model
{
    public class ExecuteWorkflowPostData
    {
        public List<string> StartIds { get; set; }
        public Variables Input { get; set; }
        public string CorrelationId { get; set; }
    }
}
