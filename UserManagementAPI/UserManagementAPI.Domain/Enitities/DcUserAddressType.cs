using System;
using System.Collections.Generic;

namespace UserManagementAPI.Domain.Entities;

public partial class DcUserAddressType
{
    public int AddressTypeId { get; set; }

    public string AddressTypeName { get; set; } = null!;

    public virtual ICollection<DcUserAddress> DcUserAddresses { get; set; } = new List<DcUserAddress>();
}
