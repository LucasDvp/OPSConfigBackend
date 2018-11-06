using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvisionController : ControllerBase
    {
        [HttpPost("{repoId}/{docsetName}")]
        public JsonResult Post(string repoId, string docsetName)
        {
            Console.WriteLine(repoId, docsetName);
            // TODO: Create new docfx.yaml
            return new JsonResult(new { created = true });
        }
    }
}
