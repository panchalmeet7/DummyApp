using Dapper;
using DummyApp.Entities.Data;
using DummyApp.Entities.Models;
using DummyApp.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Text;



namespace DummyApp.Controllers
{
    [Authorize]
    public class CRUDController : Controller
    {
        private readonly DummyAppContext _dummyAppContext;
        private string connectionString = "Data Source=PCT38\\SQL2019;Initial Catalog=DummyApp;User Id=sa;Password=Tatva@123;TrustServerCertificate=True;Integrated Security=True";
        public CRUDController(DummyAppContext dummyAppContext)
        {
            _dummyAppContext = dummyAppContext;
        }


        #region Getting The Data 
        public IActionResult Index()
        {
            var GetAllEmployeeData = _dummyAppContext.Employees.FromSqlRaw("sp_GetAllEmolyeeData").ToList();

            return View(GetAllEmployeeData);
        }
        #endregion

        #region Add Employee Data
        public IActionResult AddEmployeeData(string firstname, string lastname, string email, string role, string position, string department, string status, Employee employee)
        {
            //Mapping Stored Procedure Parameter with Dapper

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Employee_FirstName", firstname);
                parameters.Add("@Employee_LastName", lastname);
                parameters.Add("@Employee_Email", email);
                parameters.Add("@Employee_Role", role);
                parameters.Add("@Position", position);
                parameters.Add("@Employee_Department", department);
                parameters.Add("@Status", status);
                parameters.Add("@created_at", DateTime.Now);
                var storedprocedure = "sp_AddEmpoyeeData";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var exexute = connection.Execute(storedprocedure, parameters, commandType: CommandType.StoredProcedure);
                    connection.Close();


                    //using (SqlConnection con = new SqlConnection(connectionString))
                    //{
                    //    using (SqlCommand cmd = new SqlCommand("sp_AddEmpoyeeData ", con))  // passing required params
                    //    {
                    //        cmd.CommandType = CommandType.StoredProcedure;
                    //        cmd.Parameters.Add("@Employee_FirstName", SqlDbType.VarChar).Value = firstname; // Adding Values into params
                    //        cmd.Parameters.Add("@Employee_LastName", SqlDbType.VarChar).Value = lastname;
                    //        cmd.Parameters.Add("@Employee_Email", SqlDbType.VarChar).Value = email;
                    //        cmd.Parameters.Add("@Employee_Role", SqlDbType.VarChar).Value = role;
                    //        cmd.Parameters.Add("@Employee_Department", SqlDbType.VarChar).Value = department;
                    //        cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
                    //        cmd.Parameters.Add("@Position", SqlDbType.VarChar).Value = position;
                    //        cmd.Parameters.Add("@created_at", SqlDbType.DateTime).Value = DateTime.Now;

                    //        con.Open();  // opening a connection
                    //        cmd.ExecuteNonQuery(); // executing the query with given params
                    //        con.Close();
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.Read();
            }
            //return PartialView("_EmployeeTable", employee);
            return RedirectToAction("Index", "Account");
        }
        #endregion

        #region Get Single Employee Data
        [HttpGet]
        public JsonResult GetSingleEmployeeRecord(int EmployeeId)
        {
            var jsonResult = new StringBuilder(); // StringBuilder class can be used when you want to modify a string without creating a new object.
            List<Employee> employees = new List<Employee>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))  // Establishing connection with database 
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetSingleEmolyee", con))
                    {
                        con.Open();  // Opening a connection
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Employee_id", SqlDbType.Int).Value = EmployeeId; // Adding Values into params

                        var reader = cmd.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            jsonResult.Append("[]");
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee()
                                {
                                    EmployeeId = (int)reader["Employee_id"],
                                    EmployeeFirstName = (string)reader["Employee_FirstName"],
                                    EmployeeLastName = (string)reader["Employee_LastName"],
                                    EmployeeEmail = (string)reader["Employee_Email"],
                                    EmployeeRole = (string)reader["Employee_Role"],
                                    Position = (string)reader["Position"],
                                    EmployeeDepartment = (string)reader["Employee_Department"],
                                    Status = (string)reader["Status"],
                                };
                                employees.Append(employee); //Appends information to the end of the current StringBuilder
                                employees.Add(employee);
                            }
                        }
                        con.Close();
                    }
                }

                return Json(new { data = employees });
            }
            catch (Exception)
            {
                Console.WriteLine("na thyu");
                Console.ReadLine();
            }

            return Json(false);

            //return Json(jsonResult, JsonRequestBehavior.AllowGet);
            //return Json(new { data = jsonResult });


            //<--------------- Previous Approch --------------------->

            //var Employee = _dummyAppContext.Employees.Where(emp => emp.EmployeeId == EmployeeId).FirstOrDefault();
        }
        #endregion

        #region Update Employee Data
        [HttpPost]
        public IActionResult UpdateEmployeeData(int empid, string firstname, string lastname, string email, string role, string position, string department, string status)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Employee_id", empid);
                parameters.Add("@Employee_FirstName", firstname);
                parameters.Add("@Employee_LastName", lastname);
                parameters.Add("@Employee_Email", email);
                parameters.Add("@Employee_Role", role);
                parameters.Add("@Position", position);
                parameters.Add("@Employee_Department", department);
                parameters.Add("@Status", status);
                parameters.Add("@updated_at", DateTime.Now);
                var storedprocedure = "sp_UpdateEmpoyeeData";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var exexute = connection.Execute(storedprocedure, parameters, commandType: CommandType.StoredProcedure);
                    connection.Close();
                }

                //using (SqlConnection con = new SqlConnection(connectionString))
                //{
                //    using (SqlCommand cmd = new SqlCommand("sp_UpdateEmpoyeeData", con))  // passing required params
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add("@Employee_id", SqlDbType.Int).Value = empid; // Adding Values into params
                //        cmd.Parameters.Add("@Employee_FirstName", SqlDbType.VarChar).Value = firstname;
                //        cmd.Parameters.Add("@Employee_LastName", SqlDbType.VarChar).Value = lastname;
                //        cmd.Parameters.Add("@Employee_Email", SqlDbType.VarChar).Value = email;
                //        cmd.Parameters.Add("@Employee_Role", SqlDbType.VarChar).Value = role;
                //        cmd.Parameters.Add("@Employee_Department", SqlDbType.VarChar).Value = department;
                //        cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
                //        cmd.Parameters.Add("@Position", SqlDbType.VarChar).Value = position;
                //        cmd.Parameters.Add("@updated_at", SqlDbType.DateTime).Value = DateTime.Now;

                //        con.Open();  // opening a connection
                //        cmd.ExecuteNonQuery(); // executing the query with given params
                //        con.Close();
                //    }
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.Read();
            }
            return RedirectToAction("Index", "CRUD");
        }
        #endregion

        #region Delete Employee Data
        public IActionResult DeleteEmployeeData(int EmployeeId)
        {
            if (EmployeeId != 0)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_deleteEmployee", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@deleted_at", SqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("@Employee_id", SqlDbType.Int).Value = EmployeeId;
                            con.Open();  // opening a connection
                            cmd.ExecuteNonQuery(); // executing the query with given params
                            con.Close();
                        };
                    };
                }
                catch (Exception)
                {
                    Console.WriteLine("na thyu");
                    Console.Read();
                }
            }
            else
            {
                return View("Index", new { message = "toaster msg" });
            }
            return RedirectToAction("Index", "CRUD");
            //return PartialView("_EmployeeTable");
        }
        #endregion
    }
}
