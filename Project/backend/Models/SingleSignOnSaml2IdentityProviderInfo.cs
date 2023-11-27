using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class SingleSignOnSaml2IdentityProviderInfo
{
    public int Saml2IdentityProviderId { get; set; }

    public int? DomainId { get; set; }

    public string IdentityProviderName { get; set; } = null!;

    public string RequestEntityId { get; set; } = null!;

    public string ResponseEntityId { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string CertificateFileName { get; set; } = null!;

    public string AudienceId { get; set; } = null!;

    public bool IsCertificateRequired { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public string? FederationMetaDataDocumentUri { get; set; }

    public virtual Domain? Domain { get; set; }

    public virtual ICollection<SingleSignOnCredentialMapping> SingleSignOnCredentialMappings { get; set; } = new List<SingleSignOnCredentialMapping>();

    public virtual ICollection<SingleSignOnMethod> SingleSignOnMethods { get; set; } = new List<SingleSignOnMethod>();

    public virtual ICollection<SingleSignOnSaml2IdentityProviderInfoDomain> SingleSignOnSaml2IdentityProviderInfoDomains { get; set; } = new List<SingleSignOnSaml2IdentityProviderInfoDomain>();
}
