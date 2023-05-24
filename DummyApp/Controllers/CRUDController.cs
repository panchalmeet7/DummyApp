﻿using DummyApp.Entities.Data;
using DummyApp.Entities.Models;
using DummyApp.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace DummyApp.Controllers
{
    public class CRUDController : Controller
    {
        private readonly DummyAppContext _dummyAppContext;
        private string connectionString = "Data Source=PCT38\\SQL2019;Initial Catalog=DummyApp;User Id=sa;Password=Tatva@123;TrustServerCertificate=True;Integrated Security=True";
        public CRUDController(DummyAppContext dummyAppContext)
        {
            _dummyAppContext = dummyAppContext;
        }

        /// <returns> All the records from Employee Table </returns>
        public IActionResult Index()
        {
            //create or alter procedure sp_GetAllEmolyeeData
            //as
            //begin
            //select* from Employee where deleted_at is NULL
            //end

            var GetAllEmployeeData = _dummyAppContext.Employees.FromSqlRaw("sp_GetAllEmolyeeData").ToList();

            return View(GetAllEmployeeData);
        }

        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <param name="position"></param>
        /// <param name="department"></param>
        /// <param name="status"></param>
        /// <returns> All the Index View With all the dara that is inserted </returns>
        public IActionResult AddEmployeeData(string firstname, string lastname, string email, string role, string position, string department, string status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //create or alter procedure sp_AddEmpoyeeData
                    //(
                    //    @Employee_FirstName varchar(50),
                    //    @Employee_LastName varchar(50),
                    //    @Employee_Email varchar(50),
                    //    @Employee_Role varchar(50),
                    //    @Employee_Department varchar(50),
                    //    @Status varchar(50),
                    //    @Position varchar(50),
                    //    @created_at datetime
                    //)
                    //as
                    //begin
                    //insert into[dbo].[Employee]
                    //(Employee_FirstName, Employee_LastName, Employee_Email, Employee_Role, Employee_Department, Status, Position, created_at)
                    //values
                    //(@Employee_FirstName, @Employee_LastName, @Employee_Email, @Employee_Role, @Employee_Department, @Status, @Position, @created_at)

                    //end

                    using (SqlCommand cmd = new SqlCommand("sp_AddEmpoyeeData ", con))  // passing required params
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Employee_FirstName", SqlDbType.VarChar).Value = firstname; // Adding Values into params
                        cmd.Parameters.Add("@Employee_LastName", SqlDbType.VarChar).Value = lastname;
                        cmd.Parameters.Add("@Employee_Email", SqlDbType.VarChar).Value = email;
                        cmd.Parameters.Add("@Employee_Role", SqlDbType.VarChar).Value = role;
                        cmd.Parameters.Add("@Employee_Department", SqlDbType.VarChar).Value = department;
                        cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
                        cmd.Parameters.Add("@Position", SqlDbType.VarChar).Value = position;
                        cmd.Parameters.Add("@created_at", SqlDbType.DateTime).Value = DateTime.Now;

                        con.Open();  // opening a connection
                        cmd.ExecuteNonQuery(); // executing the query with given params
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.Read();
            }
            return RedirectToAction("Index", "CRUD");
        }


        /// <param name="EmployeeId"></param>
        /// <returns> Json Result of Employee Data </returns>
        [HttpGet]
        public JsonResult GetSingleEmployeeRecord(int EmployeeId)
        {
            var jsonResult = new StringBuilder();
            List<Employee> employees = new List<Employee>();
            try
            {
                //create or alter procedure sp_GetSingleEmolyee
                //(@Employee_id int)
                //as
                //begin
                // SELECT[Employee_id]
                //      ,[Employee_FirstName]
                //      ,[Employee_LastName]
                //      ,[Employee_Email]
                //      ,[Employee_Role]
                //      ,[Employee_Department]
                //      ,[Status]
                //      ,[Position]
                //      ,[created_at]
                //      ,[updated_at]
                //      ,[deleted_at]
                //                FROM[dbo].[Employee]
                //  WHERE Employee_id = @Employee_id
                //  end
                //GO

                using (SqlConnection con = new SqlConnection(connectionString))  // establishing connection with database 
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetSingleEmolyee", con))
                    {
                        con.Open();  // opening a connection
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
                                employees.Append(employee);
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

        /// <summary>
        /// Soft Delete into Employee Data
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns>View</returns>
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
        }
    }
}
