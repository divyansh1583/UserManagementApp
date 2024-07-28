using System;
using System.Collections.Generic;

namespace UserManagementAPI.Domain.Entities;

public partial class DcCountry
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public virtual ICollection<DcState> DcStates { get; set; } = new List<DcState>();

    public virtual ICollection<DcUserAddress> DcUserAddresses { get; set; } = new List<DcUserAddress>();
}
