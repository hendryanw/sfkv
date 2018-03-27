using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

using SFKV.Contracts;

namespace SFKV.Store
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Store : StatefulService, IStore
    {
        private readonly Lazy<StringRepository> _stringRepositoryLazy;
        private readonly Lazy<HashRepository> _hashRepositoryLazy;
        private readonly Lazy<IntRepository> _intRepositoryLazy;
        private readonly Lazy<ListRepository> _listRepositoryLazy;

        private StringRepository _stringRepository => _stringRepositoryLazy.Value;
        private HashRepository _hashRepository => _hashRepositoryLazy.Value;
        private IntRepository _intRepository => _intRepositoryLazy.Value;
        private ListRepository _listRepository => _listRepositoryLazy.Value;

        public Store(StatefulServiceContext context)
            : base(context)
        {
            _stringRepositoryLazy = new Lazy<StringRepository>(() => new StringRepository(StateManager));
            _hashRepositoryLazy = new Lazy<HashRepository>(() => new HashRepository(StateManager));
            _intRepositoryLazy = new Lazy<IntRepository>(() => new IntRepository(StateManager));
            _listRepositoryLazy = new Lazy<ListRepository>(() => new ListRepository(StateManager));
        }

        public async Task<string> StringGetAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _stringRepository.StringGetAsync(key), nameof(StringGetAsync));
        }

        public async Task StringSetAsync(string key, string value)
        {
            await ExecuteServiceRequestAsync(() => _stringRepository.StringSetAsync(key, value), nameof(StringSetAsync));
        }

        public async Task StringMultipleSetAsync(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            await ExecuteServiceRequestAsync(() => _stringRepository.StringMultipleSetAsync(keyValuePairs), nameof(StringMultipleSetAsync));
        }

        public async Task StringAppendAsync(string key, string value)
        {
            await ExecuteServiceRequestAsync(() => _stringRepository.StringAppendAsync(key, value), nameof(StringAppendAsync));
        }
        
        public async Task<bool> StringExistsAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _stringRepository.ExistsAsync(key), nameof(StringExistsAsync));
        }

        public async Task<bool> StringDeleteAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _stringRepository.DeleteAsync(key), nameof(StringDeleteAsync));
        }
        
        public async Task IntSetAsync(string key, int value)
        {
            await ExecuteServiceRequestAsync(() => _intRepository.IntSetAsync(key, value), nameof(IntSetAsync));
        }

        public async Task IntMultipleSetAsync(IEnumerable<KeyValuePair<string, int>> keyValuePairs)
        {
            await ExecuteServiceRequestAsync(() => _intRepository.IntMultipleSetAsync(keyValuePairs), nameof(IntMultipleSetAsync));
        }
        
        public async Task<int> IntGetAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.IntGetAsync(key), nameof(IntGetAsync));
        }

        public async Task<int> IntIncrAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.IntIncrAsync(key), nameof(IntIncrAsync));
        }

        public async Task<int> IntIncrByAsync(string key, int incrBy)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.IntIncrByAsync(key, incrBy), nameof(IntIncrByAsync));
        }

        public async Task<bool> IntExistsAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.ExistsAsync(key), nameof(IntExistsAsync));
        }

        public async Task<bool> IntDeleteAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.DeleteAsync(key), nameof(IntDeleteAsync));
        }

        public async Task<string> HashGetAsync(string key, string field)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.HashGetAsync(key, field), nameof(HashGetAsync));
        }

        public async Task<IDictionary<string, string>> HashMultipleGetAsync(string key, IEnumerable<string> fields)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.HashMultipleGetAsync(key, fields), nameof(HashMultipleGetAsync));
        }

        public async Task<IDictionary<string, string>> HashGetAllAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.HashGetAllAsync(key), nameof(HashGetAllAsync));
        }

        public async Task HashSetAsync(string key, KeyValuePair<string, string> keyValuePair)
        {
            await ExecuteServiceRequestAsync(() => _hashRepository.HashSetAsync(key, keyValuePair), nameof(HashSetAsync));
        }

        public async Task HashMultipleSetAsync(string key, IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            await ExecuteServiceRequestAsync(() => _hashRepository.HashMultipleSetAsync(key, keyValuePairs), nameof(HashMultipleSetAsync));
        }

        public async Task<bool> HashExistsAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.ExistsAsync(key), nameof(HashExistsAsync));
        }

        public async Task<bool> HashDeleteAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.DeleteAsync(key), nameof(HashDeleteAsync));
        }

        public async Task<string[]> ListRangeAsync(string key, int firstIndex, int lastIndex)
        {
            return await ExecuteServiceRequestAsync(() => _listRepository.ListRangeAsync(key, firstIndex, lastIndex), nameof(ListRangeAsync));
        }

        public async Task ListAddFirstAsync(string key, string value)
        {
            await ExecuteServiceRequestAsync(() => _listRepository.ListAddFirstAsync(key, value), nameof(ListAddFirstAsync));
        }

        public async Task ListAddMultipleFirstAsync(string key, IEnumerable<string> values)
        {
            await ExecuteServiceRequestAsync(() => _listRepository.ListAddFirstAsync(key, values), nameof(ListAddMultipleFirstAsync));
        }

        public async Task ListAddLastAsync(string key, string value)
        {
            await ExecuteServiceRequestAsync(() => _listRepository.ListAddLastAsync(key, value), nameof(ListAddLastAsync));
        }

        public async Task ListAddMultipleLastAsync(string key, IEnumerable<string> values)
        {
            await ExecuteServiceRequestAsync(() => _listRepository.ListAddLastAsync(key, values), nameof(ListAddMultipleLastAsync));
        }

        public async Task<string> ListPopFirstAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _listRepository.ListPopFirstAsync(key), nameof(ListPopFirstAsync));
        }

        public async Task<string> ListPopLastAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _listRepository.ListPopLastAsync(key), nameof(ListPopLastAsync));
        }

        public async Task<bool> ListExistsAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _listRepository.ExistsAsync(key), nameof(ListExistsAsync));
        }

        public async Task<bool> ListDeleteAsync(string key)
        {
            return await ExecuteServiceRequestAsync(() => _listRepository.DeleteAsync(key), nameof(ListDeleteAsync));
        }

        private async Task ExecuteServiceRequestAsync(Func<Task> actionAsync, string requestTypeName)
        {
            SFKVEventSource.Current.ServiceRequestStart(nameof(requestTypeName));
            
            try
            {
                await actionAsync();
            }
            catch (Exception ex)
            {
                SFKVEventSource.Current.ServiceRequestError(nameof(requestTypeName), ex.ToString());

                // Propagate the exception to the client via service remoting.
                throw;
            }
            finally
            {
                SFKVEventSource.Current.ServiceRequestStop(nameof(requestTypeName));
            }
        }

        private async Task<T> ExecuteServiceRequestAsync<T>(Func<Task<T>> funcAsync, string requestTypeName)
        {
            SFKVEventSource.Current.ServiceRequestStart(nameof(requestTypeName));

            try
            {
                return await funcAsync();
            }
            catch (Exception ex)
            {
                SFKVEventSource.Current.ServiceRequestError(nameof(requestTypeName), ex.ToString());

                // Propagate the exception to the client via service remoting.
                throw;
            }
            finally
            {
                SFKVEventSource.Current.ServiceRequestStop(nameof(requestTypeName));
            }
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(
                    (c) => new FabricTransportServiceRemotingListener(c, this),
                    "V2Listener",
                    listenOnSecondary: true)
            };
        }

        /// <summary>
        /// This is the main entry point for the service replica.
        /// This method executes when the replica of the service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
        }
    }
}
