using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public class KeyCard
    {
        public KeyCard()
        {
           Lot = new Lot(); 
           Building = new Building();
           Address = new Address();
           zSate = new zState();
           
        }
        
        public int KeyCardID { get; set; }
        public int LotID { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public System.DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string KeyCardName { get; set; }
        public decimal? Rate { get; set; }

        public Building Building { get; set; }
        public Address Address { get; set; }
        public zState zSate { get; set; }

        public Lot Lot { get; set; }
    }
}
