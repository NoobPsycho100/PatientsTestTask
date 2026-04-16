CREATE TABLE [dbo].[Patients] (
    [Id]            UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT [DF_Patients_ID] DEFAULT (NEWSEQUENTIALID()),
    [Gender]        VARCHAR(7) NULL,
    [BirthDate]     DATETIME NOT NULL,
    [IsActive]      BIT    NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CHK_Patients_Gender] CHECK([Gender] IN ('male', 'female', 'other', 'unknown')),
);

GO
CREATE NONCLUSTERED INDEX [Idx_Patients_BirthDate]
    ON [dbo].[Patients] ([BirthDate] ASC);