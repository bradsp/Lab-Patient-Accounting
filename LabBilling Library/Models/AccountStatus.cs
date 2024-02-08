namespace LabBilling.Core.Models
{
    public static class AccountStatus
    {
        public static readonly string New = "NEW";
        public static readonly string ReadyToBill = "RTB";
        public static readonly string Professional = "1500";
        public static readonly string Institutional = "UB";
        public static readonly string ProfSubmitted = "SSI1500";
        public static readonly string InstSubmitted = "SSIUB";
        public static readonly string ClaimSubmitted = "CLAIM";
        public static readonly string Statements = "STMT";
        public static readonly string PaidOut = "PAID_OUT";
        public static readonly string Closed = "CLOSED";
        public static readonly string Hold = "HOLD";
        public static readonly string Client = "CLIENT";
        public static readonly string Collections = "COLL";

        public static bool IsValid(string status)
        {
            if (status == New) return true;
            if (status == ReadyToBill) return true;
            if (status == Professional) return true;
            if (status == Institutional) return true;
            if (status == InstSubmitted) return true;
            if (status == ClaimSubmitted) return true;
            if (status == Statements) return true;
            if (status == ProfSubmitted) return true;
            if (status == PaidOut) return true;
            if (status == Closed) return true;
            if (status == Hold) return true;
            if (status == Client) return true;
            if (status == Collections) return true;

            return false;
        }
    }
}