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
    /// Interaction logic for EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        public Employee EditableEmployee { get; set; }
        public List<Department> Departments { get; set; }
        public List<Level> Levels { get; set; }

        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            EditableEmployee = employee;
            Departments = new DepartmentService().GetDepartments();
            Levels = new LevelService().GetLevels();

            EditableEmployee.IsActiveBool = employee.EmpActive == 1;
            this.DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EditableEmployee.EmpActive = EditableEmployee.IsActiveBool ? 1 : 0;

            EmployeeService service = new EmployeeService();
            service.UpdateEmployee(EditableEmployee);
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
