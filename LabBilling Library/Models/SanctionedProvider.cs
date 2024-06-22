using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("dictionary.phy_sanc")]
[PrimaryKey("uri", AutoIncrement = true)]
public class SanctionedProvider : IBaseEntity
{
    [Column("uri")]
    public int Id { get; set; }
    [Column("lastname")]
    public string LastName { get; set; }
    [Column("firstname")]
    public string FirstName { get; set; }
    [Column("midname")]
    public string MiddleName { get; set; }
    [Column("busname")]
    public string BusinessName { get; set; }
    [Column("general")]
    public string General { get; set; }
    [Column("specialty")]
    public string Specialty { get; set; }
    [Column("upin")]
    public string Upin { get; set; }
    [Column("npi")]
    public string NPI { get; set; }
    [Column("dob")]
    public string DateOfBirth { get; set; }
    [Column("address")]
    public string Address { get; set; }
    [Column("city")]
    public string City { get; set; }
    [Column("state")]
    public string State { get; set; }
    [Column("sanctype")]
    public string SanctionType { get; set; }
    [Column("sancdate")]
    public string SanctionDate { get; set; }
    [Column("reindate")]
    public string ReinstateDate { get; set; }
    [Column("waiverdate")]
    public string WaiverDate { get; set; }
    [Column("wvrstate")]
    public string WaiverState { get; set; }

    [Ignore]
    public DateTime UpdatedDate { get; set; }
    [Ignore]
    public string UpdatedUser { get; set; }
    [Ignore]
    public string UpdatedApp { get; set; }
    [Ignore]
    public string UpdatedHost { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }
}
