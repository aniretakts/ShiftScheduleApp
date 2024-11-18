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
    /// Interaction logic for CreateScheduleScreen.xaml
    /// </summary>
    public partial class CreateScheduleScreen : Window
    {
        private readonly DepartmentService _departmentService;
        public CreateScheduleScreen()
        {
            InitializeComponent();
            _departmentService = new DepartmentService();
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            List<Department> departments = _departmentService.GetDepartments();
            Department.ItemsSource = departments;
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
            // Open the Create Schedule screen
            CreateScheduleScreen createScheduleScreen = new CreateScheduleScreen();
            createScheduleScreen.Show();
            this.Hide();
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected department
            Department selectedDepartment = (Department)Department.SelectedItem;

            if (selectedDepartment != null) {
                int departmentId = selectedDepartment.DepId;
                AddMondayVacationScreen addMondayVacationScreen = new AddMondayVacationScreen(departmentId);
                addMondayVacationScreen.Show();
                this.Hide();
            } else {
                MessageBox.Show("Please select a department before proceeding.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Enable the Start button only if a department is selected
            StartButton.IsEnabled = Department.SelectedItem != null;
        }


    }
}
