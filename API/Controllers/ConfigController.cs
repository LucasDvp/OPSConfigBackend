using System;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/repos/{repoId}/docsets/{docsetName}/configs")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        /// <summary>
        /// Return UI data contract/Raw data depends on contentType=json/file
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string repoId, string docsetName, [FromHeader(Name = "content-type")]string contentType)
        {
            Console.WriteLine(contentType);
            return new JsonResult(new { returnValue = contentType });
        }

        /// <summary>
        /// Create a new config by default(initialize)
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        [HttpPost]
        public IActionResult Post(string repoId, string docsetName)
        {
            return new JsonResult(new { created = true });
        }

        /// <summary>
        /// Update value
        /// </summary>
        /// <param name="values"></param>
        [HttpPut]
        public IActionResult Put([FromBody] object values)
        {
            return new JsonResult(new { updated = true });
        }

        /// <summary>
        /// Delete one config from configs
        /// </summary>
        /// <param name="configName"></param>
        [HttpDelete("{configName}")]
        public IActionResult Delete(string configName)
        {
            return new JsonResult(new { deleted = true });
        }
    }
}
