using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFKV.Store
{
    public class HashRepository : IRepository
    {
        public string Name { get; } = "sfkv.hash";

        private readonly IReliableStateManager _stateManager;
        private readonly IReliableDictionary<string, IDictionary<string, string>> _hashes;

        public HashRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
            _hashes = stateManager.GetOrAddAsync<IReliableDictionary<string, IDictionary<string, string>>>(Name).Result;
        }

        public async Task<string> HashGetAsync(string key, string field)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var hash = await _hashes.TryGetValueAsync(tx, key);

                if (!hash.HasValue
                    || !hash.Value.ContainsKey(field))
                {
                    return null;
                }

                return hash.Value[field];
            }
        }

        public async Task<IDictionary<string, string>> HashGetAllAsync(string key)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var hash = await _hashes.TryGetValueAsync(tx, key);
                return hash.HasValue ? hash.Value : null;
            }
        }

        public async Task HashSetAsync(string key, KeyValuePair<string, string> keyValuePair)
        {
            await HashMultipleSetAsync(key, new KeyValuePair<string, string>[] { keyValuePair });
        }

        public async Task HashMultipleSetAsync(string key, IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                await _hashes.AddOrUpdateAsync(tx, key,
                    (k) => new Dictionary<string, string>(keyValuePairs),
                    (k, existingHash) =>
                    {
                        foreach (var keyValuePair in keyValuePairs)
                        {
                            if (existingHash.ContainsKey(keyValuePair.Key))
                            {
                                existingHash[keyValuePair.Key] = keyValuePair.Value;
                            }
                            else
                            {
                                existingHash.Add(keyValuePair);
                            }
                        }

                        return existingHash;
                    });
            }
        }
    }
}
