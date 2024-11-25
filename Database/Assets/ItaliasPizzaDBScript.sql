DECLARE @DatabaseName NVARCHAR(128) = N'ItaliasPizzaDB';

-- Verificar si la base de datos existe
IF EXISTS (SELECT name FROM sys.databases WHERE name = @DatabaseName)
BEGIN
    -- Eliminar la base de datos
    DROP DATABASE [ItaliasPizzaDB];
END;

-- Crear la base de datos
CREATE DATABASE [ItaliasPizzaDB];
GO

-- Usar la nueva base de datos
USE [ItaliasPizzaDB];
GO

CREATE TABLE AccessAccount (
    IdAccessAccount INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserName VARCHAR(50) NOT NULL,
    IdEmployee UNIQUEIDENTIFIER NOT NULL,
    [Password] VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    [Status] BIT NOT NULL
);
GO

CREATE TABLE Employee (
    IdEmployee UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    [Status] BIT NOT NULL,
    IdCharge INT NOT NULL
);
GO

CREATE TABLE Charge ( 
    IdCharge INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] VARCHAR(50) NOT NULL
);
GO

CREATE TABLE Client (
    IdClient UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    FirstName VARCHAR(80) NOT NULL,
    LastName VARCHAR(80) NOT NULL,
    Phone VARCHAR(20) NOT NULL
);
GO

CREATE TABLE [Address] (
    IdAddress UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IdClient UNIQUEIDENTIFIER NOT NULL,
    Street VARCHAR(100) NOT NULL,
    [Number] INT NOT NULL,
    PostalCode VARCHAR(20) NOT NULL,
    Colony VARCHAR(50) NOT NULL,
    [Status] BIT NOT NULL,
    Reference VARCHAR(200)
);
GO

CREATE TABLE Product (
    IdProduct UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Name] VARCHAR(50) NOT NULL,
    [IdType] INT NOT NULL,
    Price DECIMAL(12, 2) NOT NULL,
    Size VARCHAR(20) NOT NULL,
    [Status] BIT NOT NULL
);
GO

CREATE TABLE ProductType (
    IdProductType INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Type] VARCHAR(50) NOT NULL
);
GO

CREATE TABLE DeliveryOrder (
    IdDeliveryOrder UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IdClient UNIQUEIDENTIFIER NOT NULL,
    IdOrderStatus INT NOT NULL,
    IdClientAddress UNIQUEIDENTIFIER NOT NULL,
    [Date] DATETIME NOT NULL,
    Total DECIMAL(12, 2) NOT NULL,
    DeliveryDriver UNIQUEIDENTIFIER NOT NULL,
    NotDeliveredReason VARCHAR(100)
);
GO

CREATE TABLE DeliveryOrderProduct (
    IdDeliveryOrderProduct UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IdDeliveryOrder UNIQUEIDENTIFIER NOT NULL,
    IdProduct UNIQUEIDENTIFIER NOT NULL,
    Quantity INT NOT NULL,
    SubTotal DECIMAL(12, 2) NOT NULL
);
GO

CREATE TABLE CashReconciliation (
    IdCashReconciliation UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    OpeningDate DATETIME NOT NULL,
    ClosingDate DATETIME NOT NULL,
    StartingAmount DECIMAL(12, 2) NOT NULL,
    FinishingAmount DECIMAL(12, 2) NOT NULL,
    SalesAmount DECIMAL(12, 2) NOT NULL,
    SpendingsAmount DECIMAL(12, 2) NOT NULL,
    CashDifference DECIMAL(12, 2) NOT NULL,
    Observations VARCHAR(100) NOT NULL,
    RegisteredBy UNIQUEIDENTIFIER NOT NULL
);
GO

CREATE TABLE InventoryReport (
    IdInventoryReport UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ReportDate DATETIME NOT NULL,
    Reporter UNIQUEIDENTIFIER NOT NULL,
    [Status] BIT NOT NULL,
    Observations VARCHAR(100)
);
GO

CREATE TABLE LocalOrder (
    IdLocalOrder UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Waiter UNIQUEIDENTIFIER NOT NULL,
    IdOrderStatus INT NOT NULL,
    [Table] INT NOT NULL,
    [Date] DATETIME NOT NULL,
    Total DECIMAL(12, 2) NOT NULL
);
GO

