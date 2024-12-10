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

        private void LoadVacationGrid()
        {
            // Retrieve vacations from the database
            List<Vacation> vacations = _vacationService.GetVacations();
            var vacationData = new List<Vacation>();

            var daysOfWeek = new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };

            // Iterate through each day of the week, starting from Monday
            foreach (DayOfWeek day in daysOfWeek)
            {
                var vacation = new Vacation
                {
                    Date = GetNextWeekday(day),
                    Employees = _employees // All employees, for multi-select dropdowns
                };

                // Retrieve vacations for the current day
                var dayVacations = vacations.Where(v => v.Date.DayOfWeek == day).ToList();

                if (dayVacations.Any())
                {
                    // Morning Shift: Get the list of employees for the morning shift on the current day
                    vacation.MorningShiftEmployees = dayVacations.SelectMany(v => v.MorningShiftEmployees).Distinct().ToList();

                    // Afternoon Shift: Get the list of employees for the afternoon shift on the current day
                    vacation.AfternoonShiftEmployees = dayVacations.SelectMany(v => v.AfternoonShiftEmployees).Distinct().ToList();

                    // Evening Shift: Get the list of employees for the evening shift on the current day
                    vacation.EveningShiftEmployees = dayVacations.SelectMany(v => v.EveningShiftEmployees).Distinct().ToList();
                }

                vacationData.Add(vacation);
            }

            // Bind the data to the grid
            VacationGrid.ItemsSource = vacationData;
        }

        private DateTime GetNextWeekday(DayOfWeek day)
        {
            // Calculate the next occurrence of the given day
            int daysUntilDay = ((int)day - (int)DateTime.Today.DayOfWeek + 7) % 7;
            return DateTime.Today.AddDays(daysUntilDay);
        }




        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            //save and continue 
        }


    }
}
