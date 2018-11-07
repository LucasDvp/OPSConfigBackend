using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.OpenPublishing.ConfigService.DataAccessors
{
    public interface IConfigAccessor
    {
        Task<ConfigEntity> GetConfigByDocsetInfo(string repoId, string docsetName);
        Task<bool> CreateConfig(string repoId, string docsetName);
        Task<bool> UpdateConfig(string repoId, string docsetName, JObject config);
    }
}
