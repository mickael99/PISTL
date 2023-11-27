using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Redirect
{
    public Guid RedirectId { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string? Action { get; set; }

    public string? Data { get; set; }

    public string Email { get; set; } = null!;

    public int DomainId { get; set; }

    public int Environment { get; set; }

    public string ProfileId { get; set; } = null!;
}
