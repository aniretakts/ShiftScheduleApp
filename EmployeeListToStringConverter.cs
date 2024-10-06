using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data; // Make sure to include this namespace

namespace WpfApp2
{
    public class EmployeeListToStringConverter : IValueConverter // Implement the interface
    {
        // Convert from List<Employee> to string
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var employees = value as List<Employee>;
            if (employees == null || !employees.Any())
            {
                return "Select employees";
            }

            return string.Join(", ", employees.Select(emp => emp.EmpFirstname));
        }

        // Convert back from string to List<Employee> (not implemented)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not needed for this use case
            throw new NotImplementedException();
        }
    }
}
