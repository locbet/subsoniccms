create table dbo.CMS_Task_Applications
(
	TaskApplicationID int not null identity(1,1) primary key,
	Name nvarchar(100) not null unique,
	FullPath nvarchar(255) not null,
	WorkingDirectory nvarchar(150) not null,
	HostName nvarchar(150) not null,
	CreatedOn datetime not null default getDate(),
	CreatedBy nvarchar(50) null,
	ModifiedOn datetime not null default getDate(),
	ModifiedBy nvarchar(50) null,
	IsDeleted bit not null default 0
)
go

