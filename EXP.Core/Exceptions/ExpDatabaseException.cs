using System;
using System.Runtime.Serialization;

namespace EXP.Core.Exceptions
{
    public class ExpDatabaseException : Exception
    {
        public ExpDatabaseException()
        {
        }

        public ExpDatabaseException(string message) : base(message)
        {
        }

        public ExpDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExpDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}