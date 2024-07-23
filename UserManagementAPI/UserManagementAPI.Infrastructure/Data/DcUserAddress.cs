using System;
using System.Collections.Generic;

namespace UserManagementAPI.Infrastructure.Data;

public partial class DcUserAddress
{
    public int AddressId { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? ZipCode { get; set; }

    public int AddressTypeId { get; set; }

    public int UserId { get; set; }

    public virtual DcAddressType AddressType { get; set; } = null!;

    public virtual DcUser User { get; set; } = null!;
}
