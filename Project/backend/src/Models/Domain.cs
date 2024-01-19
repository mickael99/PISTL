using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Domain
{
    public int DomainId { get; set; }

    public string Name { get; set; } = null!;

    public byte[]? Logo { get; set; }

    public string? Edition { get; set; }

    public bool IsSsoEnabled { get; set; }

    public string? Comment { get; set; }

    public string? ParentCompany { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<DomainEnvironment> DomainEnvironments { get; set; } = new List<DomainEnvironment>();

    public virtual ICollection<LoginDomainUser> LoginDomainUsers { get; set; } = new List<LoginDomainUser>();

    public virtual ICollection<SingleSignOnCredentialMapping> SingleSignOnCredentialMappings { get; set; } = new List<SingleSignOnCredentialMapping>();

    public virtual ICollection<SingleSignOnMethod> SingleSignOnMethods { get; set; } = new List<SingleSignOnMethod>();

    public virtual ICollection<SingleSignOnSaml2IdentityProviderInfoDomain> SingleSignOnSaml2IdentityProviderInfoDomains { get; set; } = new List<SingleSignOnSaml2IdentityProviderInfoDomain>();

    public virtual ICollection<SingleSignOnSaml2IdentityProviderInfo> SingleSignOnSaml2IdentityProviderInfos { get; set; } = new List<SingleSignOnSaml2IdentityProviderInfo>();

    public virtual ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
}
