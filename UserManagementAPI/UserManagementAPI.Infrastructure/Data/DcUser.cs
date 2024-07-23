using System;
using System.Collections.Generic;

namespace UserManagementAPI.Infrastructure.Data;

public partial class DcUser
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string MiddleName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? DateOfJoining { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public byte[] Email { get; set; } = null!;

    public byte[] Phone { get; set; } = null!;

    public byte[]? AlternatePhone { get; set; }

    public string? ImagePath { get; set; }
    public string Password { get; set; } = null!;
    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<DcUserAddress> DcUserAddresses { get; set; } = new List<DcUserAddress>();
}
