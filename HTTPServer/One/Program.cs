using System.Text;
using One;

MyServer server = new MyServer();
await server.RunServerAsync("../../../site", 8888);