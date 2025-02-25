using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CoreWcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var binding = new BasicHttpBinding()
            {
                Security = new BasicHttpSecurity()
                {
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = HttpClientCredentialType.Windows
                    }
                }
            };

            var endpoint = new EndpointAddress("http://localhost:5000/CalculatorService");

            using var channelFactory = new ChannelFactory<ICalculatorService>(binding, endpoint);
            channelFactory.Credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            var client = channelFactory.CreateChannel();

            Console.WriteLine("Calling Add service...");
            var result = client.Add(5, 10);
            Console.WriteLine($"Result: {result}");
        }
    }
}