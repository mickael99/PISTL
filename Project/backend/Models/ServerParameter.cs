using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class ServerParameter
{
    public int ServerId { get; set; }

    public string ParameterKey { get; set; } = null!;

    public string ParameterValue { get; set; } = null!;
}
