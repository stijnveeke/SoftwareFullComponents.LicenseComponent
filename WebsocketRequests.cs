using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SoftwareFullComponents.LicenseComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent
{
    public class WebsocketRequests : IWebsocketRequests
    {
        public async Task<ProductRead> GetProductById(string productSlug)
        {
            throw new NotImplementedException();
        }
        
        public async Task<UserRead> GetUserByUserId(string user_id)
        {
            Uri serviceUri = new Uri($"wss:://localhost:5003/WebSocket/ws/User/{user_id}");

            UserRead user = null;
            using (ClientWebSocket client = new ClientWebSocket())
            {
                var cts = new System.Threading.CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(120));

                await client.ConnectAsync(serviceUri, cts.Token);
                try
                {
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