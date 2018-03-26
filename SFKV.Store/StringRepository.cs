using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFKV.Store
{
    public class StringRepository : BaseRepository<string>
    {
        public const string Name = "sfkv.string";

        public StringRepository(IReliableStateManager stateManager)
            : base(stateManager, Name)
        {
        }

        public async Task<string> StringGetAsync(string key)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var result = await _dictionary.TryGetValueAsync(tx, key);
                return result.HasValue ? result.Value : null;
            }
        }

        public async Task StringSetAsync(string key, string value)
        {
            await StringMultipleSetAsync(new[] { new KeyValuePair<string, string>(key, value) });
        }

        public async Task StringMultipleSetAsync(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                foreach (var keyValuePair in keyValuePairs)
                {
                    await _dictionary.AddOrUpdateAsync(tx, keyValuePair.Key, (k) => keyValuePair.Value, (k, v) => keyValuePair.Value);
                }
                
                await tx.CommitAsync();
            }
        }

        public async Task StringAppendAsync(string key, string value)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                await _dictionary.AddOrUpdateAsync(tx, key, (k) => value, (k, v) => v + value);
                await tx.CommitAsync();
            }
        }
    }
}
