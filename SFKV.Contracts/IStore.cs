using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace SFKV.Contracts
{
    /// <summary>
    /// The service remoting interface for SFKV storage service.
    /// </summary>
    public interface IStore : IService
    {
        /// <summary>
        /// Add or update a key value pair to the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        Task StringSet(string key, string value);

        /// <summary>
        /// Get the value based on the key from the store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the value for the key.</returns>
        Task<string> StringGet(string key);
    }
}