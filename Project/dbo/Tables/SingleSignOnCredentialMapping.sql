create table [dbo].[SingleSignOnCredentialMapping] (
    [CredentialMappingID]     INT               identity (1, 1) not null,
    [DomainID]                INT               null,
    [SourcePrincipalName]     VARCHAR (100)     not null,
    [LoginID]                 INT               not null,
    [Saml2IdentityProviderID] INT               null,
    [ModifiedBy]              NVARCHAR (200)    not null,
    [ModifiedDate]            DATETIME          not null,
    constraint [PK_SingleSignOnCredentialMapping] primary key clustered ([CredentialMappingID] asc),
    constraint [FK_SingleSignOnCredentialMapping_Domain] foreign key ([DomainID]) references [dbo].[Domain] ([DomainID]),
    constraint [FK_SingleSignOnCredentialMapping_Login] foreign key ([LoginID]) references [dbo].[Login] ([LoginID]) on delete cascade,
    constraint [FK_SingleSignOnCredentialMapping_SingleSignOnSaml2IdentityProviderInfo] foreign key ([Saml2IdentityProviderID]) references [dbo].[SingleSignOnSaml2IdentityProviderInfo] ([Saml2IdentityProviderID])
);
