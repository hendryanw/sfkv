using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace SFKV.Contracts
{
    /// <summary>
    /// The service remoting interface for SFKV storage service.
    /// </summary>
    public interface IStore : IService
    {
        /// <summary>
        /// Add or update a key value pair to the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        Task StringSet(string key, string value);

        /// <summary>
        /// Get the value based on the key from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the value for the key.</returns>
        Task<string> StringGet(string key);

        /// <summary>
        /// Append the value at the end of existing string if exists. Otherwise the string will be created.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value which will be appended.</param>
        Task StringAppend(string key, string value);

        /// <summary>
        /// Get the value of a hash field.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="field">The hash field.</param>
        /// <returns>Returns the value of the hash field.</returns>
        Task<string> HashGet(string key, string field);

        /// <summary>
        /// Get the hash.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <returns>Returns the hash.</returns>
        Task<IDictionary<string, string>> HashGetAll(string key);

        /// <summary>
        /// Set the hash field and value pair.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="keyValuePair">The hash field and value pair.</param>
        Task HashSet(string key, KeyValuePair<string, string> keyValuePair);

        /// <summary>
        /// Set multiple hash field and value pairs.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="keyValuePairs">Multiple hash field and value pairs.</param>
        Task HashMultipleSet(string key, IEnumerable<KeyValuePair<string, string>> keyValuePairs);
    }
}