using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2
{
    public class AddMondayVacationViewModel : BaseViewModel
    {
        private readonly EmployeeService _employeeService;

        public ObservableCollection<Employee> MorningShiftEmployees { get; set; }
        public ObservableCollection<Employee> AfternoonShiftEmployees { get; set; }
        public ObservableCollection<Employee> EveningShiftEmployees { get; set; }

        public AddMondayVacationViewModel(int selectedDepartment)
        {
            _employeeService = new EmployeeService();

            // Get all employees from the service
            var allEmployees = _employeeService.GetEmployees();

            // Filter employees based on the selected department
            var filteredEmployees = allEmployees
                .Where(e => e.EmpDep == selectedDepartment)
                .ToList();

            // Populate ObservableCollections
            MorningShiftEmployees = new ObservableCollection<Employee>(filteredEmployees);
            AfternoonShiftEmployees = new ObservableCollection<Employee>(filteredEmployees);
            EveningShiftEmployees = new ObservableCollection<Employee>(filteredEmployees);

            // Load employees based on department (you may want to add your logic here)
            MorningShiftEmployees = new ObservableCollection<Employee>(_employeeService.GetEmployeesByShift(selectedDepartment, ShiftType.Morning));
            AfternoonShiftEmployees = new ObservableCollection<Employee>(_employeeService.GetEmployeesByShift(selectedDepartment, ShiftType.Afternoon));
            EveningShiftEmployees = new ObservableCollection<Employee>(_employeeService.GetEmployeesByShift(selectedDepartment, ShiftType.Evening));
        }


        // Implementation of INotifyPropertyChanged (if needed)
        //public event PropertyChangedEventHandler PropertyChanged;
        //private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }
}
