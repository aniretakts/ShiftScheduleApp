using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class DepartmentService
    {

        private readonly string _connectionString = @"Data Source=DESKTOP-6542;Initial Catalog=StathmosDb;Integrated Security=True";

        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            string query = "SELECT DEP_ID, DEP_NAME FROM T_DEPARTMENT";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            DepId = reader.GetInt32(0),
                            DepName = reader.GetString(1)
                        });
                    }
                }
            }

            return departments;
        }

        public void AddDepartment(Department department)
        {
            string query = "INSERT INTO T_DEPARTMENT (DEP_NAME) VALUES (@DepName)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepName", department.DepName);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            string query = "DELETE FROM T_DEPARTMENT WHERE DEP_ID = @DepId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepId", departmentId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
