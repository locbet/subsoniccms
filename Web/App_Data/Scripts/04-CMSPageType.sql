use subsoniccms
go

create table dbo.CMS_Page_Type
(
id int not null primary key,
name varchar(50) not null unique,
description varchar(1000) null,
roles nvarchar(1000) not null,
)
go

insert into CMS_Page_Type(id, name, roles) values (-1,'Menu Item Only','Administrator,Content Creator')
insert into CMS_Page_Type(id, name, roles) values (0,'Dynamic Content','Administrator,Content Creator')
insert into CMS_Page_Type(id, name, roles) values (1,'Static ASPX Page','Administrator')

go

alter table dbo.cms_page
add pageTypeID int not null default 0 references CMS_Page_Type(id)

go

alter table dbo.cms_page
add showInMenu bit not null default 1

alter table dbo.cms_page
add showEditLinks bit not null default 1

alter table dbo.cms_page
add ordinal int not null default 99

go

