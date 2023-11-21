create table [dbo].[ServerParameter] (
    [ServerID]       INT           not null,
    [ParameterKey]   VARCHAR (100) not null,
    [ParameterValue] NVARCHAR (MAX) not null,
    constraint [PK_ServerParameter] primary key clustered ([ServerID] asc, [ParameterKey] asc)
);
