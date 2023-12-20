using System;
using System.Collections.Generic;

namespace EJBMes.Models;

public partial class UserMes
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string Site { get; set; } = null!;
}
