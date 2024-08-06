using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Employee
    {
        public int EmpId { get; set; }
        public string EmpFirstname { get; set; }
        public string EmpLastname { get; set; }
        public int EmpDep { get; set; }
        public DateTime EmpBirthdate { get; set; }
        public DateTime EmpJoinDate { get; set; }
        public int EmpSalary { get; set; }
        public int EmplLevel { get; set; }


        // Additional properties for display
        public string EmpDepName { get; set; }
        public string EmplLevelName { get; set; }

    }
}
