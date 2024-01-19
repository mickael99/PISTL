using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Login
{
    public int LoginId { get; set; }

    public string? Email { get; set; }

    public string Name { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime? PasswordModifiedDate { get; set; }

    public DateTime? PasswordExpirationDate { get; set; }

    public int? InvalidAttemptCount { get; set; }

    public DateTime? ResetPasswordEndDate { get; set; }

    public string? ResetPasswordKey { get; set; }

    public short ResetPasswordSentCount { get; set; }

    public short ResetPasswordInvalidAttemptCount { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public bool TermsAccepted { get; set; }

    public bool? Datenabled { get; set; }

    public string? Phone { get; set; }

    public string? BlockedReason { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual LoginAuthorizationException? LoginAuthorizationException { get; set; }

    public virtual ICollection<LoginDomainUser> LoginDomainUsers { get; set; } = new List<LoginDomainUser>();

    public virtual ICollection<PasswordHistory> PasswordHistories { get; set; } = new List<PasswordHistory>();

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<SingleSignOnCredentialMapping> SingleSignOnCredentialMappings { get; set; } = new List<SingleSignOnCredentialMapping>();

    public virtual ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
}
