create table [dbo].[UserSettings](
	[UserSettingID] [int] identity(1,1) not null,
	[DomainID] [int] not null,
	[LoginID] [int] not null,
	[Settings] [varchar](max) not null,
	[ModifiedDate] [date] not null,
	[ModifiedBy] [varchar](50) not null,
 constraint [PK_UserSetting] primary key clustered
(
	[UserSettingID] asc
)with (PAD_INDEX = off, STATISTICS_NORECOMPUTE = off, IGNORE_DUP_KEY = off, ALLOW_ROW_LOCKS = on, ALLOW_PAGE_LOCKS = on) on [PRIMARY]
) on [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

alter table [dbo].[UserSettings] add  constraint [DF_UserSetting_ModifiedDate]  default (getdate()) for [ModifiedDate]
GO

alter table [dbo].[UserSettings]  add  constraint [FK_UserSetting_Domain] foreign key([DomainID])
references [dbo].[Domain] ([DomainID])
GO

alter table [dbo].[UserSettings] check constraint [FK_UserSetting_Domain]
GO

alter table [dbo].[UserSettings]  add  constraint [FK_UserSetting_Login] foreign key([LoginID])
references [dbo].[Login] ([LoginID])
GO

alter table [dbo].[UserSettings] check constraint [FK_UserSetting_Login]
GO
