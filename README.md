# SFKV
A fast and reliable remote key-value store for Microsoft Service Fabric using service remoting / rpc.
- SFKV is designed as a standalone application / service that can run on a Service Fabric cluster along with your applications / services.
- SFKV is able to reliably store your state as key-value pair. 
- SFKV supports sharding up to 10 partitions.
- SFKV is using native service fabric state manager and it is strong-consistent.
- SFKV supports read on secondary replicas.

# Getting Started
You need to refer the `SFKV.Contracts` assembly in order to use the key-value store. Currently there is no NuGet package published, so you have to build the solution yourself. I promise that the NuGet package will be published as soon as possible.

Once you have the referenced settled. You can create the client proxy of `SFKV.Contracts.IStore` instance by using the `ServiceProxy` class.
```
var storePrimary = ServiceProxy.Create<IStore>(
    new Uri("fabric:/SFKV/SFKV.Store"), 
    new ServicePartitionKey(0), 
    TargetReplicaSelector.PrimaryReplica);
```
The `ServiceProxy.Create` method takes 3 arguments, and also 1 optional argument that we did not use for now.

| Argument Name | Type | Description |
| ---- | ---- | ---- |
| serviceUri | Uri | This is the Service Fabric namespace for SFKV that is registered to the naming service when the service is started. If you didn't make any changes to the service manifest. the name of the service should be `fabric:/SFKV/SFKV.Store`. Behind the scenes, The `ServiceProxy` class will ask the naming service for the actual rpc endpoint. |
| partitionKey | ServicePartitionKey | This is the partition key which the data will be set or get. You can use up to 10 partitions by using `0` to `9` keys. How the data is sharded is the client's responsibility, so you should plan your data partitioning strategy before hand. For example you can use some kind of hashing algorithm that is uniformed and deterministic and modulo it by 10. |
| targetReplicaSelector | TargetReplicaSelector | This is an enum which you can use to select which replica that the service proxy client will target. For more information please refer to the following [documentation](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicefabric.services.communication.client.targetreplicaselector?view=azure-dotnet). |
