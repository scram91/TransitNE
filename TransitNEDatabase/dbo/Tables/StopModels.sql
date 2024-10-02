CREATE TABLE [dbo].[StopModels] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Lng]      NVARCHAR (MAX) NULL,
    [Lat]      NVARCHAR (MAX) NULL,
    [StopId]   NVARCHAR (MAX) NULL,
    [StopName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_StopModels] PRIMARY KEY CLUSTERED ([Id] ASC)
);

