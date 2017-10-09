using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public partial class Lease
    {
        public int LeaseID { get; set; }
        public int CompanyID { get; set; }
        public int BuildingID { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
        public decimal LateFee { get; set; }
        public bool TaxExempt { get; set; }
        public bool MarketRate { get; set; }
        public string Description { get; set; }
        public string DocumentName { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public Company Company { get; set; }
        public Building Building { get; set; }

        public override string ToString()
        {
            return string.Format("LeaseID: {0}, CompanyID: {1}, BuildingID: {2}, TermStart: {3}, TermEnd: {4}, LateFee: {5}, TaxExempt :{6}, MarketRate :{7}, Description :{8}, DocumentName :{9}, UpdatedDate :{10}, CreatedDate :{11}",
                LeaseID, CompanyID, BuildingID, TermStart, TermEnd, LateFee, TaxExempt, MarketRate, Description, DocumentName, UpdatedDate, CreatedDate);
        }
    }
}
