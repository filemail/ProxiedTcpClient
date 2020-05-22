# ProxiedTcpClient
Easy to use TcpClient helper library that can create TcpClient connected through proxy server.

## TcpClient factory method

Automatically detects system proxy settings and creates TcpClient that uses proxy if configured in the system

```csharp
TcpClient client = Filemail.ProxiedTcpClient.ProxiedTcpClient.Create(destinationUri);
```

## Create proxied TcpClient

```csharp
TcpClient client = Filemail.ProxiedTcpClient.ProxiedTcpClient.Create(proxyUri, destinationUri);
```

## Get system proxy

```csharp
var proxyStatus = Filemail.ProxiedTcpClient.ProxiedTcpClient.GetProxyStatusFor(destinationUri);

Console.WriteLine(proxyStatus.IsEnabled);
Console.WriteLine(proxyStatus.Uri);
```
