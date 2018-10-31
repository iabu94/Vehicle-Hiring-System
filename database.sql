DROP DATABASE TaxiService;

CREATE DATABASE TaxiService;

USE TaxiService;

CREATE TABLE UserLogin (
	UserLoginID int IDENTITY(1,1) PRIMARY KEY,
	UserName varchar(15) UNIQUE NOT NULL,
	UserPassword varchar(30) NOT NULL
);

INSERT INTO UserLogin VALUES ('admin','admin123'),('driver','driver123'),('passenger','passenger123'),('driver1','driver123');

CREATE TABLE UserType (
	UserTypeID int IDENTITY(1,1) PRIMARY KEY,
	UserTypeName varchar(15) NOT NULL,
);

INSERT INTO UserType VALUES ('Admin'),('Driver'),('Passanger');

CREATE TABLE UserDetail (
    UserID int IDENTITY(1,1) PRIMARY KEY,
    FirstName varchar(50) NOT NULL,
    LastName varchar(50) NOT NULL,
	ContactNumber int NOT NULL,
	Email varchar(50),
	UserImage varchar(200),
	UserLoginID int UNIQUE FOREIGN KEY REFERENCES UserLogin(UserLoginID),
	UserTypeID int FOREIGN KEY REFERENCES UserType(UserTypeID)
);

INSERT INTO UserDetail VALUES ('Abdullah','Mansoor',0719994818,'abu@gmail.com','~/Images/Drivers/photo2180901061910.jpg',1,1);
INSERT INTO UserDetail VALUES ('Kethees','Kumar',0770412930,'kethees@gmail.com','~/Images/Drivers/kethees180905115435.jpg',2,2);
INSERT INTO UserDetail VALUES ('Sajith','Kumar',0771234567,'sajith@gmail.com','',3,3);
INSERT INTO UserDetail VALUES ('Fahim','Jawfer',0710712198,'fahim@gmail.com','',4,2);

CREATE TABLE Driver (
	DriverID int IDENTITY(1,1) PRIMARY KEY,
	DriverCode varchar(10) NOT NULL,
	Address varchar(200) NOT NULL,
	Age int NOT NULL,
	NICNo varchar(12) NOT NULL,
	Gender varchar(6) NOT NULL,
	DOB date NOT NULL,
	LicenseNo varchar(20) NOT NULL,
	UserLoginID int UNIQUE FOREIGN KEY REFERENCES UserLogin(UserLoginID)
);

INSERT INTO Driver VALUES ('DRV002','Haputale, Badulla',25,'934875124V','Male','12/08/1994','123458792114',2);
INSERT INTO Driver VALUES ('DRV001','New Dennawa, Kagama',25,'941610170V','Male','09/06/1994','199416100170',4);

CREATE TABLE VehicleType(
	VehicleTypeID int NOT NULL PRIMARY KEY,
	VehicleTypeName varchar(20) NOT NULL,
	MaxPassengers int NOT NULL,
	MinDistanceKm int,
	BasicFareAmount int NOT NULL,
	PerKmFare int NOT NULL
);

INSERT INTO VehicleType VALUES (1,'NanoCar',4,1,150,50),(2,'Car',4,1,175,50),(3,'Van',10,8,1000,50);

CREATE TABLE Vehicle (
	VehicleID int IDENTITY(1,1) PRIMARY KEY,
	VehicleNumber varchar(10) NOT NULL,
	VehicleOwner varchar(30) NOT NULL,
	VehicleBrand varchar(20) NOT NULL,
	IsAvailable varchar(3) NOT NULL,
	CurrentLocation varchar(200),
	VehicleTypeID int FOREIGN KEY REFERENCES VehicleType(VehicleTypeID),
	DriverID int UNIQUE FOREIGN KEY REFERENCES Driver(DriverID)
);

INSERT INTO Vehicle VALUES ('WP 7895', 'Suren', 'Bajaj','YES','Kiribathgoda, Sri Lanka',1,1);
INSERT INTO Vehicle VALUES ('WP 4589', 'Suren', 'Tata','YES','Maradana, Colombo, Sri Lanka',1,2);


CREATE TABLE HireState(
	HireStatusID int NOT NULL PRIMARY KEY,
	HireStatus varchar(15) NOT NULL
);

INSERT INTO HireState VALUES (1,'Requested'), (2,'Confirmed'), (3,'OnGoing'), (4,'Ended'), (5,'Cancelled'), (6,'Rejected');

