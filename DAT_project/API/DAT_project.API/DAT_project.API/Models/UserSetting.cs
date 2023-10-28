using System;
using System.Collections.Generic;

namespace DAT_project.API.Models;

public partial class UserSetting
{
    public int UserSettingId { get; set; }

    public int DomainId { get; set; }

    public int LoginId { get; set; }

    public string Settings { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public virtual Domain Domain { get; set; } = null!;

    public virtual Login Login { get; set; } = null!;
}
