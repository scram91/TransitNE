CREATE TABLE [dbo].[BusTrolleyRouteModels] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [RouteNumber] NVARCHAR (MAX) NULL,
    [RouteName]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_BusTrolleyRouteModels] PRIMARY KEY CLUSTERED ([ID] ASC)
);

