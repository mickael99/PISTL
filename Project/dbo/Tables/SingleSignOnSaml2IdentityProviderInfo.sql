create table [dbo].[SingleSignOnSaml2IdentityProviderInfo] (
    [Saml2IdentityProviderID]       INT            identity (1, 1) not null,
    [DomainID]                      INT            null,
    [IdentityProviderName]          VARCHAR (50)   not null,
    [RequestEntityID]               VARCHAR (500)  not null,
    [ResponseEntityID]              VARCHAR (500)  not null,
    [Subject]                       VARCHAR (500)  not null,
    [CertificateFileName]           VARCHAR (50)   not null,
    [AudienceID]                    VARCHAR (500)  not null,
	[IsCertificateRequired]         BIT			   not null		constraint [DF_SingleSignOnSaml2IdentityProviderInfo_IsCertificateRequired] default (0),
    [ModifiedBy]                    NVARCHAR (200)  not null,
    [ModifiedDate]                  DATETIME       not null,
    [FederationMetaDataDocumentUri] VARCHAR (2000) null,
    constraint [PK_SingleSignOnSaml2IdentityProviderInfo] primary key clustered ([Saml2IdentityProviderID] asc),
    constraint [FK_SingleSignOnSaml2IdentityProviderInfo_Domain] foreign key ([DomainID]) references [dbo].[Domain] ([DomainID])
);



