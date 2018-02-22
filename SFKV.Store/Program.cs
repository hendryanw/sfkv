using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;

namespace SFKV.Store
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync("SFKV.StoreType",
                    context => new Store(context)).GetAwaiter().GetResult();

                SFKVEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Store).Name);

                // Prevents this host process from terminating so services keep running.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                SFKVEventSource.Current.ServiceHostInitializationFailed(e.ToString());

                throw;
            }
        }
    }
}
