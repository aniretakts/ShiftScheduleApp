using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddMondayVacationScreen.xaml
    /// </summary>
    public partial class AddMondayVacationScreen : Window
    {
        private readonly EmployeeService _employeeService;
        private DateTime _savedDate;
        private readonly AddMondayVacationViewModel _viewModel;

        public AddMondayVacationScreen(int selectedDepartment)
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
            DataContext = new AddMondayVacationViewModel(selectedDepartment); // to load the ListBox
            _viewModel = new AddMondayVacationViewModel(selectedDepartment); // to save the data 
            //LoadShiftData();
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
            // Open the Create schedule screen
            CreateScheduleScreen createScheduleScreen = new CreateScheduleScreen();
            createScheduleScreen.Show();
            this.Hide();
        }


        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // move to the next day
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SaveDate_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = datePicker.SelectedDate.Value;
                MessageBox.Show($"Selected date: {selectedDate.ToShortDateString()}");
                _savedDate = selectedDate;
            }
            else
            {
                MessageBox.Show("Please select a date.");
            }
        }


        private void DaySave_Click(object sender, RoutedEventArgs e)
        {
            // Check if a date is selected
            if (_savedDate != DateTime.MinValue)
            {
                // Get selected date
                DateTime selectedDate = _savedDate;

                // Get selected employees from each shift
                var selectedMorningShift = MorningShiftListBox.SelectedItems.Cast<Employee>().ToList();
                var selectedAfternoonShift = AfternoonShiftListBox.SelectedItems.Cast<Employee>().ToList();
                var selectedEveningShift = EveningShiftListBox.SelectedItems.Cast<Employee>().ToList();

                // Add vacation records for selected employees
                AddVacationForShift(selectedMorningShift, selectedDate, ShiftType.Morning);
                AddVacationForShift(selectedAfternoonShift, selectedDate, ShiftType.Afternoon);
                AddVacationForShift(selectedEveningShift, selectedDate, ShiftType.Evening);

                // Optionally, notify user that save was successful
                MessageBox.Show("Vacations saved successfully!");
            }
            else
            {
                MessageBox.Show("Please select a date.");
            }
        }

        // Helper method to save vacation records for a given shift
        private void AddVacationForShift(List<Employee> employees, DateTime vacationDate, ShiftType shiftType)
        {
            foreach (var employee in employees)
            {
                var vacation = new VacationTbl
                {
                    EmployeeId = employee.EmpId, // Assuming Employee has EmpId
                    Date = vacationDate,
                    Shift = shiftType
                };

                // Save vacation to database (you might need to call your EmployeeService or use EF)
                _employeeService.AddVacation(vacation);
            }
        }


    }
}
