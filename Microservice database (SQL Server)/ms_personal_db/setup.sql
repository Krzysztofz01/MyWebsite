CREATE DATABASE PersonalWebsite;
GO
USE PersonalWebsite;

CREATE TABLE Guests (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [IpAddress] VARCHAR(25) NOT NULL,
    [Visits] INT DEFAULT 1,
    [CreateDate] DATETIME2(6) DEFAULT GETDATE(),
    [UpdateDate] DATETIME2(6) DEFAULT GETDATE()
);

GO

CREATE TABLE GithubRepos (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [HtmlUrl] VARCHAR(300) NOT NULL,
    [ImageLocation] VARCHAR(300) DEFAULT 0,
    [Description] VARCHAR(350) NOT NULL,
    [Language] VARCHAR(20),
    [RepoCreated] DATETIME2(6),
    [RepoUpdated] DATETIME2(6)
);

GO

CREATE TABLE GalleryImages (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Name] VARCHAR(70) NOT NULL,
    [FileLocation] VARCHAR(300) DEFAULT 0,
    [Collection] VARCHAR(100) NOT NULL,
    [Display] BIT DEFAULT 0,
    [CreateDate] DATETIME2(6) DEFAULT GETDATE()
);

GO

CREATE TABLE Users (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Password] VARCHAR(128) NOT NULL,
    [Email] VARCHAR(254) NOT NULL UNIQUE,
    [CreateDate] DATETIME2(6) DEFAULT GETDATE(),
    [LastLoginDate] DATETIME2(6) DEFAULT GETDATE(),
    [LastLoginIp] VARCHAR(45) DEFAULT '',
    [Active] BIT DEFAULT 0
);

GO