CREATE TABLE HireDetail(
	HireID int IDENTITY(1,1) PRIMARY KEY,
	PickupLocation varchar(200) NOT NULL,
	DropLocation varchar(200),
	DistanceKm float NOT NULL,
	PickupDateTime DATETIME NOT NULL,
	EstimatedFare float NOT NULL,
	UserID int FOREIGN KEY REFERENCES UserDetail(UserID),
	VehicleID int FOREIGN KEY REFERENCES Vehicle(VehicleID),
	VehicleTypeID int FOREIGN KEY REFERENCES VehicleType(VehicleTypeID),
	HireStatusID int FOREIGN KEY REFERENCES HireState(HireStatusID),
);



CREATE TABLE DriverDetailsTable(
	DriverID int IDENTITY(1,1) PRIMARY KEY,
	DriverCode varchar(10),
	FirstName varchar(50),
	LastName varchar(50),
	Email varchar(50),
	Mobile int NOT NULL,
	Password varchar(100) NOT NULL,
	Address varchar(200),
	CurrentLocation varchar(200),
	UserImageUrl varchar(200),
	RegisteredDate datetime,
	
	VehicleModel varchar(50),
	VehicleBrand varchar(50),
	VehicleYear varchar(5),
	VehicleColor varchar(20),
	VehicleNumber varchar(20),
	
	LicenseNo varchar(50) NOT NULL,
	
	Status int NOT NULL,
	IsOnline int NOT NULL
);


CREATE TABLE UserStatusTable(
	UserStatusID int IDENTITY(1,1) PRIMARY KEY,
	UserStatusVale varchar(10)
);

INSERT INTO UserStatusTable VALUES ('PENDING'),('APPROVED'),('REJECTED'),('RESIGNED');

CREATE TABLE UserLoginDetail(
	UserLoginID int IDENTITY(1,1) PRIMARY KEY,
	UserTableID int NOT NULL,
	UserLoginEmail varchar(100),
	UserLoginMobile int NOT NULL,
	UserLoginPassword varchar(100) NOT NULL,
	UserType int NOT NULL
);
--In here the user type 1 is admin, 2 is driver, 3 is passenger



--triggers

--INSERT trigger for Driver 
ALTER TRIGGER tr_tblDriverTable_forInsert
ON DriverDetailsTable
FOR INSERT
AS
BEGIN
	Declare @Id int
	Select @Id = DriverID from inserted
	--Update the driver code in DriverDetailsTable
	IF(@Id < 10)
	UPDATE DriverDetailsTable
	SET DriverCode = 'EGO_DR_00' + Cast(@Id as nvarchar(5))
	WHERE DriverID = @Id
	ELSE IF(@Id < 100)
	UPDATE DriverDetailsTable
	SET DriverCode = 'EGO_DR_0' + Cast(@Id as nvarchar(5))
	WHERE DriverID = @Id
	ELSE
	UPDATE DriverDetailsTable
	SET DriverCode = 'EGO_DR_' + Cast(@Id as nvarchar(5))
	WHERE DriverID = @Id
	--Update the UserLoginDetail table
	Declare @email varchar(100), @password varchar(100)
	Declare @mobile int
	Select @email = Email, @mobile = Mobile, @password = Password
	from inserted
	where DriverID = @Id

	Insert into UserLoginDetail
	values (@Id,@email,@mobile,@password, 2)
END


--DELETE trigger for Driver 
CREATE TRIGGER tr_tblDriverTable_forDelete
ON DriverDetailsTable
FOR DELETE
AS
BEGIN
	DECLARE @Id int
	Select @Id = DriverID from deleted
	
	DELETE FROM UserLoginDetail
	WHERE UserTableID = @Id
END

--UPDATE trigger for Driver 
ALTER TRIGGER tr_tblDriverTable_forUpdate
ON DriverDetailsTable
FOR UPDATE
AS
BEGIN
	DECLARE @Id int
	DECLARE @email varchar(50), @mobile int
	DECLARE @password varchar(100)

	Select @Id = DriverID from inserted

	Select @email = Email, @mobile = Mobile, @password = Password
	From inserted
	where DriverID = @Id

	UPDATE UserLoginDetail
	SET UserLoginEmail = @email, UserLoginMobile=@mobile, UserLoginPassword =@password
	WHERE UserTableID = @Id
END

CREATE TABLE RiderDetailsTable(
	RiderID int IDENTITY(1,1) PRIMARY KEY,
	RiderCode varchar(10),
	FirstName varchar(50) NOT NULL,
	LastName varchar(50),
	Email varchar(50),
	Mobile int NOT NULL,
	Password varchar(100),
	CurrentLocation varchar(200),
	RegisteredDate datetime,
	
	Status int NOT NULL,
	IsDeleted int
);


