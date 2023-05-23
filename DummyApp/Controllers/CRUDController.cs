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

        //public void GetEmployeeRecord()
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand("select * from Employee", con);
        //            DataTable dt = new DataTable();
        //            con.Open();
        //            SqlDataReader sdr = cmd.ExecuteReader();
        //            dt.Load(sdr);
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex);
        //        Console.Read();

        //    }
        //}

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
    }
}
