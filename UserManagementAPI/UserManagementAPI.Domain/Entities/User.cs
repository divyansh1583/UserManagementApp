using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AlternatePhone { get; set; }
        public string Address1 { get; set; }
        public string City1 { get; set; }
        public string State1 { get; set; }
        public string Country1 { get; set; }
        public string ZipCode1 { get; set; }
        public string? Address2 { get; set; }
        public string? City2 { get; set; }
        public string? State2 { get; set; }
        public string? Country2 { get; set; }
        public string? ZipCode2 { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
