CREATE RULE Non_negative
	AS @col >= 0
go
CREATE RULE Above_zero
	AS @col >= 0
go
CREATE TABLE Category
(
    categoryId int  NOT NULL ,
    name varchar(50)  NOT NULL
)
    go
ALTER TABLE Category
    ADD CONSTRAINT XPKCategory PRIMARY KEY  CLUSTERED (categoryId ASC)
    go
CREATE TABLE DeliveryType
(
    typeId int  NOT NULL ,
    title varchar(20)  NOT NULL
)
    go
ALTER TABLE DeliveryType
    ADD CONSTRAINT XPKDeliveryType PRIMARY KEY  NONCLUSTERED (typeId ASC)
    go

CREATE TABLE Deliveryman
(
    address varchar(100)  NOT NULL ,
    date datetime  NOT NULL ,
    deliveryNumber int  NOT NULL ,
    typeId int  NOT NULL ,
    providerId int  NOT NULL ,
    orderId int  NOT NULL
)
    go
ALTER TABLE Deliveryman
    ADD CONSTRAINT XPKDelivery PRIMARY KEY  CLUSTERED (orderId ASC)
    go
CREATE TABLE Order
(
    orderId int  NOT NULL ,
    date datetime  NOT NULL ,
    OrderStatus_id int  NOT NULL ,
    userId int  NOT NULL
)
    go
ALTER TABLE Order
    ADD CONSTRAINT XPKOrder PRIMARY KEY  CLUSTERED (orderId ASC)

    go
CREATE TABLE Order_products
(
    productId int  NOT NULL ,
    orderId int  NOT NULL ,
    price decimal(10,2)  NOT NULL ,
    number int  NOT NULL
)
    go
ALTER TABLE Order_products
    ADD CONSTRAINT XPKOrder_products PRIMARY KEY  NONCLUSTERED (productId ASC,orderId ASC)
    go
CREATE TABLE Product
(
    productId int  NOT NULL ,
    title varchar(50)  NOT NULL ,
    price decimal(10,2)  NOT NULL ,
    number int  NOT NULL ,
    description text  NOT NULL ,
    categoryId int  NOT NULL
)
    go
ALTER TABLE Product

    ADD CONSTRAINT XPKProduct PRIMARY KEY  CLUSTERED (productId ASC)
    go
CREATE TABLE Provider
(
    providerId int  NOT NULL ,
    title varchar(50)  NOT NULL
)
    go
ALTER TABLE Provider
    ADD CONSTRAINT XPKProvider PRIMARY KEY  NONCLUSTERED (providerId ASC)
    go
CREATE TABLE Role
(
    title varchar(50)  NOT NULL ,
    roleId int  NOT NULL
)
    go
ALTER TABLE RoleADD CONSTRAINT XPKRole PRIMARY KEY  CLUSTERED (roleId ASC)
    go
CREATE TABLE OrderStatus
(
    OrderStatus_id int  NOT NULL ,
    title varchar(15)  NOT NULL

)
    go
ALTER TABLE OrderStatus
    ADD CONSTRAINT XPKStatus PRIMARY KEY  NONCLUSTERED (OrderStatus_id ASC)
    go
CREATE TABLE User
(
    userId int  NOT NULL ,
    firstname varchar(60)  NOT NULL ,
    lastname varchar(60)  NOT NULL ,
    login varchar(10)  NOT NULL ,
    password varchar(10)  NOT NULL ,
    roleId int  NOT NULL
)
    go
ALTER TABLE User
    ADD CONSTRAINT XPKUser PRIMARY KEY  NONCLUSTERED (userId ASC)
    go
ALTER TABLE Deliveryman
    ADD CONSTRAINT  R_5 FOREIGN KEY (typeId) REFERENCES DeliveryType(typeId)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
    go

ALTER TABLE Deliveryman
    ADD CONSTRAINT  R_6 FOREIGN KEY (providerId) REFERENCES Provider(providerId)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
    go
ALTER TABLE Deliveryman
    ADD CONSTRAINT  R_33 FOREIGN KEY (orderId) REFERENCES Order(orderId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
    go
ALTER TABLE Order
    ADD CONSTRAINT  R_3 FOREIGN KEY (OrderStatus_id) REFERENCES OrderStatus(OrderStatus_id)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
    go
ALTER TABLE Order
    ADD CONSTRAINT  R_26 FOREIGN KEY (userId) REFERENCES User(userId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
    go
ALTER TABLE Order_products

    ADD CONSTRAINT  R_16 FOREIGN KEY (productId) REFERENCES Product(productId)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
    go
ALTER TABLE Order_products
    ADD CONSTRAINT  R_17 FOREIGN KEY (orderId) REFERENCES Order(orderId)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
    go
ALTER TABLE Product
    ADD CONSTRAINT  R_12 FOREIGN KEY (categoryId) REFERENCES Category(categoryId)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
    go
ALTER TABLE User
    ADD CONSTRAINT  R_28 FOREIGN KEY (roleId) REFERENCES Role(roleId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
    go
exec sp_bindrule 'Non_negative', 'Category.categoryId'
go
exec sp_bindrule 'Non_negative', 'DeliveryType.typeId' 

go
exec sp_bindrule 'Non_negative', 'Deliveryman.deliveryNumber'
go
exec sp_bindrule 'Non_negative', 'Order.orderId'
go
exec sp_bindrule 'Non_negative', 'Order_products.price'
go
exec sp_bindrule 'Above_zero', 'Order_products.number'
go
exec sp_bindrule 'Non_negative', 'Product.productId'
go
exec sp_bindrule 'Non_negative', 'Product.price'
go
exec sp_bindrule 'Above_zero', 'Product.number'
go
exec sp_bindrule 'Non_negative', 'Provider.providerId'
go
exec sp_bindrule 'Non_negative', 'Role.roleId'
go
exec sp_bindrule 'Non_negative', 'OrderStatus.OrderStatus_id'
go
exec sp_bindrule 'Non_negative', 'User.userId'
go

