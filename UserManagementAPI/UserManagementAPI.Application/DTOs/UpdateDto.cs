using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Application.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle name is required")]
        [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
        public string MiddleName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of joining is required")]
        public DateTime? DateOfJoining { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string Phone { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Alternate phone number must be 10 digits")]
        public string AlternatePhone { get; set; }

        public string ImagePath { get; set; }

        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "At least one address is required")]
        public List<AddressDto> Addresses { get; set; }
    }
}
