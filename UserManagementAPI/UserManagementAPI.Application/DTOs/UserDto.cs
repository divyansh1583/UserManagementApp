using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Application.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public DateOnly? DateOfJoining { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AlternatePhone { get; set; }
        public AddressDto PrimaryAddress { get; set; }
        public AddressDto SecondaryAddress { get; set; }
    }
}
