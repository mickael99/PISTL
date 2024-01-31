using System;
using System.Collections.Generic;

namespace Project.Models.DTO;

public partial class ServerDTO
{
    public int ServerId { get; set; }

    public string Address { get; set; } = null!;

    public string Name { get; set; } = null!;
}
