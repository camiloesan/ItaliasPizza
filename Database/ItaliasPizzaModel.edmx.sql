
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/20/2024 17:01:39
-- Generated from EDMX file: C:\Users\luis_\source\repos\ItaliasPizza\Database\ItaliasPizzaModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ItaliasPizzaDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AccessAccount_IdEmployee_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccessAccount] DROP CONSTRAINT [FK_AccessAccount_IdEmployee_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_Address_IdClient_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Address] DROP CONSTRAINT [FK_Address_IdClient_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_CashReconciliation_RegisteredBy_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CashReconciliation] DROP CONSTRAINT [FK_CashReconciliation_RegisteredBy_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_DeliveryOrder_DeliveryDriver_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DeliveryOrder] DROP CONSTRAINT [FK_DeliveryOrder_DeliveryDriver_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_DeliveryOrder_IdClient_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DeliveryOrder] DROP CONSTRAINT [FK_DeliveryOrder_IdClient_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_DeliveryOrderProduct_IdDeliveryOrder_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DeliveryOrderProduct] DROP CONSTRAINT [FK_DeliveryOrderProduct_IdDeliveryOrder_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_DeliveryOrderProduct_IdProduct_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DeliveryOrderProduct] DROP CONSTRAINT [FK_DeliveryOrderProduct_IdProduct_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_InventoryReport_Reporter_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventoryReport] DROP CONSTRAINT [FK_InventoryReport_Reporter_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_LocalOrder_Waiter_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LocalOrder] DROP CONSTRAINT [FK_LocalOrder_Waiter_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_LocalOrderProduct_IdLocalOrder_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LocalOrderProduct] DROP CONSTRAINT [FK_LocalOrderProduct_IdLocalOrder_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_LocalOrderProduct_IdProduct_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LocalOrderProduct] DROP CONSTRAINT [FK_LocalOrderProduct_IdProduct_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderedSupply_IdSupplierOrder_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderedSupply] DROP CONSTRAINT [FK_OrderedSupply_IdSupplierOrder_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderedSupply_IdSupply_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderedSupply] DROP CONSTRAINT [FK_OrderedSupply_IdSupply_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_Recipe_IdProduct_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Recipe] DROP CONSTRAINT [FK_Recipe_IdProduct_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_RecipeSupply_IdRecipe_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecipeSupply] DROP CONSTRAINT [FK_RecipeSupply_IdRecipe_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_RecipeSupply_IdSupply_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecipeSupply] DROP CONSTRAINT [FK_RecipeSupply_IdSupply_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierOrder_IdSupplier_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierOrder] DROP CONSTRAINT [FK_SupplierOrder_IdSupplier_fk];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AccessAccount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccessAccount];
GO
IF OBJECT_ID(N'[dbo].[Address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Address];
GO
IF OBJECT_ID(N'[dbo].[CashReconciliation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CashReconciliation];
GO
IF OBJECT_ID(N'[dbo].[Client]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Client];
GO
IF OBJECT_ID(N'[dbo].[DeliveryOrder]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeliveryOrder];
GO
IF OBJECT_ID(N'[dbo].[DeliveryOrderProduct]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeliveryOrderProduct];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[InventoryReport]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventoryReport];
GO
IF OBJECT_ID(N'[dbo].[LocalOrder]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LocalOrder];
GO
IF OBJECT_ID(N'[dbo].[LocalOrderProduct]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LocalOrderProduct];
GO
IF OBJECT_ID(N'[dbo].[OrderedSupply]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderedSupply];
GO
IF OBJECT_ID(N'[dbo].[Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product];
GO
IF OBJECT_ID(N'[dbo].[Recipe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Recipe];
GO
IF OBJECT_ID(N'[dbo].[RecipeSupply]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecipeSupply];
GO
IF OBJECT_ID(N'[dbo].[Supplier]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Supplier];
GO
IF OBJECT_ID(N'[dbo].[SupplierOrder]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierOrder];
GO
IF OBJECT_ID(N'[dbo].[Supply]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Supply];
GO
IF OBJECT_ID(N'[dbo].[SupplyInventoryReport]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplyInventoryReport];
GO
IF OBJECT_ID(N'[dbo].[Transaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transaction];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AccessAccount'
CREATE TABLE [dbo].[AccessAccount] (
    [IdAccessAccount] int IDENTITY(1,1) NOT NULL,
    [UserName] varchar(50)  NOT NULL,
    [IdEmployee] uniqueidentifier  NOT NULL,
    [Password] varchar(50)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Status] varchar(20)  NOT NULL
);
GO

-- Creating table 'Address'
CREATE TABLE [dbo].[Address] (
    [IdAddress] uniqueidentifier  NOT NULL,
    [IdClient] uniqueidentifier  NOT NULL,
    [Street] varchar(100)  NOT NULL,
    [Number] tinyint  NOT NULL,
    [PostalCode] smallint  NOT NULL,
    [Colony] varchar(50)  NOT NULL,
    [Status] varchar(20)  NOT NULL,
    [Reference] varchar(100)  NULL
);
GO

-- Creating table 'CashReconciliation'
CREATE TABLE [dbo].[CashReconciliation] (
    [IdCashReconciliation] uniqueidentifier  NOT NULL,
    [OpeningDate] datetime  NOT NULL,
    [ClosingDate] datetime  NOT NULL,
    [StartingAmount] decimal(12,2)  NOT NULL,
    [FinishingAmount] decimal(12,2)  NOT NULL,
    [SalesAmount] decimal(12,2)  NOT NULL,
    [SpendingsAmount] decimal(12,2)  NOT NULL,
    [CashDifference] decimal(12,2)  NOT NULL,
    [Observations] varchar(100)  NOT NULL,
    [RegisteredBy] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Client'
CREATE TABLE [dbo].[Client] (
    [IdClient] uniqueidentifier  NOT NULL,
    [FirstName] varchar(50)  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [Phone] varchar(20)  NOT NULL
);
GO

-- Creating table 'DeliveryOrder'
CREATE TABLE [dbo].[DeliveryOrder] (
    [IdDeliveryOrder] uniqueidentifier  NOT NULL,
    [IdClient] uniqueidentifier  NOT NULL,
    [Status] varchar(20)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Total] decimal(12,2)  NOT NULL,
    [DeliveryDriver] uniqueidentifier  NOT NULL,
    [NotDeliveredReason] varchar(100)  NULL
);
GO

-- Creating table 'DeliveryOrderProduct'
CREATE TABLE [dbo].[DeliveryOrderProduct] (
    [IdDeliveryOrderProduct] uniqueidentifier  NOT NULL,
    [IdDeliveryOrder] uniqueidentifier  NOT NULL,
    [IdProduct] uniqueidentifier  NOT NULL,
    [Quantity] int  NOT NULL
);
GO

-- Creating table 'Employee'
CREATE TABLE [dbo].[Employee] (
    [IdEmployee] uniqueidentifier  NOT NULL,
    [FirstName] varchar(50)  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [Phone] varchar(20)  NOT NULL,
    [Status] varchar(20)  NOT NULL,
    [Charge] varchar(50)  NOT NULL
);
GO

-- Creating table 'InventoryReport'
CREATE TABLE [dbo].[InventoryReport] (
    [IdInventoryReport] uniqueidentifier  NOT NULL,
    [Reporter] uniqueidentifier  NOT NULL,
    [Observations] varchar(100)  NULL
);
GO

-- Creating table 'LocalOrder'
CREATE TABLE [dbo].[LocalOrder] (
    [IdLocalOrder] uniqueidentifier  NOT NULL,
    [Waiter] uniqueidentifier  NOT NULL,
    [Status] varchar(20)  NOT NULL,
    [Table] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Total] decimal(12,2)  NOT NULL
);
GO

-- Creating table 'LocalOrderProduct'
CREATE TABLE [dbo].[LocalOrderProduct] (
    [IdLocalOrderProduct] uniqueidentifier  NOT NULL,
    [IdLocalOrder] uniqueidentifier  NOT NULL,
    [IdProduct] uniqueidentifier  NOT NULL,
    [Quantity] int  NOT NULL
);
GO

-- Creating table 'OrderedSupply'
CREATE TABLE [dbo].[OrderedSupply] (
    [IdOrderedSupply] uniqueidentifier  NOT NULL,
    [IdSupply] uniqueidentifier  NOT NULL,
    [IdSupplierOrder] uniqueidentifier  NOT NULL,
    [Quantity] int  NOT NULL,
    [MeasurementUnit] varchar(20)  NOT NULL
);
GO

-- Creating table 'Product'
CREATE TABLE [dbo].[Product] (
    [IdProduct] uniqueidentifier  NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Type] varchar(50)  NOT NULL,
    [Price] decimal(12,2)  NOT NULL,
    [Status] varchar(20)  NOT NULL
);
GO

-- Creating table 'Recipe'
CREATE TABLE [dbo].[Recipe] (
    [IdRecipe] uniqueidentifier  NOT NULL,
    [IdProduct] uniqueidentifier  NOT NULL,
    [Instructions] varchar(100)  NOT NULL
);
GO

-- Creating table 'RecipeSupply'
CREATE TABLE [dbo].[RecipeSupply] (
    [IdProductSupply] uniqueidentifier  NOT NULL,
    [IdRecipe] uniqueidentifier  NOT NULL,
    [IdSupply] uniqueidentifier  NOT NULL,
    [SupplyAmount] decimal(12,3)  NOT NULL,
    [MeasurementUnit] bigint  NULL
);
GO

-- Creating table 'Supplier'
CREATE TABLE [dbo].[Supplier] (
    [IdSupplier] uniqueidentifier  NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Category] varchar(50)  NOT NULL,
    [Phone] varchar(20)  NOT NULL
);
GO

-- Creating table 'SupplierOrder'
CREATE TABLE [dbo].[SupplierOrder] (
    [IdSupplierOrder] uniqueidentifier  NOT NULL,
    [IdSupplier] uniqueidentifier  NOT NULL,
    [OrderDate] datetime  NOT NULL,
    [ExpectedDate] datetime  NOT NULL,
    [ArrivalDate] datetime  NOT NULL,
    [Status] varchar(20)  NOT NULL
);
GO

-- Creating table 'Supply'
CREATE TABLE [dbo].[Supply] (
    [IdSupply] uniqueidentifier  NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Quantity] decimal(12,3)  NOT NULL,
    [Category] varchar(50)  NOT NULL,
    [MeasurementUnit] varchar(20)  NOT NULL,
    [Status] varchar(20)  NOT NULL
);
GO

-- Creating table 'SupplyInventoryReport'
CREATE TABLE [dbo].[SupplyInventoryReport] (
    [IdSupplyInventoryReport] uniqueidentifier  NOT NULL,
    [IdInvenroryReport] uniqueidentifier  NOT NULL,
    [IdSupply] uniqueidentifier  NOT NULL,
    [MeasurementUnit] varchar(1)  NOT NULL,
    [ExpectedAmount] decimal(12,2)  NOT NULL,
    [ReportedAmount] decimal(12,2)  NOT NULL,
    [DifferingAmountReason] varchar(1)  NULL
);
GO

-- Creating table 'Transaction'
CREATE TABLE [dbo].[Transaction] (
    [IdTransaction] uniqueidentifier  NOT NULL,
    [Type] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Amount] decimal(12,2)  NOT NULL,
    [Description] int  NOT NULL,
    [RegisteredBy] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdAccessAccount] in table 'AccessAccount'
ALTER TABLE [dbo].[AccessAccount]
ADD CONSTRAINT [PK_AccessAccount]
    PRIMARY KEY CLUSTERED ([IdAccessAccount] ASC);
GO

-- Creating primary key on [IdAddress] in table 'Address'
ALTER TABLE [dbo].[Address]
ADD CONSTRAINT [PK_Address]
    PRIMARY KEY CLUSTERED ([IdAddress] ASC);
GO

-- Creating primary key on [IdCashReconciliation] in table 'CashReconciliation'
ALTER TABLE [dbo].[CashReconciliation]
ADD CONSTRAINT [PK_CashReconciliation]
    PRIMARY KEY CLUSTERED ([IdCashReconciliation] ASC);
GO

-- Creating primary key on [IdClient] in table 'Client'
ALTER TABLE [dbo].[Client]
ADD CONSTRAINT [PK_Client]
    PRIMARY KEY CLUSTERED ([IdClient] ASC);
GO

-- Creating primary key on [IdDeliveryOrder] in table 'DeliveryOrder'
ALTER TABLE [dbo].[DeliveryOrder]
ADD CONSTRAINT [PK_DeliveryOrder]
    PRIMARY KEY CLUSTERED ([IdDeliveryOrder] ASC);
GO

-- Creating primary key on [IdDeliveryOrderProduct] in table 'DeliveryOrderProduct'
ALTER TABLE [dbo].[DeliveryOrderProduct]
ADD CONSTRAINT [PK_DeliveryOrderProduct]
    PRIMARY KEY CLUSTERED ([IdDeliveryOrderProduct] ASC);
GO

-- Creating primary key on [IdEmployee] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [PK_Employee]
    PRIMARY KEY CLUSTERED ([IdEmployee] ASC);
GO

-- Creating primary key on [IdInventoryReport] in table 'InventoryReport'
ALTER TABLE [dbo].[InventoryReport]
ADD CONSTRAINT [PK_InventoryReport]
    PRIMARY KEY CLUSTERED ([IdInventoryReport] ASC);
GO

-- Creating primary key on [IdLocalOrder] in table 'LocalOrder'
ALTER TABLE [dbo].[LocalOrder]
ADD CONSTRAINT [PK_LocalOrder]
    PRIMARY KEY CLUSTERED ([IdLocalOrder] ASC);
GO

-- Creating primary key on [IdLocalOrderProduct] in table 'LocalOrderProduct'
ALTER TABLE [dbo].[LocalOrderProduct]
ADD CONSTRAINT [PK_LocalOrderProduct]
    PRIMARY KEY CLUSTERED ([IdLocalOrderProduct] ASC);
GO

-- Creating primary key on [IdOrderedSupply] in table 'OrderedSupply'
ALTER TABLE [dbo].[OrderedSupply]
ADD CONSTRAINT [PK_OrderedSupply]
    PRIMARY KEY CLUSTERED ([IdOrderedSupply] ASC);
GO

-- Creating primary key on [IdProduct] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [PK_Product]
    PRIMARY KEY CLUSTERED ([IdProduct] ASC);
GO

-- Creating primary key on [IdRecipe] in table 'Recipe'
ALTER TABLE [dbo].[Recipe]
ADD CONSTRAINT [PK_Recipe]
    PRIMARY KEY CLUSTERED ([IdRecipe] ASC);
GO

-- Creating primary key on [IdProductSupply] in table 'RecipeSupply'
ALTER TABLE [dbo].[RecipeSupply]
ADD CONSTRAINT [PK_RecipeSupply]
    PRIMARY KEY CLUSTERED ([IdProductSupply] ASC);
GO

-- Creating primary key on [IdSupplier] in table 'Supplier'
ALTER TABLE [dbo].[Supplier]
ADD CONSTRAINT [PK_Supplier]
    PRIMARY KEY CLUSTERED ([IdSupplier] ASC);
GO

-- Creating primary key on [IdSupplierOrder] in table 'SupplierOrder'
ALTER TABLE [dbo].[SupplierOrder]
ADD CONSTRAINT [PK_SupplierOrder]
    PRIMARY KEY CLUSTERED ([IdSupplierOrder] ASC);
GO

-- Creating primary key on [IdSupply] in table 'Supply'
ALTER TABLE [dbo].[Supply]
ADD CONSTRAINT [PK_Supply]
    PRIMARY KEY CLUSTERED ([IdSupply] ASC);
GO

-- Creating primary key on [IdSupplyInventoryReport] in table 'SupplyInventoryReport'
ALTER TABLE [dbo].[SupplyInventoryReport]
ADD CONSTRAINT [PK_SupplyInventoryReport]
    PRIMARY KEY CLUSTERED ([IdSupplyInventoryReport] ASC);
GO

-- Creating primary key on [IdTransaction] in table 'Transaction'
ALTER TABLE [dbo].[Transaction]
ADD CONSTRAINT [PK_Transaction]
    PRIMARY KEY CLUSTERED ([IdTransaction] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdEmployee] in table 'AccessAccount'
ALTER TABLE [dbo].[AccessAccount]
ADD CONSTRAINT [FK_AccessAccount_IdEmployee_fk]
    FOREIGN KEY ([IdEmployee])
    REFERENCES [dbo].[Employee]
        ([IdEmployee])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccessAccount_IdEmployee_fk'
CREATE INDEX [IX_FK_AccessAccount_IdEmployee_fk]
ON [dbo].[AccessAccount]
    ([IdEmployee]);
GO

-- Creating foreign key on [IdClient] in table 'Address'
ALTER TABLE [dbo].[Address]
ADD CONSTRAINT [FK_Address_IdClient_fk]
    FOREIGN KEY ([IdClient])
    REFERENCES [dbo].[Client]
        ([IdClient])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Address_IdClient_fk'
CREATE INDEX [IX_FK_Address_IdClient_fk]
ON [dbo].[Address]
    ([IdClient]);
GO

-- Creating foreign key on [RegisteredBy] in table 'CashReconciliation'
ALTER TABLE [dbo].[CashReconciliation]
ADD CONSTRAINT [FK_CashReconciliation_RegisteredBy_fk]
    FOREIGN KEY ([RegisteredBy])
    REFERENCES [dbo].[Employee]
        ([IdEmployee])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CashReconciliation_RegisteredBy_fk'
CREATE INDEX [IX_FK_CashReconciliation_RegisteredBy_fk]
ON [dbo].[CashReconciliation]
    ([RegisteredBy]);
GO

-- Creating foreign key on [IdClient] in table 'DeliveryOrder'
ALTER TABLE [dbo].[DeliveryOrder]
ADD CONSTRAINT [FK_DeliveryOrder_IdClient_fk]
    FOREIGN KEY ([IdClient])
    REFERENCES [dbo].[Client]
        ([IdClient])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DeliveryOrder_IdClient_fk'
CREATE INDEX [IX_FK_DeliveryOrder_IdClient_fk]
ON [dbo].[DeliveryOrder]
    ([IdClient]);
GO

-- Creating foreign key on [DeliveryDriver] in table 'DeliveryOrder'
ALTER TABLE [dbo].[DeliveryOrder]
ADD CONSTRAINT [FK_DeliveryOrder_DeliveryDriver_fk]
    FOREIGN KEY ([DeliveryDriver])
    REFERENCES [dbo].[Employee]
        ([IdEmployee])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DeliveryOrder_DeliveryDriver_fk'
CREATE INDEX [IX_FK_DeliveryOrder_DeliveryDriver_fk]
ON [dbo].[DeliveryOrder]
    ([DeliveryDriver]);
GO

-- Creating foreign key on [IdDeliveryOrder] in table 'DeliveryOrderProduct'
ALTER TABLE [dbo].[DeliveryOrderProduct]
ADD CONSTRAINT [FK_DeliveryOrderProduct_IdDeliveryOrder_fk]
    FOREIGN KEY ([IdDeliveryOrder])
    REFERENCES [dbo].[DeliveryOrder]
        ([IdDeliveryOrder])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DeliveryOrderProduct_IdDeliveryOrder_fk'
CREATE INDEX [IX_FK_DeliveryOrderProduct_IdDeliveryOrder_fk]
ON [dbo].[DeliveryOrderProduct]
    ([IdDeliveryOrder]);
GO

-- Creating foreign key on [IdProduct] in table 'DeliveryOrderProduct'
ALTER TABLE [dbo].[DeliveryOrderProduct]
ADD CONSTRAINT [FK_DeliveryOrderProduct_IdProduct_fk]
    FOREIGN KEY ([IdProduct])
    REFERENCES [dbo].[Product]
        ([IdProduct])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DeliveryOrderProduct_IdProduct_fk'
CREATE INDEX [IX_FK_DeliveryOrderProduct_IdProduct_fk]
ON [dbo].[DeliveryOrderProduct]
    ([IdProduct]);
GO

-- Creating foreign key on [Reporter] in table 'InventoryReport'
ALTER TABLE [dbo].[InventoryReport]
ADD CONSTRAINT [FK_InventoryReport_Reporter_fk]
    FOREIGN KEY ([Reporter])
    REFERENCES [dbo].[Employee]
        ([IdEmployee])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryReport_Reporter_fk'
CREATE INDEX [IX_FK_InventoryReport_Reporter_fk]
ON [dbo].[InventoryReport]
    ([Reporter]);
GO

-- Creating foreign key on [Waiter] in table 'LocalOrder'
ALTER TABLE [dbo].[LocalOrder]
ADD CONSTRAINT [FK_LocalOrder_Waiter_fk]
    FOREIGN KEY ([Waiter])
    REFERENCES [dbo].[Employee]
        ([IdEmployee])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LocalOrder_Waiter_fk'
CREATE INDEX [IX_FK_LocalOrder_Waiter_fk]
ON [dbo].[LocalOrder]
    ([Waiter]);
GO

-- Creating foreign key on [IdLocalOrder] in table 'LocalOrderProduct'
ALTER TABLE [dbo].[LocalOrderProduct]
ADD CONSTRAINT [FK_LocalOrderProduct_IdLocalOrder_fk]
    FOREIGN KEY ([IdLocalOrder])
    REFERENCES [dbo].[LocalOrder]
        ([IdLocalOrder])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LocalOrderProduct_IdLocalOrder_fk'
CREATE INDEX [IX_FK_LocalOrderProduct_IdLocalOrder_fk]
ON [dbo].[LocalOrderProduct]
    ([IdLocalOrder]);
GO

-- Creating foreign key on [IdProduct] in table 'LocalOrderProduct'
ALTER TABLE [dbo].[LocalOrderProduct]
ADD CONSTRAINT [FK_LocalOrderProduct_IdProduct_fk]
    FOREIGN KEY ([IdProduct])
    REFERENCES [dbo].[Product]
        ([IdProduct])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LocalOrderProduct_IdProduct_fk'
CREATE INDEX [IX_FK_LocalOrderProduct_IdProduct_fk]
ON [dbo].[LocalOrderProduct]
    ([IdProduct]);
GO

-- Creating foreign key on [IdSupplierOrder] in table 'OrderedSupply'
ALTER TABLE [dbo].[OrderedSupply]
ADD CONSTRAINT [FK_OrderedSupply_IdSupplierOrder_fk]
    FOREIGN KEY ([IdSupplierOrder])
    REFERENCES [dbo].[SupplierOrder]
        ([IdSupplierOrder])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderedSupply_IdSupplierOrder_fk'
CREATE INDEX [IX_FK_OrderedSupply_IdSupplierOrder_fk]
ON [dbo].[OrderedSupply]
    ([IdSupplierOrder]);
GO

-- Creating foreign key on [IdSupply] in table 'OrderedSupply'
ALTER TABLE [dbo].[OrderedSupply]
ADD CONSTRAINT [FK_OrderedSupply_IdSupply_fk]
    FOREIGN KEY ([IdSupply])
    REFERENCES [dbo].[Supply]
        ([IdSupply])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderedSupply_IdSupply_fk'
CREATE INDEX [IX_FK_OrderedSupply_IdSupply_fk]
ON [dbo].[OrderedSupply]
    ([IdSupply]);
GO

-- Creating foreign key on [IdProduct] in table 'Recipe'
ALTER TABLE [dbo].[Recipe]
ADD CONSTRAINT [FK_Recipe_IdProduct_fk]
    FOREIGN KEY ([IdProduct])
    REFERENCES [dbo].[Product]
        ([IdProduct])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Recipe_IdProduct_fk'
CREATE INDEX [IX_FK_Recipe_IdProduct_fk]
ON [dbo].[Recipe]
    ([IdProduct]);
GO

-- Creating foreign key on [IdRecipe] in table 'RecipeSupply'
ALTER TABLE [dbo].[RecipeSupply]
ADD CONSTRAINT [FK_RecipeSupply_IdRecipe_fk]
    FOREIGN KEY ([IdRecipe])
    REFERENCES [dbo].[Recipe]
        ([IdRecipe])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecipeSupply_IdRecipe_fk'
CREATE INDEX [IX_FK_RecipeSupply_IdRecipe_fk]
ON [dbo].[RecipeSupply]
    ([IdRecipe]);
GO

-- Creating foreign key on [IdSupply] in table 'RecipeSupply'
ALTER TABLE [dbo].[RecipeSupply]
ADD CONSTRAINT [FK_RecipeSupply_IdSupply_fk]
    FOREIGN KEY ([IdSupply])
    REFERENCES [dbo].[Supply]
        ([IdSupply])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecipeSupply_IdSupply_fk'
CREATE INDEX [IX_FK_RecipeSupply_IdSupply_fk]
ON [dbo].[RecipeSupply]
    ([IdSupply]);
GO

-- Creating foreign key on [IdSupplier] in table 'SupplierOrder'
ALTER TABLE [dbo].[SupplierOrder]
ADD CONSTRAINT [FK_SupplierOrder_IdSupplier_fk]
    FOREIGN KEY ([IdSupplier])
    REFERENCES [dbo].[Supplier]
        ([IdSupplier])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierOrder_IdSupplier_fk'
CREATE INDEX [IX_FK_SupplierOrder_IdSupplier_fk]
ON [dbo].[SupplierOrder]
    ([IdSupplier]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------