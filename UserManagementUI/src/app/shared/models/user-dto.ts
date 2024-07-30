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
  
  interface AddressDto {
    address: string;
    cityId: number;
    stateId: number;
    countryId: number;
    zipCode: string;
    addressTypeId: number;
  }