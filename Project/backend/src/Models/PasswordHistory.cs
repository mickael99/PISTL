using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class PasswordHistory
{
    public int PasswordId { get; set; }

    public int LoginId { get; set; }

    public DateTime Date { get; set; }

    public byte[] Password { get; set; } = null!;

    public string? PasswordSalt { get; set; }

    public virtual Login Login { get; set; } = null!;
}
