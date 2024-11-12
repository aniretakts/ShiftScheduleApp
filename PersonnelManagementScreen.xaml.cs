using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
    /// Interaction logic for PersonnelManagementScreen.xaml
    /// </summary>
    public partial class PersonnelManagementScreen : Window
    {
        //private ObservableCollection<Employee> employees;
        public PersonnelManagementScreen()
        {
            InitializeComponent();
            LoadEmployees();
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

        private void PersonnelManagementButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the Personnel Management screen
            PersonnelManagementScreen personnelWindow = new PersonnelManagementScreen();
            personnelWindow.Show();
            this.Hide();
        }

        private void mainMenu_Click(object sender, RoutedEventArgs e)
        {
            // Open the Welcome screen
            WelcomeScreen welcomeScreen = new WelcomeScreen();
            welcomeScreen.Show();
            this.Hide();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeScreen employeeScreen = new EmployeeScreen();
            employeeScreen.Show();
            this.Hide();
        }

        private void CreateSchedule_Click(object sender, RoutedEventArgs e)
        {
            // Open the Create Schedule screen
            CreateScheduleScreen createScheduleScreen = new CreateScheduleScreen();
            createScheduleScreen.Show();
            this.Hide();
        }

        private void Department_Click(object sender, RoutedEventArgs e)
        {
            // Open the Department screen
            DepartmentScreen departmentScreen = new DepartmentScreen();
            departmentScreen.Show();
            this.Hide();
        }



        //here 

        private void LoadEmployees()
        {
            //using (var context = new MyDbContext())
            //{
            //    employees = new ObservableCollection<Employee>(context.Employees.ToList());
            //}

            //EmployeesDataGrid.ItemsSource = employees;
        }


        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            //var selectedEmployee = EmployeesDataGrid.SelectedItem as Employee;
            //if (selectedEmployee != null)
            //{
            //    using (var context = new YourDbContext())
            //    {
            //        var employeeToDelete = context.Employees.Find(selectedEmployee.EmployeeId);
            //        if (employeeToDelete != null)
            //        {
            //            context.Employees.Remove(employeeToDelete);
            //            context.SaveChanges();
            //        }
            //    }

            //    employees.Remove(selectedEmployee);
            //}
        }

        private void ClearInputFields()
        {
            //FirstNameTextBox.Text = string.Empty;
            //LastNameTextBox.Text = string.Empty;
            //DepartmentTextBox.Text = string.Empty;
            //RoleTextBox.Text = string.Empty;
        }


    }
}
