using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Vacation
    {
        public DateTime Date { get; set; }
        public int MorningShiftEmployeeId { get; set; }
        public int AfternoonShiftEmployeeId { get; set; }
        public int EveningShiftEmployeeId { get; set; }

        public List<Employee> Employees { get; set; }

        public Vacation()
        {
            Employees = new List<Employee>();
        }
    }
}
