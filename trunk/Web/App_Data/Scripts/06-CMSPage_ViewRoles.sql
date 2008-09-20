use subsoniccms
go
--be sure to update your DAL after running this!
alter table dbo.cms_page
add EditRoles nvarchar(500) not null
constraint DF_CMS_Page_EditRoles
DEFAULT (N'+')
go

alter table dbo.cms_page
add ViewRoles nvarchar(500) not null
constraint DF_CMS_Page_ViewRoles
DEFAULT (N'*')
go

update dbo.cms_page
set EditRoles = (case when roles = '*' then '+' else roles end),
ViewRoles = roles,
modifiedOn = GetDate(),
modifiedBy = 'admin'
go

ALTER TABLE [dbo].[CMS_Page] DROP CONSTRAINT [DF_CMS_Page_ListOrder]  
go

alter table dbo.cms_page
drop column roles
go
