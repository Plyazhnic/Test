using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public class TableList
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string SearchString { get; set; }
        public int? BuildingID { get; set; }
        public int MakeID { get; set; }
        public int LotID { get; set; }
        public int StallID { get; set; }
        public int CompanyID { get; set; }

        public override string ToString()
        {
            return string.Format("Start: {0}, End: {1}, SearchString: {2}, BuildingID: {3}, MakeID: {4}, LotID: {5}, StallID: {6}, CompanyID: {7}",
                Start, End, SearchString, BuildingID, MakeID, LotID, StallID, CompanyID);
        }
    }
    
}
