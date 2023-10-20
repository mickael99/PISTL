if not exists (select top 1 1 from [dbo].[SY_Domain])
begin

insert [dbo].[SY_Domain] ([DomainID], [DomainName], [ParentCompany], [CreatedDate], [ModifiedDate], [ModifiedBy]) values (0, N'Domain 0', null, null, CAST(N'1900-01-01T00:00:00.000' as DateTime), null)

end
