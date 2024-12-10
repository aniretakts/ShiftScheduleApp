using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for LogInScreen.xaml
    /// </summary>
    public partial class LogInScreen : Window
    {

        public static LogInScreen Current { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public LogInScreen()
        {
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

        private void Button_LogIn(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-6542;Initial Catalog=StathmosDb;Integrated Security=True");
                con.Open();
                string add_data = "SELECT * FROM [dbo].[User] WHERE username = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(add_data, con);

                cmd.Parameters.AddWithValue("@username", username.Text);
                cmd.Parameters.AddWithValue("@password", password.Password);
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                con.Close();
                username.Text = "";
                password.Password = "";
                if (count > 0)
                {
                    WelcomeScreen welcomeScreen = new WelcomeScreen();
                    welcomeScreen.Owner = this;
                    this.Hide();
                    welcomeScreen.Show();
                    LogInScreen.Current = this;
                }
                else {
                    MessageBox.Show("Password or Username is not correct!");
                    username.Text = "";
                    password.Password = "";
                    LogInScreen.Current = null;
                }
            }
            catch
            {
                
            }

            
        }
    }
}
