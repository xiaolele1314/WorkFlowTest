using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkflowWebTest.MyPersistence
{
    public class MyWorkflowDefinationRelation
    {
        [Key]
        public long PersistenceId { get; set; }
        public string Id { get; set; }
        public string WorkflowId { get; set; }
        public string WorkflowDefinationId { get; set; }
    }
}
