using System;
using System.Collections.Generic;

namespace DAT_project.API.Models;

public partial class ServerParameter
{
    public int ServerId { get; set; }

    public string ParameterKey { get; set; } = null!;

    public string ParameterValue { get; set; } = null!;
}
