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
using System.Windows.Shell;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for AddVacationScreen.xaml
    /// </summary>
    public partial class AddVacationScreen : Window
    {
        private readonly EmployeeService _employeeService;
        private readonly VacationService _vacationService;
        private readonly List<Employee> _employees;

        public AddVacationScreen()
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
            _vacationService = new VacationService();
            _employees = _employeeService.GetEmployees();
            LoadVacationGrid();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
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

        private void LoadVacationGrid()
        {
            List<Vacation> vacations = _vacationService.GetVacations();
            var vacationData = new List<Vacation>();

            // Get today's date
            DateTime today = DateTime.Now;

            // Calculate the Monday of the current week
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));

            // Loop through the 7 days of the week starting from Monday
            for (int i = 0; i < 7; i++)
            {
                DateTime currentDate = startOfWeek.AddDays(i); // Start from Monday and add i days

                var vacation = new Vacation
                {
                    Date = currentDate,
                    Employees = _employees
                };

                // Find vacations for the current day
                var dayVacations = vacations.Where(v => v.Date.Date == currentDate.Date).ToList();
                if (dayVacations.Any())
                {
                    vacation.MorningShiftEmployeeId = dayVacations.FirstOrDefault(v => v.MorningShiftEmployeeId != 0)?.MorningShiftEmployeeId ?? 0;
                    vacation.AfternoonShiftEmployeeId = dayVacations.FirstOrDefault(v => v.AfternoonShiftEmployeeId != 0)?.AfternoonShiftEmployeeId ?? 0;
                    vacation.EveningShiftEmployeeId = dayVacations.FirstOrDefault(v => v.EveningShiftEmployeeId != 0)?.EveningShiftEmployeeId ?? 0;
                }

                vacationData.Add(vacation);
            }

            // Bind the data to the DataGrid
            VacationGrid.ItemsSource = vacationData;
        }




        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            //save and continue 
        }


    }
}
