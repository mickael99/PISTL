create table [dbo].[LoginPasswordRule] (
    [PasswordRuleID]      UNIQUEIDENTIFIER not null,
    [DomainID]            INT              not null,
    [InternalDescription] VARCHAR (510)    not null,
    [DictionaryItemCode]  NVARCHAR (100)   not null,
    [Pattern]             NVARCHAR (MAX)   not null
);

