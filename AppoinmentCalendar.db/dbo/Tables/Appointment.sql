CREATE TABLE [dbo].[Appointment] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Summary]   VARCHAR (255) NOT NULL,
    [Location]  VARCHAR (255) NOT NULL,
    [StartDate] DATETIME      NOT NULL,
    [EndDate]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

