
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/14/2021 12:14:38
-- Generated from EDMX file: C:\Users\DELL\source\repos\Capstone2021\Capstone2021\DbEntitiesModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [capstone2021];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_company_recruiter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[company] DROP CONSTRAINT [FK_company_recruiter];
GO
IF OBJECT_ID(N'[dbo].[FK_cv_student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[cv] DROP CONSTRAINT [FK_cv_student];
GO
IF OBJECT_ID(N'[dbo].[FK_job_has_category_category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[job_has_category] DROP CONSTRAINT [FK_job_has_category_category];
GO
IF OBJECT_ID(N'[dbo].[FK_job_has_category_job_has_category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[job_has_category] DROP CONSTRAINT [FK_job_has_category_job_has_category];
GO
IF OBJECT_ID(N'[dbo].[FK_job_manager]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[job] DROP CONSTRAINT [FK_job_manager];
GO
IF OBJECT_ID(N'[dbo].[FK_job_recruiter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[job] DROP CONSTRAINT [FK_job_recruiter];
GO
IF OBJECT_ID(N'[dbo].[FK_student_apply_job_cv]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[student_apply_job] DROP CONSTRAINT [FK_student_apply_job_cv];
GO
IF OBJECT_ID(N'[dbo].[FK_student_apply_job_job]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[student_apply_job] DROP CONSTRAINT [FK_student_apply_job_job];
GO
IF OBJECT_ID(N'[dbo].[FK_student_apply_job_student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[student_apply_job] DROP CONSTRAINT [FK_student_apply_job_student];
GO
IF OBJECT_ID(N'[dbo].[FK_student_save_job_job]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[student_save_job] DROP CONSTRAINT [FK_student_save_job_job];
GO
IF OBJECT_ID(N'[dbo].[FK_student_save_job_student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[student_save_job] DROP CONSTRAINT [FK_student_save_job_student];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[banner]', 'U') IS NOT NULL
    DROP TABLE [dbo].[banner];
GO
IF OBJECT_ID(N'[dbo].[category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[category];
GO
IF OBJECT_ID(N'[dbo].[company]', 'U') IS NOT NULL
    DROP TABLE [dbo].[company];
GO
IF OBJECT_ID(N'[dbo].[cv]', 'U') IS NOT NULL
    DROP TABLE [dbo].[cv];
GO
IF OBJECT_ID(N'[dbo].[job]', 'U') IS NOT NULL
    DROP TABLE [dbo].[job];
GO
IF OBJECT_ID(N'[dbo].[job_has_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[job_has_category];
GO
IF OBJECT_ID(N'[dbo].[manager]', 'U') IS NOT NULL
    DROP TABLE [dbo].[manager];
GO
IF OBJECT_ID(N'[dbo].[recruiter]', 'U') IS NOT NULL
    DROP TABLE [dbo].[recruiter];
GO
IF OBJECT_ID(N'[dbo].[student]', 'U') IS NOT NULL
    DROP TABLE [dbo].[student];
GO
IF OBJECT_ID(N'[dbo].[student_apply_job]', 'U') IS NOT NULL
    DROP TABLE [dbo].[student_apply_job];
GO
IF OBJECT_ID(N'[dbo].[student_save_job]', 'U') IS NOT NULL
    DROP TABLE [dbo].[student_save_job];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'banners'
CREATE TABLE [dbo].[banners] (
    [id] int IDENTITY(1,1) NOT NULL,
    [image_url] varchar(max)  NOT NULL,
    [url] varchar(max)  NOT NULL
);
GO

-- Creating table 'categories'
CREATE TABLE [dbo].[categories] (
    [id] int IDENTITY(1,1) NOT NULL,
    [code] int  NOT NULL,
    [value] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'companies'
CREATE TABLE [dbo].[companies] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [headquarters] nvarchar(max)  NOT NULL,
    [website] varchar(500)  NULL,
    [description] nvarchar(max)  NULL,
    [avatar] varchar(max)  NULL,
    [create_date] datetime  NULL,
    [recruiter_id] int  NOT NULL
);
GO

-- Creating table 'cvs'
CREATE TABLE [dbo].[cvs] (
    [id] int IDENTITY(1,1) NOT NULL,
    [student_id] int  NOT NULL,
    [name] nvarchar(50)  NULL,
    [sex] bit  NULL,
    [dob] datetime  NULL,
    [avatar] varchar(max)  NULL,
    [school] nvarchar(max)  NULL,
    [experience] nvarchar(max)  NULL,
    [foreign_language] nvarchar(max)  NULL,
    [desired_salary_minimum] int  NULL,
    [working_form] int  NULL,
    [create_date] datetime  NULL,
    [is_public] bit  NULL,
    [cv_name] nvarchar(max)  NULL,
    [skill] nvarchar(max)  NULL,
    [phone] varchar(15)  NULL
);
GO

-- Creating table 'jobs'
CREATE TABLE [dbo].[jobs] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [working_form] int  NOT NULL,
    [location] int  NOT NULL,
    [working_place] nvarchar(max)  NOT NULL,
    [description] nvarchar(max)  NOT NULL,
    [requirement] nvarchar(max)  NOT NULL,
    [type] bit  NOT NULL,
    [offer] nvarchar(max)  NOT NULL,
    [sex] int  NULL,
    [quantity] int  NOT NULL,
    [salary_min] int  NOT NULL,
    [salary_max] int  NOT NULL,
    [recruiter_id] int  NOT NULL,
    [create_date] datetime  NOT NULL,
    [manager_id] int  NULL,
    [status] int  NOT NULL,
    [string_for_suggestion] varchar(50)  NULL
);
GO

-- Creating table 'job_has_category'
CREATE TABLE [dbo].[job_has_category] (
    [id] int IDENTITY(1,1) NOT NULL,
    [job_id] int  NOT NULL,
    [category_id] int  NOT NULL,
    [create_date] datetime  NOT NULL
);
GO

-- Creating table 'managers'
CREATE TABLE [dbo].[managers] (
    [id] int IDENTITY(1,1) NOT NULL,
    [username] varchar(20)  NOT NULL,
    [password] varchar(150)  NOT NULL,
    [full_name] nvarchar(50)  NULL,
    [create_date] datetime  NULL,
    [role] varchar(20)  NOT NULL,
    [is_banned] bit  NULL
);
GO

-- Creating table 'recruiters'
CREATE TABLE [dbo].[recruiters] (
    [id] int IDENTITY(1,1) NOT NULL,
    [username] varchar(20)  NOT NULL,
    [password] varchar(150)  NOT NULL,
    [gmail] varchar(50)  NOT NULL,
    [phone] varchar(max)  NOT NULL,
    [avatar] varchar(max)  NULL,
    [create_date] datetime  NULL,
    [role] varchar(20)  NOT NULL,
    [is_banned] bit  NULL,
    [sex] bit  NOT NULL,
    [first_name] nvarchar(50)  NOT NULL,
    [last_name] nvarchar(50)  NOT NULL,
    [status] int  NULL,
    [forgot_password_string] varchar(10)  NULL
);
GO

-- Creating table 'students'
CREATE TABLE [dbo].[students] (
    [id] int IDENTITY(1,1) NOT NULL,
    [gmail] varchar(50)  NOT NULL,
    [is_banned] bit  NULL,
    [create_date] datetime  NOT NULL,
    [profile_status] bit  NOT NULL,
    [avatar] varchar(max)  NULL,
    [google_id] varchar(700)  NOT NULL,
    [last_applied_job_string] varchar(50)  NULL
);
GO

-- Creating table 'student_apply_job'
CREATE TABLE [dbo].[student_apply_job] (
    [id] int IDENTITY(1,1) NOT NULL,
    [job_id] int  NOT NULL,
    [student_id] int  NOT NULL,
    [create_date] datetime  NOT NULL,
    [cv_id] int  NOT NULL
);
GO

-- Creating table 'student_save_job'
CREATE TABLE [dbo].[student_save_job] (
    [id] int IDENTITY(1,1) NOT NULL,
    [job_id] int  NOT NULL,
    [student_id] int  NOT NULL,
    [createDate] datetime  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'banners'
ALTER TABLE [dbo].[banners]
ADD CONSTRAINT [PK_banners]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'categories'
ALTER TABLE [dbo].[categories]
ADD CONSTRAINT [PK_categories]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'companies'
ALTER TABLE [dbo].[companies]
ADD CONSTRAINT [PK_companies]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'cvs'
ALTER TABLE [dbo].[cvs]
ADD CONSTRAINT [PK_cvs]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'jobs'
ALTER TABLE [dbo].[jobs]
ADD CONSTRAINT [PK_jobs]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'job_has_category'
ALTER TABLE [dbo].[job_has_category]
ADD CONSTRAINT [PK_job_has_category]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'managers'
ALTER TABLE [dbo].[managers]
ADD CONSTRAINT [PK_managers]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'recruiters'
ALTER TABLE [dbo].[recruiters]
ADD CONSTRAINT [PK_recruiters]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'students'
ALTER TABLE [dbo].[students]
ADD CONSTRAINT [PK_students]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'student_apply_job'
ALTER TABLE [dbo].[student_apply_job]
ADD CONSTRAINT [PK_student_apply_job]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'student_save_job'
ALTER TABLE [dbo].[student_save_job]
ADD CONSTRAINT [PK_student_save_job]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [category_id] in table 'job_has_category'
ALTER TABLE [dbo].[job_has_category]
ADD CONSTRAINT [FK_job_has_category_category]
    FOREIGN KEY ([category_id])
    REFERENCES [dbo].[categories]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_job_has_category_category'
CREATE INDEX [IX_FK_job_has_category_category]
ON [dbo].[job_has_category]
    ([category_id]);
GO

-- Creating foreign key on [recruiter_id] in table 'companies'
ALTER TABLE [dbo].[companies]
ADD CONSTRAINT [FK_company_recruiter]
    FOREIGN KEY ([recruiter_id])
    REFERENCES [dbo].[recruiters]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_company_recruiter'
CREATE INDEX [IX_FK_company_recruiter]
ON [dbo].[companies]
    ([recruiter_id]);
GO

-- Creating foreign key on [student_id] in table 'cvs'
ALTER TABLE [dbo].[cvs]
ADD CONSTRAINT [FK_cv_student]
    FOREIGN KEY ([student_id])
    REFERENCES [dbo].[students]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_cv_student'
CREATE INDEX [IX_FK_cv_student]
ON [dbo].[cvs]
    ([student_id]);
GO

-- Creating foreign key on [cv_id] in table 'student_apply_job'
ALTER TABLE [dbo].[student_apply_job]
ADD CONSTRAINT [FK_student_apply_job_cv]
    FOREIGN KEY ([cv_id])
    REFERENCES [dbo].[cvs]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_student_apply_job_cv'
CREATE INDEX [IX_FK_student_apply_job_cv]
ON [dbo].[student_apply_job]
    ([cv_id]);
GO

-- Creating foreign key on [job_id] in table 'job_has_category'
ALTER TABLE [dbo].[job_has_category]
ADD CONSTRAINT [FK_job_has_category_job_has_category]
    FOREIGN KEY ([job_id])
    REFERENCES [dbo].[jobs]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_job_has_category_job_has_category'
CREATE INDEX [IX_FK_job_has_category_job_has_category]
ON [dbo].[job_has_category]
    ([job_id]);
GO

-- Creating foreign key on [manager_id] in table 'jobs'
ALTER TABLE [dbo].[jobs]
ADD CONSTRAINT [FK_job_manager]
    FOREIGN KEY ([manager_id])
    REFERENCES [dbo].[managers]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_job_manager'
CREATE INDEX [IX_FK_job_manager]
ON [dbo].[jobs]
    ([manager_id]);
GO

-- Creating foreign key on [recruiter_id] in table 'jobs'
ALTER TABLE [dbo].[jobs]
ADD CONSTRAINT [FK_job_recruiter]
    FOREIGN KEY ([recruiter_id])
    REFERENCES [dbo].[recruiters]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_job_recruiter'
CREATE INDEX [IX_FK_job_recruiter]
ON [dbo].[jobs]
    ([recruiter_id]);
GO

-- Creating foreign key on [job_id] in table 'student_apply_job'
ALTER TABLE [dbo].[student_apply_job]
ADD CONSTRAINT [FK_student_apply_job_job]
    FOREIGN KEY ([job_id])
    REFERENCES [dbo].[jobs]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_student_apply_job_job'
CREATE INDEX [IX_FK_student_apply_job_job]
ON [dbo].[student_apply_job]
    ([job_id]);
GO

-- Creating foreign key on [job_id] in table 'student_save_job'
ALTER TABLE [dbo].[student_save_job]
ADD CONSTRAINT [FK_student_save_job_job]
    FOREIGN KEY ([job_id])
    REFERENCES [dbo].[jobs]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_student_save_job_job'
CREATE INDEX [IX_FK_student_save_job_job]
ON [dbo].[student_save_job]
    ([job_id]);
GO

-- Creating foreign key on [student_id] in table 'student_apply_job'
ALTER TABLE [dbo].[student_apply_job]
ADD CONSTRAINT [FK_student_apply_job_student]
    FOREIGN KEY ([student_id])
    REFERENCES [dbo].[students]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_student_apply_job_student'
CREATE INDEX [IX_FK_student_apply_job_student]
ON [dbo].[student_apply_job]
    ([student_id]);
GO

-- Creating foreign key on [student_id] in table 'student_save_job'
ALTER TABLE [dbo].[student_save_job]
ADD CONSTRAINT [FK_student_save_job_student]
    FOREIGN KEY ([student_id])
    REFERENCES [dbo].[students]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_student_save_job_student'
CREATE INDEX [IX_FK_student_save_job_student]
ON [dbo].[student_save_job]
    ([student_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------