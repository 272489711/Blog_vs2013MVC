
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/21/2016 23:42:34
-- Generated from EDMX file: d:\USER\Documents\Visual Studio 2013\Projects\Blog\Blog.Domain\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Blog];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BlogTagsBlogs_BlogTags]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BlogTagsBlogs] DROP CONSTRAINT [FK_BlogTagsBlogs_BlogTags];
GO
IF OBJECT_ID(N'[dbo].[FK_BlogTagsBlogs_Blogs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BlogTagsBlogs] DROP CONSTRAINT [FK_BlogTagsBlogs_Blogs];
GO
IF OBJECT_ID(N'[dbo].[FK_BlogCommentsBlogs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BlogComment] DROP CONSTRAINT [FK_BlogCommentsBlogs];
GO
IF OBJECT_ID(N'[dbo].[FK_BlogTypesBlogs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Blogs] DROP CONSTRAINT [FK_BlogTypesBlogs];
GO
IF OBJECT_ID(N'[dbo].[FK_BlogTagsBlogUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BlogTags] DROP CONSTRAINT [FK_BlogTagsBlogUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_BlogsBlogUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Blogs] DROP CONSTRAINT [FK_BlogsBlogUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_BlogTypesBlogUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BlogTypes] DROP CONSTRAINT [FK_BlogTypesBlogUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_BlogCommentsBlogUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BlogComment] DROP CONSTRAINT [FK_BlogCommentsBlogUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_UserFilesBlogUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFiles] DROP CONSTRAINT [FK_UserFilesBlogUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_ReadInfoBlogUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReadInfo] DROP CONSTRAINT [FK_ReadInfoBlogUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_BlogsReadInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReadInfo] DROP CONSTRAINT [FK_BlogsReadInfo];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Blogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Blogs];
GO
IF OBJECT_ID(N'[dbo].[BlogTags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlogTags];
GO
IF OBJECT_ID(N'[dbo].[BlogTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlogTypes];
GO
IF OBJECT_ID(N'[dbo].[BlogComment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlogComment];
GO
IF OBJECT_ID(N'[dbo].[BlogUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlogUsers];
GO
IF OBJECT_ID(N'[dbo].[UserFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserFiles];
GO
IF OBJECT_ID(N'[dbo].[ReadInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReadInfo];
GO
IF OBJECT_ID(N'[dbo].[BlogTagsBlogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlogTagsBlogs];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Blogs'
CREATE TABLE [dbo].[Blogs] (
    [BlogId] int IDENTITY(1,1) NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpTime] datetime  NULL,
    [Title] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [BlogUrl] nvarchar(max)  NULL,
    [IsForwarding] bit  NULL,
    [BlogReadNum] int  NOT NULL,
    [BlogRemarks] nvarchar(max)  NULL,
    [IsDel] bit  NOT NULL,
    [IsShowHome] bit  NULL,
    [BlogTypeId] int  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'BlogTags'
CREATE TABLE [dbo].[BlogTags] (
    [BlogTagId] int IDENTITY(1,1) NOT NULL,
    [BlogTagName] nvarchar(max)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpTime] datetime  NULL,
    [TagRemarks] nvarchar(max)  NULL,
    [IsDel] bit  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'BlogTypes'
CREATE TABLE [dbo].[BlogTypes] (
    [BlogTypeId] int IDENTITY(1,1) NOT NULL,
    [TypeName] nvarchar(max)  NULL,
    [CreateTime] datetime  NOT NULL,
    [UpTime] datetime  NULL,
    [TypeRemarks] nvarchar(max)  NULL,
    [IsDel] bit  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'BlogComment'
CREATE TABLE [dbo].[BlogComment] (
    [BlogCommentId] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NULL,
    [CreateTime] datetime  NOT NULL,
    [UpTime] datetime  NULL,
    [IsDel] bit  NOT NULL,
    [IsReply] bit  NOT NULL,
    [ToCommentId] int  NULL,
    [ToUserId] int  NULL,
    [BlogId] int  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'BlogUsers'
CREATE TABLE [dbo].[BlogUsers] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpTime] datetime  NULL,
    [IsDel] bit  NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [UserPass] nvarchar(max)  NOT NULL,
    [UserNickname] nvarchar(max)  NULL,
    [Portrait] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [DefaultCSS] bit  NULL,
    [HasCSS] bit  NULL,
    [HasHTML] bit  NULL,
    [HasJS] bit  NULL,
    [Theme] int  NULL,
    [Introduction] nvarchar(max)  NULL,
    [Sex] bit  NULL,
    [Age] int  NULL
);
GO

-- Creating table 'UserFiles'
CREATE TABLE [dbo].[UserFiles] (
    [FileId] int IDENTITY(1,1) NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [IsImage] bit  NOT NULL,
    [FileUrl] nvarchar(max)  NOT NULL,
    [FileSize] float  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'ReadInfo'
CREATE TABLE [dbo].[ReadInfo] (
    [ReadInfoId] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [BlogId] int  NOT NULL,
    [GoodOrBad] nvarchar(max)  NULL,
    [IsDel] bit  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpTime] datetime  NULL
);
GO

-- Creating table 'BlogTagsBlogs'
CREATE TABLE [dbo].[BlogTagsBlogs] (
    [BlogTags_BlogTagId] int  NOT NULL,
    [Blogs_BlogId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [BlogId] in table 'Blogs'
ALTER TABLE [dbo].[Blogs]
ADD CONSTRAINT [PK_Blogs]
    PRIMARY KEY CLUSTERED ([BlogId] ASC);
GO

-- Creating primary key on [BlogTagId] in table 'BlogTags'
ALTER TABLE [dbo].[BlogTags]
ADD CONSTRAINT [PK_BlogTags]
    PRIMARY KEY CLUSTERED ([BlogTagId] ASC);
GO

-- Creating primary key on [BlogTypeId] in table 'BlogTypes'
ALTER TABLE [dbo].[BlogTypes]
ADD CONSTRAINT [PK_BlogTypes]
    PRIMARY KEY CLUSTERED ([BlogTypeId] ASC);
GO

-- Creating primary key on [BlogCommentId] in table 'BlogComment'
ALTER TABLE [dbo].[BlogComment]
ADD CONSTRAINT [PK_BlogComment]
    PRIMARY KEY CLUSTERED ([BlogCommentId] ASC);
GO

-- Creating primary key on [UserId] in table 'BlogUsers'
ALTER TABLE [dbo].[BlogUsers]
ADD CONSTRAINT [PK_BlogUsers]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [FileId] in table 'UserFiles'
ALTER TABLE [dbo].[UserFiles]
ADD CONSTRAINT [PK_UserFiles]
    PRIMARY KEY CLUSTERED ([FileId] ASC);
GO

-- Creating primary key on [ReadInfoId] in table 'ReadInfo'
ALTER TABLE [dbo].[ReadInfo]
ADD CONSTRAINT [PK_ReadInfo]
    PRIMARY KEY CLUSTERED ([ReadInfoId] ASC);
GO

-- Creating primary key on [BlogTags_BlogTagId], [Blogs_BlogId] in table 'BlogTagsBlogs'
ALTER TABLE [dbo].[BlogTagsBlogs]
ADD CONSTRAINT [PK_BlogTagsBlogs]
    PRIMARY KEY CLUSTERED ([BlogTags_BlogTagId], [Blogs_BlogId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BlogTags_BlogTagId] in table 'BlogTagsBlogs'
ALTER TABLE [dbo].[BlogTagsBlogs]
ADD CONSTRAINT [FK_BlogTagsBlogs_BlogTags]
    FOREIGN KEY ([BlogTags_BlogTagId])
    REFERENCES [dbo].[BlogTags]
        ([BlogTagId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Blogs_BlogId] in table 'BlogTagsBlogs'
ALTER TABLE [dbo].[BlogTagsBlogs]
ADD CONSTRAINT [FK_BlogTagsBlogs_Blogs]
    FOREIGN KEY ([Blogs_BlogId])
    REFERENCES [dbo].[Blogs]
        ([BlogId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BlogTagsBlogs_Blogs'
CREATE INDEX [IX_FK_BlogTagsBlogs_Blogs]
ON [dbo].[BlogTagsBlogs]
    ([Blogs_BlogId]);
GO

-- Creating foreign key on [BlogId] in table 'BlogComment'
ALTER TABLE [dbo].[BlogComment]
ADD CONSTRAINT [FK_BlogCommentsBlogs]
    FOREIGN KEY ([BlogId])
    REFERENCES [dbo].[Blogs]
        ([BlogId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BlogCommentsBlogs'
CREATE INDEX [IX_FK_BlogCommentsBlogs]
ON [dbo].[BlogComment]
    ([BlogId]);
GO

-- Creating foreign key on [BlogTypeId] in table 'Blogs'
ALTER TABLE [dbo].[Blogs]
ADD CONSTRAINT [FK_BlogTypesBlogs]
    FOREIGN KEY ([BlogTypeId])
    REFERENCES [dbo].[BlogTypes]
        ([BlogTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BlogTypesBlogs'
CREATE INDEX [IX_FK_BlogTypesBlogs]
ON [dbo].[Blogs]
    ([BlogTypeId]);
GO

-- Creating foreign key on [UserId] in table 'BlogTags'
ALTER TABLE [dbo].[BlogTags]
ADD CONSTRAINT [FK_BlogTagsBlogUsers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[BlogUsers]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BlogTagsBlogUsers'
CREATE INDEX [IX_FK_BlogTagsBlogUsers]
ON [dbo].[BlogTags]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Blogs'
ALTER TABLE [dbo].[Blogs]
ADD CONSTRAINT [FK_BlogsBlogUsers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[BlogUsers]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BlogsBlogUsers'
CREATE INDEX [IX_FK_BlogsBlogUsers]
ON [dbo].[Blogs]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'BlogTypes'
ALTER TABLE [dbo].[BlogTypes]
ADD CONSTRAINT [FK_BlogTypesBlogUsers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[BlogUsers]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BlogTypesBlogUsers'
CREATE INDEX [IX_FK_BlogTypesBlogUsers]
ON [dbo].[BlogTypes]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'BlogComment'
ALTER TABLE [dbo].[BlogComment]
ADD CONSTRAINT [FK_BlogCommentsBlogUsers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[BlogUsers]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BlogCommentsBlogUsers'
CREATE INDEX [IX_FK_BlogCommentsBlogUsers]
ON [dbo].[BlogComment]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'UserFiles'
ALTER TABLE [dbo].[UserFiles]
ADD CONSTRAINT [FK_UserFilesBlogUsers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[BlogUsers]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserFilesBlogUsers'
CREATE INDEX [IX_FK_UserFilesBlogUsers]
ON [dbo].[UserFiles]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'ReadInfo'
ALTER TABLE [dbo].[ReadInfo]
ADD CONSTRAINT [FK_ReadInfoBlogUsers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[BlogUsers]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReadInfoBlogUsers'
CREATE INDEX [IX_FK_ReadInfoBlogUsers]
ON [dbo].[ReadInfo]
    ([UserId]);
GO

-- Creating foreign key on [BlogId] in table 'ReadInfo'
ALTER TABLE [dbo].[ReadInfo]
ADD CONSTRAINT [FK_BlogsReadInfo]
    FOREIGN KEY ([BlogId])
    REFERENCES [dbo].[Blogs]
        ([BlogId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BlogsReadInfo'
CREATE INDEX [IX_FK_BlogsReadInfo]
ON [dbo].[ReadInfo]
    ([BlogId]);
GO


-- Set Default Value
ALTER TABLE [dbo].[Blogs] ADD  CONSTRAINT [DF_Blogs_BlogReadNum]  DEFAULT ((0)) FOR [BlogReadNum]
GO

ALTER TABLE [dbo].[Blogs] ADD  CONSTRAINT [DF_Blogs_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
ALTER TABLE [dbo].[BlogComment] ADD  CONSTRAINT [DF_BlogComment_Isdel]  DEFAULT ((0)) FOR [IsDel]
GO
ALTER TABLE [dbo].[BlogTypes] ADD  CONSTRAINT [DF_BlogTypes_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
ALTER TABLE [dbo].[BlogUsers] ADD  CONSTRAINT [DF_BlogUsers_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
ALTER TABLE [dbo].[BlogTags] ADD  CONSTRAINT [DF_BlogTags_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
ALTER TABLE [dbo].[BlogComment] ADD  CONSTRAINT [DF_BlogComment_IsReply]  DEFAULT ((0)) FOR [IsReply]
GO



-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------