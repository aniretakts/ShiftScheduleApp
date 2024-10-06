using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class VacationMorningShift
    {
        public int VacationId { get; set; }
        public Vacation Vacation { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
