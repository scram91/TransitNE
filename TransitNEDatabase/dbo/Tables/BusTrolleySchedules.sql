CREATE TABLE [dbo].[BusTrolleySchedules] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [Stopname]      NVARCHAR (MAX) NULL,
    [Route]         NVARCHAR (MAX) NULL,
    [date]          NVARCHAR (MAX) NULL,
    [day]           NVARCHAR (MAX) NULL,
    [Direction]     NVARCHAR (MAX) NULL,
    [DateCalendar]  NVARCHAR (MAX) NULL,
    [DirectionDesc] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_BusTrolleySchedules] PRIMARY KEY CLUSTERED ([ID] ASC)
);

