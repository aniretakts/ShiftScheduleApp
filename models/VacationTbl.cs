using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class VacationTbl
    {
        public int VacationId { get; set; } 
        public int EmployeeId { get; set; } 
        public DateTime Date { get; set; } 
        public ShiftType Shift { get; set; } 
        public virtual Employee Employee { get; set; }
    }

    public enum ShiftType
    {
        Morning,
        Afternoon,
        Evening
    }

}
