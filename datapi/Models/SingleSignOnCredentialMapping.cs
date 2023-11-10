using System;
using System.Collections.Generic;

namespace DAT_project.API.Models;

public partial class SingleSignOnCredentialMapping
{
    public int CredentialMappingId { get; set; }

    public int? DomainId { get; set; }

    public string SourcePrincipalName { get; set; } = null!;

    public int LoginId { get; set; }

    public int? Saml2IdentityProviderId { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public virtual Domain? Domain { get; set; }

    public virtual Login Login { get; set; } = null!;

    public virtual SingleSignOnSaml2IdentityProviderInfo? Saml2IdentityProvider { get; set; }
}
