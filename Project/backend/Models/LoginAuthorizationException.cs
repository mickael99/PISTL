using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class LoginAuthorizationException
{
    public int LoginId { get; set; }

    public virtual Login Login { get; set; } = null!;
}
