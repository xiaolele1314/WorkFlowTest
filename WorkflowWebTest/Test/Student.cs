using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkflowWebTest.Test
{
    [Table("student")]
    public class Student
    {
        [Key]
        public string id { get; set; }
    }
}
