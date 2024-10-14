CREATE TABLE [dbo].[RailScheduleModels] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [station]  NVARCHAR (MAX) NULL,
    [sched_tm] NVARCHAR (MAX) NULL,
    [est_tm]   NVARCHAR (MAX) NULL,
    [act_tm]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_RailScheduleModels] PRIMARY KEY CLUSTERED ([ID] ASC)
);

