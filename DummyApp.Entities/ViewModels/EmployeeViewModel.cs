using DummyApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Entities.ViewModels
{
    public class EmployeeViewModel
    {
        public List<Employee> Employees { get; set;} = new List<Employee>();
    }
}
