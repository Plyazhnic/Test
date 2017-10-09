using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class AlertType
    {
        public AlertType()
        {
            this.Alert = new HashSet<Alert>();
        }

        public int AlertTypeID { get; set; }
        public string AlertType1 { get; set; }
        public int? AlertPeriodValue { get; set; }
        public string AlertTypeDescription { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual ICollection<Alert> Alert { get; set; }
    }
}
