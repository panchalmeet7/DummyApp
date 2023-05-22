using DummyApp.Entities.Data;
using DummyApp.Entities.Models;
using DummyApp.Entities.ViewModels;
using DummyApp.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Repository.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DummyAppContext _dummyAppContext;

        public AccountRepository(DummyAppContext dummyAppContext)
        {
            _dummyAppContext = dummyAppContext;
        }
        private string connectionString = "Data Source=PCT38\\SQL2019;Initial Catalog=DummyApp;User Id=sa;Password=Tatva@123;TrustServerCertificate=True;Integrated Security=True";

        /// <summary>
        /// Adding Users Data into DB using Stored Procedure
        /// </summary>
        /// <param name="model"></param>

        public void AddUser(RegistrationViewModel model)
        {
            try
            {
                //bool status = Validation_Input_UserEmail_Twice(model);

                using (SqlConnection con = new SqlConnection(connectionString))  // establishing connection with database 
                {
                    //<!-------- Registration Using Stored procedure -------->

                    //CREATE or alter PROCEDURE sp_user_insert
                    //(
                    //    @FirstName VARCHAR(50),
                    //    @LastName VARCHAR(50),
                    //    @Email VARCHAR(150),
                    //    @PhoneNumber varchar(15),
                    //    @Password varchar(250)
                    //)
                    //AS
                    //BEGIN
                    //INSERT INTO dbo.[User]
                    //(FirstName, LastName, Email, PhoneNumber, Password)
                    //VALUES(@FirstName, @LastName, @Email, @PhoneNumber, @Password)
                    //END

                    using (SqlCommand cmd = new SqlCommand("sp_user_insert", con)) // giving sp required params
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = model.FirstName;
                        cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = model.LastName;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = model.Email;
                        cmd.Parameters.Add("@PhoneNumber", SqlDbType.VarChar).Value = model.PhoneNumber;
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = model.Password;


                        con.Open();  // opening a connection
                        cmd.ExecuteNonQuery(); // executing the query with given params
                        con.Close();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        //public bool Validation_Input_UserEmail_Twice(RegistrationViewModel model)
        //{
        //    bool isExists = false;
        //    using (SqlConnection con = new SqlConnection(connectionString))  // establishing connection with database 
        //    {
        //        using (SqlCommand cmd = new SqlCommand()) // giving sp required params
        //        {
        //            con.Open();
        //            cmd.Connection = con;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = "select Email from [dbo].[User] where Email =" + model.Email;
        //            cmd.Parameters.AddWithValue("@Email", model.Email);
        //            string email = Convert.ToString(cmd.ExecuteScalar());
        //            if (!string.IsNullOrEmpty(email))
        //            {
        //                isExists = true;
        //            }
        //            else
        //            {
        //                AddUser(model);
        //                isExists = false;
        //            }
        //            con.Close();
        //        }

        //    }
        //    return isExists;
        //}


    }
}
