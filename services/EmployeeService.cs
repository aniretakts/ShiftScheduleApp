﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{

    public class EmployeeService
    {
        private readonly string _connectionString = @"Data Source=DESKTOP-6542;Initial Catalog=StathmosDb;Integrated Security=True";

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string query = "SELECT EmpId, EmpFirstname, EmpLastname, EmpDep, EmpBirthdate, EmpJoinDate, EmpSalary FROM EmployeeTbl";

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
                            EmpDep = reader.GetInt32(3),
                            EmpBirthdate = reader.GetDateTime(4),
                            EmpJoinDate = reader.GetDateTime(5),
                            EmpSalary = reader.GetInt32(6)
                        });
                    }
                }
            }

            return employees;
        }

        public void AddEmployee(Employee employee)
        {
            string query = "INSERT INTO EmployeeTbl (EmpFirstname, EmpLastname, EmpDep, EmpBirthdate, EmpJoinDate, EmpSalary) VALUES (@EmpFirstname, @EmpLastname, @EmpDep, @EmpBirthdate, @EmpJoinDate, @EmpSalary)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmpFirstname", employee.EmpFirstname);
                command.Parameters.AddWithValue("@EmpLastname", employee.EmpLastname);
                command.Parameters.AddWithValue("@EmpDep", employee.EmpDep);
                command.Parameters.AddWithValue("@EmpBirthdate", employee.EmpBirthdate);
                command.Parameters.AddWithValue("@EmpJoinDate", employee.EmpJoinDate);
                command.Parameters.AddWithValue("@EmpSalary", employee.EmpSalary);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

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

        public List<VacationTbl> GetVacationsForWeek(DateTime weekEndDate)
        {
            List<VacationTbl> vacations = new List<VacationTbl>();
            DateTime weekStartDate = weekEndDate.AddDays(-6); // Week starts 6 days before the end date

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
                command.Parameters.AddWithValue("@WeekStartDate", weekStartDate);
                command.Parameters.AddWithValue("@WeekEndDate", weekEndDate);

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
            string query = "SELECT EmpId, EmpFirstname, EmpLastname, EmpDep, EmpBirthdate, EmpJoinDate, EmpSalary, EmplLevel " +
                           "FROM EmployeeTbl " +
                           "WHERE EmpDep = @DepartmentId";

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
                            EmpDep = reader.GetInt32(3),
                            EmpBirthdate = reader.GetDateTime(4),
                            EmpJoinDate = reader.GetDateTime(5),
                            EmpSalary = reader.GetInt32(6),
                            EmplLevel = reader.GetInt32(7)
                        });
                    }
                }
            }

            return employees;
        }

    }
}
