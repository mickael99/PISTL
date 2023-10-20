set identity_insert [dbo].[SingleSignOnMethod] on

insert [dbo].[SingleSignOnMethod] ([SingleSignOnID],[DomainID],[UrlHost],[MethodUrl],[Name],[Saml2IdentityProviderID],[ModifiedBy],[ModifiedDate]) values (1,null,'upclearlocal.blueplanner.com','SingleSignOn/Saml20',N'UpClear',1,N'tech@upclear.com','2017-08-11 23:00:00.000')

set identity_insert [dbo].[SingleSignOnMethod] off
