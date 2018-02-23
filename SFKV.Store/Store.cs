using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly StateRepository _stateRepository;

        public Store(StatefulServiceContext context)
            : base(context)
        {
            _stateRepository = new StateRepository(StateManager);
        }

        public async Task<string> StringGet(string key)
        {
            SFKVEventSource.Current.ServiceRequestStart(nameof(StringGet));

            try
            {
                return await _stateRepository.StringGetAsync(key);
            }
            catch (Exception ex)
            {
                SFKVEventSource.Current.ServiceRequestError(nameof(StringGet), ex.ToString());

                // Propagate the exception to the client via service remoting.
                throw;
            }
            finally
            {
                SFKVEventSource.Current.ServiceRequestStop(nameof(StringGet));
            }
        }

        public async Task StringSet(string key, string value)
        {
            SFKVEventSource.Current.ServiceRequestStart(nameof(StringSet));

            try
            {
                await _stateRepository.StringSetAsync(key, value);
            }
            catch (Exception ex)
            {
                SFKVEventSource.Current.ServiceRequestError(nameof(StringSet), ex.ToString());

                // Propagate the exception to the client via service remoting.
                throw;
            }
            finally
            {
                SFKVEventSource.Current.ServiceRequestStop(nameof(StringSet));
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
        // protected override async Task RunAsync(CancellationToken cancellationToken)
        // {
        // }
    }
}
