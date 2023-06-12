using Dapper;
using DummyApp.Entities.Data;
using DummyApp.Entities.Models;
using DummyApp.Entities.ViewModels;
using DummyApp.Repository.Interface;
using DummyApp.Repository.Repository;
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
        #region PROPERTIES
        private readonly DummyAppContext _dummyAppContext;
        private readonly CRUDRepository _crudRepository;
        private readonly string connectionString = "Data Source=PCT38\\SQL2019;Initial Catalog=DummyApp;User Id=sa;Password=Tatva@123;TrustServerCertificate=True;Integrated Security=True";
        #endregion

        #region CONSTRUCTOR
        public CRUDController(DummyAppContext dummyAppContext, ICRUDRepository crudRepository)
        {
            _dummyAppContext = dummyAppContext;
            _crudRepository = (CRUDRepository?)crudRepository;
        }

        #endregion

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
            _crudRepository.AddNewEmployee(firstname, lastname, email, role, position, department, status, employee);

            return RedirectToAction("Index", "CRUD");
        }
        #endregion

        #region Update Employee Data
        /// <summary>
        /// This method updates the existing employee data by id
        /// </summary>
        /// <param name="empid"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <param name="position"></param>
        /// <param name="department"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateEmployeeData(int empid, string firstname, string lastname, string email, string role, string position, string department, string status)
        {
            if (empid != 0)
            {
                _crudRepository.UpdateEmployeeData(empid, firstname, lastname, email, role, position, department, status);
            }

            return RedirectToAction("Index", "CRUD");
        }
        #endregion

        #region Delete Employee Data
        /// <summary>
        /// This method deletes the empolyee data by id
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public IActionResult DeleteEmployeeData(int EmployeeId)
        {
            if (EmployeeId != 0)
            {
                _crudRepository.DeleteEmployeeData(EmployeeId);
            }
            else
            {
                return View("Index", new { message = "toaster msg" });
            }
            return RedirectToAction("Index", "CRUD");
            //return PartialView("_EmployeeTable");
        }
        #endregion

        #region Get Single Employee Data
        /// <summary>
        /// this method returns json data of single employee by id
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSingleEmployeeRecord(int EmployeeId)
        {
            List<Employee> employees = new();

            try
            {
                using (SqlConnection con = new(connectionString))
                using (SqlCommand cmd = new("sp_GetSingleEmolyee", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Employee_id", EmployeeId);
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
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

                            employees.Add(employee);
                        }
                    }
                }

                return Json(new { data = employees });
            }

            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //public JsonResult GetSingleEmployeeRecord(int EmployeeId)
        //{
        //    var jsonResult = new StringBuilder(); // StringBuilder class can be used when you want to modify a string without creating a new object.
        //    List<Employee> employees = new List<Employee>();
        //    try
        //    {
        //        using (SqlConnection con = new(connectionString))  // Establishing connection with database 
        //        {
        //            using (SqlCommand cmd = new SqlCommand("sp_GetSingleEmolyee", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@Employee_id", EmployeeId);
        //                con.Open();  // Opening a connection

        //                var reader = cmd.ExecuteReader();
        //                if (!reader.HasRows)
        //                {
        //                    jsonResult.Append("[]");
        //                }

        //                while (reader.Read())
        //                {
        //                    Employee employee = new Employee()
        //                    {
        //                        EmployeeId = (int)reader["Employee_id"],
        //                        EmployeeFirstName = (string)reader["Employee_FirstName"],
        //                        EmployeeLastName = (string)reader["Employee_LastName"],
        //                        EmployeeEmail = (string)reader["Employee_Email"],
        //                        EmployeeRole = (string)reader["Employee_Role"],
        //                        Position = (string)reader["Position"],
        //                        EmployeeDepartment = (string)reader["Employee_Department"],
        //                        Status = (string)reader["Status"],
        //                    };
        //                    employees.Append(employee); //Appends information to the end of the current StringBuilder
        //                    employees.Add(employee);
        //                }

        //            }
        //        }

        //        return Json(new { data = employees });
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    //return Json(jsonResult, JsonRequestBehavior.AllowGet);
        //    //return Json(new { data = jsonResult });


        //    //<--------------- Previous Approch --------------------->

        //    //var Employee = _dummyAppContext.Employees.Where(emp => emp.EmployeeId == EmployeeId).FirstOrDefault();
        //}

    }
}
