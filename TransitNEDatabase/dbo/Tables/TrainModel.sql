CREATE TABLE [dbo].[TrainModel] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [lat]          NVARCHAR (MAX) NULL,
    [lon]          NVARCHAR (MAX) NULL,
    [trainno]      NVARCHAR (MAX) NULL,
    [service]      NVARCHAR (MAX) NULL,
    [dest]         NVARCHAR (MAX) NULL,
    [currentstop]  NVARCHAR (MAX) NULL,
    [nextstop]     NVARCHAR (MAX) NULL,
    [line]         NVARCHAR (MAX) NULL,
    [consist]      NVARCHAR (MAX) NULL,
    [heading]      NVARCHAR (MAX) NULL,
    [late]         INT            NULL,
    [SOURCE]       NVARCHAR (MAX) NULL,
    [TRACK]        NVARCHAR (MAX) NULL,
    [TRACK_CHANGE] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_TrainModel] PRIMARY KEY CLUSTERED ([ID] ASC)
);

