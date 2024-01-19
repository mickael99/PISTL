set identity_insert [dbo].[SingleSignOnSaml2IdentityProviderInfo] on

insert [dbo].[SingleSignOnSaml2IdentityProviderInfo] ([Saml2IdentityProviderID],[DomainID],[IdentityProviderName],[RequestEntityID],[ResponseEntityID],[Subject],[CertificateFileName],[AudienceID],[IsCertificateRequired],[ModifiedBy],[ModifiedDate],[FederationMetaDataDocumentUri]) values (1,null,'Upclear AAD','https://upclear.com/c1d39bc0-0016-4867-b5d1-24031dbeac88','https://sts.windows.net/66238243-ed7b-4bad-b41f-adc6f703dc9c/','https://login.windows.net/66238243-ed7b-4bad-b41f-adc6f703dc9c/saml2','UpClear_Azure.cer','https://upclear.com/c1d39bc0-0016-4867-b5d1-24031dbeac88',0,N'tech@upclear.com','2019-12-05T16:54:30.250','https://login.microsoftonline.com/66238243-ed7b-4bad-b41f-adc6f703dc9c/federationmetadata/2007-06/federationmetadata.xml')

set identity_insert [dbo].[SingleSignOnSaml2IdentityProviderInfo] off
