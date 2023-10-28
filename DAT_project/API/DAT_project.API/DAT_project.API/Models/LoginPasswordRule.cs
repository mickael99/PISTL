using System;
using System.Collections.Generic;

namespace DAT_project.API.Models;

public partial class LoginPasswordRule
{
    public Guid PasswordRuleId { get; set; }

    public int DomainId { get; set; }

    public string InternalDescription { get; set; } = null!;

    public string DictionaryItemCode { get; set; } = null!;

    public string Pattern { get; set; } = null!;
}
