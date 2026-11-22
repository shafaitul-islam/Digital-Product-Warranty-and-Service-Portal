IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [Brands] (
    [BrandId] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Brands] PRIMARY KEY ([BrandId])
);

CREATE TABLE [Categories] (
    [CategoryId] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] int NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Notifications] (
    [NotificationId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Title] nvarchar(150) NOT NULL,
    [Message] nvarchar(2000) NOT NULL,
    [IsRead] bit NOT NULL DEFAULT CAST(0 AS bit),
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([NotificationId]),
    CONSTRAINT [FK_Notifications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);

CREATE TABLE [Products] (
    [ProductId] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [ModelNumber] nvarchar(100) NOT NULL,
    [Description] nvarchar(1000) NULL,
    [BrandId] int NOT NULL,
    [CategoryId] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId]),
    CONSTRAINT [FK_Products_Brands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [Brands] ([BrandId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE NO ACTION
);

CREATE TABLE [ProductRegistrations] (
    [ProductRegistrationId] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [CustomerId] int NOT NULL,
    [SerialNumber] nvarchar(100) NOT NULL,
    [PurchaseDate] date NOT NULL,
    [RegistrationDate] datetime2 NOT NULL,
    [Status] int NOT NULL DEFAULT 1,
    [InvoiceImagePath] nvarchar(500) NULL,
    [ReviewedById] int NULL,
    [ReviewedAt] datetime2 NULL,
    [AdminRemarks] nvarchar(1000) NULL,
    CONSTRAINT [PK_ProductRegistrations] PRIMARY KEY ([ProductRegistrationId]),
    CONSTRAINT [FK_ProductRegistrations_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_ProductRegistrations_AspNetUsers_ReviewedById] FOREIGN KEY ([ReviewedById]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_ProductRegistrations_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId])
);

CREATE TABLE [ServiceRequests] (
    [ServiceRequestId] int NOT NULL IDENTITY,
    [ProductRegistrationId] int NOT NULL,
    [IssueTitle] nvarchar(150) NOT NULL,
    [IssueDescription] nvarchar(2000) NOT NULL,
    [RequestDate] datetime2 NOT NULL,
    [Status] int NOT NULL DEFAULT 1,
    [CustomerNotes] nvarchar(1000) NULL,
    [AssignedStaffId] int NULL,
    [AssignedById] int NULL,
    [AssignedAt] datetime2 NULL,
    [CompletedAt] datetime2 NULL,
    [StaffNotes] nvarchar(2000) NULL,
    [AdminRemarks] nvarchar(1000) NULL,
    CONSTRAINT [PK_ServiceRequests] PRIMARY KEY ([ServiceRequestId]),
    CONSTRAINT [FK_ServiceRequests_AspNetUsers_AssignedById] FOREIGN KEY ([AssignedById]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequests_AspNetUsers_AssignedStaffId] FOREIGN KEY ([AssignedStaffId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceRequests_ProductRegistrations_ProductRegistrationId] FOREIGN KEY ([ProductRegistrationId]) REFERENCES [ProductRegistrations] ([ProductRegistrationId])
);

CREATE TABLE [Warranties] (
    [WarrantyId] int NOT NULL IDENTITY,
    [ProductRegistrationId] int NOT NULL,
    [StartDate] date NOT NULL,
    [EndDate] date NOT NULL,
    [DurationInMonths] int NOT NULL,
    [QrCodeToken] nvarchar(32) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Warranties] PRIMARY KEY ([WarrantyId]),
    CONSTRAINT [FK_Warranties_ProductRegistrations_ProductRegistrationId] FOREIGN KEY ([ProductRegistrationId]) REFERENCES [ProductRegistrations] ([ProductRegistrationId])
);

CREATE TABLE [ServiceHistories] (
    [ServiceHistoryId] int NOT NULL IDENTITY,
    [ServiceRequestId] int NOT NULL,
    [PreviousStatus] int NULL,
    [NewStatus] int NOT NULL,
    [UpdatedById] int NULL,
    [Remarks] nvarchar(1000) NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ServiceHistories] PRIMARY KEY ([ServiceHistoryId]),
    CONSTRAINT [FK_ServiceHistories_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_ServiceHistories_ServiceRequests_ServiceRequestId] FOREIGN KEY ([ServiceRequestId]) REFERENCES [ServiceRequests] ([ServiceRequestId])
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE UNIQUE INDEX [IX_Brands_Name] ON [Brands] ([Name]);

CREATE UNIQUE INDEX [IX_Categories_Name] ON [Categories] ([Name]);

CREATE INDEX [IX_Notifications_UserId_IsRead] ON [Notifications] ([UserId], [IsRead]);

CREATE INDEX [IX_ProductRegistrations_CustomerId] ON [ProductRegistrations] ([CustomerId]);

CREATE UNIQUE INDEX [IX_ProductRegistrations_ProductId_SerialNumber] ON [ProductRegistrations] ([ProductId], [SerialNumber]);

CREATE INDEX [IX_ProductRegistrations_ReviewedById] ON [ProductRegistrations] ([ReviewedById]);

CREATE INDEX [IX_Products_BrandId] ON [Products] ([BrandId]);

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);

CREATE INDEX [IX_ServiceHistories_ServiceRequestId] ON [ServiceHistories] ([ServiceRequestId]);

CREATE INDEX [IX_ServiceHistories_UpdatedById] ON [ServiceHistories] ([UpdatedById]);

CREATE INDEX [IX_ServiceRequests_AssignedById] ON [ServiceRequests] ([AssignedById]);

CREATE INDEX [IX_ServiceRequests_AssignedStaffId] ON [ServiceRequests] ([AssignedStaffId]);

CREATE INDEX [IX_ServiceRequests_ProductRegistrationId] ON [ServiceRequests] ([ProductRegistrationId]);

CREATE UNIQUE INDEX [IX_Warranties_ProductRegistrationId] ON [Warranties] ([ProductRegistrationId]);

CREATE UNIQUE INDEX [IX_Warranties_QrCodeToken] ON [Warranties] ([QrCodeToken]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261111134538_InitialCreate', N'10.0.14');

COMMIT;
GO

