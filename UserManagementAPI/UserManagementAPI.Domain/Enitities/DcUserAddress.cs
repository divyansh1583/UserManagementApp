using System;
using System.Collections.Generic;

namespace UserManagementAPI.Domain.Entities;

public partial class DcUserAddress
{
    public int AddressId { get; set; }

    public string? Address { get; set; }

    public int CityId { get; set; }

    public int StateId { get; set; }

    public int CountryId { get; set; }

    public string? ZipCode { get; set; }

    public int AddressTypeId { get; set; }

    public int UserId { get; set; }

    public virtual DcUserAddressType AddressType { get; set; } = null!;

    public virtual DcCity City { get; set; } = null!;

    public virtual DcCountry Country { get; set; } = null!;

    public virtual DcState State { get; set; } = null!;

    public virtual DcUser User { get; set; } = null!;
}
