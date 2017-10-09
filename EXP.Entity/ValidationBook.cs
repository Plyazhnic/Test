using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
   public class ValidationBook
    {
        public ValidationBook()
        {
           Lot = new Lot(); 
           Building = new Building();
           Address = new Address();
           zSate = new zState();
           
        }
        
        public int BookID { get; set; }
        public int LotID { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public System.DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string BookName { get; set; }
        public int? TicketCount { get; set; }
        public decimal? Rate { get; set; }

        public Building Building { get; set; }
        public Address Address { get; set; }
        public zState zSate { get; set; }

        public Lot Lot { get; set; }

        
    }
}
