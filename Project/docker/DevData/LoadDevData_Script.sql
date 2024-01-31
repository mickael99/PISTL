set NOCOUNT ON

begin transaction

:r Login.Table.sql
:r Domain.Table.sql
:r LoginDomainUser.Table.sql
:r SingleSignOnSaml2IdentityProviderInfo.Table.sql
:r SingleSignOnSaml2IdentityProviderInfoDomain.Table.sql
:r SingleSignOnMethod.Table.sql
:r SingleSignOnCredentialMapping.Table.sql
:r LoginImage.Table.sql
-- :r LoginNews.Table.sql
:r Server.Table.sql
:r ServerParameter.Table.sql
:r Database.Table.sql
:r DomainEnvironment.Table.sql

commit transaction
