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
        public const int ExperiencedLevel = 3;

        public DownloadWeeklyScheduleScreen(int selectedDepartment, DateTime lastValidDate)
        {
            _selectedDepartment = selectedDepartment;
            _lastValidDate = lastValidDate;
            InitializeComponent();
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
            PersonnelManagementScreen personnelWindow = new PersonnelManagementScreen();
            personnelWindow.Show();
            this.Hide();
        }

        private void mainMenu_Click(object sender, RoutedEventArgs e)
        {
            WelcomeScreen welcomeScreen = new WelcomeScreen();
            welcomeScreen.Show();
            this.Hide();
        }

        private void CreateSchedule_Click(object sender, RoutedEventArgs e)
        {
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
                // Calculate week range (Monday to Sunday)
                DateTime monday = _lastValidDate.AddDays(-(int)_lastValidDate.DayOfWeek + (int)DayOfWeek.Monday);
                DateTime sunday = monday.AddDays(6);

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
                gfx.DrawString($"Vacation Report - {departmentName}", headerFont, XBrushes.Black, new XRect(0, 50, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);
                gfx.DrawString($"Week: {monday:MMMM dd, yyyy} - {sunday:MMMM dd, yyyy}", contentFont, XBrushes.Black, new XRect(0, 90, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);
                gfx.DrawLine(XPens.Black, 50, 110, page.Width.Point - 50, 110);

                // Table headers
                double startY = 130;
                double lineHeight = 20;

                gfx.DrawString("Employee ID", contentFont, XBrushes.Black, new XRect(50, startY, 100, lineHeight), XStringFormats.TopLeft);
                gfx.DrawString("Date", contentFont, XBrushes.Black, new XRect(200, startY, 100, lineHeight), XStringFormats.TopLeft);
                gfx.DrawString("Shift", contentFont, XBrushes.Black, new XRect(350, startY, 100, lineHeight), XStringFormats.TopLeft);
                gfx.DrawLine(XPens.Black, 50, startY + lineHeight - 5, page.Width.Point - 50, startY + lineHeight - 5);

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

                        // Redraw headers on the new page
                        gfx.DrawString("Employee ID", contentFont, XBrushes.Black, new XRect(50, startY, 100, lineHeight), XStringFormats.TopLeft);
                        gfx.DrawString("Day", contentFont, XBrushes.Black, new XRect(200, startY, 100, lineHeight), XStringFormats.TopLeft);
                        gfx.DrawString("Shift", contentFont, XBrushes.Black, new XRect(350, startY, 100, lineHeight), XStringFormats.TopLeft);
                        gfx.DrawLine(XPens.Black, 50, startY + lineHeight - 5, page.Width.Point - 50, startY + lineHeight - 5);

                        startY += lineHeight;
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


        private void DownloadWeeklySchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initialize the EmployeeService
                EmployeeService service = new EmployeeService();

                // Calculate week range (Monday to Sunday)
                DateTime monday = _lastValidDate.AddDays(-(int)_lastValidDate.DayOfWeek + (int)DayOfWeek.Monday);
                DateTime sunday = monday.AddDays(6);

                // Fetch employees for scheduling
                var allEmployees = service.GetAllEmployeesPerDepartment(_selectedDepartment); // Fetch all employees for the department
                var experiencedEmployees = allEmployees.Where(emp => emp.EmplLevel == ExperiencedLevel).ToList();
                var otherEmployees = allEmployees.Except(experiencedEmployees).ToList();

                // Create a new PDF document
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Weekly Schedule";

                // Add a new A4 page
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont headerFont = new XFont("Verdana", 20);
                XFont contentFont = new XFont("Verdana", 12);

                // Define column positions and widths
                double dayColumnX = 50;
                double dayColumnWidth = 80; // Reduced width for "Day"

                double shiftColumnX = dayColumnX + dayColumnWidth + 10;
                double shiftColumnWidth = 80; // Reduced width for "Shift"

                double employeesColumnX = shiftColumnX + shiftColumnWidth + 10;
                double employeesColumnWidth = page.Width.Point - employeesColumnX - 50;


                // Header
                gfx.DrawString("Weekly Schedule", headerFont, XBrushes.Black, new XRect(0, 50, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);
                gfx.DrawString($"Week: {monday:MMMM dd, yyyy} - {sunday:MMMM dd, yyyy}", contentFont, XBrushes.Black, new XRect(0, 90, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);
                gfx.DrawLine(XPens.Black, 50, 110, page.Width.Point - 50, 110);

                // Table headers
                double startY = 140;
                double lineHeight = 20;

                gfx.DrawString("Day", contentFont, XBrushes.Black, new XRect(dayColumnX, startY, dayColumnWidth, lineHeight), XStringFormats.TopLeft);
                gfx.DrawString("Shift", contentFont, XBrushes.Black, new XRect(shiftColumnX, startY, shiftColumnWidth, lineHeight), XStringFormats.TopLeft);
                gfx.DrawString("Employees", contentFont, XBrushes.Black, new XRect(employeesColumnX, startY, employeesColumnWidth, lineHeight), XStringFormats.TopLeft);
                gfx.DrawLine(XPens.Black, 50, startY + lineHeight - 5, page.Width.Point - 50, startY + lineHeight - 5);

                startY += lineHeight;

                // Create schedule
                for (DateTime day = monday; day <= sunday; day = day.AddDays(1))
                {
                    for (int shift = 1; shift <= 3; shift++)
                    {
                        var availableEmployees = allEmployees.Where(emp => emp.IsAvailableForShift(day, shift)).ToList();
                        var shiftEmployees = new List<Employee>();

                        // Assign at least one experienced employee if available
                        var experiencedEmployee = availableEmployees.FirstOrDefault(emp => emp.EmplLevel == ExperiencedLevel);
                        if (experiencedEmployee != null)
                        {
                            shiftEmployees.Add(experiencedEmployee);
                            experiencedEmployee.AssignedShifts.Add(new AssignedShift { Date = day, Shift = shift });
                            availableEmployees.Remove(experiencedEmployee);
                        }

                        // Fill remaining slots with other employees
                        foreach (var employee in availableEmployees)
                        {
                            if (shiftEmployees.Count < 3 && employee.IsAvailableForShift(day, shift))
                            {
                                shiftEmployees.Add(employee);
                                employee.AssignedShifts.Add(new AssignedShift { Date = day, Shift = shift });
                            }

                            if (shiftEmployees.Count == 3) break; // Stop after assigning 3 employees
                        }

                        // Record shift in PDF
                        gfx.DrawString(day.ToString("dddd"), contentFont, XBrushes.Black,
                            new XRect(dayColumnX, startY, dayColumnWidth, lineHeight), XStringFormats.TopLeft);
                        gfx.DrawString($"Shift {shift}", contentFont, XBrushes.Black,
                            new XRect(shiftColumnX, startY, shiftColumnWidth, lineHeight), XStringFormats.TopLeft);

                        // Draw each employee name one below the other
                        double employeeStartY = startY; // Start position for employee names
                        foreach (var employee in shiftEmployees)
                        {
                            gfx.DrawString(employee.FullName, contentFont, XBrushes.Black, new XRect(employeesColumnX, employeeStartY, employeesColumnWidth, lineHeight), XStringFormats.TopLeft);
                            employeeStartY += lineHeight; // Move down for the next employee
                        }

                        // Adjust startY for the next shift based on the number of employee rows drawn
                        startY += lineHeight * Math.Max(1, shiftEmployees.Count);

                        // Add new page if needed
                        if (startY > page.Height.Point - 50)
                        {
                            page = document.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            startY = 50; // Reset start position

                            // Redraw headers on the new page
                            gfx.DrawString("Day", contentFont, XBrushes.Black, new XRect(dayColumnX, startY, dayColumnWidth, lineHeight), XStringFormats.TopLeft);
                            gfx.DrawString("Shift", contentFont, XBrushes.Black, new XRect(shiftColumnX, startY, shiftColumnWidth, lineHeight), XStringFormats.TopLeft);
                            gfx.DrawString("Employees", contentFont, XBrushes.Black, new XRect(employeesColumnX, startY, employeesColumnWidth, lineHeight), XStringFormats.TopLeft);
                            gfx.DrawLine(XPens.Black, 50, startY + lineHeight - 5, page.Width.Point - 50, startY + lineHeight - 5);

                            startY += lineHeight;
                        }
                    }
                }

                // Save the PDF to the Downloads folder
                string downloadsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                string filePath = System.IO.Path.Combine(downloadsFolderPath, "WeeklySchedule.pdf");
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
