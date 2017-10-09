using System;

namespace EXP.Entity
{
    public class Email
    {
        public int EmailID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Destination { get; set; }
        public bool Sent { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}