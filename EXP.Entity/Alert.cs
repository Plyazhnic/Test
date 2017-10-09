using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class Alert
    {
        public int AlertID { get; set; }
        public int AlertTypeID { get; set; }
        public int UserProfileID { get; set; }
        public int EntityTypeID { get; set; }
        public int EntityID { get; set; }
        /// <summary>
        /// Not exists in database. Need to be filled in DataAccess
        /// </summary>
        public string AlertText { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual AlertType AlertType { get; set; }

        public override string ToString()
        {
            return string.Format("AlertID: {0}, AlertTypeID: {1}, UserProfileID: {2}, EntityTypeID: {3}, EntityID: {4}, isActive: {5}, UpdatedDate: {6}, CreatedDate: {7}",
                AlertID, AlertTypeID, UserProfileID, EntityTypeID, EntityID, isActive, UpdatedDate, CreatedDate);
        }
    }
}
