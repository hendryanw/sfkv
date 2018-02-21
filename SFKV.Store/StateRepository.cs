using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFKV.Store
{
    /// <summary>
    /// This class is responsible for talking with Service Fabric state manager.
    /// </summary>
    internal class StateRepository
    {
        private readonly IReliableStateManager _stateManager;

        public StateRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task StringSetAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var sfkvDictionary = await _stateManager.GetOrAddAsync<IReliableDictionary<string, string>>("sfkv");

            using (var tx = _stateManager.CreateTransaction())
            {
                await sfkvDictionary.AddOrUpdateAsync(tx, key, (k) => value, (k, v) => value);

                // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                // discarded, and nothing is saved to the secondary replicas.
                await tx.CommitAsync();
            }
        }

        public async Task<string> StringGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var sfkvDictionary = await _stateManager.GetOrAddAsync<IReliableDictionary<string, string>>("sfkv");

            using (var tx = _stateManager.CreateTransaction())
            {
                var result = await sfkvDictionary.TryGetValueAsync(tx, key);

                return result.HasValue ? result.Value : null;
            }
        }
    }
}
