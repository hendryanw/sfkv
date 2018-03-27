using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFKV.Store
{
    public class ListRepository : BaseRepository<LinkedList<string>>
    {
        public const string Name = "sfkv.list";

        public ListRepository(IReliableStateManager stateManager)
            : base(stateManager, Name)
        {
        }

        public async Task<string[]> ListRangeAsync(string key, int firstIndex, int lastIndex)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var list = await _dictionary.TryGetValueAsync(tx, key);

                if (list.HasValue
                    && list.Value.Count > 0)
                {
                    // Copy using built-in CopyTo LinkedList method.
                    if (lastIndex < 0)
                    {
                        var retVal = new string[list.Value.Count - firstIndex];
                        list.Value.CopyTo(retVal, firstIndex);

                        return retVal;
                    }
                    else
                    {
                        var retVal = new string[(lastIndex - firstIndex) + 1];
                        list.Value.CopyTo(retVal, firstIndex);

                        return retVal;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task ListAddFirstAsync(string key, string value)
        {
            await ListAddAsync(key, new[] { value }, ListPosition.First);
        }

        public async Task ListAddFirstAsync(string key, IEnumerable<string> values)
        {
            await ListAddAsync(key, values, ListPosition.First);
        }

        public async Task ListAddLastAsync(string key, string value)
        {
            await ListAddAsync(key, new[] { value }, ListPosition.Last);
        }

        public async Task ListAddLastAsync(string key, IEnumerable<string> values)
        {
            await ListAddAsync(key, values, ListPosition.Last);
        }

        public async Task<string> ListPopFirstAsync(string key)
        {
            return await ListPopAsync(key, ListPosition.First);
        }

        public async Task<string> ListPopLastAsync(string key)
        {
            return await ListPopAsync(key, ListPosition.Last);
        }

        private async Task ListAddAsync(string key, IEnumerable<string> values, ListPosition position)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                await _dictionary.AddOrUpdateAsync(tx, key,
                    (k) =>
                    {
                        return new LinkedList<string>(values);
                    },
                    (k, ov) =>
                    {
                        switch (position)
                        {
                            case ListPosition.First:
                                foreach (var value in values)
                                {
                                    ov.AddFirst(value);
                                }
                                break;
                            case ListPosition.Last:
                                foreach (var value in values)
                                {
                                    ov.AddLast(value);
                                }
                                break;
                            default:
                                throw new NotImplementedException($"{nameof(position)} value {position} is not implemented.");
                        }
                        
                        return ov;
                    });

                await tx.CommitAsync();
            }
        }

        private async Task<string> ListPopAsync(string key, ListPosition position)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var list = await _dictionary.TryGetValueAsync(tx, key);

                if (list.HasValue
                    && list.Value.Count > 0)
                {
                    try
                    {
                        string retVal = null;
                        await _dictionary.AddOrUpdateAsync(tx, key,
                            (k) =>
                            {
                                throw new AddValueRestrictedException();
                            },
                            (k, ov) =>
                            {
                                switch (position)
                                {
                                    case ListPosition.First:
                                        retVal = ov.First.Value;
                                        ov.RemoveFirst();
                                        break;
                                    case ListPosition.Last:
                                        retVal = ov.Last.Value;
                                        ov.RemoveLast();
                                        break;
                                    default:
                                        throw new NotImplementedException($"{nameof(position)} value {position} is not implemented.");
                                }

                                return ov;
                            });

                        await tx.CommitAsync();
                        return retVal;
                    }
                    catch (AddValueRestrictedException)
                    {
                        // There should be no value added / created during this operation.
                        // If the list contains no elements, return null instead.
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private enum ListPosition
        {
            First = 0,

            Last = 1
        }
    }
}
