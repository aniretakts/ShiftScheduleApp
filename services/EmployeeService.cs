using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace WpfApp2
{

    public class EmployeeService
    {
        private readonly string _connectionString = @"Data Source=DESKTOP-6542;Initial Catalog=StathmosDb;Integrated Security=True";
        //private readonly List<VacationTbl> _vacationTable;

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string query = @"
                            SELECT 
                                e.Emp_Id, 
                                e.Emp_Firstname, 
                                e.Emp_Lastname, 
                                e.Emp_Birthdate, 
                                e.Emp_JoinDate, 
                                e.Emp_Salary, 
                                e.Emp_Level, 
                                e.Emp_health_cert_expiration, 
                                e.Emp_work_contract_expiration, 
                                e.Emp_working_days_per_week, 
                                d.DEP_ID, 
                                d.DEP_NAME,
                                e.Emp_Active
                            FROM 
                                T_EMPLOYEE e
                            JOIN 
                                T_EMPLOYEE_DEPARTMENT ed ON e.Emp_Id = ed.Emp_Id
                            JOIN 
                                T_DEPARTMENT d ON ed.DEP_ID = d.DEP_ID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            EmpId = reader.GetInt32(0),
                            EmpFirstname = reader.GetString(1),
                            EmpLastname = reader.GetString(2),
                            EmpBirthdate = reader.GetDateTime(3),
                            EmpJoinDate = reader.GetDateTime(4),
                            EmpSalary = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                            EmpLevel = reader.GetInt32(6),
                            EmpHealthCertExpiration = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7),  // nullable
                            EmpWorkContractExpiration = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8), // nullable
                            EmpWorkingDaysPerWeek = reader.GetInt32(9),
                            EmpDep = reader.GetInt32(10),
                            EmpLevelName = reader.GetString(11),
                            EmpActive = reader.IsDBNull(12) ? 0 : reader.GetInt32(12)
                        });
                    }
                }
            }

            return employees;
        }

        public void AddEmployee(Employee employee)
        {
            string insertEmployeeQuery = @"
                                        INSERT INTO T_EMPLOYEE 
                                        (Emp_Firstname, Emp_Lastname, Emp_Birthdate, Emp_JoinDate, Emp_Salary, Emp_Level, 
                                         Emp_health_cert_expiration, Emp_work_contract_expiration, Emp_working_days_per_week, Emp_Active) 
                                        VALUES 
                                        (@EmpFirstname, @EmpLastname, @EmpBirthdate, @EmpJoinDate, @EmpSalary, @EmpLevel, 
                                         @HealthCertExpiration, @WorkContractExpiration, @WorkingDaysPerWeek, @EmpActive );
                                        SELECT SCOPE_IDENTITY();";  // Get the inserted Emp_Id

            string getDepartmentIdQuery = "SELECT DEP_ID FROM T_DEPARTMENT WHERE DEP_NAME = @DepName";

            string insertDepartmentLinkQuery = @"
                                        INSERT INTO T_EMPLOYEE_DEPARTMENT (Emp_Id, DEP_ID) 
                                        VALUES (@EmpId, @DepId);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into T_EMPLOYEE
                        SqlCommand insertEmpCommand = new SqlCommand(insertEmployeeQuery, connection, transaction);
                        insertEmpCommand.Parameters.AddWithValue("@EmpFirstname", employee.EmpFirstname);
                        insertEmpCommand.Parameters.AddWithValue("@EmpLastname", employee.EmpLastname);
                        insertEmpCommand.Parameters.AddWithValue("@EmpBirthdate", employee.EmpBirthdate);
                        insertEmpCommand.Parameters.AddWithValue("@EmpJoinDate", employee.EmpJoinDate);
                        insertEmpCommand.Parameters.AddWithValue("@EmpSalary", employee.EmpSalary);
                        insertEmpCommand.Parameters.AddWithValue("@EmpLevel", employee.EmpLevel);
                        insertEmpCommand.Parameters.AddWithValue("@HealthCertExpiration", employee.EmpHealthCertExpiration);
                        insertEmpCommand.Parameters.AddWithValue("@WorkContractExpiration", employee.EmpWorkContractExpiration);
                        insertEmpCommand.Parameters.AddWithValue("@WorkingDaysPerWeek", employee.EmpWorkingDaysPerWeek);
                        insertEmpCommand.Parameters.AddWithValue("@EmpActive", employee.EmpActive);

                        int insertedEmpId = Convert.ToInt32(insertEmpCommand.ExecuteScalar());

                        // Get department ID
                        SqlCommand getDepCommand = new SqlCommand(getDepartmentIdQuery, connection, transaction);
                        getDepCommand.Parameters.AddWithValue("@DepId", employee.EmpDep);
                        object depIdObj = getDepCommand.ExecuteScalar();

                        if (depIdObj == null)
                            throw new Exception("Department not found: " + employee.EmpDepName);

                        int depId = Convert.ToInt32(depIdObj);

                        // Insert into T_EMPLOYEE_DEPARTMENT
                        SqlCommand insertDepCommand = new SqlCommand(insertDepartmentLinkQuery, connection, transaction);
                        insertDepCommand.Parameters.AddWithValue("@EmpId", insertedEmpId);
                        insertDepCommand.Parameters.AddWithValue("@DepId", employee.EmpDep);
                        insertDepCommand.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error while inserting employee and department link: " + ex.Message);
                    }
                }
            }

        }

        public void PauseEmployee(int empId)
        {
            string query = @"
                            UPDATE T_EMPLOYEE
                            SET 
                                Emp_work_contract_expiration = '1900-01-01',
                                Emp_health_cert_expiration = '1900-01-01',
                                Emp_Salary = 0,
                                Emp_Active = 0
                            WHERE Emp_Id = @EmpId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmpId", empId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // not used
        public void DeleteEmployee(int empId)
        {
            string query = "DELETE FROM EmployeeTbl WHERE EmpId = @EmpId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmpId", empId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Get employees by department and shift type (dummy implementation assuming shifts are assigned elsewhere)
        public List<Employee> GetEmployeesByShift(int departmentId, ShiftType shiftType)
        {
            // Ideally, you'd have a way to assign employees to shifts in your system.
            // This is a simple placeholder implementation:
            List<Employee> employees = GetEmployees();

            // Filter employees based on department (and possibly shift, if there's a way to store shift data)
            return employees.Where(emp => emp.EmpDep == departmentId).ToList();
        }


        public void AddVacation(VacationTbl vacation)
        {
            string query = "INSERT INTO VacationTbl (EmployeeId, Date, Shift) VALUES (@EmployeeId, @Date, @Shift)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", vacation.EmployeeId);
                command.Parameters.AddWithValue("@Date", vacation.Date);
                command.Parameters.AddWithValue("@Shift", vacation.Shift.ToString()); // Shift enum as string
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<VacationTbl> GetVacationsForWeek(DateTime monday, DateTime sunday)
        {
            List<VacationTbl> vacations = new List<VacationTbl>();

            string query = @"SELECT v.VacationId, 
                            v.EmployeeId, 
                            v.Date, 
                            v.Shift, 
                            e.EmpId, 
                            e.EmpFirstname, 
                            e.EmpLastname, 
                            d.DepName 
                    FROM VacationTbl v 
                    JOIN EmployeeTbl e ON v.EmployeeId = e.EmpId 
                    JOIN DepartmentTbl d ON e.EmpDep = d.DepId
                    WHERE v.Date BETWEEN @WeekStartDate AND @WeekEndDate";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@WeekStartDate", monday);
                command.Parameters.AddWithValue("@WeekEndDate", sunday);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vacations.Add(new VacationTbl
                        {
                            VacationId = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                            Shift = (ShiftType)Enum.Parse(typeof(ShiftType), reader.GetString(3)),
                            Employee = new Employee
                            {
                                EmpId = reader.GetInt32(4),
                                EmpFirstname = reader.GetString(5),
                                EmpLastname = reader.GetString(6),
                                EmpDepName = reader.GetString(7) // Department Name
                            }
                        });
                    }
                }
            }

            return vacations;
        }


        public List<Employee> GetAllEmployeesPerDepartment(int departmentId)
        {
            List<Employee> employees = new List<Employee>();
            string query = @"
                            SELECT 
                                e.Emp_Id, 
                                e.Emp_Firstname, 
                                e.Emp_Lastname, 
                                e.Emp_Birthdate, 
                                e.Emp_JoinDate, 
                                e.Emp_Salary, 
                                e.Emp_Level,
                                e.Emp_health_cert_expiration,
                                e.Emp_work_contract_expiration,
                                e.Emp_working_days_per_week,
                                e.Emp_Active
                            FROM T_EMPLOYEE e
                            INNER JOIN T_EMPLOYEE_DEPARTMENT ed ON e.Emp_Id = ed.Emp_Id
                            WHERE ed.DEP_ID = @DepartmentId;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentId", departmentId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            EmpId = reader.GetInt32(0),
                            EmpFirstname = reader.GetString(1),
                            EmpLastname = reader.GetString(2),
                            EmpBirthdate = reader.GetDateTime(3),
                            EmpJoinDate = reader.GetDateTime(4),
                            EmpSalary = reader.GetInt32(5),
                            EmpLevel = reader.GetInt32(6),
                            EmpHealthCertExpiration = reader.GetDateTime(7),
                            EmpWorkContractExpiration = reader.GetDateTime(8),
                            EmpWorkingDaysPerWeek = reader.GetInt32(9),
                            EmpActive = reader.GetInt32(10)
                        });
                    }
                }
            }

            return employees;
        }


    }
}
