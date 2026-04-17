CREATE TABLE [dbo].[PatientGivenNames] (
    [PatientId]     UNIQUEIDENTIFIER NOT NULL,
    [Position]      TINYINT NOT NULL,
    [Name]          VARCHAR(100) NOT NULL,
    CONSTRAINT [PK_PatientGivenNames] PRIMARY KEY CLUSTERED ([PatientId] ASC, [Position] ASC),
    CONSTRAINT [PK_PatientGivenNames_Patients] 
        FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Patients]([ID]) ON DELETE CASCADE,
);