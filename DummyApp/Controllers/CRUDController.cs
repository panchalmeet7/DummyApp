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
        private readonly DummyAppContext _dummyAppContext;
        private readonly CRUDRepository _crudRepository;
        private string connectionString = "Data Source=PCT38\\SQL2019;Initial Catalog=DummyApp;User Id=sa;Password=Tatva@123;TrustServerCertificate=True;Integrated Security=True";
        public CRUDController(DummyAppContext dummyAppContext, ICRUDRepository crudRepository)
        {
            _dummyAppContext = dummyAppContext;
            _crudRepository = (CRUDRepository?)crudRepository;
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
            _crudRepository.AddNewEmployee(firstname, lastname, email, role, position, department, status, employee);

            return RedirectToAction("Index", "CRUD");
        }
        #endregion

        #region Update Employee Data
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
    }
}
