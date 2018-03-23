using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFKV.Store
{
    public class StringRepository : IRepository
    {
        public string Name { get; } = "sfkv.string";

        private readonly IReliableStateManager _stateManager;
        private readonly IReliableDictionary<string, string> _strings;

        public StringRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
            _strings = stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(Name).Result;
        }

        public async Task<string> StringGetAsync(string key)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var result = await _strings.TryGetValueAsync(tx, key);
                return result.HasValue ? result.Value : null;
            }
        }

        public async Task StringSetAsync(string key, string value)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                await _strings.AddOrUpdateAsync(tx, key, (k) => value, (k, v) => value);
                await tx.CommitAsync();
            }
        }

        public async Task StringAppendAsync(string key, string value)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                await _strings.AddOrUpdateAsync(tx, key, (k) => value, (k, v) => v + value);
                await tx.CommitAsync();
            }
        }
    }
}