CREATE TABLE LocalOrderProduct (
    IdLocalOrderProduct UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IdLocalOrder UNIQUEIDENTIFIER NOT NULL,
    IdProduct UNIQUEIDENTIFIER NOT NULL,
    Quantity INT NOT NULL,
    SubTotal DECIMAL(12, 2) NOT NULL
);
GO

CREATE TABLE OrderedSupply (
    IdOrderedSupply INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    IdSupply UNIQUEIDENTIFIER NOT NULL,
    IdSupplierOrder UNIQUEIDENTIFIER NOT NULL,
    OrderIdentifier UNIQUEIDENTIFIER NOT NULL,
    Quantity DECIMAL(12, 2) NOT NULL,
    IdMeasurementUnit INT NOT NULL
);
GO

CREATE TABLE Recipe (
    IdRecipe UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IdProduct UNIQUEIDENTIFIER NOT NULL,
    Instructions VARCHAR(800) NOT NULL
);
GO

CREATE TABLE MeasurementUnit (
	IdMeasurementUnit INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	MeasurementUnit VARCHAR(20) NOT NULL
	);
GO

CREATE TABLE RecipeSupply (
    IdRecipeSupply UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IdRecipe UNIQUEIDENTIFIER NOT NULL,
    IdSupply UNIQUEIDENTIFIER NOT NULL,
    SupplyAmount DECIMAL(12, 3) NOT NULL,
    IdMeasurementUnit INT NOT NULL
);
GO

CREATE TABLE Supplier (
    IdSupplier UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Name] VARCHAR(50) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    [Status] BIT NOT NULL
);
GO

CREATE TABLE SupplierOrder (
    IdSupplierOrder UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IdSupplier UNIQUEIDENTIFIER NOT NULL,
	IdSupply UNIQUEIDENTIFIER NOT NULL,
    OrderDate DATE NOT NULL,
    ExpectedDate DATE NOT NULL,
    ArrivalDate DATE NOT NULL,
    IdOrderStatus INT NOT NULL
);
GO

CREATE TABLE OrderStatus (
    IdOrderStatus INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Status] VARCHAR(20) NOT NULL
);
GO

CREATE TABLE Supply (
    IdSupply UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Name] VARCHAR(50) NOT NULL,
    Quantity DECIMAL(12, 3) NOT NULL,
    IdSupplyCategory INT NOT NULL,
    IdMeasurementUnit INT NOT NULL,
    ExpirationDate DATE NOT NULL,
    [Status] BIT NOT NULL
);
GO

CREATE TABLE SupplyCategory (
    IdSupplyCategory INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [SupplyCategory] VARCHAR(50) NOT NULL
);
GO

CREATE TABLE SupplierSupplyCategory (
    IdSupplierSupplyCategory INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdSupplier UNIQUEIDENTIFIER NOT NULL,
    IdSupplyCategory INT NOT NULL
);
GO

CREATE TABLE SupplyInventoryReport (
    IdSupplyInventoryReport UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IdInventoryReport UNIQUEIDENTIFIER NOT NULL,
    IdSupply UNIQUEIDENTIFIER NOT NULL,
    IdMeasurementUnit INT NOT NULL,
    ExpectedAmount DECIMAL(12, 2)NOT NULL,
    ReportedAmount DECIMAL(12, 2) NOT NULL,
    DifferingAmountReason VARCHAR (100)
);
GO

CREATE TABLE [Transaction] (
    IdTransaction UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IdTransactionType INT NOT NULL,
    [Date] DATETIME NOT NULL,
    Amount DECIMAL(12, 2) NOT NULL,
    [Description] VARCHAR(512) NOT NULL,
    RegisteredBy UNIQUEIDENTIFIER NOT NULL
);
GO

CREATE TABLE TransactionType (
    IdTransactionType INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TransactionType VARCHAR(20) NOT NULL
);
GO

-- Agregar restricciones de clave foranea
ALTER TABLE AccessAccount 
    ADD CONSTRAINT AccessAccount_IdEmployee_fk FOREIGN KEY (IdEmployee) REFERENCES Employee (IdEmployee);
GO

ALTER TABLE [Address] 
    ADD CONSTRAINT Address_IdClient_fk FOREIGN KEY (IdClient) REFERENCES Client (IdClient);
