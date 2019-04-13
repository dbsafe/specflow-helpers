using Newtonsoft.Json.Linq;
using System;

namespace Specflow.Steps.Object.Collections
{
    public class CollectionException : Exception
    {
        public JToken JToken { get; }

        public CollectionException(string message, JToken jToken)
            : this(message)
        {
            JToken = jToken;
        }

        public CollectionException(string message)
            : base(message)
        {
        }
    }
}
