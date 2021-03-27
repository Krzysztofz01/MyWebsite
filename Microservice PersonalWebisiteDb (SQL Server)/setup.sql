CREATE DATABASE PersonalWebsite;
GO
USE PersonalWebsite;

CREATE TABLE GithubProjects (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Name] VARCHAR(35) NOT NULL,
    [HtmlUrl] VARCHAR(300) NOT NULL,
    [ImageUrl] VARCHAR(300) NOT NULL,
    [Description] VARCHAR(400) NOT NULL,
    [Language] VARCHAR(20) NOT NULL,
    [ProjectCreated] DATETIME2(6) NOT NULL,
    [ProjectUpdated] DATETIME2(6) NOT NULL,
    [Display] BIT DEFAULT 1,
    [CreateDate] DATETIME2(6) DEFAULT GETDATE()
);

GO

CREATE TABLE GalleryImages (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Name] VARCHAR(50),
    [Category] VARCHAR(50),
    [Filename] VARCHAR(300),
    [Display] BIT DEFAULT 1,
    [CreateDate] DATETIME2(6) DEFAULT GETDATE()
);

GO

CREATE TABLE Users (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Password] VARCHAR(128) NOT NULL,
    [Email] VARCHAR(254) NOT NULL,
    [Active] BIT DEFAULT 0,
    [LastLoginDate] DATETIME2(6) DEFAULT GETDATE(),
    [LastLoginIp] VARCHAR(45) DEFAULT '',
);
