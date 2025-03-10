use test

CREATE TABLE DC_Users(

UserId INT IDENTITY(1,1) PRIMARY KEY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50),
MiddleName VARCHAR(50)NOT NULL,
Gender VARCHAR(10),
DateOfJoining DATE,
DateOfBirth DATE,
Email VARBINARY(MAX) NOT NULL,
Phone VARBINARY(MAX) NOT NULL,
AlternatePhone VARBINARY(MAX),
ImagePath VARCHAR(255),
Password VARCHAR(255) NOT NULL,

CreatedBy VARCHAR(50),
UpdatedBy VARCHAR(50),
UpdatedDate DATETIME,
IsActive BIT DEFAULT 0, 
IsDeleted BIT DEFAULT 0,
);

CREATE TABLE DC_AddressTypes (
    AddressTypeID INT PRIMARY KEY IDENTITY(1,1),
    AddressTypeName VARCHAR(20) NOT NULL UNIQUE
);

INSERT INTO DC_AddressTypes (AddressTypeName) VALUES ('Primary'), ('Secondary');

CREATE TABLE DC_UserAddresses(
AddressId INT IDENTITY(1,1) PRIMARY KEY,
Address VARCHAR(100),
City VARCHAR(50),
State VARCHAR(50),
Country VARCHAR(50),
ZipCode VARCHAR(20),
AddressTypeId INT NOT NULL FOREIGN KEY REFERENCES DC_AddressTypes(AddressTypeID),
UserId INT NOT NULL FOREIGN KEY REFERENCES DC_Users(UserId),
);