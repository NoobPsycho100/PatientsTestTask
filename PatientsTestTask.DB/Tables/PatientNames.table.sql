CREATE TABLE [dbo].[PatientNames] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Use]           VARCHAR(100) NULL,
    [Family]        VARCHAR(100) NOT NULL,
    CONSTRAINT [PK_PatientNames] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [PK_PatientNames_Patients] 
        FOREIGN KEY ([Id]) REFERENCES [dbo].[Patients]([ID]) ON DELETE CASCADE,
);