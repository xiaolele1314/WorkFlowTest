using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowWebTest.CheckFlow;

namespace WorkflowWebTest.Test
{
    public class WorkflowPostData
    {
        public string WorkflowId { get; set; }
        public int? Version { get; set; }
        public string Reference { get; set; }
        public CheckData Data { get; set; }
    }
}
