using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SoftwareFullComponents.LicenseComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent
{
    public class WebsocketRequests : IWebsocketRequests
    {
        public async Task<IEnumerable<LicenseRead>> GetUsersForLicenseList(List<LicenseRead> licenses)
        {
            Uri serviceUri = new Uri($"wss://localhost:5003/WebSocket/ws/MultiUserCall/");
            List<LicenseRead> convertedLicenses = new List<LicenseRead>();
            using (ClientWebSocket client = new ClientWebSocket())
            {
                var cts = new System.Threading.CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(120));

                try
                {
                    await client.ConnectAsync(serviceUri, cts.Token);
                    while (client.State == WebSocketState.Open)
                    {
                        var responseBuffer = new byte[1024];
                        var offset = 0;
                        var packet = 1024;
                            
                        ArraySegment<byte> byteRecieved = new ArraySegment<byte>(responseBuffer, offset, packet);
                        WebSocketReceiveResult response = await client.ReceiveAsync(byteRecieved, cts.Token);
                        var responseAccepted = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                        
                        foreach (var license in licenses)
                        {
                            ArraySegment<byte> userIdToSend =
                                new ArraySegment<byte>(Encoding.UTF8.GetBytes(license.UserIdentifier));
                            await client.SendAsync(userIdToSend, WebSocketMessageType.Text, false, cts.Token);
                            
                            ArraySegment<byte> userBytesRecieved = new ArraySegment<byte>(responseBuffer, offset, packet);
                            WebSocketReceiveResult userResponse = await client.ReceiveAsync(userBytesRecieved, cts.Token);
                            var userResponseMessage = Encoding.UTF8.GetString(responseBuffer, offset, userResponse.Count);
                            
                            UserRead user = JsonSerializer.Deserialize<UserRead>(userResponseMessage);
                            license.User = user;
                            convertedLicenses.Add(license);
                        }
                        
                        ArraySegment<byte> finishMessage =
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes("Finished Sending!"));
                        await client.SendAsync(finishMessage, WebSocketMessageType.Text, true, cts.Token);
                        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "End this",
                            cts.Token);
                    }
                }
                catch (WebSocketException e)
                {
                    return null;
                }

                return convertedLicenses;
            }
        }
        
        public async Task<IEnumerable<LicenseRead>> GetProductsForLicenseList(List<LicenseRead> licenses)
        {
            Uri serviceUri = new Uri($"wss://localhost:5005/WebSocket/ws/MultiProductCall/");
            List<LicenseRead> convertedLicenses = new List<LicenseRead>();
            using (ClientWebSocket client = new ClientWebSocket())
            {
                var cts = new System.Threading.CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(120));

                try
                {
                    await client.ConnectAsync(serviceUri, cts.Token);
                    while (client.State == WebSocketState.Open)
                    {
                        var responseBuffer = new byte[1024];
                        var offset = 0;
                        var packet = 1024;
                            
                        ArraySegment<byte> byteRecieved = new ArraySegment<byte>(responseBuffer, offset, packet);
                        WebSocketReceiveResult response = await client.ReceiveAsync(byteRecieved, cts.Token);
                        var responseAccepted = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                        
                        foreach (var license in licenses)
                        {
                            if (license.ProductSlug == null)
                            {
                                convertedLicenses.Add(license);
                                continue;    
                            }
                            
                            ArraySegment<byte> productSlugToSend =
                                new ArraySegment<byte>(Encoding.UTF8.GetBytes(license.ProductSlug));
                            await client.SendAsync(productSlugToSend, WebSocketMessageType.Text, false, cts.Token);
                            
                            ArraySegment<byte> productBytesRecieved = new ArraySegment<byte>(responseBuffer, offset, packet);
                            WebSocketReceiveResult productResponse = await client.ReceiveAsync(productBytesRecieved, cts.Token);
                            var productResponseMessage = Encoding.UTF8.GetString(responseBuffer, offset, productResponse.Count);
                            
                            ProductRead product = JsonSerializer.Deserialize<ProductRead>(productResponseMessage);
                            license.Product = product;
                            convertedLicenses.Add(license);
                        }
                        
                        ArraySegment<byte> finishMessage =
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes("Finished Sending!"));
                        await client.SendAsync(finishMessage, WebSocketMessageType.Text, true, cts.Token);
                        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "End this",
                            cts.Token);
                    }
                }
                catch (WebSocketException e)
                {
                    return null;
                }

                return convertedLicenses;
            }
        }
        
        
        public async Task<ProductRead> GetProductById(string productSlug)
        {
            Uri serviceUri = new Uri($"wss://softwarefullproductcomponent.azurewebsites.net/WebSocket/ws/Product/{productSlug}");

            ProductRead product = null;
            using (ClientWebSocket client = new ClientWebSocket())
            {
                var cts = new System.Threading.CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(120));

                try
                {
                    await client.ConnectAsync(serviceUri, cts.Token);
                    while (client.State == WebSocketState.Open)
                    {
                        ArraySegment<byte> byteToSend =
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Get product by {productSlug}"));
                        await client.SendAsync(byteToSend, WebSocketMessageType.Text, true, cts.Token);
                        
                        var responseBuffer = new byte[1024];
                        var offset = 0;
                        var packet = 1024;
                        while (true)
                        {
                            ArraySegment<byte> byteRecieved = new ArraySegment<byte>(responseBuffer, offset, packet);
                            WebSocketReceiveResult response = await client.ReceiveAsync(byteRecieved, cts.Token);
                            var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                            if (responseMessage == "")
                            {
                                break;
                            }
                            product = JsonSerializer.Deserialize<ProductRead>(responseMessage);
                            
                            if (response.EndOfMessage)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (WebSocketException e)
                {
                    return null;
                }
            }

            return product;
        }
        
        public async Task<UserRead> GetUserByUserId(string user_id)
        {
            Uri serviceUri = new Uri($"wss://softwarefullusercomponent.azurewebsites.net/WebSocket/ws/User/{user_id}");

            UserRead user = null;
            using (ClientWebSocket client = new ClientWebSocket())
            {
                var cts = new System.Threading.CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(120));

                
                try
                {
                    await client.ConnectAsync(serviceUri, cts.Token);
                    while (client.State == WebSocketState.Open)
                    {
                        ArraySegment<byte> byteToSend =
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Get user by {user_id}"));
                        await client.SendAsync(byteToSend, WebSocketMessageType.Text, true, cts.Token);
                        
                        var responseBuffer = new byte[1024];
                        var offset = 0;
                        var packet = 1024;
                        while (true)
                        {
                            ArraySegment<byte> byteRecieved = new ArraySegment<byte>(responseBuffer, offset, packet);
                            WebSocketReceiveResult response = await client.ReceiveAsync(byteRecieved, cts.Token);
                            var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                            if (responseMessage == "")
                            {
                                break;
                            }
                            user = JsonSerializer.Deserialize<UserRead>(responseMessage);
                            
                            if (response.EndOfMessage)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (WebSocketException e)
                {
                    return null;
                }
            }

            return user;
        }
    }
}