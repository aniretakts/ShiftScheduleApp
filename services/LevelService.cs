using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class LevelService
    {
        private readonly string _connectionString = @"Data Source=DESKTOP-6542;Initial Catalog=StathmosDb;Integrated Security=True";

        public List<Level> GetLevels()
        {
            List<Level> levels = new List<Level>();
            string query = "SELECT LEVEL_ID, LEVEL_NAME FROM T_LEVEL";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        levels.Add(new Level
                        {
                            LevelId = reader.GetInt32(0),
                            LevelName = reader.GetString(1)
                        });
                    }
                }
            }

            return levels;
        }
    }
}
