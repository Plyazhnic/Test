using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Services.Models
{
    [Serializable()]
    public class RegistrationData
    {
        public RegistrationData()
        {
            this.Step = 0;
            this.ProfileTypeID = 2;
        }
       
        public static RegistrationData Current
        {
            get
            {
                RegistrationData data =
                  (RegistrationData)System.Web.HttpContext.Current.Session["RegistrationData"];
                if (data == null)
                {
                    data = new RegistrationData();
                    HttpContext.Current.Session["RegistrationData"] = data;
                }
                return data;
            }
        }

        public int RegistrationDataID { get; set; }
        public int ProfileTypeID { get; set; }
        public byte Step { get; set; }
        public int? LotID { get; set; }
        public int? BuildingToLotID { get; set; }
        public int? TenantID { get; set; }
        public string ServerPath { get; set; }
        public string RootUrl { get; set; }

        public virtual EmployeeModel Employee { get; set; }
        public virtual TenantModel Tenant { get; set; }
        public virtual BuildingOwnerModel Owner { get; set; }

        public void Save()
        {
            HttpContext.Current.Session["RegistrationData"] = this;
        }
    }
}
