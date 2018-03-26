using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFKV.Store
{
    public class HashRepository : BaseRepository<IDictionary<string, string>>
    {
        public const string Name = "sfkv.hash";
        
        public HashRepository(IReliableStateManager stateManager)
            : base(stateManager, Name)
        {
        }

        public async Task<string> HashGetAsync(string key, string field)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var hash = await _dictionary.TryGetValueAsync(tx, key);

                if (!hash.HasValue
                    || !hash.Value.ContainsKey(field))
                {
                    return null;
                }

                return hash.Value[field];
            }
        }

        public async Task<IDictionary<string, string>> HashMultipleGetAsync(string key, IEnumerable<string> fields)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var hash = await _dictionary.TryGetValueAsync(tx, key);

                if (!hash.HasValue)
                {
                    return null;
                }

                var retVal = new Dictionary<string, string>();
                foreach (var field in fields)
                {
                    retVal.Add(field, hash.Value[field]);
                }

                return retVal;
            }
        }

        public async Task<IDictionary<string, string>> HashGetAllAsync(string key)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var hash = await _dictionary.TryGetValueAsync(tx, key);
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
                await _dictionary.AddOrUpdateAsync(tx, key,
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
                
                await tx.CommitAsync();
            }
        }
    }
}
