use subsoniccms
go

declare @root int, @lvl1 int, @lvl2 int, @lvl3 int

insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('Welcome To The CMS!',null,'en-US',null,Newid(),'Home','*','Get started with Great Cross-Sell today!','default.aspx',null,GetDate(),'admin',getdate(),'admin',0,'1',1,1,'-1')
select @root = scope_identity()
insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('Admin',null,'en-US',null,Newid(),'Admin','Administrator, Content Editor, Content Creator','Administrative functions','Admin/default.aspx',null,GetDate(),'admin',getdate(),'admin',0,'-1',1,0,'99')
select @lvl1 = scope_identity()

insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('Membership',null,'en-US',@root,Newid(),'Membership','Administrator','Administrative functions','Admin/membership.aspx',null,GetDate(),'admin',getdate(),'admin',0,'-1',1,0,'1')
select @lvl2 = scope_identity()
insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('Site Users',null,'en-US',@lvl2,Newid(),'Users','Administrator','Administer site users from this page.','Admin/Users.aspx',null,GetDate(),'admin',getdate(),'admin',0,'1',1,1,'99')
select @lvl3 = scope_identity()
insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('Add / Edit User',null,'en-US',@lvl3,Newid(),'Users','Administrator','Administer site users from this page.','Admin/Users.aspx',null,GetDate(),'admin',getdate(),'admin',0,'1',1,1,'99')
insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('Site Roles',null,'en-US',@lvl2,Newid(),'Roles','Administrator','Administer site roles from this page.','Admin/Roles.aspx',null,GetDate(),'admin',getdate(),'admin',0,'1',1,1,'99')


insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('CMS',null,'en-US',@lvl1,Newid(),'CMS','Administrator, Content Creator','Content Management System (CMS)','CMS/cms.aspx',null,GetDate(),'admin',getdate(),'admin',0,'-1',1,0,'2')
select @lvl2 = scope_identity()
insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('CMS Page List',null,'en-US',@lvl2,Newid(),'Page List','Administrator','List of pages in the Content Management System (CMS)','CMS/CMSPageList.aspx',null,GetDate(),'admin',getdate(),'admin',0,'1',1,1,'99')

insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('Login to SubSonicCMS',null,'en-US',@root,Newid(),'Login','Administrator','Use the user ID and password that you used when you registered with the site to log in.','login.aspx',null,GetDate(),'admin',getdate(),'admin',0,'1',0,1,'99')
select @lvl1 = scope_identity()

insert into cms_page (title, body, locale, parentID, pageGuid, MenuTitle, Roles, Summary, pageurl, keywords, createdon, createdby, modifiedon, modifiedby, deleted, pageTypeID, showInMenu, showEditLinks, ordinal) values ('Password Recovery',null,'en-US',@lvl1,Newid(),'Password Recovery','Administrator','Forget your password? Remember it from here.','PasswordRecover.aspx',null,GetDate(),'admin',getdate(),'admin',0,'1',0,1,'99')
