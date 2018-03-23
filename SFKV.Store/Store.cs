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

        private StringRepository _stringRepository => _stringRepositoryLazy.Value;
        private HashRepository _hashRepository => _hashRepositoryLazy.Value;

        public Store(StatefulServiceContext context)
            : base(context)
        {
            _stringRepositoryLazy = new Lazy<StringRepository>(() => new StringRepository(StateManager));
            _hashRepositoryLazy = new Lazy<HashRepository>(() => new HashRepository(StateManager));
        }

        public async Task<string> StringGet(string key)
        {
            return await ExecuteServiceRequestAsync(_stringRepository.StringGetAsync(key), nameof(StringGet));
        }

        public async Task StringSet(string key, string value)
        {
            await ExecuteServiceRequestAsync(_stringRepository.StringSetAsync(key, value), nameof(StringSet));
        }

        public async Task StringAppend(string key, string value)
        {
            await ExecuteServiceRequestAsync(_stringRepository.StringAppendAsync(key, value), nameof(StringAppend));
        }

        private async Task ExecuteServiceRequestAsync(Task task, string requestTypeName)
        {
            SFKVEventSource.Current.ServiceRequestStart(nameof(requestTypeName));

            try
            {
                await task;
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

        private async Task<T> ExecuteServiceRequestAsync<T>(Task<T> task, string requestTypeName)
        {
            SFKVEventSource.Current.ServiceRequestStart(nameof(requestTypeName));

            try
            {
                return await task;
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
            // Initialize the sfkv reliable dictionary.
            var repositoryType = typeof(IRepository);
            var allRepositoryTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => repositoryType.IsAssignableFrom(p));

            foreach (var type in allRepositoryTypes)
            {
                var name = type.GetProperty(nameof(IRepository.Name)).GetConstantValue().ToString();
                await StateManager.GetOrAddAsync<IReliableDictionary<string, string>>(name);
            }
        }
    }
}
