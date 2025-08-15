using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Utilities
{
    public interface IHelper
    {
        public Task<HttpResponseMessage> SendRequestToExternalApi(Method method, string requestUrl);
    }
}
