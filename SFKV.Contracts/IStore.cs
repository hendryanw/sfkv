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
        Task StringSetAsync(string key, string value);

        /// <summary>
        /// Add or update multiple key value pairs to the store.
        /// </summary>
        /// <param name="keyValuePairs">The key value pairs.</param>
        /// <returns>Returns awaitable task.</returns>
        Task StringMultipleSetAsync(IEnumerable<KeyValuePair<string, string>> keyValuePairs);

        /// <summary>
        /// Get the value based on the key from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the value for the key.</returns>
        Task<string> StringGetAsync(string key);

        /// <summary>
        /// Append the value at the end of existing string if exists. Otherwise the string will be created.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value which will be appended.</param>
        /// <returns>Returns awaitable task.</returns>
        Task StringAppendAsync(string key, string value);

        /// <summary>
        /// Check whether the specified key exists on the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key existence.</returns>
        Task<bool> StringExistsAsync(string key);

        /// <summary>
        /// Delete the specified key and its value from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key value pair is removed from the store.</returns>
        Task<bool> StringDeleteAsync(string key);

        /// <summary>
        /// Add or update a key and integer value pair to the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The integer value.</param>
        /// <returns>Returns awaitable task.</returns>
        Task IntSetAsync(string key, int value);

        /// <summary>
        /// Add or update multiple key and integer value pairs to the store.
        /// </summary>
        /// <param name="keyValuePairs">The key and integer value pairs.</param>
        /// <returns>Returns awaitable task.</returns>
        Task IntMultipleSetAsync(IEnumerable<KeyValuePair<string, int>> keyValuePairs);

        /// <summary>
        /// Get the integer value based on the key from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the integer value for the key.</returns>
        Task<int> IntGetAsync(string key);

        /// <summary>
        /// Increment the integer value by 1.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the incremented value.</returns>
        Task<int> IntIncrAsync(string key);

        /// <summary>
        /// Increment the integer value with specified number.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the incremented value.</returns>
        Task<int> IntIncrByAsync(string key, int incrBy);

        /// <summary>
        /// Check whether the specified key exists on the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key existence.</returns>
        Task<bool> IntExistsAsync(string key);

        /// <summary>
        /// Delete the specified key and its value from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key value pair is removed from the store.</returns>
        Task<bool> IntDeleteAsync(string key);

        /// <summary>
        /// Get the value of a hash field.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="field">The hash field.</param>
        /// <returns>Returns the value of the hash field.</returns>
        Task<string> HashGetAsync(string key, string field);

        /// <summary>
        /// Get the values of multiple hash fields.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="fields">The hash fields.</param>
        /// <returns>Returns the values of the multiple hash fields. Returns empty value if the field is not found.</returns>
        Task<IDictionary<string, string>> HashMultipleGetAsync(string key, IEnumerable<string> fields);

        /// <summary>
        /// Get the hash.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <returns>Returns the hash.</returns>
        Task<IDictionary<string, string>> HashGetAllAsync(string key);

        /// <summary>
        /// Set the hash field and value pair.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="keyValuePair">The hash field and value pair.</param>
        /// <returns>Returns awaitable task.</returns>
        Task HashSetAsync(string key, KeyValuePair<string, string> keyValuePair);

        /// <summary>
        /// Set multiple hash field and value pairs.
        /// </summary>
        /// <param name="key">The hash key.</param>
        /// <param name="keyValuePairs">Multiple hash field and value pairs.</param>
        /// <returns>Returns awaitable task.</returns>
        Task HashMultipleSetAsync(string key, IEnumerable<KeyValuePair<string, string>> keyValuePairs);

        /// <summary>
        /// Check whether the specified key exists on the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key existence.</returns>
        Task<bool> HashExistsAsync(string key);

        /// <summary>
        /// Delete the specified key and its value from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key value pair is removed from the store.</returns>
        Task<bool> HashDeleteAsync(string key);

        /// <summary>
        /// List the values by its index position.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="firstIndex">The first index of the value range.</param>
        /// <param name="lastIndex">The last index of the value range. If less than 0 is specified, will return the values until the end of the list.</param>
        /// <returns>Returns the list of values.</returns>
        Task<string[]> ListRangeAsync(string key, int firstIndex, int lastIndex);

        /// <summary>
        /// Add the value at the first position of the list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns awaitable task.</returns>
        Task ListAddFirstAsync(string key, string value);

        /// <summary>
        /// Add multiple values at the first position of the list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns>Returns awaitable task.</returns>
        Task ListAddMultipleFirstAsync(string key, IEnumerable<string> values);

        /// <summary>
        /// Add the value at the last position of the list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns awaitable task.</returns>
        Task ListAddLastAsync(string key, string value);

        /// <summary>
        /// Add multiple values at the last position of the list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns>Returns awaitable task.</returns>
        Task ListAddMultipleLastAsync(string key, IEnumerable<string> values);

        /// <summary>
        /// Remove and return the first value of the list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the first value of the list.</returns>
        Task<string> ListPopFirstAsync(string key);

        /// <summary>
        /// Remove and return the last value of the list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the last value of the list.</returns>
        Task<string> ListPopLastAsync(string key);

        /// <summary>
        /// Check whether the specified key exists on the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key existence.</returns>
        Task<bool> ListExistsAsync(string key);

        /// <summary>
        /// Delete the specified key and its value from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns a boolean indicating the key value pair is removed from the store.</returns>
        Task<bool> ListDeleteAsync(string key);
    }
}