using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using SFKV.Contracts;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;

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
            _stateRepository = new StateRepository(this.StateManager);
        }

        public async Task<string> StringGet(string key)
        {
            return await _stateRepository.StringGetAsync(key);
        }

        public async Task StringSet(string key, string value)
        {
            await _stateRepository.StringSetAsync(key, value);
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
