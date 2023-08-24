using Grpc.Core;
using Grpc.Net.Client;
using GrpcService1;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/Test")]
    public class TestController : ControllerBase
    {
        [HttpGet("Test")]
        public async Task<string> Test() => $"test";

        [HttpGet("TestGrpc")]
        public async Task<string> TestGrpc()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7065");
            
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var client = new Greeter.GreeterClient(channel);

                var call = await client.SayHelloAsync(new HelloRequest { Name = "test" });

                return call.Message;
            }
            catch (RpcException ex)// when (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                Console.WriteLine("Service timeout.");
            }

            //watch.Stop();
            //var elapsedMs = watch.Elapsed.TotalMinutes;

            //Console.WriteLine($"Stream ended: Total Records:{Count.ToString()} in {watch.Elapsed.TotalMinutes} minutes and {watch.Elapsed.TotalSeconds} seconds.");
            //Console.Read();

            return "a";
        }
    }
}
