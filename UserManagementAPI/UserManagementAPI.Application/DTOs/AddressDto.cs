using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Application.DTOs
{
    public class AddressDto
    {
        public string Address { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string ZipCode { get; set; }
        public int AddressTypeId { get; set; }
    }
}
