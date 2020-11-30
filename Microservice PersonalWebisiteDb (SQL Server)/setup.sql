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
    [ImageUrl] VARCHAR(300) NOT NULL,
    [Category] VARCHAR(50) NOT NULL,
    [Display] BIT DEFAULT 1,
    [FileExist] BIT DEFAULT 1,
    [CreateDate] DATETIME2(6) DEFAULT GETDATE()
);

GO
