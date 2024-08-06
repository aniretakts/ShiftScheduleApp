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

            // List of days starting from Monday
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

            // Create an entry for each day and shift
            foreach (DayOfWeek day in daysOfWeek)
            {
                for (int shift = 1; shift <= 3; shift++)
                {
                    // Find the vacation entry if it exists
                    var vacation = vacations.FirstOrDefault(v => v.Date.DayOfWeek == day && v.Shift == shift);

                    if (vacation == null)
                    {
                        vacation = new Vacation
                        {
                            Date = DateTime.Now.AddDays((int)day - (int)DateTime.Now.DayOfWeek + (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 1 : 0)),
                            Shift = shift,
                            Employees = _employees
                        };
                    }
                    else
                    {
                        vacation.Employees = _employees;
                    }

                    vacationData.Add(vacation);
                }
            }

            VacationGrid.ItemsSource = vacationData;
        }


        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            //save and continue 
        }


    }
}
