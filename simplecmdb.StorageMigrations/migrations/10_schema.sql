IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SimpleCMDBStatus]') AND type in (N'U'))
BEGIN
    CREATE TABLE SimpleCMDBStatus (
        Value int NOT NULL PRIMARY KEY,
        [Name] varchar(50) NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SimpleCMDBEventStatus]') AND type in (N'U'))
BEGIN
    CREATE TABLE SimpleCMDBEventStatus (
        Value int NOT NULL PRIMARY KEY,
        [Name] varchar(50) NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SimpleCMDBEventSubStatus]') AND type in (N'U'))
BEGIN
    CREATE TABLE SimpleCMDBEventSubStatus (
        Value int NOT NULL PRIMARY KEY,
        [Name] varchar(50) NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SimpleCMDBs]') AND type in (N'U'))
BEGIN
    CREATE TABLE SimpleCMDBs (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(MAX) NOT NULL DEFAULT '',
        Slug NVARCHAR(MAX) NOT NULL DEFAULT '',
        Url NVARCHAR(MAX) NOT NULL DEFAULT '',
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CreatedBy NVARCHAR(MAX) NOT NULL DEFAULT '',
        Owner NVARCHAR(MAX) NOT NULL DEFAULT '',
        Project NVARCHAR(MAX) NOT NULL DEFAULT '',
        Status int NOT NULL,
        FOREIGN KEY (Status) REFERENCES SimpleCMDBStatus(Value)
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SimpleCMDBEvents]') AND type in (N'U'))
BEGIN
    CREATE TABLE SimpleCMDBEvents (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        SimpleCMDBId UNIQUEIDENTIFIER NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Payload NVARCHAR(MAX) NULL,
        Status int NOT NULL,
        SubStatus int NOT NULL,
        StatusResultText NVARCHAR(MAX) NULL,
        FOREIGN KEY (SimpleCMDBId) REFERENCES SimpleCMDBs(Id),
        FOREIGN KEY (Status) REFERENCES SimpleCMDBEventStatus(Value),
        FOREIGN KEY (SubStatus) REFERENCES SimpleCMDBEventSubStatus(Value)
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Migrations]') AND type in (N'U'))
BEGIN
    CREATE TABLE Migrations (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        MigrationName NVARCHAR(255) NOT NULL,
        AppliedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Backend NVARCHAR(50) NOT NULL,
        Description NVARCHAR(MAX) NULL
    );
END

IF NOT EXISTS (SELECT * FROM SimpleCMDBStatus)
BEGIN
    INSERT INTO SimpleCMDBStatus (Value, Name) VALUES
    (1,'enabled'),
    (2,'disabled'),
    (3,'suspended')
END

IF NOT EXISTS (SELECT * FROM SimpleCMDBEventStatus)
BEGIN
    INSERT INTO SimpleCMDBEventStatus (Value, Name) VALUES
    (1,'new'),
    (2,'received'),
    (3,'processed')
END

IF NOT EXISTS (SELECT * FROM SimpleCMDBEventSubStatus)
BEGIN
    INSERT INTO SimpleCMDBEventSubStatus (Value, Name) VALUES
    (0,'pending'),
    (1,'success'),
    (2,'failed'),
    (3,'retry'),
    (4, 'skipped')
END
