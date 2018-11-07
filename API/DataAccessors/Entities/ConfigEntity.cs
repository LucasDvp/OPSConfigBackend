using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.OpenPublishing.ConfigService.DataAccessors
{
    public class ConfigEntity : Resource
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("repoId")]
        public string RepoId { get; set; }

        [JsonProperty("docsetName")]
        public string DocsetName { get; set; }

        [JsonProperty("config")]
        public JObject Config { get; set; }
    }
}
