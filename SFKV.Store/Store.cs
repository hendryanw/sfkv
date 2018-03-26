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

        private StringRepository _stringRepository => _stringRepositoryLazy.Value;
        private HashRepository _hashRepository => _hashRepositoryLazy.Value;
        private IntRepository _intRepository => _intRepositoryLazy.Value;

        public Store(StatefulServiceContext context)
            : base(context)
        {
            _stringRepositoryLazy = new Lazy<StringRepository>(() => new StringRepository(StateManager));
            _hashRepositoryLazy = new Lazy<HashRepository>(() => new HashRepository(StateManager));
            _intRepositoryLazy = new Lazy<IntRepository>(() => new IntRepository(StateManager));
        }

        public async Task<string> StringGet(string key)
        {
            return await ExecuteServiceRequestAsync(() => _stringRepository.StringGetAsync(key), nameof(StringGet));
        }

        public async Task StringSet(string key, string value)
        {
            await ExecuteServiceRequestAsync(() => _stringRepository.StringSetAsync(key, value), nameof(StringSet));
        }

        public async Task StringMultipleSet(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            await ExecuteServiceRequestAsync(() => _stringRepository.StringMultipleSetAsync(keyValuePairs), nameof(StringMultipleSet));
        }

        public async Task StringAppend(string key, string value)
        {
            await ExecuteServiceRequestAsync(() => _stringRepository.StringAppendAsync(key, value), nameof(StringAppend));
        }
        
        public async Task<bool> StringExists(string key)
        {
            return await ExecuteServiceRequestAsync(() => _stringRepository.ExistsAsync(key), nameof(StringExists));
        }

        public async Task<bool> StringDelete(string key)
        {
            return await ExecuteServiceRequestAsync(() => _stringRepository.DeleteAsync(key), nameof(StringDelete));
        }
        
        public async Task IntSet(string key, int value)
        {
            await ExecuteServiceRequestAsync(() => _intRepository.IntSetAsync(key, value), nameof(IntSet));
        }

        public async Task IntMultipleSet(IEnumerable<KeyValuePair<string, int>> keyValuePairs)
        {
            await ExecuteServiceRequestAsync(() => _intRepository.IntMultipleSetAsync(keyValuePairs), nameof(IntMultipleSet));
        }
        
        public async Task<int> IntGet(string key)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.IntGetAsync(key), nameof(IntGet));
        }

        public async Task<int> IntIncr(string key)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.IntIncrAsync(key), nameof(IntIncr));
        }

        public async Task<int> IntIncrBy(string key, int incrBy)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.IntIncrByAsync(key, incrBy), nameof(IntIncrBy));
        }

        public async Task<bool> IntExists(string key)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.ExistsAsync(key), nameof(IntExists));
        }

        public async Task<bool> IntDelete(string key)
        {
            return await ExecuteServiceRequestAsync(() => _intRepository.DeleteAsync(key), nameof(IntDelete));
        }

        public async Task<string> HashGet(string key, string field)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.HashGetAsync(key, field), nameof(HashGet));
        }

        public async Task<IDictionary<string, string>> HashMultipleGet(string key, IEnumerable<string> fields)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.HashMultipleGetAsync(key, fields), nameof(HashMultipleGet));
        }

        public async Task<IDictionary<string, string>> HashGetAll(string key)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.HashGetAllAsync(key), nameof(HashGetAll));
        }

        public async Task HashSet(string key, KeyValuePair<string, string> keyValuePair)
        {
            await ExecuteServiceRequestAsync(() => _hashRepository.HashSetAsync(key, keyValuePair), nameof(HashSet));
        }

        public async Task HashMultipleSet(string key, IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            await ExecuteServiceRequestAsync(() => _hashRepository.HashMultipleSetAsync(key, keyValuePairs), nameof(HashMultipleSet));
        }

        public async Task<bool> HashExists(string key)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.ExistsAsync(key), nameof(HashExists));
        }

        public async Task<bool> HashDelete(string key)
        {
            return await ExecuteServiceRequestAsync(() => _hashRepository.DeleteAsync(key), nameof(HashDelete));
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
