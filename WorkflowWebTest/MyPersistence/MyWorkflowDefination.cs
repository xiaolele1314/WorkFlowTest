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
        public string InstanceId { get; set; }
        public string Id { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public string WorkflowData { get; set; }
    }
}
