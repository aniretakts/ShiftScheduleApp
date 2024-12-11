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

        public string FullName => $"{EmpFirstname} {EmpLastname}";


        // List of assigned shifts for the employee
        public List<AssignedShift> AssignedShifts { get; set; } = new List<AssignedShift>();

        public bool IsAvailableForShift(DateTime date, int shift)
        {
            // Define the time ranges for shifts
            var shiftTimes = new Dictionary<int, (TimeSpan Start, TimeSpan End)>
        {
            { 1, (TimeSpan.FromHours(6), TimeSpan.FromHours(14)) }, // Morning shift: 6 AM - 2 PM
            { 2, (TimeSpan.FromHours(14), TimeSpan.FromHours(22)) }, // Evening shift: 2 PM - 10 PM
            { 3, (TimeSpan.FromHours(22), TimeSpan.FromHours(6)) } // Night shift: 10 PM - 6 AM
        };

            if (!shiftTimes.ContainsKey(shift))
            {
                throw new ArgumentException("Invalid shift number.");
            }

            var (newShiftStart, newShiftEnd) = shiftTimes[shift];
            var newShiftDateTimeStart = date.Add(newShiftStart);
            var newShiftDateTimeEnd = shift == 3 ? date.AddDays(1).Add(newShiftEnd) : date.Add(newShiftEnd);

            // Check all assigned shifts for conflicts
            foreach (var assignedShift in AssignedShifts)
            {
                var existingShiftStart = assignedShift.Date.Add(shiftTimes[assignedShift.Shift].Start);
                var existingShiftEnd = assignedShift.Shift == 3
                    ? assignedShift.Date.AddDays(1).Add(shiftTimes[assignedShift.Shift].End)
                    : assignedShift.Date.Add(shiftTimes[assignedShift.Shift].End);

                // Ensure at least 11 hours break
                if (newShiftDateTimeStart < existingShiftEnd.AddHours(11) &&
                    newShiftDateTimeEnd > existingShiftStart.AddHours(-11))
                {
                    return false;
                }
            }

            return true;
        }
    }

    // Helper class for assigned shifts
    public class AssignedShift
    {
        public DateTime Date { get; set; }
        public int Shift { get; set; } // Shift number (1, 2, or 3)
    }

}
