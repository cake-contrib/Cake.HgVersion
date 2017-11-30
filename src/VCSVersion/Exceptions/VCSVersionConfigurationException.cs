using System;
using System.Collections.Generic;
using System.Text;

namespace VCSVersion.Exceptions
{
    [Serializable]
    public class VCSVersionConfigurationException : Exception
    {
        public VCSVersionConfigurationException() { }
        public VCSVersionConfigurationException(string message) : base(message) { }
        public VCSVersionConfigurationException(string message, Exception inner) : base(message, inner) { }
        protected VCSVersionConfigurationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
