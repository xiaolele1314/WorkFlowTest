using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlowTest.RoleWorkflow
{
    public class RoleService : IRoleSrvice
    {
        public Role Role;
        public void SetStudentRole()
        {
            this.Role = Role.Student;
        }

        public void SetTeacherRole()
        {
            this.Role = Role.Teacher;
        }

        public Role GetRole()
        {
            return this.Role;
        }
    }
}
