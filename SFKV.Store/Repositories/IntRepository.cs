using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFKV.Store
{
    public class IntRepository : BaseRepository<int>
    {
        public const string Name = "sfkv.int";
        
        public IntRepository(IReliableStateManager stateManager)
            : base(stateManager, Name)
        {
        }

        public async Task<int> IntGetAsync(string key)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var result = await _dictionary.TryGetValueAsync(tx, key);
                return result.HasValue ? result.Value : 0;
            }
        }

        public async Task IntSetAsync(string key, int value)
        {
            await IntMultipleSetAsync(new[] { new KeyValuePair<string, int>(key, value) });
        }

        public async Task IntMultipleSetAsync(IEnumerable<KeyValuePair<string, int>> keyValuePairs)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                foreach (var keyValuePair in keyValuePairs)
                {
                    await _dictionary.AddOrUpdateAsync(tx, keyValuePair.Key, keyValuePair.Value, (k, ov) => keyValuePair.Value);
                }

                await tx.CommitAsync();
            }
        }

        public async Task<int> IntIncrAsync(string key)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                int retVal = 0;

                await _dictionary.AddOrUpdateAsync(tx, key,
                    (k) =>
                    {
                        retVal = 1;
                        return retVal;
                    },
                    (k, ov) =>
                    {
                        retVal = ++ov;
                        return retVal;
                    });
                
                await tx.CommitAsync();

                return retVal;
            }
        }

        public async Task<int> IntIncrByAsync(string key, int incrBy)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                int retVal = 0;

                await _dictionary.AddOrUpdateAsync(tx, key,
                    (k) =>
                    {
                        retVal = 1;
                        return retVal;
                    },
                    (k, ov) =>
                    {
                        retVal = ov + incrBy;
                        return retVal;
                    });

                await tx.CommitAsync();

                return retVal;
            }
        }
    }
}
