using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class SingleSignOnSaml2IdentityProviderInfoDomain
{
    public int Saml2IdentityProviderDomainId { get; set; }

    public int Saml2IdentityProviderId { get; set; }

    public int? DomainId { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public virtual Domain? Domain { get; set; }

    public virtual SingleSignOnSaml2IdentityProviderInfo Saml2IdentityProvider { get; set; } = null!;
}
