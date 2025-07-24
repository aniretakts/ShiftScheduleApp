using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for EmployeeScreen.xaml
    /// </summary>
    public partial class EmployeeScreen : Window
    {
        private readonly EmployeeService _employeeService;
        private readonly DepartmentService _departmentService;
        private readonly LevelService _levelService;

        public EmployeeScreen()
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
            _departmentService = new DepartmentService();
            _levelService = new LevelService();
            LoadEmployees();
            LoadDepartments();
            LoadLevels();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void mainMenu_Click(object sender, RoutedEventArgs e)
        {
            WelcomeScreen welcomeScreen = new WelcomeScreen();
            welcomeScreen.Show();
            this.Hide();
        }

        private void PersonnelManagementButton_Click(object sender, RoutedEventArgs e)
        {
            PersonnelManagementScreen personnelWindow = new PersonnelManagementScreen();
            personnelWindow.Show();
            this.Hide();
        }

        private void CreateSchedule_Click(object sender, RoutedEventArgs e)
        {
            CreateScheduleScreen createScheduleScreen = new CreateScheduleScreen();
            createScheduleScreen.Show();
            this.Hide();
        }

        private void LoadEmployees()
        {
            List<Employee> employees = _employeeService.GetEmployees();
            List<Department> departments = _departmentService.GetDepartments();
            List<Level> levels = _levelService.GetLevels();

            foreach (var employee in employees)
            {
                var department = departments.FirstOrDefault(d => d.DepId == employee.EmpDep);
                if (department != null)
                {
                    employee.EmpDepName = department.DepName;
                }

                var level = levels.FirstOrDefault(l => l.LevelId == employee.EmpLevel);
                if (level != null)
                {
                    employee.EmpLevelName = level.LevelName;
                }
            }

            EmpList.ItemsSource = employees;
        }

        private void LoadDepartments()
        {
            List<Department> departments = _departmentService.GetDepartments();
            NewEmpDep.ItemsSource = departments;
        }

        private void LoadLevels()
        {
            List<Level> levels = _levelService.GetLevels();
            NewEmpLevel.ItemsSource = levels;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string empFirstname = NewEmpFirstname.Text;
            string empLastname = NewEmpLastname.Text;
            int empDep = (NewEmpDep.SelectedValue != null) ? (int)NewEmpDep.SelectedValue : 0;
            DateTime empBirthdate = NewEmpBirthdate.SelectedDate ?? DateTime.MinValue;
            DateTime empJoinDate = NewEmpJoinDate.SelectedDate ?? DateTime.MinValue;
            int empSalary = int.TryParse(NewEmpSalary.Text, out var salary) ? salary : 0;
            int empLevel = (NewEmpLevel.SelectedValue != null) ? (int)NewEmpLevel.SelectedValue : 0;
            DateTime empHealthCertExpiration = NewEmpHealthCertExpiration.SelectedDate ?? DateTime.MinValue;
            DateTime empWorkContractExpiration = NewEmpWorkContractExpiration.SelectedDate ?? DateTime.MinValue;
            int empWorkingDaysPerWeek = int.TryParse(NewEmpWorkingDaysPerWeek.Text, out var days) ? days : 0;

            if (!string.IsNullOrWhiteSpace(empFirstname) && !string.IsNullOrWhiteSpace(empLastname) && empDep > 0 && empBirthdate != DateTime.MinValue && empJoinDate != DateTime.MinValue && empSalary > 0 && empLevel > 0 && empHealthCertExpiration != DateTime.MinValue && empWorkContractExpiration != DateTime.MinValue && empWorkingDaysPerWeek > 0)
            {
                Employee newEmployee = new Employee
                {
                    EmpFirstname = empFirstname,
                    EmpLastname = empLastname,
                    EmpDep = empDep,
                    EmpBirthdate = empBirthdate,
                    EmpJoinDate = empJoinDate,
                    EmpSalary = empSalary,
                    EmpLevel = empLevel,
                    EmpHealthCertExpiration = empHealthCertExpiration,
                    EmpWorkContractExpiration = empWorkContractExpiration,
                    EmpWorkingDaysPerWeek = empWorkingDaysPerWeek
                };

                _employeeService.AddEmployee(newEmployee);
                LoadEmployees(); // Refresh the DataGrid
                ClearForm();
            }
            else
            {
                MessageBox.Show("Please enter valid employee details.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearForm()
        {
            NewEmpFirstname.Text = string.Empty;
            NewEmpLastname.Text = string.Empty;
            NewEmpDep.SelectedValue = null;
            NewEmpBirthdate.SelectedDate = null;
            NewEmpJoinDate.SelectedDate = null;
            NewEmpSalary.Text = string.Empty;
            NewEmpLevel.Text = string.Empty;
            NewEmpHealthCertExpiration.Text = String.Empty;
            NewEmpWorkContractExpiration.Text = String.Empty;   
            NewEmpWorkingDaysPerWeek.Text = String.Empty;
        }
    }
}
