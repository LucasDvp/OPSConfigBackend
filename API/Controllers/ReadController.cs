using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadController : ControllerBase
    {
        /// <summary>
        /// TODO: UI data contract?
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        /// <returns></returns>
        [HttpGet("ui/{repoId}/{docsetName}")]
        public JsonResult GetUIConfig(string repoId, string docsetName)
        {
            // TODO: return the UI data contract
            return new JsonResult(new ConfigContract());
        }

        /// <summary>
        /// TODO: return raw docfx.yaml
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        /// <returns></returns>
        [HttpGet("build/{repoId}/{docsetName}")]
        public JsonResult GetBuildConfig(string repoId, string docsetName)
        {
            return new JsonResult(new ConfigRaw());
        }
    }
}
