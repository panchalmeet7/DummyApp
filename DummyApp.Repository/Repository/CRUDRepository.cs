using Dapper;
using DummyApp.Entities.Models;
using DummyApp.Entities.ViewModels;
using DummyApp.Repository.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Repository.Repository
{
    public class CRUDRepository : ICRUDRepository
    {
        private string connectionString = "Data Source=PCT38\\SQL2019;Initial Catalog=DummyApp;User Id=sa;Password=Tatva@123;TrustServerCertificate=True;Integrated Security=True";

        #region Add New Employee
        public void AddNewEmployee(string firstname, string lastname, string email, string role, string position, string department, string status, Employee employee)
        {
            try
            {
                var parameters = new DynamicParameters(); ////Mapping Stored Procedure Parameter with Dapper
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
        }
        #endregion

        #region Update Employee Data
        public void UpdateEmployeeData(int empid, string firstname, string lastname, string email, string role, string position, string department, string status)
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
        }
        #endregion

        #region Delete Employee by ID
        public void DeleteEmployeeData(int EmployeeId)
        {
            try
            {
                using (SqlConnection con = new(connectionString))
                {
                    using (SqlCommand cmd = new("sp_deleteEmployee", con))
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
        #endregion

    }
}
