// user.dto.ts
export interface UserDto {
    userId?: number;
    firstName: string;
    lastName: string;
    middleName?: string;
    gender: string;
    dateOfJoining: Date;
    dateOfBirth: Date;
    email: string;
    phone: string;
    alternatePhone?: string;
    imagePath?: string;
    password: string;
    isActive: boolean;
    addresses?: AddressDto[];
  }
  
  export interface AddressDto {
    address: string;
    cityId: number;
    stateId: number;
    countryId: number;
    zipCode: string;
    addressTypeId: number;
  }
  export interface UpdateUserDto {
    UserId: number; // Required to identify the user to update
    FirstName?: string;
    LastName?: string;
    MiddleName?: string;
    Gender?: string;
    DateOfJoining?: Date;
    DateOfBirth?: Date;
    Phone?: string;
    AlternatePhone?: string;
    ImagePath?: string;
    IsActive?: boolean;
    Addresses?: AddressDto[];
  }