using System.Reflection;
using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using trifenix.agro.functions;

[assembly: WebJobsStartup(typeof(SwashBuckleStartup))]
namespace trifenix.agro.functions {

    internal class SwashBuckleStartup : IWebJobsStartup {

        public void Configure(IWebJobsBuilder builder) {
            //Register the extension
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
        }

    }

}