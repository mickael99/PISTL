	--v11, HF 2017-02-03, remove auto generate DataTypeID, to be supply by DAT EF.
	--                    Data should come from REF2..DataType

create table [EAI].[DataType] (
    [DomainID]     INT              constraint [DF_DataType_DomainID] default ((0)) not null,
    [DataTypeID]   UNIQUEIDENTIFIER not null,
    [DataTypeDesc] VARCHAR (50)     not null,
    [DataFlow]     VARCHAR (10)     not null,
    [ModifiedBy]   NVARCHAR (200)   null,
    [ModifiedDate] DATETIME         null,
    constraint [PK_DataType] primary key clustered ([DataTypeID] asc)
);

