-- User table with password reset columns
CREATE TABLE DC_User (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50),
    MiddleName VARCHAR(50) NOT NULL,
    Gender VARCHAR(10),
    DateOfJoining DATE,
    DateOfBirth DATE,
    Email VARBINARY(MAX) NOT NULL,
    Phone VARBINARY(MAX) NOT NULL,
    AlternatePhone VARBINARY(MAX),
    ImagePath VARCHAR(255),
    Password VARCHAR(255) NOT NULL,
    PasswordResetToken VARCHAR(100),
    PasswordResetTokenExpiry DATETIME,
    CreatedBy VARCHAR(50),
    UpdatedBy VARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    IsActive BIT DEFAULT 0, 
    IsDeleted BIT DEFAULT 0
);

-- Address Type table
CREATE TABLE DC_UserAddressType (
    AddressTypeId INT PRIMARY KEY IDENTITY(1,1),
    AddressTypeName VARCHAR(20) NOT NULL UNIQUE
);

-- User Address table
CREATE TABLE DC_UserAddress (
    AddressId INT IDENTITY(1,1) PRIMARY KEY,
    Address VARCHAR(100),
    CityId INT NOT NULL,
    StateId INT NOT NULL,
    CountryId INT NOT NULL,
    ZipCode VARCHAR(20),
    AddressTypeId INT NOT NULL,
    UserId INT NOT NULL,
    FOREIGN KEY (CityId) REFERENCES DC_City(CityId),
    FOREIGN KEY (StateId) REFERENCES DC_State(StateId),
    FOREIGN KEY (CountryId) REFERENCES DC_Country(CountryId),
    FOREIGN KEY (AddressTypeId) REFERENCES DC_UserAddressType(AddressTypeId),
    FOREIGN KEY (UserId) REFERENCES DC_User(UserId)
);

-- Insert address types
INSERT INTO DC_UserAddressType (AddressTypeName) VALUES ('Primary'), ('Secondary');

SELECT * FROM DC_User
SELECT * FROM DC_UserAddress
SELECT * FROM DC_UserAddressType

DROP TABLE DC_User
DROP TABLE DC_UserAddress
DROP TABLE DC_UserAddressType