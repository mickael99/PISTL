using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class LoginDomainUser
{
    public int LoginId { get; set; }

    public int DomainId { get; set; }

    public string UserId { get; set; } = null!;

    public int Environment { get; set; }

    public string? UserName { get; set; }

    public bool UserActive { get; set; }

    public bool LoginEnabled { get; set; }

    public int? LoginTypeId { get; set; }

    public bool? AnalyticsEnabled { get; set; }

    public bool? IsLight { get; set; }

    public bool SysAdmin { get; set; }

    public DateTime? SysAdminStartDate { get; set; }

    public DateTime? SysAdminEndDate { get; set; }

    public DateTime? DomainLastLoginDate { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Domain Domain { get; set; } = null!;

    public virtual Login Login { get; set; } = null!;
}
