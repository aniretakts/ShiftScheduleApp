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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for DownloadWeeklyScheduleScreen.xaml
    /// </summary>
    public partial class DownloadWeeklyScheduleScreen : Window
    {
        public DownloadWeeklyScheduleScreen()
        {
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

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Step 1: Create a new PDF document
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Empty PDF";

                // Step 2: Create an A4-sized page
                PdfPage page = new PdfPage();
                page.Width = XUnit.FromPoint(595); // A4 width in points
                page.Height = XUnit.FromPoint(842); // A4 height in points
                document.AddPage(page);

                // Optional: Draw something on the page (e.g., text or shapes)
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Verdana", 20);
                gfx.DrawString("This is an empty A4 PDF", font, XBrushes.Black,
                    new XRect(XUnit.FromPoint(0).Point, XUnit.FromPoint(0).Point, page.Width.Point, page.Height.Point), XStringFormats.Center);

                // Step 3: Save the PDF to a temporary location
                string tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EmptyA4File.pdf");
                document.Save(tempFilePath);

                // Step 4: Open the PDF file
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempFilePath,
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
