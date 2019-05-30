USE master
GO
sp_configure 'show advanced options', 1
GO
RECONFIGURE WITH OVERRIDE
GO 
sp_configure 'contained database authentication', 1
GO
RECONFIGURE WITH OVERRIDE
GO 
sp_configure 'show advanced options', 0 
GO
RECONFIGURE WITH OVERRIDE 
GO

DROP DATABASE LegacyApp
GO

CREATE DATABASE LegacyApp
CONTAINMENT = PARTIAL
GO

USE LegacyApp

CREATE USER LegacyUser WITH PASSWORD = N'Password-1'
ALTER ROLE db_datareader ADD MEMBER LegacyUser
ALTER ROLE db_datawriter ADD MEMBER LegacyUser 

CREATE TABLE dbo.Product (
	ProductId int IDENTITY (1,1) NOT NULL,
	Name varchar (100) NOT NULL,
	Price decimal NOT NULL,
	CONSTRAINT PK_Product PRIMARY KEY (ProductId)
)

CREATE TABLE dbo.Customer (
	CustomerId int IDENTITY (1,1) NOT NULL,
	FirstName varchar (50) NOT NULL,
	LastName varchar (50) NOT NULL,
	AddressLine1 varchar (100) NOT NULL,
	AddressLine2 varchar (100) NOT NULL,
	City varchar (50) NOT NULL,
	State varchar (2) NOT NULL,
	Zip varchar (9) NOT NULL,
	CONSTRAINT PK_Customer PRIMARY KEY (CustomerId)
)

CREATE TABLE dbo.Cart (
	CartId int IDENTITY (1,1) NOT NULL,
	CreateDate datetime NOT NULL,
	LastUpdate datetime NOT NULL,
	CONSTRAINT PK_Cart PRIMARY KEY (CartId)
)

CREATE TABLE dbo.CartItem (
	CartItemId int IDENTITY (1,1) NOT NULL,
	CartId int NOT NULL,
	ProductId int NOT NULL,
	Quantity int NOT NULL CONSTRAINT DF_CartItem_Quantity DEFAULT (1),
	CONSTRAINT PK_CartItem PRIMARY KEY (CartItemId),
	CONSTRAINT FK_Cart_Product FOREIGN KEY (ProductId) REFERENCES dbo.Product (ProductId),
	CONSTRAINT FK_Cart_CartItem FOREIGN KEY (CartId) REFERENCES dbo.Cart (CartId)
)

CREATE TABLE dbo.[Order] (
	OrderId int IDENTITY (1,1) NOT NULL,
	OrderDate datetime NOT NULL,
	CustomerId int NOT NULL,
	OrderStatus varchar (20) NOT NULL,
	FirstName varchar (50) NOT NULL,
	LastName varchar (50) NOT NULL,
	AddressLine1 varchar (100) NOT NULL,
	AddressLine2 varchar (100) NOT NULL,
	City varchar (50) NOT NULL,
	State varchar (2) NOT NULL,
	Zip varchar (9) NOT NULL,
	CreditCardNumber varchar (16) NOT NULL,
	CreditCardExpirationDate varchar (7) NOT NULL,
	CONSTRAINT PK_Order PRIMARY KEY (OrderId),
	CONSTRAINT FK_Order_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Customer (CustomerId)
)

CREATE TABLE dbo.OrderItem (
	OrderItemId int IDENTITY (1,1) NOT NULL,
	OrderId int NOT NULL,
	ProductId int NOT NULL,
	Price decimal NOT NULL,
	Quantity int NOT NULL,
	CONSTRAINT PK_OrderItem PRIMARY KEY (OrderItemId),
	CONSTRAINT FK_OrderItem_Order FOREIGN KEY (OrderId) REFERENCES dbo.[Order] (OrderId),
	CONSTRAINT FK_OrderItem_Product FOREIGN KEY (ProductId) REFERENCES dbo.Product (ProductId)
)

CREATE TABLE dbo.Ingredient (
	IngredientId int IDENTITY (1,1) NOT NULL,
	Name varchar (50) NOT NULL,
	InventoryQuantity decimal NOT NULL,
	Measurement varchar (10) NOT NULL,
	CONSTRAINT PK_Ingredient PRIMARY KEY (IngredientId)
)

CREATE TABLE dbo.ProductIngredient (
	ProductIngredientId int IDENTITY (1,1) NOT NULL,
	ProductId int NOT NULL,
	IngredientId int NOT NULL,
	Quantity decimal NOT NULL,
	CONSTRAINT PK_ProductIngredient PRIMARY KEY (ProductIngredientId),
	CONSTRAINT FK_ProductIngredient_Product FOREIGN KEY (ProductId) REFERENCES dbo.Product (ProductId),
	CONSTRAINT FK_ProductIngredient_Ingredient FOREIGN KEY (IngredientId) REFERENCES dbo.Ingredient (IngredientId)
)
GO

CREATE PROCEDURE dbo.CheckOut (
	@CartId int,
	@FirstName varchar (50),
	@LastName varchar (50),
	@AddressLine1 varchar (100),
	@AddressLine2 varchar (100),
	@City varchar (50),
	@State varchar (2),
	@Zip varchar (9),
	@CreditCardNumber varchar (16),
	@CreditCardExpirationDate varchar (7)
)
AS
BEGIN
	DECLARE @CustomerId int
	DECLARE @OrderId int

	INSERT INTO dbo.Customer (
		FirstName ,
		LastName,
		AddressLine1,
		AddressLine2,
		City,
		State,
		Zip
	) VALUES (
		@FirstName ,
		@LastName,
		@AddressLine1,
		@AddressLine2,
		@City,
		@State,
		@Zip
	)
	SELECT @CustomerId = SCOPE_IDENTITY()

	INSERT INTO dbo.[Order] (
		OrderDate,
		CustomerId,
		OrderStatus,
		FirstName ,
		LastName,
		AddressLine1,
		AddressLine2,
		City,
		State,
		Zip,
		CreditCardNumber,
		CreditCardExpirationDate
	) VALUES (
		GETDATE(),
		@CustomerId,
		'Pending',
		@FirstName ,
		@LastName,
		@AddressLine1,
		@AddressLine2,
		@City,
		@State,
		@Zip,
		@CreditCardNumber,
		@CreditCardExpirationDate
	)
	SELECT @OrderId = SCOPE_IDENTITY()

	INSERT INTO dbo.OrderItem (
		OrderId,
		ProductId,
		Price,
		Quantity
	)
	SELECT
		@OrderId,
		i.ProductId,
		p.Price,
		i.Quantity
	FROM dbo.CartItem i
	INNER JOIN dbo.Product p ON p.ProductId = i.ProductId
	WHERE i.CartId = @CartId

	DELETE FROM dbo.CartItem WHERE CartId = @CartId
	DELETE FROM dbo.Cart WHERE CartId = @CartId
END
GO

GRANT EXEC ON dbo.CheckOut TO LegacyUser
GO