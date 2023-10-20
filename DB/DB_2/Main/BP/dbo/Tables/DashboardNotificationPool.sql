create table [dbo].[DashboardNotificationPool] (
    [NotificationID]     INT identity(1,1)   not null,
    [NotificationTypeID] INT                 not null,
    [GenerationDate]     DATETIME            not null,
    [Message]            NVARCHAR (MAX)      not null,
    [OverrideColor]      NVARCHAR (20)       null,
    [ClickUrl]           NVARCHAR (200)      null,
    [Feedback]           BIT                 null,
    [ExpiryDate]         DATETIME            null constraint [DF_DashboardNotificationPool_ExpiryDate] default '12/31/9999 23:59:59.997',
    [Active]             BIT                 constraint [DF_DashboardNotificationPool_Active] default ((1)) not null,
    [ModifiedDate]       DATETIME            constraint [DF_DashboardNotificationPool_ModifiedDate] default (getdate()) not null,
    [ModifiedBy]         NVARCHAR (200)      null,
    constraint [PK_DashboardNotificationPool] primary key clustered ([NotificationID] asc),
    constraint [FK_DashboardNotificationPool_DashboardNotificationType] foreign key ([NotificationTypeID]) references [dbo].[DashboardNotificationType] ([NotificationTypeID])
);
GO

create nonclustered index [IX_DashboardNotificationPool_Composite_Includes]
  on [dbo].[DashboardNotificationPool] ([GenerationDate] asc,[ExpiryDate] asc,[Active] asc)
  INCLUDE ([NotificationID]);
GO