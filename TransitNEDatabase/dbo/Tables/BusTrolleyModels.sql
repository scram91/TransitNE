CREATE TABLE [dbo].[BusTrolleyModels] (
    [ID]                          INT            IDENTITY (1, 1) NOT NULL,
    [Lat]                         NVARCHAR (MAX) NULL,
    [Lon]                         NVARCHAR (MAX) NULL,
    [Label]                       NVARCHAR (MAX) NULL,
    [VehicleID]                   NVARCHAR (MAX) NULL,
    [BlockID]                     NVARCHAR (MAX) NULL,
    [Direction]                   NVARCHAR (MAX) NULL,
    [Destination]                 NVARCHAR (MAX) NULL,
    [Heading]                     INT            NULL,
    [Late]                        BIT            NOT NULL,
    [Original_late]               BIT            NOT NULL,
    [Offeset_sec]                 NVARCHAR (MAX) NULL,
    [Trip]                        NVARCHAR (MAX) NULL,
    [Next_stop_name]              NVARCHAR (MAX) NULL,
    [Next_stop_sequence]          INT            NOT NULL,
    [Estimated_seat_availability] NVARCHAR (MAX) NULL,
    [Timestamp]                   DATETIME2 (7)  NULL,
    CONSTRAINT [PK_BusTrolleyModels] PRIMARY KEY CLUSTERED ([ID] ASC)
);

