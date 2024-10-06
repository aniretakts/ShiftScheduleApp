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
            string query = @"SELECT v.VacationId, v.[Date] FROM Vacation v";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var vacationId = reader.GetInt32(0);
                        var vacationDate = reader.GetDateTime(1);

                        vacations.Add(new Vacation
                        {
                            Date = vacationDate,
                            MorningShiftEmployees = GetEmployeesForShift(vacationId, "MorningShift"),
                            AfternoonShiftEmployees = GetEmployeesForShift(vacationId, "AfternoonShift"),
                            EveningShiftEmployees = GetEmployeesForShift(vacationId, "EveningShift")
                        });
                    }
                }
            }

            return vacations;
        }

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

        public void SaveVacation(Vacation vacation)
        {
            string query = @"
                        IF EXISTS (SELECT 1 FROM VacationTbl WHERE Date = @Date)
                        BEGIN
                            UPDATE VacationTbl
                            SET MorningShiftEmployeeIds = @MorningShiftEmployeeIds,
                                AfternoonShiftEmployeeIds = @AfternoonShiftEmployeeIds,
                                EveningShiftEmployeeIds = @EveningShiftEmployeeIds
                            WHERE Date = @Date
                        END
                        ELSE
                        BEGIN
                            INSERT INTO VacationTbl (Date, MorningShiftEmployeeIds, AfternoonShiftEmployeeIds, EveningShiftEmployeeIds)
                            VALUES (@Date, @MorningShiftEmployeeIds, @AfternoonShiftEmployeeIds, @EveningShiftEmployeeIds)
                        END";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", vacation.Date);
                command.Parameters.AddWithValue("@MorningShiftEmployeeIds", string.Join(",", vacation.MorningShiftEmployees.Select(e => e.EmpId)));
                command.Parameters.AddWithValue("@AfternoonShiftEmployeeIds", string.Join(",", vacation.AfternoonShiftEmployees.Select(e => e.EmpId)));
                command.Parameters.AddWithValue("@EveningShiftEmployeeIds", string.Join(",", vacation.EveningShiftEmployees.Select(e => e.EmpId)));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }


}

