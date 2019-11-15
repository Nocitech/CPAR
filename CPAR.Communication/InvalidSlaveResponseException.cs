using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CPAR.Communication
{
   [Serializable]
   public class InvalidSlaveResponseException : Exception
   {
      public InvalidSlaveResponseException(String message) : base(message) { }
      public InvalidSlaveResponseException(String message, Exception inner) : base(message, inner) { }
      protected InvalidSlaveResponseException(SerializationInfo info, StreamingContext context)
         : base(info, context) 
      { }
   }
}
