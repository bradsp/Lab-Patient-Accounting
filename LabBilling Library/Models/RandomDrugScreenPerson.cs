using PetaPoco;
using System;

namespace LabBilling.Core.Models;
[TableName("dbo.rds")]
[PrimaryKey("uri", AutoIncrement = true)]
public class RandomDrugScreenPerson : IBaseEntity
{
    [Column("uri")]
    public int Id { get; set; }
    [Column("deleted")]
    public bool IsDeleted { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("cli_mnem")]
    public string ClientMnemonic { get; set; }
    [Column("shift")]
    public string Shift { get; set; }
    [Column("test_date")]
    public DateTime? TestDate { get; set; }
    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }
    [Ignore]
    public Client Client { get; set; }
}
