using System;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.IO;

namespace TheEyeTribeWebSocketServer
{
    public class EyeTribeClient
    {
		private TcpClient socket;
		private Timer timer;
		public event EventHandler<string> OnData;

		public EyeTribeClient(string host = "localhost", int port = 6555)
        {
			try
			{
				socket = new TcpClient(host, port);
			}
			catch
			{
			}

			var connect = "{\"values\":{\"push\":true,\"version\":1},\"category\":\"tracker\",\"request\":\"set\"}";
			Send(connect);

			var heartbeat = "{\"category\":\"heartbeat\",\"request\":null}";
			timer = new Timer(300);
			timer.Elapsed += (object sender, ElapsedEventArgs e) => Send(heartbeat);
			timer.Start();

			ListenerLoop();
        }

		private async void ListenerLoop()
		{
			var reader = new StreamReader(socket.GetStream());

			while (true)
			{
				var line = await reader.ReadLineAsync();

				if (OnData != null)
				{
					OnData(this, line);
				}
			}
		}

		private async void Send(string message)
		{
			if (socket == null || !socket.Connected)
			{
				return;
			}

			var writer = new StreamWriter(socket.GetStream());
			await writer.WriteLineAsync(message);
			await writer.FlushAsync();
		}

        public void Close()
        {
            timer.Stop();
            socket.Close();
        }
    }
}