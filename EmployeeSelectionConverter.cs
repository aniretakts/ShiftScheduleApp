using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfApp2
{
    public class EmployeeSelectionConverter : IValueConverter
    {
        // Convert: Determines if the CheckBox should be checked based on whether the employee is part of the shift
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rowContext = value as VacationMorningShift; // or whichever shift class you're using

            if (rowContext == null || rowContext.Employee == null)
                return false;

            // Access the employee and shift list from the context
            var employee = rowContext.Employee;
            var shiftEmployees = rowContext.Vacation.MorningShiftEmployees; // Access the correct shift list

            if (shiftEmployees == null || employee == null)
                return false;

            // Return true if the employee is assigned to this shift
            return shiftEmployees.Any(shift => shift.EmpId == employee.EmpId);
        }

        // ConvertBack: Adds/removes the employee to/from the shift based on the checkbox state
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value; // The CheckBox checked state
            var rowContext = parameter as VacationMorningShift; // or your equivalent model

            if (rowContext == null)
                return null;

            var employee = rowContext.Employee; // Get the employee from the row context
            var shiftEmployees = rowContext.Vacation.MorningShiftEmployees; // Access the correct shift list

            if (shiftEmployees == null || employee == null)
                return null;

            if (isChecked)
            {
                // Add the employee to the shift if checked
                if (!shiftEmployees.Any(e => e.EmpId == employee.EmpId))
                {
                    shiftEmployees.Add(employee); // Add Employee, not VacationMorningShift
                }
            }
            else
            {
                // Remove the employee from the shift if unchecked
                var employeeToRemove = shiftEmployees.FirstOrDefault(e => e.EmpId == employee.EmpId);
                if (employeeToRemove != null)
                {
                    shiftEmployees.Remove(employeeToRemove); // Remove Employee, not VacationMorningShift
                }
            }

            return shiftEmployees; // Return the updated list of employees for the shift
        }

    }




}
