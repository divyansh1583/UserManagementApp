using System;
using System.Collections.Generic;

namespace UserManagementAPI.Domain.Entities;

public partial class DcState
{
    public int StateId { get; set; }

    public int CountryId { get; set; }

    public string StateName { get; set; } = null!;

    public virtual DcCountry Country { get; set; } = null!;

    public virtual ICollection<DcCity> DcCities { get; set; } = new List<DcCity>();

    public virtual ICollection<DcUserAddress> DcUserAddresses { get; set; } = new List<DcUserAddress>();
}
