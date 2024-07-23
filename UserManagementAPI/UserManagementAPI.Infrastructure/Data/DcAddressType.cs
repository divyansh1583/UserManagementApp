using System;
using System.Collections.Generic;

namespace UserManagementAPI.Infrastructure.Data;

public partial class DcAddressType
{
    public int AddressTypeId { get; set; }

    public string AddressTypeName { get; set; } = null!;

    public virtual ICollection<DcUserAddress> DcUserAddresses { get; set; } = new List<DcUserAddress>();
}
