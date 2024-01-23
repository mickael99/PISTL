using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class LoginLog
{
    public int LoginLogId { get; set; }

    public string LogAction { get; set; } = null!;

    public DateTime LogDate { get; set; }

    public int LoginId { get; set; }

    public string? Email { get; set; }

    public string Name { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public string? PasswordSalt { get; set; }

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
}
