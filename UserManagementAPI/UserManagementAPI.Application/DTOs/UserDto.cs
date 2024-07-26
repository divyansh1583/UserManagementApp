using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Application.DTOs
{

    public class UserDto
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AlternatePhone { get; set; }
        public string ImagePath { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
