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
        /// <returns>Returns awaitable task.</returns>
        Task StringSet(string key, string value);

        /// <summary>
        /// Add or update multiple key value pairs to the store.
        /// </summary>
        /// <param name="keyValuePairs">The key value pairs.</param>
        /// <returns>Returns awaitable task.</returns>
        Task StringMultipleSet(IEnumerable<KeyValuePair<string, string>> keyValuePairs);

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
        /// <returns>Returns awaitable task.</returns>
        Task StringAppend(string key, string value);

        /// <summary>
        /// Check whether the specified key exists on the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key existence.</returns>
        Task<bool> StringExists(string key);

        /// <summary>
        /// Delete the specified key and its value from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key value pair is removed from the store.</returns>
        Task<bool> StringDelete(string key);

        /// <summary>
        /// Add or update a key and integer value pair to the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The integer value.</param>
        /// <returns>Returns awaitable task.</returns>
        Task IntSet(string key, int value);

        /// <summary>
        /// Add or update multiple key and integer value pairs to the store.
        /// </summary>
        /// <param name="keyValuePairs">The key and integer value pairs.</param>
        /// <returns>Returns awaitable task.</returns>
        Task IntMultipleSet(IEnumerable<KeyValuePair<string, int>> keyValuePairs);

        /// <summary>
        /// Get the integer value based on the key from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the integer value for the key.</returns>
        Task<int> IntGet(string key);

        /// <summary>
        /// Increment the integer value by 1.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the incremented value.</returns>
        Task<int> IntIncr(string key);

        /// <summary>
        /// Increment the integer value with specified number.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the incremented value.</returns>
        Task<int> IntIncrBy(string key, int incrBy);

        /// <summary>
        /// Check whether the specified key exists on the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key existence.</returns>
        Task<bool> IntExists(string key);

        /// <summary>
        /// Delete the specified key and its value from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key value pair is removed from the store.</returns>
        Task<bool> IntDelete(string key);

        /// <summary>
        /// Get the value of a hash field.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="field">The hash field.</param>
        /// <returns>Returns the value of the hash field.</returns>
        Task<string> HashGet(string key, string field);

        /// <summary>
        /// Get the values of multiple hash fields.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="fields">The hash fields.</param>
        /// <returns>Returns the values of the multiple hash fields. Returns empty value if the field is not found.</returns>
        Task<IDictionary<string, string>> HashMultipleGet(string key, IEnumerable<string> fields);

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
        /// <returns>Returns awaitable task.</returns>
        Task HashSet(string key, KeyValuePair<string, string> keyValuePair);

        /// <summary>
        /// Set multiple hash field and value pairs.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="keyValuePairs">Multiple hash field and value pairs.</param>
        /// <returns>Returns awaitable task.</returns>
        Task HashMultipleSet(string key, IEnumerable<KeyValuePair<string, string>> keyValuePairs);

        /// <summary>
        /// Check whether the specified key exists on the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key existence.</returns>
        Task<bool> HashExists(string key);

        /// <summary>
        /// Delete the specified key and its value from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key value pair is removed from the store.</returns>
        Task<bool> HashDelete(string key);
    }
}