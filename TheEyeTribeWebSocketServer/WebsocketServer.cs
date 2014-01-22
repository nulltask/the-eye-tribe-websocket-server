using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace TheEyeTribeWebSocketServer
{
    public class WebsocketServer
    {
        private List<WebSocket> clients = new List<WebSocket>();

        public WebsocketServer()
        {
        }

        public async void Start(string prefix)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                var context = await listener.GetContextAsync();
                Console.WriteLine("Got request.");

                if (context.Request.IsWebSocketRequest)
                {
                    HandleRequest(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        public void Close()
        {
            Parallel.ForEach(clients, ws =>
            {
                if (ws.State == WebSocketState.Open)
                {
                    ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", System.Threading.CancellationToken.None);
                }
            });
        }

        private async void HandleRequest(HttpListenerContext context)
        {
            Console.WriteLine("New Session.");
            var ws = (await context.AcceptWebSocketAsync(subProtocol: null)).WebSocket;
            clients.Add(ws);

            while (ws.State == WebSocketState.Open)
            {
                try
                {
                    var buf = new ArraySegment<byte>(new byte[1024]);
                    var ret = await ws.ReceiveAsync(buf, System.Threading.CancellationToken.None);

                    if (ret.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine("Session Close.");
                        break;
                    }
                    Console.WriteLine("Got Message.");
                }
                catch
                {
                    break;
                }
            }

            clients.Remove(ws);
            ws.Dispose();
        }

        public void Broadcast(string message)
        {
            var buf = new ArraySegment<byte>(System.Text.Encoding.ASCII.GetBytes(message));

            Parallel.ForEach(clients, ws =>
            {
                if (ws.State == WebSocketState.Open)
                {
                    ws.SendAsync(buf, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
                }
            });
        }
    }
}