--INSERT trigger for rider 
ALTER TRIGGER tr_tblRiderTable_forInsert
ON RiderDetailsTable
FOR INSERT
AS
BEGIN
	Declare @Id int
	Select @Id = RiderID from inserted
	--Update the driver code in DriverDetailsTable
	IF(@Id < 10)
	UPDATE RiderDetailsTable
	SET RiderCode = 'EGO_RR_00' + Cast(@Id as nvarchar(5))
	WHERE RiderID = @Id
	ELSE IF(@Id < 100)
	UPDATE RiderDetailsTable
	SET RiderCode = 'EGO_RR_0' + Cast(@Id as nvarchar(5))
	WHERE RiderID = @Id
	ELSE
	UPDATE RiderDetailsTable
	SET RiderCode = 'EGO_RR_' + Cast(@Id as nvarchar(5))
	WHERE RiderID = @Id
	--Update the UserLoginDetail table
	Declare @email varchar(100), @password varchar(100)
	Declare @mobile int
	Select @email = Email, @mobile = Mobile, @password = Password
	from inserted
	where RiderID = @Id

	Insert into UserLoginDetail
	values (@Id,@email,@mobile,@password, 3)
END

--DELETE trigger for rider 
CREATE TRIGGER tr_tblRiderTable_forDelete
ON RiderDetailsTable
FOR DELETE
AS
BEGIN
	DECLARE @Id int
	Select @Id = RiderID from deleted
	
	DELETE FROM UserLoginDetail
	WHERE UserTableID = @Id
END

--update trigger for rider 
ALTER TRIGGER tr_tblRiderTable_forUpdate
ON RiderDetailsTable
FOR UPDATE
AS
BEGIN
	DECLARE @Id int
	DECLARE @email varchar(50), @mobile int, @password varchar(100)

	Select @Id = RiderID from inserted

	Select @email = Email, @mobile = Mobile, @password = Password
	From inserted
	where RiderID = @Id

	UPDATE UserLoginDetail
	SET UserLoginEmail = @email, UserLoginMobile=@mobile, UserLoginPassword = @password
	WHERE UserTableID = @Id
END

CREATE TABLE AdminDetailsTable(
	AdminID int IDENTITY(1,1) PRIMARY KEY,
	AdminCode varchar(10),
	FirstName varchar(50),
	LastName varchar(50),
	Email varchar(50),
	Mobile int,
	Password varchar(100),

	UserImageUrl varchar(200),
	RegisteredDate datetime
);

INSERT into AdminDetailsTable VALUES (null,'Abdullah','Mansoor','mansoorabdullah001@gmail.com',719994818,'admin',null,GetDate());

ALTER TRIGGER tr_tblAdminTable_forInsert
ON AdminDetailsTable
FOR INSERT
AS
BEGIN
	Declare @Id int
	Select @Id = AdminID from inserted
	--Update the driver code in DriverDetailsTable
	IF(@Id < 10)
	UPDATE AdminDetailsTable
	SET AdminCode = 'EGO_AD_00' + Cast(@Id as nvarchar(5))
	WHERE AdminID = @Id
	ELSE IF(@Id < 100)
	UPDATE AdminDetailsTable
	SET AdminCode = 'EGO_AD_0' + Cast(@Id as nvarchar(5))
	WHERE AdminID = @Id
	ELSE
	UPDATE AdminDetailsTable
	SET AdminCode = 'EGO_AD_' + Cast(@Id as nvarchar(5))
	WHERE AdminID = @Id
	--Update the UserLoginDetail table
	Declare @email varchar(100), @password varchar(100)
	Declare @mobile int
	Select @email = Email, @mobile = Mobile, @password = Password
	from inserted
	where AdminID = @Id

	Insert into UserLoginDetail
	values (@Id,@email,@mobile,@password, 1)
END

CREATE TRIGGER tr_tblAdminTable_forDelete
ON AdminDetailsTable
FOR DELETE
AS
BEGIN
	DECLARE @Id int
	Select @Id = AdminID from deleted
	
	DELETE FROM UserLoginDetail
	WHERE UserTableID = @Id
END

CREATE TRIGGER tr_tblAdminTable_forUpdate
ON AdminDetailsTable
FOR UPDATE
AS
BEGIN
	DECLARE @Id int
	DECLARE @email varchar(50), @mobile int, @password varchar(100)

	Select @Id = AdminID from inserted

	Select @email = Email, @mobile = Mobile, @password = Password
	From inserted
	where AdminID = @Id

	UPDATE UserLoginDetail
	SET UserLoginEmail = @email, UserLoginMobile=@mobile, UserLoginPassword = @password
	WHERE UserTableID = @Id
