using DummyApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Repository.Interface
{
    public interface ICRUDRepository
    {
        public void AddNewEmployee(string firstname, string lastname, string email, string role, string position, string department, string status, Employee employee);
        public void UpdateEmployeeData(int empid, string firstname, string lastname, string email, string role, string position, string department, string status);
        public void DeleteEmployeeData(int EmployeeId);
    }
}
