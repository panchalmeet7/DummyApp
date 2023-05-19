using System;
using System.Collections.Generic;

namespace DummyApp.Entities.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Gender { get; set; }

    public string Password { get; set; } = null!;
}
