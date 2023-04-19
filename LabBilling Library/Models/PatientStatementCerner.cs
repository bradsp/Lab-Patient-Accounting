using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dbo.pat_statements_cerner")]
    public sealed class PatientStatementCerner : IBaseEntity
    {
        [Column("statement_type")]
        public string StatementType { get; set; }
        [Column("statement_type_id")]
        public string StatementTypeId { get; set; }
        [Column("account")]
        public string Account { get; set; }
        [Column("statement_text")]
        public string StatementText { get; set; }
        [Column("batch_id")]
        public string BatchId { get; set; }

        [Ignore]
        public DateTime mod_date { get; set; }
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
