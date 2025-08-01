﻿using System;
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
using System.Data;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using static WpfApp2.DepartmentScreen;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for DepartmentScreen.xaml
    /// </summary>
    public partial class DepartmentScreen : Window
    {
        private readonly DepartmentService _departmentService;
        private readonly EmployeeService _employeeService;

        public DepartmentScreen()
        {
            InitializeComponent();
            _departmentService = new DepartmentService();
            _employeeService = new EmployeeService();
            LoadData();
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
        private void LoadData()
        {
            List<Department> departments = _departmentService.GetDepartments();
            DepList.ItemsSource = departments;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string departmentName = NewDepartmentName.Text;

            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                Department newDepartment = new Department
                {
                    DepName = departmentName 
                };

                _departmentService.AddDepartment(newDepartment);
                LoadData();

                NewDepartmentName.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Παρακαλώ εισάγετε ένα έγκυρο όνομα τμήματος.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int departmentId)
            {
                
                var employeesInDepartment = _employeeService.GetEmployees()
                                                            .Where(emp => emp.EmpDep == departmentId)
                                                            .ToList();

                if (employeesInDepartment.Any())
                {
                    MessageBox.Show("Αυτό το τμήμα δεν μπορεί να διαγραφεί επειδή υπάρχουν υπάλληλοι που έχουν αντιστοιχιστεί σε αυτό.",
                                    "Deletion Blocked",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Είστε σίγουροι ότι θέλετε να διαγράψετε αυτό το τμήμα;",
                                                          "Επιβεβαίωση Διαγραφής",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _departmentService.DeleteDepartment(departmentId);
                    LoadData();
                }
            }
        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
