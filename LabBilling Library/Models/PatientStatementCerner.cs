using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dbo.pat_statements")]
    public sealed class PatientStatementCerner : IBaseEntity
    {
        public int StatementId { get; set; }
        [Column("account")]
        public string Account { get; set; }
        [Column("batch_id")]
        public string BatchId { get; set; }
        [Column("statement_type")]
        public string StatementType { get; set; }
        [Column("statement_type_id")]
        public string StatementTypeId { get; set; }
        [Column("statement_text")]
        public string StatementText { get; set; }

        public DateTime UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public string UpdatedApp { get; set; }
        public string UpdatedHost { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
