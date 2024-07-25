using System;
using System.Collections.Generic;

namespace UserManagementAPI.Domain.Entities;

public partial class DcCity
{
    public int CityId { get; set; }

    public int StateId { get; set; }

    public string CityName { get; set; } = null!;

    public virtual ICollection<DcUserAddress> DcUserAddresses { get; set; } = new List<DcUserAddress>();

    public virtual DcState State { get; set; } = null!;
}
