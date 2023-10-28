using System;
using System.Collections.Generic;

namespace DAT_project.API.Models;

public partial class LoginAuthorizationException
{
    public int LoginId { get; set; }

    public virtual Login Login { get; set; } = null!;
}
