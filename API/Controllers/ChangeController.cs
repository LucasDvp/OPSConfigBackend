using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeController : ControllerBase
    {
        /// <summary>
        /// TODO: do we have to seperate into several API like which category changed?
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        [HttpPost("{repoId}/{docsetName}")]
        public JsonResult Post(string repoId, string docsetName, [FromBody] object configs)
        {
            // TODO: configs is a list of key-value pairs, need to check validation
            return new JsonResult(new { updated = true });
        }

        /// <summary>
        /// TODO: delete the specific config
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        [HttpDelete("{repoId}/{docsetName}/{configName}")]
        public JsonResult Delete(string repoId, string docsetName, string configName)
        {
            //TODO: delete the specific config
            return new JsonResult(new { deleted = true });
        }
    }
}
