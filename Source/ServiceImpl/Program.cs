using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Tandem.Web.Apps.ServiceImpl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .Build()
                .Run();
        }
    }
}
