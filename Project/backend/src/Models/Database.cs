using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Database
{
    public int DatabaseId { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Context { get; set; }

    public int ServerId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<DomainEnvironment> DomainEnvironmentBpdatabases { get; set; } = new List<DomainEnvironment>();

    public virtual ICollection<DomainEnvironment> DomainEnvironmentEaidatabases { get; set; } = new List<DomainEnvironment>();

    public virtual Server Server { get; set; } = null!;
}
