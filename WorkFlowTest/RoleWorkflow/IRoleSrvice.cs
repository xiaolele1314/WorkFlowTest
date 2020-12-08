using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlowTest.RoleWorkflow
{
    public interface IRoleSrvice
    {
        public void SetStudentRole();
        public void SetTeacherRole();
        public Role GetRole();
    }
}
