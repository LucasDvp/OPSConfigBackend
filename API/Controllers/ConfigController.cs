using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenPublishing.ConfigService.DataAccessors;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Microsoft.OpenPublishing.ConfigService
{
    [Route("api/repos/{repoId}/docsets/{docsetName}/config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigAccessor _configAccessor;

        public ConfigController(IConfigAccessor configAccessor)
        {
            _configAccessor = configAccessor;
        }
        /// <summary>
        /// Return UI data contract/Raw data depends on contentType=json/file
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> Get(string repoId, string docsetName)
        {
            var config = await _configAccessor.GetConfigByDocsetInfo(repoId, docsetName);

            return new JsonResult(config?.Config);
        }

        /// <summary>
        /// Create a new config by default(initialize)
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        [HttpPost]
        public async Task<JsonResult> Post(string repoId, string docsetName)
        {
            return new JsonResult(new { created = await _configAccessor.CreateConfig(repoId, docsetName) });
        }

        /// <summary>
        /// Update value
        /// </summary>
        /// <param name="values"></param>
        [HttpPut]
        public async Task<JsonResult> Put(string repoId, string docsetName, [FromBody] JObject values)
        {
            return new JsonResult(new { updated = await _configAccessor.UpdateConfig(repoId, docsetName, values)});
        }
    }
}
