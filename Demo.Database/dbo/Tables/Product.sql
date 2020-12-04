CREATE TABLE [dbo].[Product] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Code]        VARCHAR (50)   NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Cost]        MONEY          NULL,
    [ListPrice]   MONEY          NULL,
    [CategoryId]  INT            NOT NULL,
    [SupplierId]  INT            NOT NULL,
    [ReleaseDate] DATE           NULL,
    [IsActive]    BIT            NOT NULL,
    [CreatedOn]   DATETIME2 (7)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

