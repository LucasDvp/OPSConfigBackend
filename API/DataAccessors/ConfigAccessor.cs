using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.OpenPublishing.ConfigService.Common;
using Microsoft.OpenPublishing.ConfigService.Library;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.OpenPublishing.ConfigService.DataAccessors
{
    public class ConfigAccessor : CosmosAccessor, IConfigAccessor
    {
        public ConfigAccessor(DocumentClient client, string databaseId, string collectionId) : base(client, databaseId, collectionId)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        /// <returns></returns>
        public async Task<bool> CreateConfig(string repoId, string docsetName)
        {
            Guard.ArgumentNotNullOrEmpty(repoId);
            Guard.ArgumentNotNullOrEmpty(docsetName);

            var newConfig = new ConfigEntity
            {
                RepoId = repoId,
                DocsetName = docsetName,
                Config = JObject.FromObject(new Config())
            };

            return await AddOrUpdate(repoId, newConfig).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        /// <returns></returns>
        public async Task<ConfigEntity> GetConfigByDocsetInfo(string repoId, string docsetName)
        {
            Guard.ArgumentNotNullOrEmpty(repoId);
            Guard.ArgumentNotNullOrEmpty(docsetName);

            var queryString = "SELECT * FROM c WHERE c.repoId=@repoId AND c.docsetName=@docsetName";

            var parameters = new SqlParameterCollection()
            {
                new SqlParameter("@repoId", repoId),
                new SqlParameter("@docsetName", docsetName)
            };

            var configs = await GetQueryResult<ConfigEntity>(queryString, parameters);

            return configs.Count > 0 ? configs[0] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repoId"></param>
        /// <param name="docsetName"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<bool> UpdateConfig(string repoId, string docsetName, JObject config)
        {
            Guard.ArgumentNotNullOrEmpty(repoId);
            Guard.ArgumentNotNullOrEmpty(docsetName);

            var oldConfig = await GetConfigByDocsetInfo(repoId, docsetName);

            if (oldConfig == null)
            {
                return false;
            }
            else
            {
                oldConfig.Config = config;
                return await AddOrUpdate(repoId, oldConfig);
            }
        }
    }
}
