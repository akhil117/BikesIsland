using System;
using System.Collections.Generic;
using System.Text;

namespace BikesIsland.Integrations.Common
{
    public class OperationError
    {
        public string Details { get; }

        public OperationError(string details) => (Details) = (details);
    }
}