GO

ALTER TABLE DeliveryOrder 
    ADD CONSTRAINT DeliveryOrder_IdClient_fk FOREIGN KEY (IdClient) REFERENCES Client (IdClient);
GO

ALTER TABLE DeliveryOrder 
    ADD CONSTRAINT DeliveryOrder_DeliveryDriver_fk FOREIGN KEY (DeliveryDriver) REFERENCES Employee (IdEmployee);
GO

ALTER TABLE DeliveryOrder
    ADD CONSTRAINT DeliveryOrder_CliendAdress_fk FOREIGN KEY (IdClientAddress) REFERENCES [Address] (IdAddress);
GO

ALTER TABLE DeliveryOrderProduct 
    ADD CONSTRAINT DeliveryOrderProduct_IdDeliveryOrder_fk FOREIGN KEY (IdDeliveryOrder) REFERENCES DeliveryOrder (IdDeliveryOrder);
GO

ALTER TABLE DeliveryOrderProduct
    ADD CONSTRAINT DeliveryOrderProduct_IdProduct_fk FOREIGN KEY (IdProduct) REFERENCES Product (IdProduct);
GO

ALTER TABLE CashReconciliation 
    ADD CONSTRAINT CashReconciliation_RegisteredBy_fk FOREIGN KEY (RegisteredBy) REFERENCES Employee (IdEmployee);
GO

ALTER TABLE InventoryReport 
    ADD CONSTRAINT InventoryReport_Reporter_fk FOREIGN KEY (Reporter) REFERENCES Employee (IdEmployee);
GO

ALTER TABLE LocalOrder 
    ADD CONSTRAINT LocalOrder_Waiter_fk FOREIGN KEY (Waiter) REFERENCES Employee (IdEmployee);
GO

ALTER TABLE LocalOrderProduct 
    ADD CONSTRAINT LocalOrderProduct_IdLocalOrder_fk FOREIGN KEY (IdLocalOrder) REFERENCES LocalOrder (IdLocalOrder);
GO

ALTER TABLE LocalOrderProduct
    ADD CONSTRAINT LocalOrderProduct_IdProduct_fk FOREIGN KEY (IdProduct) REFERENCES Product (IdProduct);
GO

ALTER TABLE Recipe 
    ADD CONSTRAINT Recipe_IdProduct_fk FOREIGN KEY (IdProduct) REFERENCES Product (IdProduct);
GO

ALTER TABLE RecipeSupply 
    ADD CONSTRAINT RecipeSupply_IdRecipe_fk FOREIGN KEY (IdRecipe) REFERENCES Recipe (IdRecipe);
GO

ALTER TABLE RecipeSupply 
    ADD CONSTRAINT RecipeSupply_IdSupply_fk FOREIGN KEY (IdSupply) REFERENCES Supply (IdSupply);
GO

ALTER TABLE SupplierOrder 
    ADD CONSTRAINT SupplierOrder_IdSupplier_fk FOREIGN KEY (IdSupplier) REFERENCES Supplier (IdSupplier);
GO

ALTER TABLE OrderedSupply 
    ADD CONSTRAINT OrderedSupply_IdSupply_fk FOREIGN KEY (IdSupply) REFERENCES Supply (IdSupply);
GO

ALTER TABLE OrderedSupply 
    ADD CONSTRAINT OrderedSupply_IdSupplierOrder_fk FOREIGN KEY (IdSupplierOrder) REFERENCES SupplierOrder (IdSupplierOrder);
GO

ALTER TABLE Employee 
    ADD CONSTRAINT Employee_Charge_fk FOREIGN KEY (IdCharge) REFERENCES Charge (IdCharge);
GO

ALTER TABLE Product
    ADD CONSTRAINT Product_IdType_fk FOREIGN KEY (IdType) REFERENCES ProductType (IdProductType);
GO

ALTER TABLE Supply
    ADD CONSTRAINT Supply_IdSupplyCategory_fk FOREIGN KEY (IdSupplyCategory) REFERENCES SupplyCategory (IdSupplyCategory);
GO

ALTER TABLE SupplyInventoryReport
    ADD CONSTRAINT SupplyInventoryReport_IdInventoryReport_fk FOREIGN KEY (IdInventoryReport) REFERENCES InventoryReport (IdInventoryReport);
