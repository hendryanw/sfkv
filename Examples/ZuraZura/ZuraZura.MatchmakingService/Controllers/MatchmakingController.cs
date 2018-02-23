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
            MatchmakingEventSource.Current.ServiceRequestStart(nameof(ListMembers), DateTime.Now.ToString("o"), null);

            try
            {
                return await GetMatchmakingMembersFromStoreAsync();
            }
            catch (Exception ex)
            {
                MatchmakingEventSource.Current.ServiceRequestError(nameof(ListMembers), ex.ToString());

                throw;
            }
            finally
            {
                MatchmakingEventSource.Current.ServiceRequestStop(nameof(ListMembers), DateTime.Now.ToString("o"));
            }
        }
        
        [HttpPost]
        public async Task Join([FromForm]string name)
        {
            MatchmakingEventSource.Current.ServiceRequestStart(nameof(Join), DateTime.Now.ToString("o"), JsonConvert.SerializeObject(new { name }));

            try
            {
                var currentMembers = await GetMatchmakingMembersFromStoreAsync();

                currentMembers.Add(name);
                await SetMatchmakingMembersToStoreAsync(currentMembers);
            }
            catch (Exception ex)
            {
                MatchmakingEventSource.Current.ServiceRequestError(nameof(Join), ex.ToString());

                throw;
            }
            finally
            {
                MatchmakingEventSource.Current.ServiceRequestStop(nameof(Join), DateTime.Now.ToString("o"));
            }
        }

        [HttpDelete("{name}")]
        public async Task Leave(string name)
        {
            MatchmakingEventSource.Current.ServiceRequestStart(nameof(Leave), DateTime.Now.ToString("o"), JsonConvert.SerializeObject(new { name }));

            try
            {
                var currentMembers = await GetMatchmakingMembersFromStoreAsync();

                currentMembers.Remove(name);
                await SetMatchmakingMembersToStoreAsync(currentMembers);
            }
            catch (Exception ex)
            {
                MatchmakingEventSource.Current.ServiceRequestError(nameof(Leave), ex.ToString());

                throw;
            }
            finally
            {
                MatchmakingEventSource.Current.ServiceRequestStop(nameof(Leave), DateTime.Now.ToString("o"));
            }
        }

        private async Task<MatchmakingMembers> GetMatchmakingMembersFromStoreAsync()
        {
            // The matchmaking controller use random secondary replicas for read operation.
            var storeProxy = CreateStoreProxy(TargetReplicaSelector.RandomSecondaryReplica);

            var result = await storeProxy.StringGet(_matchmakingStoreKey);

            if (result != null)
            {
                return JsonConvert.DeserializeObject<MatchmakingMembers>(result);
            }
            else
            {
                return new MatchmakingMembers();
            }
        }

        private Task SetMatchmakingMembersToStoreAsync(MatchmakingMembers members)
        {
            var storeProxy = CreateStoreProxy(TargetReplicaSelector.PrimaryReplica);
            return storeProxy.StringSet(_matchmakingStoreKey, JsonConvert.SerializeObject(members));
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
