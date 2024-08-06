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
    /// Interaction logic for AddEmployeeScreen.xaml
    /// </summary>
    public partial class AddEmployeeScreen : Window
    {
        Functions Con;
        public AddEmployeeScreen()
        {
            InitializeComponent();
            Con = new Functions();
            ShowEmployees();
        }

        private void ShowEmployees() {
            //string Query = "Select * from Employees";
            //EmployeeList.DataSource = Con.GetData(Query);


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

        private void SaveNewEmployee_Click(object sender, RoutedEventArgs e) { 
        //    // save new employee 
        //    try
        //    {
        //        if (firstname.Text == ""  || GenCb.selectedIndex == -1 ) {
        //            MessageBox.Show("Missing Data");
        //        }
        //        else {
        //            string Emp = EmployeesDataTable.Text;
        //            string Query = "insert into Employee values('{0}')";
        //            Query = string.Format(Query, Employee.Text);
        //            Con.SetData(Query);
        //            ShowEmployees();
        //            MessageBox.Show("Employee Added");

        //        }
        //    } catch (Exception Ex) {
        //        MessageBox.Show("Ex.Message");
        //    }

        }
    }
}
