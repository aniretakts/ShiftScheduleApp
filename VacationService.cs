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
            string query = "SELECT Date, MorningShiftEmployeeIds, AfternoonShiftEmployeeIds, EveningShiftEmployeeIds FROM VacationTbl";

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
                            Date = reader.GetDateTime(0),
                            MorningShiftEmployeeId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                            AfternoonShiftEmployeeId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            EveningShiftEmployeeId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                        });
                    }
                }
            }

            return vacations;
        }

        public void SaveVacation(Vacation vacation)
        {
            string query = @"
            IF EXISTS (SELECT 1 FROM VacationTbl WHERE Date = @Date)
            BEGIN
                UPDATE VacationTbl
                SET MorningShiftEmployeeId = @MorningShiftEmployeeId,
                    AfternoonShiftEmployeeId = @AfternoonShiftEmployeeId,
                    EveningShiftEmployeeId = @EveningShiftEmployeeId
                WHERE Date = @Date
            END
            ELSE
            BEGIN
                INSERT INTO VacationTbl (Date, MorningShiftEmployeeId, AfternoonShiftEmployeeId, EveningShiftEmployeeId)
                VALUES (@Date, @MorningShiftEmployeeId, @AfternoonShiftEmployeeId, @EveningShiftEmployeeId)
            END";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", vacation.Date);
                command.Parameters.AddWithValue("@MorningShiftEmployeeId", vacation.MorningShiftEmployeeId == 0 ? (object)DBNull.Value : vacation.MorningShiftEmployeeId);
                command.Parameters.AddWithValue("@AfternoonShiftEmployeeId", vacation.AfternoonShiftEmployeeId == 0 ? (object)DBNull.Value : vacation.AfternoonShiftEmployeeId);
                command.Parameters.AddWithValue("@EveningShiftEmployeeId", vacation.EveningShiftEmployeeId == 0 ? (object)DBNull.Value : vacation.EveningShiftEmployeeId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


}

