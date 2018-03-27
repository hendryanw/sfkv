using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFKV.Store
{
    public class SetRepository : BaseRepository<HashSet<string>>
    {
        public const string Name = "sfkv.set";

        public SetRepository(IReliableStateManager stateManager)
            : base(stateManager, Name)
        {
        }

        public async Task<string[]> SetGetAllAsync(string key)
        {
            return await SetGetAsync(key, -1);
        }

        public async Task<string[]> SetGetAsync(string key, int count)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var result = await _dictionary.TryGetValueAsync(tx, key);

                if (result.HasValue)
                {
                    if (count < 0)
                    {
                        return result.Value.ToArray();
                    }
                    else
                    {
                        return result.Value.Take(count).ToArray();
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task SetAddAsync(string key, string value)
        {
            await SetAddMultipleAsync(key, new[] { value });
        }

        public async Task SetAddMultipleAsync(string key, IEnumerable<string> values)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                await _dictionary.AddOrUpdateAsync(tx, key,
                    new HashSet<string>(values, StringComparer.Ordinal),
                    (k, ov) =>
                    {
                        foreach (var value in values)
                        {
                            ov.Add(value);
                        }
                        
                        return ov;
                    });

                await tx.CommitAsync();
            }
        }

        public async Task SetRemoveAsync(string key, string value)
        {
            await SetRemoveMultipleAsync(key, new[] { value });
        }

        public async Task SetRemoveMultipleAsync(string key, IEnumerable<string> values)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                try
                {
                    await _dictionary.AddOrUpdateAsync(tx, key,
                        (k) =>
                        {
                            throw new AddValueRestrictedException();
                        },
                        (k, ov) =>
                        {
                            foreach (var value in values)
                            {
                                ov.Remove(value);
                            }

                            return ov;
                        });

                    await tx.CommitAsync();
                }
                catch (AddValueRestrictedException)
                {
                    // Do nothing.
                }
            }
        }

        public async Task<int> SetCountAsync(string key)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var result = await _dictionary.TryGetValueAsync(tx, key);

                if (result.HasValue)
                {
                    return result.Value.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        public async Task<bool> SetContainsAsync(string key, string value)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var result = await _dictionary.TryGetValueAsync(tx, key);

                if (result.HasValue)
                {
                    return result.Value.Contains(value);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
