using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class LoginImage
{
    public int LoginImageId { get; set; }

    public string ExecutionContext { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;
}
