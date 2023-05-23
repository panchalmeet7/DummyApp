using DummyApp.Entities.Data;
using DummyApp.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        public JsonResult GetSingleEmployeeRecord(int EmpID)
        {
            var Employee = _dummyAppContext.Employees.Where(emp => emp.EmployeeId == EmpID).FirstOrDefault();
            return Json(Employee);
        }
    }
}
