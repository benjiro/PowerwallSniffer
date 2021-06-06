namespace PowerwallSniffer
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            var hostedService = new HostBuilder().BuildApplication(args);

            await hostedService.RunConsoleAsync();
        }
    }
}