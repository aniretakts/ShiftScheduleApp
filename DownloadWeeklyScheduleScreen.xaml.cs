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
        private readonly List<VacationTbl> _vacationTable;

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

                // Calculate week range (Monday to Sunday)
                DateTime monday = _lastValidDate.AddDays(-6);
                DateTime sunday = _lastValidDate;

                // Fetch vacation data for the week ending _lastValidDate
                var lastWeekVacations = service.GetVacationsForWeek(monday, sunday);

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

                // Format the dates to "yyyy-MM-dd" (or any preferred format)
                string formattedMonday = monday.ToString("yyyy-MM-dd");
                string formattedSunday = sunday.ToString("yyyy-MM-dd");

                // Save the PDF to the Downloads folder
                string downloadsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                string filePath = System.IO.Path.Combine(downloadsFolderPath, $"VacationReport-{formattedMonday}-{formattedSunday}.pdf");
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
                DateTime monday = _lastValidDate.AddDays(-6);
                DateTime sunday = _lastValidDate;

                // Fetch employees for scheduling
                var allEmployees = service.GetAllEmployeesPerDepartment(_selectedDepartment) ?? new List<Employee>();
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
                double dayColumnWidth = 200;

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

                // Draw table logic with null checks
                for (DateTime day = monday; day <= sunday; day = day.AddDays(1))
                {
                    var shifts = Enum.GetValues(typeof(ShiftType)).Cast<ShiftType>();
                    foreach (ShiftType shift in shifts)
                    {
                        var availableEmployees = allEmployees
                            .Where(emp => emp.IsAvailableForShift(emp.EmpId, day, shift))
                            .ToList() ?? new List<Employee>();

                        var shiftEmployees = new List<Employee>();

                        var experiencedEmployee = availableEmployees.FirstOrDefault(emp => emp.EmplLevel == ExperiencedLevel);
                        if (experiencedEmployee != null)
                        {
                            shiftEmployees.Add(experiencedEmployee);
                            if (experiencedEmployee.AssignedShifts == null)
                            {
                                experiencedEmployee.AssignedShifts = new List<AssignedShift>();
                            }
                            experiencedEmployee.AssignedShifts.Add(new AssignedShift { Date = day, Shift = shift });
                            availableEmployees.Remove(experiencedEmployee);
                        }

                        foreach (var employee in availableEmployees)
                        {
                            if (shiftEmployees.Count < 3)
                            {
                                shiftEmployees.Add(employee);
                                if (employee.AssignedShifts == null)
                                {
                                    employee.AssignedShifts = new List<AssignedShift>();
                                }
                                employee.AssignedShifts.Add(new AssignedShift { Date = day, Shift = shift });
                            }
                            if (shiftEmployees.Count == 3) break;
                        }

                        // Record shift in PDF
                        gfx.DrawString(day.ToString("dddd, MMMM dd, yyyy"), contentFont, XBrushes.Black,
                            new XRect(dayColumnX, startY, dayColumnWidth, lineHeight), XStringFormats.TopLeft);
                        gfx.DrawString($"Shift {shift}", contentFont, XBrushes.Black,
                            new XRect(shiftColumnX, startY, shiftColumnWidth, lineHeight), XStringFormats.TopLeft);

                        // Draw each employee name one below the other
                        double employeeStartY = startY; // Start position for employee names
                        foreach (var employee in shiftEmployees)
                        {
                            gfx.DrawString(employee?.FullName ?? "N/A", contentFont, XBrushes.Black, new XRect(employeesColumnX, employeeStartY, employeesColumnWidth, lineHeight), XStringFormats.TopLeft);
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

                // Format the dates to "yyyy-MM-dd" (or any preferred format)
                string formattedMonday = monday.ToString("yyyy-MM-dd");
                string formattedSunday = sunday.ToString("yyyy-MM-dd");

                // Save the PDF to the Downloads folder
                string downloadsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                string filePath = System.IO.Path.Combine(downloadsFolderPath, $"WeeklySchedule-{formattedMonday}-{formattedSunday}.pdf");
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



        private void DownloadWeeklySchedule_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initialize the EmployeeService
                EmployeeService service = new EmployeeService();

                // Calculate week range (Monday to Sunday)
                DateTime monday = _lastValidDate.AddDays(-6);
                DateTime sunday = _lastValidDate;

                // Fetch employees and vacations
                List<Employee> employees = service.GetAllEmployeesPerDepartment(_selectedDepartment); // Replace with your actual service call
                List<VacationTbl> vacations = service.GetVacationsForWeek(monday, sunday); // Replace with your actual service call

                // Group vacations by date and shift
                var vacationMap = vacations
                    .GroupBy(v => new { v.Date, v.Shift })
                    .ToDictionary(g => g.Key, g => g.Select(v => v.EmployeeId).ToList());

                // Create schedule
                Dictionary<DateTime, Dictionary<ShiftType, List<Employee>>> weeklySchedule = new Dictionary<DateTime, Dictionary<ShiftType, List<Employee>>>();

                for (DateTime day = monday; day <= sunday; day = day.AddDays(1))
                {
                    weeklySchedule[day] = new Dictionary<ShiftType, List<Employee>>();

                    foreach (ShiftType shift in Enum.GetValues(typeof(ShiftType)))
                    {
                        // Exclude employees on vacation
                        var availableEmployees = employees
                            .Where(emp => !(vacationMap.ContainsKey(new { Date = day, Shift = shift }) &&
                                            vacationMap[new { Date = day, Shift = shift }].Contains(emp.EmpId)))
                            .ToList();

                        // Prioritize experienced employees (Level > 3)
                        int experienceThreshold = 3;
                        var experiencedEmployees = availableEmployees.Where(emp => emp.EmplLevel > experienceThreshold).ToList();

                        // Fallback to level 2 employees if no experienced employees are available
                        List<Employee> shiftEmployees = new List<Employee>();

                        if (experiencedEmployees.Any())
                        {
                            // Add one experienced employee to the shift
                            shiftEmployees.Add(experiencedEmployees.First());
                        }
                        else
                        {
                            // Add one level 2 employee if no experienced employee is available
                            var level2Employees = availableEmployees.Where(emp => emp.EmplLevel == 2).ToList();
                            if (level2Employees.Any())
                            {
                                shiftEmployees.Add(level2Employees.First());
                            }
                        }

                        // Fill remaining slots with other available employees
                        var remainingSlots = 3 - shiftEmployees.Count;
                        shiftEmployees.AddRange(availableEmployees.Except(shiftEmployees).Take(remainingSlots));

                        // Ensure the shift has enough employees
                        if (shiftEmployees.Count < 3)
                        {
                            throw new Exception($"Not enough employees available for {shift} on {day:yyyy-MM-dd}");
                        }

                        weeklySchedule[day][shift] = shiftEmployees;
                    }
                }

                // Generate PDF
                var document = new PdfDocument();
                var page = document.AddPage();
                var graphics = XGraphics.FromPdfPage(page);
                var font = new XFont("Verdana", 12);

                double yPoint = 20;
                foreach (var day in weeklySchedule.Keys)
                {
                    graphics.DrawString($"Schedule for {day:yyyy-MM-dd}", font, XBrushes.Black, new XPoint(20, yPoint));
                    yPoint += 20;

                    foreach (var shift in weeklySchedule[day].Keys)
                    {
                        string employeeNames = string.Join(", ", weeklySchedule[day][shift].Select(emp => emp.FullName));
                        graphics.DrawString($"{shift}: {employeeNames}", font, XBrushes.Black, new XPoint(40, yPoint));
                        yPoint += 20;
                    }
                    yPoint += 20;
                }

                // Save the PDF to the Downloads folder
                string downloadsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                string filePath = System.IO.Path.Combine(downloadsFolderPath, $"WeeklySchedule-{monday:yyyy-MM-dd}-{sunday:yyyy-MM-dd}.pdf");
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
