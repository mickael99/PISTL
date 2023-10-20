set identity_insert [dbo].[DomainEnvironment] on

insert [dbo].[DomainEnvironment] ([DomainEnvironmentID], [DomainID], [Environment], [BPWebServerID], [BPDatabaseID], [EAIDatabaseID], [SSRSServerID], [TableauServerID], [EAIFTPServerID], [IsBp5Enabled], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) values (2205, 351, 1, 235, 236, 236, 236, 13, 227, 1, CAST(N'2020-02-25T17:14:14.807' as DateTime), N'tech@upclear.com', CAST(N'2022-02-10T16:16:29.480' as DateTime), N'tech@upclear.com')

set identity_insert [dbo].[DomainEnvironment] off