GO

ALTER TABLE SupplyInventoryReport
    ADD CONSTRAINT SupplyInventoryReport_IdSupply_fk FOREIGN KEY (IdSupply) REFERENCES Supply (IdSupply);
GO

ALTER TABLE [Transaction]
    ADD CONSTRAINT Transaction_RegisteredBy_fk FOREIGN KEY (RegisteredBy) REFERENCES Employee (IdEmployee);
GO

ALTER TABLE [Transaction]
    ADD CONSTRAINT Transaction_IdTransactionType_fk FOREIGN KEY (IdTransactionType) REFERENCES TransactionType (IdTransactionType);
GO

ALTER TABLE OrderedSupply
	ADD CONSTRAINT OrderedSupply_IdMeasurementUnit_fk FOREIGN KEY (IdMeasurementUnit) REFERENCES MeasurementUnit (IdMeasurementUnit);
GO

ALTER TABLE RecipeSupply
	ADD CONSTRAINT RecipeSupply_IdMeasurementUnit_fk FOREIGN KEY (IdMeasurementUnit) REFERENCES MeasurementUnit (IdMeasurementUnit);
GO

ALTER TABLE Supply
	ADD CONSTRAINT Supply_IdMeasurementUnit_fk FOREIGN KEY (IdMeasurementUnit) REFERENCES MeasurementUnit (IdMeasurementUnit);
GO

ALTER TABLE SupplyInventoryReport
	ADD CONSTRAINT SupplyInventoryReport_IdMeasurementUnit_fk FOREIGN KEY (IdMeasurementUnit) REFERENCES MeasurementUnit (IdMeasurementUnit);
GO

ALTER TABLE DeliveryOrder
	ADD CONSTRAINT DeliveryOrder_IdOrderStatus_fk FOREIGN KEY (IdOrderStatus) REFERENCES OrderStatus (IdOrderStatus);
GO

ALTER TABLE LocalOrder
	ADD CONSTRAINT LocalOrder_IdOrderStatus_fk FOREIGN KEY (IdOrderStatus) REFERENCES OrderStatus (IdOrderStatus);
GO

ALTER TABLE SupplierOrder
	ADD CONSTRAINT SupplierOrder_IdOrderStatus_fk FOREIGN KEY (IdOrderStatus) REFERENCES OrderStatus (IdOrderStatus);
GO

ALTER TABLE SupplierSupplyCategory
    ADD CONSTRAINT SupplierSupplyCategory_IdSupplier_fk FOREIGN KEY (IdSupplier) REFERENCES Supplier (IdSupplier); 
GO

ALTER TABLE SupplierSupplyCategory
    ADD CONSTRAINT SupplierSupplyCategory_IdSupplyCategory_fk FOREIGN KEY (IdSupplyCategory) REFERENCES SupplyCategory (IdSupplyCategory); 
GO

--Agregar datos de precarga
INSERT INTO Charge ([Name])
VALUES 
    ('Mesero'),
    ('Repartidor'),
    ('Cocinero'),
    ('Cajero'),
    ('Gerente');
GO

INSERT INTO MeasurementUnit (MeasurementUnit)
VALUES 
    ('Kilogramos'),
    ('Unidades'),
    ('Litros');
GO

INSERT INTO SupplyCategory ([SupplyCategory])
VALUES
	('Harinas'),
	('Salsas'),
	('Quesos'),
	('Carnes'),
	('Vegetales'),
	('Frutas'),
	('Condimentos'),
	('Aceites'),
	('Bebidas')
GO

INSERT INTO OrderStatus([Status])
VALUES
    ('Realizado'),
	('Entregado'),
	('En camino'),
	('Cancelado'),
	('No entregado'),
    ('Pendiente'),
    ('En preparacion'),
    ('Listo para entregar');
GO

INSERT INTO ProductType ([Type])
VALUES
    ('Pizza'),
    ('Bebida'),
    ('Postre');
GO

INSERT INTO TransactionType (TransactionType)
VALUES
    ('Venta'),
    ('Salida');
GO

INSERT INTO Employee (IdEmployee, FirstName, LastName, Phone, [Status], IdCharge)
VALUES
    ('00000000-0000-0000-0000-000000000000', 'empty', 'empty', 0000000000, 1, 2);
GO