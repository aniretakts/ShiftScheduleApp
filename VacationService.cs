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

        public List<Vacation> GetVacations()
        {
            List<Vacation> vacations = new List<Vacation>();
            string query = "SELECT v.Id, v.EmployeeId, v.Date, v.Shift, e.EmpFirstname + ' ' + e.EmpLastname AS EmployeeName FROM VacationTbl v JOIN EmployeeTbl e ON v.EmployeeId = e.EmpId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vacations.Add(new Vacation
                        {
                            Id = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                            Shift = reader.GetInt32(3),
                            EmployeeName = reader.GetString(4)
                        });
                    }
                }
            }

            return vacations;
        }
    }

}

