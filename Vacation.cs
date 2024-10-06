using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Vacation
    {
        public int VacationId { get; set; }
        public DateTime Date { get; set; }
        public List<Employee> MorningShiftEmployees { get; set; }
        public List<Employee> AfternoonShiftEmployees { get; set; }
        public List<Employee> EveningShiftEmployees { get; set; }

        public List<Employee> Employees { get; set; }

        public Vacation()
        {
            Employees = new List<Employee>();
            MorningShiftEmployees = new List<Employee>();
            AfternoonShiftEmployees = new List<Employee>();
            EveningShiftEmployees = new List<Employee>();
        }
    }
}
