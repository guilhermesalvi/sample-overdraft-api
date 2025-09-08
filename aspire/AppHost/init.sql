-- DB opcional (remova se já existir/for gerenciado fora)
IF DB_ID(N'CustomerEnrollment') IS NULL
BEGIN
  CREATE DATABASE [CustomerEnrollment];
END;
GO
USE [CustomerEnrollment];
GO

/* =========================
   TABELA: dbo.Accounts
   ========================= */
IF OBJECT_ID(N'dbo.Accounts', N'U') IS NULL
BEGIN
CREATE TABLE dbo.Accounts(
                             Id                     UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                             CustomerId               UNIQUEIDENTIFIER NOT NULL,
                             CustomerType             SMALLINT         NOT NULL,
                             IsBankAccountActive    BIT              NOT NULL,
                             CreatedAt              DATETIMEOFFSET(7) NOT NULL 
      CONSTRAINT DF_Accounts_CreatedAt DEFAULT (SYSUTCDATETIME())
);
END;
GO

/* =========================
   TABELA: dbo.Contracts
   ========================= */
IF OBJECT_ID(N'dbo.Contracts', N'U') IS NULL
BEGIN
CREATE TABLE dbo.Contracts(
                              Id                                UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                              AccountId                         UNIQUEIDENTIFIER NOT NULL,
                              SignatureDate                     DATETIMEOFFSET(7) NOT NULL,
                              IsContractActive                  BIT              NOT NULL,
                              GracePeriodDays                   INT              NOT NULL
                                  CONSTRAINT CK_Contracts_GracePeriodDays CHECK (GracePeriodDays >= 0),

    -- Assumindo taxas em FRAÇÃO (0–1). Ajuste a escala conforme necessário.
                              MonthlyInterestRate               DECIMAL(9,6)    NOT NULL
                                  CONSTRAINT CK_Contracts_MonthlyInterestRate CHECK (MonthlyInterestRate BETWEEN 0 AND 1),
                              MonthlyOverLimitInterestRate      DECIMAL(9,6)    NOT NULL
                                  CONSTRAINT CK_Contracts_MonthlyOverLimitInterestRate CHECK (MonthlyOverLimitInterestRate BETWEEN 0 AND 1),
                              OverLimitFixedFee                 DECIMAL(19,4)   NOT NULL
                                  CONSTRAINT CK_Contracts_OverLimitFixedFee CHECK (OverLimitFixedFee >= 0),
                              MonthlyLatePaymentInterestRate    DECIMAL(9,6)    NOT NULL
                                  CONSTRAINT CK_Contracts_MonthlyLatePaymentInterestRate CHECK (MonthlyLatePaymentInterestRate BETWEEN 0 AND 1),
                              LatePaymentPenaltyRate            DECIMAL(9,6)    NOT NULL
                                  CONSTRAINT CK_Contracts_LatePaymentPenaltyRate CHECK (LatePaymentPenaltyRate BETWEEN 0 AND 1),

                              CreatedAt                         DATETIMEOFFSET(7) NOT NULL 
      CONSTRAINT DF_Contracts_CreatedAt DEFAULT (SYSUTCDATETIME())
);

ALTER TABLE dbo.Contracts
    ADD CONSTRAINT FK_Contracts_Accounts
        FOREIGN KEY (AccountId) REFERENCES dbo.Accounts(Id);
END;
GO

/* =========================
   ÍNDICES (idempotentes)
   ========================= */

-- Accounts.CustomerId
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = N'IX_Accounts_CustomerId' AND object_id = OBJECT_ID(N'dbo.Accounts')
)
BEGIN
CREATE INDEX IX_Accounts_CustomerId ON dbo.Accounts (CustomerId);
END;
GO

-- Accounts.IsBankAccountActive (FILTRADO em = 1)
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = N'IX_Accounts_IsBankAccountActive_1' AND object_id = OBJECT_ID(N'dbo.Accounts')
)
BEGIN
CREATE INDEX IX_Accounts_IsBankAccountActive_1
    ON dbo.Accounts (IsBankAccountActive) WHERE IsBankAccountActive = 1;
END;
GO

-- Contracts.AccountId
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = N'IX_Contracts_AccountId' AND object_id = OBJECT_ID(N'dbo.Contracts')
)
BEGIN
CREATE INDEX IX_Contracts_AccountId ON dbo.Contracts(AccountId);
END;
GO

-- Contracts.IsContractActive (FILTRADO em = 1)
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = N'IX_Contracts_IsContractActive_1' AND object_id = OBJECT_ID(N'dbo.Contracts')
)
BEGIN
CREATE INDEX IX_Contracts_IsContractActive_1
    ON dbo.Contracts(IsContractActive)
    WHERE IsContractActive = 1;
END;
GO