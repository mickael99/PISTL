create table [dbo].[SingleSignOnMethod] (
    [SingleSignOnID]          INT               identity (1, 1) not null,
    [DomainID]                INT               null,
    [UrlHost]                 VARCHAR (255)     not null,
    [MethodUrl]               VARCHAR (500)     not null,
    [Name]                    NVARCHAR(100)     null,
    [Saml2IdentityProviderID] INT               not null,
    [ModifiedBy]              NVARCHAR (200)    not null,
    [ModifiedDate]            DATETIME          not null,
    primary key clustered ([SingleSignOnID] asc),
    constraint [FK_SingleSignOnMethod_Domain] foreign key ([DomainID]) references [dbo].[Domain] ([DomainID]),
    constraint [Fk_SingleSignOnMethod_SingleSignOnSaml2IdentityProviderInfo] foreign key ([Saml2IdentityProviderID]) references [dbo].[SingleSignOnSaml2IdentityProviderInfo] ([Saml2IdentityProviderID]),
    constraint [UQ_SingleSignOnMethod_UrlHost] unique nonclustered ([UrlHost] asc)
);
GO

