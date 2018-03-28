using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Newtonsoft.Json;
using SFKV.Contracts;

namespace ZuraZura.MatchmakingService.Controllers
{
    /// <summary>
    /// The matchmaking members class contains the name of each participants.
    /// </summary>
    public class MatchmakingMembers : List<string>
    {
        public MatchmakingMembers()
        {
        }

        public MatchmakingMembers(string[] members)
            : base(members)
        {
        }
    }

    /// <summary>
    /// In this matchmaking controller, we are storing the temporary list of members who are joining the matchmaking in SFKV.
    /// </summary>
    [Route("api/[controller]")]
    public class MatchmakingController : Controller
    {
        private const string _matchmakingStoreKey = "zurazura.matchmaking";

        [HttpGet]
        public async Task<MatchmakingMembers> ListMembers()
        {
            // The matchmaking controller use random secondary replicas for read operation.
            var storeProxy = CreateStoreProxy(TargetReplicaSelector.RandomSecondaryReplica);
            var result = await storeProxy.SetGetAllAsync(_matchmakingStoreKey);

            if (result != null)
            {
                return new MatchmakingMembers(result);
            }
            else
            {
                return new MatchmakingMembers();
            }
        }

        [HttpPost]
        public async Task Join([FromForm]string name)
        {
            var storeProxy = CreateStoreProxy(TargetReplicaSelector.PrimaryReplica);
            await storeProxy.SetAddAsync(_matchmakingStoreKey, name);
        }

        [HttpDelete("{name}")]
        public async Task Leave(string name)
        {
            var storeProxy = CreateStoreProxy(TargetReplicaSelector.PrimaryReplica);
            await storeProxy.SetRemoveAsync(_matchmakingStoreKey, name);
        }

        private IStore CreateStoreProxy(TargetReplicaSelector targetReplicaSelector)
        {
            // The matchmaking service only store the matchmaking members in a single partition.
            return ServiceProxy.Create<IStore>(
                new Uri("fabric:/SFKV/SFKV.Store"),
                new ServicePartitionKey(0),
                targetReplicaSelector);
        }
    }
}
