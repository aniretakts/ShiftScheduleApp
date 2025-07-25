using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp2
{
    public class Employee
    {
        public int EmpId { get; set; }
        public string EmpFirstname { get; set; }
        public string EmpLastname { get; set; }
        public DateTime EmpBirthdate { get; set; }
        public DateTime EmpJoinDate { get; set; }
        public int EmpSalary { get; set; }
        public int EmpLevel { get; set; }
        public DateTime EmpHealthCertExpiration { get; set; }
        public DateTime EmpWorkContractExpiration { get; set; }
        public int EmpWorkingDaysPerWeek { get; set; }
        public int EmpActive { get; set; }
        public int DepartmentId { get; set; }  // Foreign key to T_DEPARTMENT


        // Additional properties for display
        public int EmpDep { get; set; }
        public string EmpDepName { get; set; }
        public string EmpLevelName { get; set; }
        public string EmpActiveDisplay => EmpActive == 1 ? "Ναι" : "Όχι";  // for display only

        public string FullName => $"{EmpFirstname} {EmpLastname}";

        private readonly List<VacationTbl> _vacationTable;

        public List<AssignedShift> AssignedShifts { get; set; } = new List<AssignedShift>();

        public bool IsAvailableForShift(int employeeId, DateTime date, ShiftType shift)
        {
            // Check if the employee is listed in the vacation table for the given date and shift
            return _vacationTable != null &&
           !_vacationTable.Any(v => v.EmployeeId == employeeId && v.Date.Date == date.Date && v.Shift == shift);
        }

        
    }

    // Helper class for assigned shifts
    public class AssignedShift
    {
        public DateTime Date { get; set; }
        public ShiftType Shift { get; set; }
    }



}
