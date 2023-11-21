create table [dbo].[SingleSignOnSaml2IdentityProviderInfoDomain](
	[Saml2IdentityProviderDomainID]		INT				identity(1,1) not null,
	[Saml2IdentityProviderID]			INT				not null,
	[DomainID]							INT				null,
	[ModifiedBy]						NVARCHAR(200)	not null,
	[ModifiedDate]						DATETIME		not null,
	constraint [PK_SingleSignOnSaml2IdentityProviderInfoDomain] primary key clustered (	[Saml2IdentityProviderDomainID] asc),
    constraint [FK_SingleSignOnSaml2IdentityProviderInfoDomain_Domain] foreign key([DomainID]) references [dbo].[Domain] ([DomainID]),
    constraint [FK_SingleSignOnSaml2IdentityProviderInfoDomain_SingleSignOnSaml2IdentityProviderInfo] foreign key([Saml2IdentityProviderID]) references [dbo].[SingleSignOnSaml2IdentityProviderInfo] ([Saml2IdentityProviderID])
	)