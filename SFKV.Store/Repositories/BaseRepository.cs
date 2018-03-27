using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFKV.Store
{
    public abstract class BaseRepository<T>
    {
        protected readonly IReliableStateManager _stateManager;
        protected readonly IReliableDictionary<string, T> _dictionary;
        
        public BaseRepository(IReliableStateManager stateManager, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
            _dictionary = stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(name).Result;
        }

        public async Task<bool> ExistsAsync(string key)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                return await _dictionary.ContainsKeyAsync(tx, key);
            }
        }

        public async Task<bool> DeleteAsync(string key)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var result = await _dictionary.TryRemoveAsync(tx, key);

                if (result.HasValue)
                {
                    await tx.CommitAsync();
                }

                return result.HasValue;
            }
        }
    }
}
