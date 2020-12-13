using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace cloudworkApi.Services
{
    public class ChatService
    {
        public static List<ChatSockets> sockets = new List<ChatSockets>();
        public async Task GetAndSendMessage(HttpContext context)
        {
            using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                var socketId = context.Request.Query["chatId"];

                var s = new ChatSockets();
                s.chatId = socketId;
                s.socket = webSocket;
                
                sockets.Add(s);

                var buffer = new byte[1024 * 4];
                
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {
                    var txt = Encoding.UTF8.GetString(buffer);
                    var bytes = Encoding.UTF8.GetBytes(txt);

                    //await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                    var opponentSockets = sockets.FindAll(x => x.chatId == socketId);
                    opponentSockets.ForEach(async s => // send everyone who has connection to this chatId (socketId), including himself(message sender too)
                    {
                        if (s.socket.State == WebSocketState.Open)
                            await s.socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    });

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
        }
    }
    public class ChatSockets
    {
        public string chatId { get; set; }
        public WebSocket socket { get; set; }
    }
}
