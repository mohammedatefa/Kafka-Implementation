using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text;

namespace Consumer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private static readonly ConcurrentBag<StreamWriter> clients = new ConcurrentBag<StreamWriter>();

        [HttpGet("stream")]
        public async Task GetStream(CancellationToken cancellationToken)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");

            var clientStream = new StreamWriter(Response.Body, Encoding.UTF8, leaveOpen: true);
            clients.Add(clientStream);

            cancellationToken.Register(() => clients.TryTake(out _));

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await clientStream.WriteAsync("data: Test message\n\n");
                    await clientStream.FlushAsync();
                    await Task.Delay(1000); // Send a message every 10 seconds
                }
            }
            catch (Exception ex)
            {
                // Handle disconnection logic
                clients.TryTake(out _);
            }
        }

        public static void SendNotification(string message)
        {
            foreach (var client in clients)
            {
                try
                {
                    client.WriteAsync($"data: {message}\n\n").GetAwaiter().GetResult();
                    client.FlushAsync().GetAwaiter().GetResult();
                }
                catch
                {
                    // Handle client disconnection
                }
            }
        }
    }
}