END


CREATE TABLE RideDetailsTable (
	RideID int IDENTITY(1,1) PRIMARY KEY,
	UserID int NOT NULL,
	DriverID int NOT NULL,

	PickupAddress varchar(500),
	DropAddress varchar(500),
	PickupDate date,
	PickupTime time,

	Distance varchar(50),
	Duration varchar(20),
	RideStatus int,
	
	PaymentStatus bit DEFAULT 0,
	PayDriver int DEFAULT 0,
	PaymentMode int DEFAULT 1,

	Amount varchar(10) NOT NULL
)

CREATE TABLE RideStatusTable(
	RideStatusID int primary key,
	RideStatus varchar(10)
)

Insert into RideStatusTable values (1,'PENDING'),(2,'ACCEPTED'),(3,'COMPLETED'),(4,'CANCELLED');

CREATE TABLE PaymentStatusTable(
	PaymentStatusID bit primary key,
	PaymentStatus varchar(10)
)

INSERT INTO PaymentStatusTable values (0,'UNPAID'),(1,'PAID');

CREATE TABLE PaymentModeTable(
	PaymentModeID int primary key,
	PaymentMode varchar(10)
)

INSERT INTO PaymentModeTable values (1,'CASH'),(2,'CARD');

CREATE TABLE TripDetaisTable(
	TripID int PRIMARY KEY,
	TripFrom varchar(100),
	TripTo varchar(100),
	VanPerDay int NOT NULL,
	BusPerDay int NOT NULL,
	VanPerKm int,
	BusPerKm int,
	VanPerHr int,
	BusPerHr int,
	DistanceKm varchar(10),
	DurationHr varchar(10),
	CarPerDay int NOT NULL,
	CarPerKm int NOT NULL,
	CarPerHr int NOT NULL
)

CREATE TABLE TripsTable (
	TripID int IDENTITY(1,1) PRIMARY KEY,
	UserID int NOT NULL,
	DriverID int,

	TripDetailsTableID int NOT NULL,

	PickupDate date,
	PickupTime time,

	RideStatus int,
	
	PaymentStatus bit DEFAULT 0,
	PayDriver int DEFAULT 0,
	PaymentMode int DEFAULT 1,

	Amount varchar(10) NOT NULL
)


CREATE TABLE SellingVehicleDetail (
	SellingID int IDENTITY(1,1) PRIMARY KEY,
	SellingHead varchar(50) NOT NULL,
	MileageKm int NOT NULL,
	Price varchar(15) NOT NULL,
	VehicleDescription text NOT NULL,
	VehicleBrand varchar(50),
	VehicleModel varchar(50),
	VehicleModelYear int,
	Condition varchar(50),
	EngineCapacity varchar(50),
	ContactNumber int,
	UploadedDate datetime
);

CREATE TABLE SellingVehicleImageTable(
	SVITID int IDENTITY(1,1) PRIMARY KEY,
	SellingID int,
	ImageUrl varchar(500)
);


--INSERT trigger for rider 
ALTER TRIGGER tr_tblTripsTable_forInsert
ON TripsTable
FOR INSERT
AS
BEGIN
	Declare @Id int, @userId int, @tripDetailId int
	Select @Id = TripID, @userId = UserID, @tripDetailId = TripDetailsTableID from inserted

	Declare @name varchar(50), @number varchar(50)
	select @name = FirstName+' '+LastName, @number = Mobile
	from RiderDetailsTable where RiderID = @userId

	Declare @tripFrom varchar(50), @tripTo varchar(50)
	select @tripFrom = TripFrom, @tripTo = TripTo from TripDetaisTable where TripID = @tripDetailId

	UPDATE TripsTable
	SET RequestedBy = @name, RequesterContact = @number, TripDetail = @tripFrom+' to '+@tripTo
	where TripID = @Id

END

ALTER TRIGGER tr_tblTripsTable_forUpdate
ON TripsTable
FOR UPDATE
AS
BEGIN
	DECLARE @Id int, @driverId int
	DECLARE @driverName varchar(50), @driverContact varchar(15)

	Select @Id = TripID, @driverId = DriverID from inserted

	select @driverName = FirstName+' '+LastName, @driverContact = Mobile from DriverDetailsTable where DriverID = @driverId

	UPDATE TripsTable
	SET DriverName = @driverName, DriverContact = @driverContact
	where TripID = @Id
END