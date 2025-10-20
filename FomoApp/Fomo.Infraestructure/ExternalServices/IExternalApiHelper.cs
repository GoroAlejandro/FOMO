using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Infraestructure.ExternalServices
{
    public interface IExternalApiHelper
    {
        Task<T> GetAsync<T>(string endpoint);
    }
}
