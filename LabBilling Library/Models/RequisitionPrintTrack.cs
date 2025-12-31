using PetaPoco;
using System;

namespace LabBilling.Core.Models;

/// <summary>
/// Model for tracking requisition form printing for audit purposes
/// Maps to the rpt_track database table
/// </summary>
[TableName("rpt_track")]
[PrimaryKey("uri", AutoIncrement = true)]
public sealed class RequisitionPrintTrack : IBaseEntity
{
    /// <summary>
    /// Unique record identifier (identity column)
    /// </summary>
    [Column("uri")]
    public int Uri { get; set; }

    /// <summary>
    /// Date and time the record was created
    /// </summary>
    [Column("mod_date")]
    public DateTime ModDate { get; set; }

    /// <summary>
    /// User who created the record
    /// </summary>
    [Column("mod_user")]
    public string ModUser { get; set; }

    /// <summary>
    /// Host machine name
    /// </summary>
    [Column("mod_host")]
    public string ModHost { get; set; }

    /// <summary>
    /// Application name
    /// </summary>
    [Column("mod_app")]
    public string ModApp { get; set; }

    /// <summary>
    /// Form type printed (CLIREQ, PTHREQ, CYTREQ, etc.)
    /// </summary>
    [Column("form_printed")]
    public string FormPrinted { get; set; }

    /// <summary>
    /// Client name
    /// </summary>
    [Column("cli_nme")]
    public string ClientName { get; set; }

    /// <summary>
    /// Quantity of forms printed
    /// </summary>
    [Column("qty_printed")]
    public int QuantityPrinted { get; set; }

    /// <summary>
    /// Printer name used
    /// </summary>
    [Column("printer_name")]
    public string PrinterName { get; set; }

    [Ignore]
    public Guid rowguid { get; set; }

    // IBaseEntity implementation - map to existing columns
    [Ignore]
    public DateTime UpdatedDate
    {
        get => ModDate;
        set => ModDate = value;
    }

    [Ignore]
    public string UpdatedUser
    {
        get => ModUser;
        set => ModUser = value;
    }

    [Ignore]
    public string UpdatedApp
    {
        get => ModApp;
        set => ModApp = value;
    }

    [Ignore]
    public string UpdatedHost
    {
        get => ModHost;
        set => ModHost = value;
    }
}
