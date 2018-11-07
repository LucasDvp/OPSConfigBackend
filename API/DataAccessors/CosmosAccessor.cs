using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.OpenPublishing.ConfigService.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.OpenPublishing.ConfigService.DataAccessors
{
    public class CosmosAccessor
    {
        protected readonly DocumentClient _client;
        protected readonly Uri _collectionUri;
        protected readonly string _databaseId;
        protected readonly string _collectionId;

        private const int DBRetryTime = 5;

        public CosmosAccessor(DocumentClient client, string databaseId, string collectionId)
        {
            _client = client;
            _databaseId = databaseId;
            _collectionId = collectionId;
            _collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
        }

        #region Create

        public static DocumentClient CreateClient(string cosmosUriString, string primaryKey)
        {
            Guard.ArgumentNotNullOrEmpty(cosmosUriString);
            Guard.ArgumentNotNullOrEmpty(primaryKey);

            return CreateClientAsync(cosmosUriString, primaryKey).Result;
        }

        public static async Task<DocumentClient> CreateClientAsync(string cosmosUriString, string primaryKey)
        {
            Guard.ArgumentNotNullOrEmpty(cosmosUriString);
            Guard.ArgumentNotNullOrEmpty(primaryKey);

            var uri = new Uri(cosmosUriString);
            var client = new DocumentClient(uri, primaryKey);

            await client.OpenAsync().ConfigureAwait(false);

            return client;
        }
        #endregion

        #region CRUD Interfaces
        /// <summary>
        /// Qurey document DB in SQL method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery">SQL string</param>
        /// <param name="parameters">Params defined in SQL</param>
        /// <returns></returns>
        protected async Task<List<T>> GetQueryResult<T>(string sqlQuery, SqlParameterCollection parameters) where T : class
        {
            var options = new FeedOptions
            {
                MaxItemCount = -1,
                EnableCrossPartitionQuery = false
            };

            var query = _client.CreateDocumentQuery<T>(_collectionUri, new SqlQuerySpec()
            {
                QueryText = sqlQuery,
                Parameters = parameters
            }, options).AsDocumentQuery();

            var result = new List<T>();
            while (query.HasMoreResults)
            {
                /// Return "null" when db error
                var response = await WithRetry(() => query.ExecuteNextAsync<T>());
                result.AddRange(response);
            }
            return result;
        }

        protected async Task<bool> AddOrUpdate<T>(string partitionId, T entity) where T : Resource
        {
            Guard.ArgumentNotNullOrEmpty(partitionId);
            Guard.ArgumentNotNull(entity);

            var options = new RequestOptions
            {
                AccessCondition = new AccessCondition
                {
                    Type = AccessConditionType.IfMatch,
                    Condition = entity.ETag
                },
                PartitionKey = new PartitionKey(partitionId)
            };

            var result = await WithRetry(() => _client.UpsertDocumentAsync(_collectionUri, entity, options, false));

            return result.StatusCode < HttpStatusCode.BadRequest;
        }
        #endregion

        #region Database Guard 
        /// <summary>
        /// Database Guard with DocumentClientException with Retry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected static async Task<T> WithRetry<T>(Func<Task<T>> func) where T : class
        {
            int count = 0;

            while (true)
            {
                try
                {
                    return await func().ConfigureAwait(false);
                }
                catch (DocumentClientException ex) when (count < DBRetryTime)
                {
                    ++count;

                    if (ex.Error == null || string.IsNullOrEmpty(ex.Error.Code))
                    {
                        throw;
                    }

                    switch (ex.Error.Code)
                    {
                        case "429":
                            // TooManyRequestException
                            await Task.Delay(ex.RetryAfter + TimeSpan.FromSeconds(10)).ConfigureAwait(false);
                            continue;
                        case "412":
                            return null;
                        case "404":
                            return null;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
