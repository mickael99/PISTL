create table [dbo].[DashboardNotificationPoolUser] (
    [DomainID]                        INT            not null,
    [DashboardNotificationPoolUserID] INT            identity (1, 1) not null,
    [NotificationID]                  INT            not null,
    [UserID]                          VARCHAR (50)   null,
    [LoginID]                         int            null,
    [ClientAdminOnly]                 BIT            constraint [DF_DashboardNotificationPoolUser_ClientAdminOnly] default (0) null,
    [ModifiedDate]                    DATETIME       constraint [DF_DashboardNotificationPoolUser_ModifiedDate] default (getdate()) not null,
    [ModifiedBy]                      NVARCHAR (200) null,
    constraint [PK_DashboardNotificationPoolUser] primary key clustered ([DashboardNotificationPoolUserID] asc),
    constraint [FK_DashboardNotificationPoolUser_DashboardNotificationPool] foreign key ([NotificationID]) references [dbo].[DashboardNotificationPool] ([NotificationID]) on delete cascade,
    constraint [FK_DashboardNotificationPoolUser_SY_Domain] foreign key ([DomainID]) references [dbo].[SY_Domain] ([DomainID]),
    constraint [FK_DashboardNotificationPoolUser_Users] foreign key ([UserID]) references [dbo].[Users] ([UserID])
);
GO

create nonclustered index [IX_DashboardNotificationPoolUser_NotificationID]
    on [dbo].[DashboardNotificationPoolUser]([NotificationID] asc)
    INCLUDE ( [ClientAdminOnly],[UserID],[DomainID]);
GO

create nonclustered index [IXN_DashboardNotificationPoolUser_DomainID]
    on [dbo].[DashboardNotificationPoolUser]([DomainID] asc)
GO
