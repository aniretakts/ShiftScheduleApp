using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{

    public class Vacation
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public int Shift { get; set; }


        // Additional properties for display
        public string EmployeeName { get; set; }

        // Properties to hold employee lists
        public List<Employee> Employees { get; set; }

        // New properties for multiple selections
        public List<int> MorningShiftEmployees { get; set; }
        public List<int> AfternoonShiftEmployees { get; set; }
        public List<int> EveningShiftEmployees { get; set; }


        public Vacation()
        {
            MorningShiftEmployees = new List<int>();
            AfternoonShiftEmployees = new List<int>();
            EveningShiftEmployees = new List<int>();
        }

    }
}
