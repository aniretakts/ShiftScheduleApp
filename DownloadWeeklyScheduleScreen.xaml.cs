using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
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
using System.Web.Configuration;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for DownloadWeeklyScheduleScreen.xaml
    /// </summary>
    public partial class DownloadWeeklyScheduleScreen : Window
    {
        private readonly int _selectedDepartment;
        private readonly DateTime _lastValidDate;

        public DownloadWeeklyScheduleScreen(int selectedDepartment, DateTime lastValidDate)
        {
            _selectedDepartment = selectedDepartment;
            _lastValidDate = lastValidDate;
            InitializeComponent();
        }

        // TODO: to be removed? In all pages that it exists
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

        private void CreateSchedule_Click(object sender, RoutedEventArgs e)
        {
            // Open the Create schedule screen
            CreateScheduleScreen createScheduleScreen = new CreateScheduleScreen();
            createScheduleScreen.Show();
            this.Hide();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the About screen
            //AboutWindow aboutWindow = new AboutWindow();
            //aboutWindow.Show();
        }

        private void DownloadVacationReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initialize the EmployeeService
                EmployeeService service = new EmployeeService();

                // Fetch vacation data for the week ending _lastValidDate
                var lastWeekVacations = service.GetVacationsForWeek(_lastValidDate);
                var firstValidDate = _lastValidDate.AddDays(-6);

                // Create a new PDF document
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Vacation Report";

                // Add a new A4 page
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont headerFont = new XFont("Verdana", 20);
                XFont contentFont = new XFont("Verdana", 12);

                string departmentName = lastWeekVacations.FirstOrDefault()?.Employee?.EmpDepName ?? "Unknown Department";

                // Header
                gfx.DrawString($"Vacation Report - {departmentName}", headerFont, XBrushes.Black,
                    new XRect(0, 50, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);
                gfx.DrawString($"Week starting: {firstValidDate:MMMM dd, yyyy}", contentFont, XBrushes.Black,
                    new XRect(0, 90, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);
                gfx.DrawString($"Week ending: {_lastValidDate:MMMM dd, yyyy}", contentFont, XBrushes.Black,
                    new XRect(0, 120, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);

                // Table headers
                double startY = 160;
                double lineHeight = 20;

                gfx.DrawString("Employee ID", contentFont, XBrushes.Black, new XRect(50, startY, 100, lineHeight), XStringFormats.TopLeft);
                gfx.DrawString("Date", contentFont, XBrushes.Black, new XRect(200, startY, 100, lineHeight), XStringFormats.TopLeft);
                gfx.DrawString("Shift", contentFont, XBrushes.Black, new XRect(350, startY, 100, lineHeight), XStringFormats.TopLeft);

                startY += lineHeight;

                // Populate table
                foreach (var vacation in lastWeekVacations)
                {
                    gfx.DrawString(vacation.Employee.FullName.ToString(), contentFont, XBrushes.Black, new XRect(50, startY, 100, lineHeight), XStringFormats.TopLeft);
                    gfx.DrawString(vacation.Date.ToString("MMMM dd, yyyy"), contentFont, XBrushes.Black, new XRect(200, startY, 100, lineHeight), XStringFormats.TopLeft);
                    gfx.DrawString(vacation.Shift.ToString(), contentFont, XBrushes.Black, new XRect(350, startY, 100, lineHeight), XStringFormats.TopLeft);

                    startY += lineHeight;

                    // Add new page if needed
                    if (startY > page.Height.Point - 50)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        startY = 50; // Reset start position
                    }
                }

                // Save the PDF to the Downloads folder
                string downloadsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                string filePath = System.IO.Path.Combine(downloadsFolderPath, "VacationReport.pdf");
                document.Save(filePath);

                // Open the file
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating PDF: {ex.Message}");
            }

        }
    }
}
