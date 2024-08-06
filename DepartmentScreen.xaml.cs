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
        Functions Con;
        private readonly DepartmentService _departmentService;
        public DepartmentScreen()
        {
            InitializeComponent();
            _departmentService = new DepartmentService();
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
            // Open the Welcome screen
            WelcomeScreen welcomeScreen = new WelcomeScreen();
            welcomeScreen.Show();
            this.Hide();
        }

        private void PersonnelManagementButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the Personnel Management screen
            PersonnelManagementScreen personnelWindow = new PersonnelManagementScreen();
            personnelWindow.Show();
            this.Hide();
        }

        private void CreateSchedule_Click(object sender, RoutedEventArgs e)
        {
            // Open the Welcome screen
            AddVacationScreen addVacationScreen = new AddVacationScreen();
            addVacationScreen.Show();
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
                    DepName = departmentName  // Assuming your property is DepartmentName
                };

                _departmentService.AddDepartment(newDepartment);
                LoadData(); // Refresh the DataGrid
            }
            else
            {
                MessageBox.Show("Please enter a valid department name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int departmentId)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this department?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _departmentService.DeleteDepartment(departmentId);
                    LoadData(); // Refresh the DataGrid
                }
            }

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
