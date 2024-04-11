using LabBilling.Core.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.ViewModel
{
    public class Charge
    {
        public bool IsCredited { get; set; }
        public int ChrgId { get; set; }
        public string AccountNo { get; set; }
        public string Status { get; set; }
        public DateTime? ServiceDate { get; set; }
        public DateTime? HistoryDate { get; set; }
        public string CDMCode { get; set; }
        public int Quantity { get; set; }
        public double NetAmount { get; set; }
        public string Comment { get; set; }
        public string Invoice { get; set; }
        public string FinancialType { get; set; }
        public string LISReqNo { get; set; }
        public DateTime? PostingDate { get; set; }
        public string ClientMnem { get; set; }
        public string FinCode { get; set; }
        public string PerformingSite { get; set; }
        public string BillMethod { get; set; }
        public string OrderingSite { get; set; }
        public string Location { get; set; }
        public string ResponsibleProvider { get; set; }
        public string OrderMnem { get; set; }
        public string Facility { get; set; }
        public string ReferenceReq { get; set; }
        public double RetailAmount { get; set; }
        public double HospAmount { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public string UpdatedApp { get; set; }
        public string UpdatedHost { get; set; }

        public Guid rowguid { get; set; }

        public string CdmDescription { get; set; }

        public string RevenueCode { get; set; }
        public string Cpt4 { get; set; }
        public string Modifier { get; set; }
        public string Modifer2 { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string OrderCode { get; set; }
        public bool PointerSet { get; set; }

        public string CptDescription { get; set; }

        public int uri { get; set; }

        public RevenueCode RevenueCodeDetail { get; set; }
        public ChrgDiagnosisPointer DiagnosisPointer { get; set; }
        public string DiagCodePointer { get; set; }

    }
}
