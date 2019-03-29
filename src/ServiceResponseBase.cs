using System.Collections.Generic;

namespace Epinova.Infrastructure
{
    public abstract class ServiceResponseBase
    {
        protected ServiceResponseBase()
        {
            ErrorList = new List<string>();
        }

        public List<string> ErrorList { get; }
    }
}