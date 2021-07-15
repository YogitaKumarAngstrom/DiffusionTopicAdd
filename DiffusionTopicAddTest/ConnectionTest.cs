using NUnit.Framework;
using System.IO;
using PushTechnology.ClientInterface.Client.Factories;
using Microsoft.Extensions.Configuration;

namespace DiffusionTopicAddTest
{
    public class Tests
    {

        
        [Test]
        public void TestConnection()
        {
            //var config = new ConfigurationBuilder()
            //    .AddJsonFile("app.config")
            //    .Build();

           //var username = config["Principal"];

            var username = System.Configuration.ConfigurationManager.AppSettings["Principal"];
            var pwd = System.Configuration.ConfigurationManager.AppSettings["Password"];
            var host = System.Configuration.ConfigurationManager.AppSettings["Host"];
            var session = Diffusion.Sessions
             .Principal(username)
            .Password(pwd)
            .Open(host);

            bool connected = session.State.Connected;

            Assert.IsTrue(connected);
        }

    }
}