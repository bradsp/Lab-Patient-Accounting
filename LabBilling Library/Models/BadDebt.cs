using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [TableName("bad_debt")]
    [PrimaryKey("rowguid", AutoIncrement = false)]
    public sealed class BadDebt : IBaseEntity
    {
        public Guid rowguid { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("debtor_last_name")]
        public string DebtorLastName { get; set; }

        [Column("debtor_first_name")]
        public string DebtorFirstName { get; set; }

        [Column("st_addr_1")]
        public string StreetAddress1 { get; set; }

        [Column("st_addr_2")]
        public string StreetAddress2 { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("state_zip")]
        public string StateZip { get; set; }

        [Column("spouse")]
        public string Spouse { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("soc_security")]
        public string SocialSecurityNo { get; set; }

        [Column("license_number")]
        public string LicenseNumber { get; set; }

        [Column("employment")]
        public string Employment { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("account_no")]
        public string AccountNo { get; set; }

        [Column("patient_name")]
        public string PatientName { get; set; }

        [Column("remarks2")]
        public string Remarks2 { get; set; }

        [Column("misc")]
        public string Misc { get; set; }

        [Column("service_date")]
        public DateTime? ServiceDate { get; set; }

        [Column("payment_date")]
        public DateTime? PaymentDate { get; set; }

        [Column("balance")]
        public double Balance { get; set; }

        [Column("date_entered")]
        public DateTime? DateEntered { get; set; }

        [Column("date_sent")]
        public DateTime? DateSent { get; set; }
        [Column("mod_date")]
        [ResultColumn]
        public DateTime UpdatedDate { get; set; }
        [Column("mod_user")]
        [ResultColumn]
        public string UpdatedUser { get; set; }
        [Column("mod_host")]
        [ResultColumn]
        public string UpdatedHost { get; set; }
        [Column("mod_prg")]
        [ResultColumn]
        public string UpdatedApp { get; set; }

        [Ignore]
        public string State { get; set; }
        [Ignore]
        public string Zip { get; set; }

        [Ignore]
        public Pat Pat { get; set; } = new Pat();
    }
}
