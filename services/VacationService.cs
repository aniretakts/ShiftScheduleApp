using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class VacationService
    {
        private readonly string _connectionString = @"Data Source=DESKTOP-6542;Initial Catalog=StathmosDb;Integrated Security=True";


        // Helper method to retrieve employee IDs for a given shift
        private List<Employee> GetEmployeesForShift(int vacationId, string shift)
        {
            List<Employee> employees = new List<Employee>();
            string query = $@"SELECT EmployeeId FROM Vacation_{shift} WHERE VacationId = @VacationId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@VacationId", vacationId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int employeeId = reader.GetInt32(0);
                        employees.Add(new Employee { EmpId = employeeId });
                    }
                }
            }

            return employees;
        }

        // Helper method to parse a comma-separated string of IDs into a list of Employees
        private List<Employee> ParseEmployeeIds(string employeeIds)
        {
            var ids = employeeIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return ids.Select(id => new Employee { EmpId = int.Parse(id) }).ToList();
        }

    }


}

