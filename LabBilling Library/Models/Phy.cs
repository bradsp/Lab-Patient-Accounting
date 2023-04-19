using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("phy")]
    [PrimaryKey("uri", AutoIncrement = true)]
    public sealed class Phy : IBaseEntity
    {
        public Guid rowguid { get; set; }
        [Column("deleted")]
        public bool IsDeleted { get; set; }
        [Column("upin")]
        public string Upin { get; set; }
        [Column("ub92_upin")]
        public string Ub92Upin { get; set; }
        [Column("tnh_num")]
        public string NpiId { get; set; }
        [Column("billing_npi")]
        public string BillingNpi { get; set; }
        [Column("pc_code")]
        public string PathologistCode { get; set; }
        [Column("cl_mnem")]
        public string ClientMnem { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("mid_init")]
        public string MiddleInitial { get; set; }
        [Column("group1")]
        public string ProviderGroup { get; set; }
        [Column("addr_1")]
        public string Address1 { get; set; }
        [Column("addr_2")]
        public string Address2 { get; set; }
        [Column("city")]
        public string City { get; set; }
        [Column("state")]
        public string State { get; set; }
        [Column("zip")]
        public string ZipCode { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("reserved")]
        public string reserved { get; set; }
        [Column("num_labels")]
        public int LabelPrintCount { get; set; }
        [Column("mod_date")]
        public DateTime mod_date { get; set; }
        [Column("mod_user")]
        public string mod_user { get; set; }
        [Column("mod_prg")]
        public string mod_prg { get; set; }
        [Column("uri")]
        public double uri { get; set; }
        [Column("mt_mnem")]
        public string LISMnem { get; set; }
        [Column("credentials")]
        public string Credentials { get; set; }
        [Column("ov_code")]
        public string OvCode { get; set; }
        [Column("docnbr")]
        public string DoctorNumber { get; set; }
        [Ignore]
        [Column("mod_host")]
        public string mod_host { get; set; }

        [Ignore]
        public Pth Pathologist { get; set; } = new Pth();

        [Ignore]
        public string FullName 
        { 
            get
            {
                return $"{LastName}, {FirstName} {MiddleInitial}";
            }
        }

        public override string ToString()
        {
            return $"{FullName} - {NpiId}";
        }

    }
}
