USE
[Pizza]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 15.07.2022 12:43:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category]
(
    [
    Id] [
    int]
    NOT
    NULL
    IDENTITY
(
    1,
    1
),
    [Name] [varchar]
(
    60
) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED
(
[
    Id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
  ON [PRIMARY]
    )
  ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Delivery]    Script Date: 15.07.2022 12:43:15 ******/
    SET ANSI_NULLS
  ON
    GO
    SET QUOTED_IDENTIFIER
  ON
    GO
CREATE TABLE [dbo].[Delivery]
(
    [
    Id] [
    int]
    NOT
    NULL
    IDENTITY
(
    1,
    1
),
    [Address] [text] NOT NULL,
    [Date] [datetime] NOT NULL,
    [provider_id] [int] NOT NULL,
    [type_id] [int] NOT NULL,
    [order_Id] [int] NOT NULL
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[DeliveryProvider]    Script Date: 15.07.2022 12:43:15 ******/
    SET ANSI_NULLS
      ON
    GO
    SET QUOTED_IDENTIFIER
      ON
    GO
CREATE TABLE [dbo].[DeliveryProvider]
(
    [
    Id] [
    int]
    NOT
    NULL
    IDENTITY
(
    1,
    1
),
    [name] [varchar]
(
    50
) NOT NULL,
    CONSTRAINT [PK_DeliveryProvider] PRIMARY KEY CLUSTERED
(
[
    Id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
  ON [PRIMARY]
    )
  ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[DeliveryType]    Script Date: 15.07.2022 12:43:15 ******/
    SET ANSI_NULLS
  ON
    GO
    SET QUOTED_IDENTIFIER
  ON
    GO
CREATE TABLE [dbo].[DeliveryType]
(
    [
    Id] [
    int]
    NOT
    NULL
    IDENTITY
(
    1,
    1
),
    [name] [varchar]
(
    50
) NOT NULL,
    CONSTRAINT [PK_DeliveryType] PRIMARY KEY CLUSTERED
(
[
    Id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
  ON [PRIMARY]
    )
  ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Order]    Script Date: 15.07.2022 12:43:15 ******/
    SET ANSI_NULLS
  ON
    GO
    SET QUOTED_IDENTIFIER
  ON
    GO
CREATE TABLE [dbo].[Order]
(
    [
    Id] [
    int]
    NOT
    NULL
    IDENTITY
(
    1,
    1
),
    [Date] [datetime] NOT NULL,
    [user_id] [int] NOT NULL,
    [orderStatus_id] [int] NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED
(
[
    Id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
  ON [PRIMARY]
    )
  ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[OrderProduct]    Script Date: 15.07.2022 12:43:15 ******/
    SET ANSI_NULLS
  ON
    GO
    SET QUOTED_IDENTIFIER
  ON
    GO
CREATE TABLE [dbo].[OrderProduct]
(
    [
    product_id] [
    int]
    NOT
    NULL, [
    order_id] [
    int]
    NOT
    NULL, [
    _Price] [
    decimal]
(
    10,
    2
) NOT NULL,
    [_Quantity] [int] NOT NULL
    ) ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[orderStatus]    Script Date: 15.07.2022 12:43:15 ******/
    SET ANSI_NULLS
      ON
    GO
    SET QUOTED_IDENTIFIER
      ON
    GO
CREATE TABLE [dbo].[OrderStatus]
(
    [
    Id] [
    int]
    NOT
    NULL
    IDENTITY
(
    1,
    1
),
    [Name] [varchar]
(
    50
) NOT NULL,
    CONSTRAINT [PK_orderStatus] PRIMARY KEY CLUSTERED
(
[
    Id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
  ON [PRIMARY]
    )
  ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Product]    Script Date: 15.07.2022 12:43:15 ******/
    SET ANSI_NULLS
  ON
    GO
    SET QUOTED_IDENTIFIER
  ON
    GO
CREATE TABLE [dbo].[Product]
(
    [
    Id] [
    int]
    NOT
    NULL
    IDENTITY
(
    1,
    1
),
    [Title] [varchar]
(
    60
) NOT NULL,
    [Description] [text] NOT NULL,
    [Price] [decimal]
(
    10,
    2
) NOT NULL,
    [category_id] [int] NOT NULL,
    [Quantity] [int] NOT NULL,
    [Image] [text] NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED
(
[
    Id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
  ON [PRIMARY]
    )
  ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Role]    Script Date: 15.07.2022 12:43:15 ******/
    SET ANSI_NULLS
  ON
    GO
    SET QUOTED_IDENTIFIER
  ON
    GO
CREATE TABLE [dbo].[Role]
(
    [
    Id] [
    int]
    NOT
    NULL
    IDENTITY
(
    1,
    1
),
    [Name] [varchar]
(
    50
) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED
(
[
    Id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
  ON [PRIMARY]
    )
  ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[User]    Script Date: 15.07.2022 12:43:15 ******/

    SET ANSI_NULLS
  ON
    GO
    SET QUOTED_IDENTIFIER
  ON
    GO
CREATE TABLE [dbo].[User_]
(
    [
    Id] [
    int]
    NOT
    NULL
    IDENTITY
(
    1,
    1
),
    [UserName] [varchar]
(
    50
) NOT NULL,
    [Name] [varchar]
(
    60
) NOT NULL,
    [Surname] [varchar]
(
    60
) NOT NULL,
    [PasswordHash] [varchar]
(
    200
) NOT NULL,
    [PhoneNumber] [varchar]
(
    20
) NOT NULL,
    [role_Id] [int] NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED
(
[
    Id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
  ON [PRIMARY]
    )
  ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Delivery] WITH CHECK ADD CONSTRAINT [FK_Delivery_DeliveryProvider] FOREIGN KEY ([provider_id])
    REFERENCES [dbo].[DeliveryProvider] ([Id])
    GO
ALTER TABLE [dbo].[Delivery] CHECK CONSTRAINT [FK_Delivery_DeliveryProvider]
    GO
ALTER TABLE [dbo].[Delivery] WITH CHECK ADD CONSTRAINT [FK_Delivery_DeliveryType] FOREIGN KEY ([type_id])
    REFERENCES [dbo].[DeliveryType] ([Id])
    GO
ALTER TABLE [dbo].[Delivery] CHECK CONSTRAINT [FK_Delivery_DeliveryType]
    GO
ALTER TABLE [dbo].[Delivery] WITH CHECK ADD CONSTRAINT [FK_Delivery_Order] FOREIGN KEY ([order_Id])
    REFERENCES [dbo].[Order] ([Id])
    GO
ALTER TABLE [dbo].[Delivery] CHECK CONSTRAINT [FK_Delivery_Order]
    GO
ALTER TABLE [dbo].[Order] WITH CHECK ADD CONSTRAINT [FK_Order_orderStatus] FOREIGN KEY ([orderStatus_id])
    REFERENCES [dbo].[OrderStatus] ([Id])
    GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_orderStatus]
    GO
ALTER TABLE [dbo].[Order] WITH CHECK ADD CONSTRAINT [FK_Order_User] FOREIGN KEY ([user_id])
    REFERENCES [dbo].[User_] ([Id])
    GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_User]
    GO
ALTER TABLE [dbo].[OrderProduct] WITH CHECK ADD CONSTRAINT [FK_OrderProduct_Order] FOREIGN KEY ([order_id])
    REFERENCES [dbo].[Order] ([Id])
    GO
ALTER TABLE [dbo].[OrderProduct] CHECK CONSTRAINT [FK_OrderProduct_Order]
    GO
ALTER TABLE [dbo].[OrderProduct] WITH CHECK ADD CONSTRAINT [FK_OrderProduct_Product] FOREIGN KEY ([product_id])
    REFERENCES [dbo].[Product] ([Id])
    GO
ALTER TABLE [dbo].[OrderProduct] CHECK CONSTRAINT [FK_OrderProduct_Product]
    GO
ALTER TABLE [dbo].[Product] WITH CHECK ADD CONSTRAINT [FK_Product_Category] FOREIGN KEY ([category_id])
    REFERENCES [dbo].[Category] ([Id])
    GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
    GO



ALTER TABLE dbo.User_
    ADD Email varchar(50);

ALTER TABLE dbo.User_
    ADD LoginProvider varchar(250);

ALTER TABLE dbo.User_
    ADD ProviderKey varchar(250);



INSERT INTO dbo.Role (Name)
VALUES ('admin');
INSERT INTO dbo.Role (Name)
VALUES ('user');

INSERT INTO dbo.Category (Name)
VALUES ('Best price');
INSERT INTO dbo.Category (Name)
VALUES ('Tasty');
INSERT INTO dbo.Category (Name)
VALUES ('Mini pizza');

INSERT INTO dbo.OrderStatus (Name)
VALUES ('in progress');
INSERT INTO dbo.OrderStatus (Name)
VALUES ('confirmed');
INSERT INTO dbo.OrderStatus (Name)
VALUES ('rejected');

INSERT INTO dbo.DeliveryProvider (Name)
VALUES ('Glovo');
INSERT INTO dbo.DeliveryProvider (Name)
VALUES ('Bolt Food');

INSERT INTO dbo.DeliveryType (Name)
VALUES ('Usual');
INSERT INTO dbo.DeliveryType (Name)
VALUES ('Super delivery');