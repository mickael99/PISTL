using System;
using System.Collections.Generic;

namespace Project.Models.DTO;

public partial class ServerParameterDTO
{
    public int ServerId { get; set; }

    public string ParameterKey { get; set; } = null!;

    public string ParameterValue { get; set; } = null!;
}
