CREATE OR ALTER PROCEDURE DC_AddUser
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @MiddleName VARCHAR(50),
    @Gender VARCHAR(10),
    @DateOfJoining DATE,
    @DateOfBirth DATE,
	@Email VARCHAR(100),
	@Phone VARCHAR(20),
	@AlternatePhone VARCHAR(20),
    @ImagePath VARCHAR(255),
    @CreatedBy VARCHAR(50),
    @IsActive BIT,
    @PrimaryAddress VARCHAR(100),
    @PrimaryCity VARCHAR(50),
    @PrimaryState VARCHAR(50),
    @PrimaryCountry VARCHAR(50),
    @PrimaryZipCode VARCHAR(20),
    @SecondaryAddress VARCHAR(100) = NULL,
    @SecondaryCity VARCHAR(50) = NULL,
    @SecondaryState VARCHAR(50) = NULL,
    @SecondaryCountry VARCHAR(50) = NULL,
    @SecondaryZipCode VARCHAR(20) = NULL
AS
BEGIN
    DECLARE @UserId INT;
    DECLARE @PrimaryId INT;
    DECLARE @SecondaryId INT;

	BEGIN TRY
        BEGIN TRANSACTION;
			 INSERT INTO DC_Users (
				FirstName, LastName, MiddleName, Gender, DateOfJoining, DateOfBirth, 
				Email, Phone, AlternatePhone, 
				ImagePath, CreatedBy, IsActive
				)
			 VALUES (
				@FirstName, @LastName, @MiddleName, @Gender, @DateOfJoining, @DateOfBirth, 
				ENCRYPTBYPASSPHRASE('YourSecretKey', @Email), 
				ENCRYPTBYPASSPHRASE('YourSecretKey', @Phone),
				ENCRYPTBYPASSPHRASE('YourSecretKey', @AlternatePhone),
				@ImagePath, @CreatedBy, @IsActive
				);


			 SET @UserId = SCOPE_IDENTITY();


			 SELECT @PrimaryId = AddressTypeID FROM DC_AddressTypes WHERE AddressTypeName = 'Primary';
			 SELECT @SecondaryId = AddressTypeID FROM DC_AddressTypes WHERE AddressTypeName = 'Secondary';


			 INSERT INTO DC_UserAddresses (Address, City, State, Country, ZipCode, AddressTypeId, UserId)
			 VALUES (@PrimaryAddress, @PrimaryCity, @PrimaryState, @PrimaryCountry, @PrimaryZipCode, @PrimaryId, @UserId);

			 IF @SecondaryAddress IS NOT NULL
			 BEGIN
			     INSERT INTO DC_UserAddresses (Address, City, State, Country, ZipCode, AddressTypeId, UserId)
			     VALUES (@SecondaryAddress, @SecondaryCity, @SecondaryState, @SecondaryCountry, @SecondaryZipCode, @SecondaryId, @UserId);
			 END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END

	EXEC DC_AddUser
		@FirstName = 'John',
		@LastName = 'Doe',
		@MiddleName = 'Michael',
		@Gender = 'Male',
		@DateOfJoining = '2024-07-22',
		@DateOfBirth = '1990-01-01',
		@Email = 'john.doe@example.com',
		@Phone = '1234567890',
		@AlternatePhone = NULL,
		@ImagePath = '/images/john_doe.jpg',
		@CreatedBy = 'Admin',
		@IsActive = 1,
		@PrimaryAddress = '123 Main St',
		@PrimaryCity = 'Anytown',
		@PrimaryState = 'State',
		@PrimaryCountry = 'Country',
		@PrimaryZipCode = '12345',
		@SecondaryAddress = '456 Oak Ave',
		@SecondaryCity = 'OtherTown',
		@SecondaryState = 'State',
		@SecondaryCountry = 'Country',
		@SecondaryZipCode = '67890'

		SELECT * FROM DC_Users
		SELECT * FROM DC_AddressTypes
		SELECT * FROM DC_UserAddresses


		DROP TABLE DC_Users
		DROP TABLE DC_AddressTypes
		DROP TABLE DC_UserAddresses