using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class DomainEnvironment
{
    public int DomainEnvironmentId { get; set; }

    public int DomainId { get; set; }

    public int Environment { get; set; }

    public int BpwebServerId { get; set; }

    public int? BpDatabaseId { get; set; }

    public int? EaiDatabaseId { get; set; }

    public int? SsrsServerId { get; set; }

    public int? TableauServerId { get; set; }

    public int? EaiftpServerId { get; set; }

    public bool IsBp5Enabled { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Database? Bpdatabase { get; set; }

    public virtual Server BpwebServer { get; set; } = null!;

    public virtual Domain Domain { get; set; } = null!;

    public virtual Database? Eaidatabase { get; set; }

    public virtual Server? Eaiftpserver { get; set; }

    public virtual Server? Ssrsserver { get; set; }

    public virtual Server? TableauServer { get; set; }
}
