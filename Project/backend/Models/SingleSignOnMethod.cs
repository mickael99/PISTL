using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class SingleSignOnMethod
{
    public int SingleSignOnId { get; set; }

    public int? DomainId { get; set; }

    public string UrlHost { get; set; } = null!;

    public string MethodUrl { get; set; } = null!;

    public string? Name { get; set; }

    public int Saml2IdentityProviderId { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public virtual Domain? Domain { get; set; }

    public virtual SingleSignOnSaml2IdentityProviderInfo Saml2IdentityProvider { get; set; } = null!;
}
