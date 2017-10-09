using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public class SearchBuilding
    {
        public string BuildingName { get; set; }
        public string OwnersFirstName { get; set; }
        public string OwnersLastName { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public string ZipCode { get; set; }

        public override string ToString()
        {
            return string.Format("BuildingName: {0}, OwnersFirstName: {1}, OwnersLastName: {2}, ManagerFirstName: {3}, ManagerLastName: {4}, City: {5}, StateID: {6}, ZipCode: {7}"
                , BuildingName, OwnersFirstName, OwnersLastName, ManagerFirstName, ManagerLastName, City, StateID, ZipCode);
        }
    }
}
