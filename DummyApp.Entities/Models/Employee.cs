using System;
using System.Collections.Generic;

namespace DummyApp.Entities.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? EmployeeFirstName { get; set; }

    public string? EmployeeLastName { get; set; }

    public string? EmployeeEmail { get; set; }

    public string? EmployeeRole { get; set; }

    public string? EmployeeDepartment { get; set; }

    public string? Status { get; set; }

    public string? Position { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
