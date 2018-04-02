[![Build Status](https://hendryanwar.visualstudio.com/_apis/public/build/definitions/5ce4cc85-dce0-4265-ab08-97950ba30968/1/badge)](https://hendryanwar.visualstudio.com/SFKV/_build/index?definitionId=1)

# SFKV
A fast and reliable remote key-value store for Microsoft Service Fabric via service remoting.
- SFKV is designed as a standalone application / service that can run on a Service Fabric cluster along with your applications / services.
- SFKV is able to reliably store your state as key-value pair. 
- SFKV supports sharding up to 10 partitions.
- SFKV is using native service fabric state manager and it is strong-consistent.
- SFKV supports read on secondary replicas.

# Getting Started
- Deploy SFKV to your Service Fabric cluster.
- Add SFKV.Contracts NuGet package to your service from https://www.nuget.org/packages/SFKV.Contracts.
- Use SFKV by creating a client proxy instance of `SFKV.Contracts.IStore`. Please read through for more details.

# Usage
You can create the client proxy of `SFKV.Contracts.IStore` instance by using the `ServiceProxy` class.
```
var storePrimary = ServiceProxy.Create<IStore>(
    new Uri("fabric:/SFKV/SFKV.Store"), 
    new ServicePartitionKey(0), 
    TargetReplicaSelector.PrimaryReplica);
    
await storePrimary.StringSetAsync("key", "value");
var val = await storePrimary.StringGetAsync("key");
```
The `ServiceProxy.Create` method takes 3 arguments, and also 1 optional argument that we did not use for now.

| Argument Name | Type | Description |
| ---- | ---- | ---- |
| serviceUri | Uri | This is the Service Fabric namespace for SFKV that is registered to the naming service when the service is started. If you didn't make any changes to the service manifest. the name of the service should be `fabric:/SFKV/SFKV.Store`. Behind the scenes, The `ServiceProxy` class will ask the naming service for the actual service endpoint. |
| partitionKey | ServicePartitionKey | This is the partition key which the data will be set or get. You can use up to 10 partitions by using `0` to `9` keys. How the data is sharded is the client's responsibility, so you should plan your data partitioning strategy before hand. For example you can use some kind of hashing algorithm that is uniformed and deterministic and modulo it by 10. |
| targetReplicaSelector | TargetReplicaSelector | This is an enum which you can use to select which replica that the service proxy client will target. For more information please refer to the following [documentation](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicefabric.services.communication.client.targetreplicaselector?view=azure-dotnet). |

# Data Types
## String
TODO

## Int
TODO

## Hash
TODO

## List
TODO

## Set
TODO

<a name='contents'></a>
# Service Interface [#](#contents 'Go To Here')

- [IStore](#T-SFKV-Contracts-IStore 'SFKV.Contracts.IStore')
  - [HashDeleteAsync(key)](#M-SFKV-Contracts-IStore-HashDeleteAsync-System-String- 'SFKV.Contracts.IStore.HashDeleteAsync(System.String)')
  - [HashExistsAsync(key)](#M-SFKV-Contracts-IStore-HashExistsAsync-System-String- 'SFKV.Contracts.IStore.HashExistsAsync(System.String)')
  - [HashGetAllAsync(key)](#M-SFKV-Contracts-IStore-HashGetAllAsync-System-String- 'SFKV.Contracts.IStore.HashGetAllAsync(System.String)')
  - [HashGetAsync(key,field)](#M-SFKV-Contracts-IStore-HashGetAsync-System-String,System-String- 'SFKV.Contracts.IStore.HashGetAsync(System.String,System.String)')
  - [HashMultipleGetAsync(key,fields)](#M-SFKV-Contracts-IStore-HashMultipleGetAsync-System-String,System-String[]- 'SFKV.Contracts.IStore.HashMultipleGetAsync(System.String,System.String[])')
  - [HashMultipleSetAsync(key,keyValuePairs)](#M-SFKV-Contracts-IStore-HashMultipleSetAsync-System-String,System-Collections-Generic-KeyValuePair{System-String,System-String}[]- 'SFKV.Contracts.IStore.HashMultipleSetAsync(System.String,System.Collections.Generic.KeyValuePair{System.String,System.String}[])')
  - [HashSetAsync(key,keyValuePair)](#M-SFKV-Contracts-IStore-HashSetAsync-System-String,System-Collections-Generic-KeyValuePair{System-String,System-String}- 'SFKV.Contracts.IStore.HashSetAsync(System.String,System.Collections.Generic.KeyValuePair{System.String,System.String})')
  - [IntDeleteAsync(key)](#M-SFKV-Contracts-IStore-IntDeleteAsync-System-String- 'SFKV.Contracts.IStore.IntDeleteAsync(System.String)')
  - [IntExistsAsync(key)](#M-SFKV-Contracts-IStore-IntExistsAsync-System-String- 'SFKV.Contracts.IStore.IntExistsAsync(System.String)')
  - [IntGetAsync(key)](#M-SFKV-Contracts-IStore-IntGetAsync-System-String- 'SFKV.Contracts.IStore.IntGetAsync(System.String)')
  - [IntIncrAsync(key)](#M-SFKV-Contracts-IStore-IntIncrAsync-System-String- 'SFKV.Contracts.IStore.IntIncrAsync(System.String)')
  - [IntIncrByAsync(key,incrBy)](#M-SFKV-Contracts-IStore-IntIncrByAsync-System-String,System-Int32- 'SFKV.Contracts.IStore.IntIncrByAsync(System.String,System.Int32)')
  - [IntMultipleSetAsync(keyValuePairs)](#M-SFKV-Contracts-IStore-IntMultipleSetAsync-System-Collections-Generic-KeyValuePair{System-String,System-Int32}[]- 'SFKV.Contracts.IStore.IntMultipleSetAsync(System.Collections.Generic.KeyValuePair{System.String,System.Int32}[])')
  - [IntSetAsync(key,value)](#M-SFKV-Contracts-IStore-IntSetAsync-System-String,System-Int32- 'SFKV.Contracts.IStore.IntSetAsync(System.String,System.Int32)')
  - [ListAddFirstAsync(key,value)](#M-SFKV-Contracts-IStore-ListAddFirstAsync-System-String,System-String- 'SFKV.Contracts.IStore.ListAddFirstAsync(System.String,System.String)')
  - [ListAddLastAsync(key,value)](#M-SFKV-Contracts-IStore-ListAddLastAsync-System-String,System-String- 'SFKV.Contracts.IStore.ListAddLastAsync(System.String,System.String)')
  - [ListAddMultipleFirstAsync(key,values)](#M-SFKV-Contracts-IStore-ListAddMultipleFirstAsync-System-String,System-String[]- 'SFKV.Contracts.IStore.ListAddMultipleFirstAsync(System.String,System.String[])')
  - [ListAddMultipleLastAsync(key,values)](#M-SFKV-Contracts-IStore-ListAddMultipleLastAsync-System-String,System-String[]- 'SFKV.Contracts.IStore.ListAddMultipleLastAsync(System.String,System.String[])')
  - [ListDeleteAsync(key)](#M-SFKV-Contracts-IStore-ListDeleteAsync-System-String- 'SFKV.Contracts.IStore.ListDeleteAsync(System.String)')
  - [ListExistsAsync(key)](#M-SFKV-Contracts-IStore-ListExistsAsync-System-String- 'SFKV.Contracts.IStore.ListExistsAsync(System.String)')
  - [ListPopFirstAsync(key)](#M-SFKV-Contracts-IStore-ListPopFirstAsync-System-String- 'SFKV.Contracts.IStore.ListPopFirstAsync(System.String)')
  - [ListPopLastAsync(key)](#M-SFKV-Contracts-IStore-ListPopLastAsync-System-String- 'SFKV.Contracts.IStore.ListPopLastAsync(System.String)')
  - [ListRangeAsync(key,firstIndex,lastIndex)](#M-SFKV-Contracts-IStore-ListRangeAsync-System-String,System-Int32,System-Int32- 'SFKV.Contracts.IStore.ListRangeAsync(System.String,System.Int32,System.Int32)')
  - [SetAddAsync(key,value)](#M-SFKV-Contracts-IStore-SetAddAsync-System-String,System-String- 'SFKV.Contracts.IStore.SetAddAsync(System.String,System.String)')
  - [SetAddMultipleAsync(key,values)](#M-SFKV-Contracts-IStore-SetAddMultipleAsync-System-String,System-String[]- 'SFKV.Contracts.IStore.SetAddMultipleAsync(System.String,System.String[])')
  - [SetContainsAsync(key,value)](#M-SFKV-Contracts-IStore-SetContainsAsync-System-String,System-String- 'SFKV.Contracts.IStore.SetContainsAsync(System.String,System.String)')
  - [SetCountAsync(key)](#M-SFKV-Contracts-IStore-SetCountAsync-System-String- 'SFKV.Contracts.IStore.SetCountAsync(System.String)')
  - [SetDeleteAsync(key)](#M-SFKV-Contracts-IStore-SetDeleteAsync-System-String- 'SFKV.Contracts.IStore.SetDeleteAsync(System.String)')
  - [SetExistsAsync(key)](#M-SFKV-Contracts-IStore-SetExistsAsync-System-String- 'SFKV.Contracts.IStore.SetExistsAsync(System.String)')
  - [SetGetAllAsync(key)](#M-SFKV-Contracts-IStore-SetGetAllAsync-System-String- 'SFKV.Contracts.IStore.SetGetAllAsync(System.String)')
  - [SetGetAsync(key,count)](#M-SFKV-Contracts-IStore-SetGetAsync-System-String,System-Int32- 'SFKV.Contracts.IStore.SetGetAsync(System.String,System.Int32)')
  - [SetRemoveAsync(key,value)](#M-SFKV-Contracts-IStore-SetRemoveAsync-System-String,System-String- 'SFKV.Contracts.IStore.SetRemoveAsync(System.String,System.String)')
  - [SetRemoveMultipleAsync(key,values)](#M-SFKV-Contracts-IStore-SetRemoveMultipleAsync-System-String,System-String[]- 'SFKV.Contracts.IStore.SetRemoveMultipleAsync(System.String,System.String[])')
  - [StringAppendAsync(key,value)](#M-SFKV-Contracts-IStore-StringAppendAsync-System-String,System-String- 'SFKV.Contracts.IStore.StringAppendAsync(System.String,System.String)')
  - [StringDeleteAsync(key)](#M-SFKV-Contracts-IStore-StringDeleteAsync-System-String- 'SFKV.Contracts.IStore.StringDeleteAsync(System.String)')
  - [StringExistsAsync(key)](#M-SFKV-Contracts-IStore-StringExistsAsync-System-String- 'SFKV.Contracts.IStore.StringExistsAsync(System.String)')
  - [StringGetAsync(key)](#M-SFKV-Contracts-IStore-StringGetAsync-System-String- 'SFKV.Contracts.IStore.StringGetAsync(System.String)')
  - [StringMultipleSetAsync(keyValuePairs)](#M-SFKV-Contracts-IStore-StringMultipleSetAsync-System-Collections-Generic-KeyValuePair{System-String,System-String}[]- 'SFKV.Contracts.IStore.StringMultipleSetAsync(System.Collections.Generic.KeyValuePair{System.String,System.String}[])')
  - [StringSetAsync(key,value)](#M-SFKV-Contracts-IStore-StringSetAsync-System-String,System-String- 'SFKV.Contracts.IStore.StringSetAsync(System.String,System.String)')

<a name='assembly'></a>
# SFKV.Contracts [#](#assembly 'Go To Here') [=](#contents 'Back To Contents')

<a name='T-SFKV-Contracts-IStore'></a>
## IStore [#](#T-SFKV-Contracts-IStore 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

SFKV.Contracts

##### Summary

The service remoting interface for SFKV storage service.

<a name='M-SFKV-Contracts-IStore-HashDeleteAsync-System-String-'></a>
### HashDeleteAsync(key) `method` [#](#M-SFKV-Contracts-IStore-HashDeleteAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Delete the specified key and its value from the store.

##### Returns

Returns a boolean indicating the key value pair is removed from the store.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-HashExistsAsync-System-String-'></a>
### HashExistsAsync(key) `method` [#](#M-SFKV-Contracts-IStore-HashExistsAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Check whether the specified key exists on the store.

##### Returns

Returns a boolean indicating the key existence.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-HashGetAllAsync-System-String-'></a>
### HashGetAllAsync(key) `method` [#](#M-SFKV-Contracts-IStore-HashGetAllAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Get the hash.

##### Returns

Returns the hash.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The hash key. |

<a name='M-SFKV-Contracts-IStore-HashGetAsync-System-String,System-String-'></a>
### HashGetAsync(key,field) `method` [#](#M-SFKV-Contracts-IStore-HashGetAsync-System-String,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Get the value of a hash field.

##### Returns

Returns the value of the hash field.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The hash key. |
| field | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The hash field. |

<a name='M-SFKV-Contracts-IStore-HashMultipleGetAsync-System-String,System-String[]-'></a>
### HashMultipleGetAsync(key,fields) `method` [#](#M-SFKV-Contracts-IStore-HashMultipleGetAsync-System-String,System-String[]- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Get the values of multiple hash fields.

##### Returns

Returns the values of the multiple hash fields. Returns empty value if the field is not found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The hash key. |
| fields | [System.String[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String[] 'System.String[]') | The hash fields. |

<a name='M-SFKV-Contracts-IStore-HashMultipleSetAsync-System-String,System-Collections-Generic-KeyValuePair{System-String,System-String}[]-'></a>
### HashMultipleSetAsync(key,keyValuePairs) `method` [#](#M-SFKV-Contracts-IStore-HashMultipleSetAsync-System-String,System-Collections-Generic-KeyValuePair{System-String,System-String}[]- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Set multiple hash field and value pairs.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The hash key. |
| keyValuePairs | [System.Collections.Generic.KeyValuePair{System.String,System.String}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.KeyValuePair 'System.Collections.Generic.KeyValuePair{System.String,System.String}[]') | Multiple hash field and value pairs. |

<a name='M-SFKV-Contracts-IStore-HashSetAsync-System-String,System-Collections-Generic-KeyValuePair{System-String,System-String}-'></a>
### HashSetAsync(key,keyValuePair) `method` [#](#M-SFKV-Contracts-IStore-HashSetAsync-System-String,System-Collections-Generic-KeyValuePair{System-String,System-String}- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Set the hash field and value pair.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The hash key. |
| keyValuePair | [System.Collections.Generic.KeyValuePair{System.String,System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.KeyValuePair 'System.Collections.Generic.KeyValuePair{System.String,System.String}') | The hash field and value pair. |

<a name='M-SFKV-Contracts-IStore-IntDeleteAsync-System-String-'></a>
### IntDeleteAsync(key) `method` [#](#M-SFKV-Contracts-IStore-IntDeleteAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Delete the specified key and its value from the store.

##### Returns

Returns a boolean indicating the key value pair is removed from the store.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-IntExistsAsync-System-String-'></a>
### IntExistsAsync(key) `method` [#](#M-SFKV-Contracts-IStore-IntExistsAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Check whether the specified key exists on the store.

##### Returns

Returns a boolean indicating the key existence.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-IntGetAsync-System-String-'></a>
### IntGetAsync(key) `method` [#](#M-SFKV-Contracts-IStore-IntGetAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Get the integer value based on the key from the store.

##### Returns

Returns the integer value for the key.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-IntIncrAsync-System-String-'></a>
### IntIncrAsync(key) `method` [#](#M-SFKV-Contracts-IStore-IntIncrAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Increment the integer value by 1.

##### Returns

Returns the incremented value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-IntIncrByAsync-System-String,System-Int32-'></a>
### IntIncrByAsync(key,incrBy) `method` [#](#M-SFKV-Contracts-IStore-IntIncrByAsync-System-String,System-Int32- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Increment the integer value with specified number.

##### Returns

Returns the incremented value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| incrBy | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The number which will be incremented to value. |

<a name='M-SFKV-Contracts-IStore-IntMultipleSetAsync-System-Collections-Generic-KeyValuePair{System-String,System-Int32}[]-'></a>
### IntMultipleSetAsync(keyValuePairs) `method` [#](#M-SFKV-Contracts-IStore-IntMultipleSetAsync-System-Collections-Generic-KeyValuePair{System-String,System-Int32}[]- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add or update multiple key and integer value pairs to the store.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| keyValuePairs | [System.Collections.Generic.KeyValuePair{System.String,System.Int32}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.KeyValuePair 'System.Collections.Generic.KeyValuePair{System.String,System.Int32}[]') | The key and integer value pairs. |

<a name='M-SFKV-Contracts-IStore-IntSetAsync-System-String,System-Int32-'></a>
### IntSetAsync(key,value) `method` [#](#M-SFKV-Contracts-IStore-IntSetAsync-System-String,System-Int32- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add or update a key and integer value pair to the store.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| value | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The integer value. |

<a name='M-SFKV-Contracts-IStore-ListAddFirstAsync-System-String,System-String-'></a>
### ListAddFirstAsync(key,value) `method` [#](#M-SFKV-Contracts-IStore-ListAddFirstAsync-System-String,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add the value at the first position of the list.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The value. |

<a name='M-SFKV-Contracts-IStore-ListAddLastAsync-System-String,System-String-'></a>
### ListAddLastAsync(key,value) `method` [#](#M-SFKV-Contracts-IStore-ListAddLastAsync-System-String,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add the value at the last position of the list.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The value. |

<a name='M-SFKV-Contracts-IStore-ListAddMultipleFirstAsync-System-String,System-String[]-'></a>
### ListAddMultipleFirstAsync(key,values) `method` [#](#M-SFKV-Contracts-IStore-ListAddMultipleFirstAsync-System-String,System-String[]- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add multiple values at the first position of the list.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| values | [System.String[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String[] 'System.String[]') | The values. |

<a name='M-SFKV-Contracts-IStore-ListAddMultipleLastAsync-System-String,System-String[]-'></a>
### ListAddMultipleLastAsync(key,values) `method` [#](#M-SFKV-Contracts-IStore-ListAddMultipleLastAsync-System-String,System-String[]- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add multiple values at the last position of the list.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| values | [System.String[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String[] 'System.String[]') | The values. |

<a name='M-SFKV-Contracts-IStore-ListDeleteAsync-System-String-'></a>
### ListDeleteAsync(key) `method` [#](#M-SFKV-Contracts-IStore-ListDeleteAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Delete the specified key and its value from the store.

##### Returns

Returns a boolean indicating the key value pair is removed from the store.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-ListExistsAsync-System-String-'></a>
### ListExistsAsync(key) `method` [#](#M-SFKV-Contracts-IStore-ListExistsAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Check whether the specified key exists on the store.

##### Returns

Returns a boolean indicating the key existence.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-ListPopFirstAsync-System-String-'></a>
### ListPopFirstAsync(key) `method` [#](#M-SFKV-Contracts-IStore-ListPopFirstAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Remove and return the first value of the list.

##### Returns

Returns the first value of the list.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-ListPopLastAsync-System-String-'></a>
### ListPopLastAsync(key) `method` [#](#M-SFKV-Contracts-IStore-ListPopLastAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Remove and return the last value of the list.

##### Returns

Returns the last value of the list.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-ListRangeAsync-System-String,System-Int32,System-Int32-'></a>
### ListRangeAsync(key,firstIndex,lastIndex) `method` [#](#M-SFKV-Contracts-IStore-ListRangeAsync-System-String,System-Int32,System-Int32- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

List the values by its index position.

##### Returns

Returns the list of values.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| firstIndex | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The first index of the value range. |
| lastIndex | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The last index of the value range. If less than 0 is specified, will return the values until the end of the list. |

<a name='M-SFKV-Contracts-IStore-SetAddAsync-System-String,System-String-'></a>
### SetAddAsync(key,value) `method` [#](#M-SFKV-Contracts-IStore-SetAddAsync-System-String,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add a value into the set.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The value. |

<a name='M-SFKV-Contracts-IStore-SetAddMultipleAsync-System-String,System-String[]-'></a>
### SetAddMultipleAsync(key,values) `method` [#](#M-SFKV-Contracts-IStore-SetAddMultipleAsync-System-String,System-String[]- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add multiple values into the set.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| values | [System.String[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String[] 'System.String[]') | The value. |

<a name='M-SFKV-Contracts-IStore-SetContainsAsync-System-String,System-String-'></a>
### SetContainsAsync(key,value) `method` [#](#M-SFKV-Contracts-IStore-SetContainsAsync-System-String,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Check whether the value is a member of the set.

##### Returns

Returns a boolean indicating whether the value is a member of the set.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The value. |

<a name='M-SFKV-Contracts-IStore-SetCountAsync-System-String-'></a>
### SetCountAsync(key) `method` [#](#M-SFKV-Contracts-IStore-SetCountAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Get the number of elements in a set.

##### Returns

Returns the number of elements in a set.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-SetDeleteAsync-System-String-'></a>
### SetDeleteAsync(key) `method` [#](#M-SFKV-Contracts-IStore-SetDeleteAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Delete the specified key and its value from the store.

##### Returns

Returns a boolean indicating the key value pair is removed from the store.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-SetExistsAsync-System-String-'></a>
### SetExistsAsync(key) `method` [#](#M-SFKV-Contracts-IStore-SetExistsAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Check whether the specified key exists on the store.

##### Returns

Returns a boolean indicating the key existence.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-SetGetAllAsync-System-String-'></a>
### SetGetAllAsync(key) `method` [#](#M-SFKV-Contracts-IStore-SetGetAllAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Get all values in a set.

##### Returns

Returns all values in a set.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-SetGetAsync-System-String,System-Int32-'></a>
### SetGetAsync(key,count) `method` [#](#M-SFKV-Contracts-IStore-SetGetAsync-System-String,System-Int32- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Get specified number of values in a set.

##### Returns

Returns the values in the specified set.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| count | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The number of values to be retrieved. |

<a name='M-SFKV-Contracts-IStore-SetRemoveAsync-System-String,System-String-'></a>
### SetRemoveAsync(key,value) `method` [#](#M-SFKV-Contracts-IStore-SetRemoveAsync-System-String,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Remove a value from the set.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The value. |

<a name='M-SFKV-Contracts-IStore-SetRemoveMultipleAsync-System-String,System-String[]-'></a>
### SetRemoveMultipleAsync(key,values) `method` [#](#M-SFKV-Contracts-IStore-SetRemoveMultipleAsync-System-String,System-String[]- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Remove multiple values from the set.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| values | [System.String[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String[] 'System.String[]') | The values. |

<a name='M-SFKV-Contracts-IStore-StringAppendAsync-System-String,System-String-'></a>
### StringAppendAsync(key,value) `method` [#](#M-SFKV-Contracts-IStore-StringAppendAsync-System-String,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Append the value at the end of existing string if exists. Otherwise the string will be created.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The value which will be appended. |

<a name='M-SFKV-Contracts-IStore-StringDeleteAsync-System-String-'></a>
### StringDeleteAsync(key) `method` [#](#M-SFKV-Contracts-IStore-StringDeleteAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Delete the specified key and its value from the store.

##### Returns

Returns a boolean indicating the key value pair is removed from the store.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-StringExistsAsync-System-String-'></a>
### StringExistsAsync(key) `method` [#](#M-SFKV-Contracts-IStore-StringExistsAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Check whether the specified key exists on the store.

##### Returns

Returns a boolean indicating the key existence.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-StringGetAsync-System-String-'></a>
### StringGetAsync(key) `method` [#](#M-SFKV-Contracts-IStore-StringGetAsync-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Get the value based on the key from the store.

##### Returns

Returns the value for the key.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |

<a name='M-SFKV-Contracts-IStore-StringMultipleSetAsync-System-Collections-Generic-KeyValuePair{System-String,System-String}[]-'></a>
### StringMultipleSetAsync(keyValuePairs) `method` [#](#M-SFKV-Contracts-IStore-StringMultipleSetAsync-System-Collections-Generic-KeyValuePair{System-String,System-String}[]- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add or update multiple key value pairs to the store.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| keyValuePairs | [System.Collections.Generic.KeyValuePair{System.String,System.String}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.KeyValuePair 'System.Collections.Generic.KeyValuePair{System.String,System.String}[]') | The key value pairs. |

<a name='M-SFKV-Contracts-IStore-StringSetAsync-System-String,System-String-'></a>
### StringSetAsync(key,value) `method` [#](#M-SFKV-Contracts-IStore-StringSetAsync-System-String,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Add or update a key value pair to the store.

##### Returns

Returns awaitable task.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key. |
| value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The value. |

# Releases
TODO
