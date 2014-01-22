using System;
using System.Net.Sockets;

namespace TheEyeTribeWebSocketServer
{
	class Program
	{
		public static void Main(string[] args)
		{
			var server = new WebsocketServer();
			server.Start("http://+:6556/");

			var client = new EyeTribeClient();
			client.OnData += (object sender, string e) =>
			{
				Console.WriteLine(e);
				server.Broadcast(e);
			};

			Console.WriteLine("Press any key to exit...");
			Console.ReadLine();

            server.Close();
            client.Close();
		}
	}
}