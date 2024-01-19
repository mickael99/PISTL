using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Server
{
    public int ServerId { get; set; }

    public string Address { get; set; } = null!;

    public string? Bp5Address { get; set; }

    public string? Bp5FrontAddress { get; set; }

    public string Name { get; set; } = null!;

    public int? Context { get; set; }

    public string? Type { get; set; }

    public Guid? SubscriptionId { get; set; }

    public string? ResourceGroup { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<Database> Databases { get; set; } = new List<Database>();

    public virtual ICollection<DomainEnvironment> DomainEnvironmentBpwebServers { get; set; } = new List<DomainEnvironment>();

    public virtual ICollection<DomainEnvironment> DomainEnvironmentEaiftpservers { get; set; } = new List<DomainEnvironment>();

    public virtual ICollection<DomainEnvironment> DomainEnvironmentSsrsservers { get; set; } = new List<DomainEnvironment>();

    public virtual ICollection<DomainEnvironment> DomainEnvironmentTableauServers { get; set; } = new List<DomainEnvironment>();
}
