using System.ServiceModel;

namespace CoreWcfClient;

[ServiceContract]
public interface ICalculatorService
{
    [OperationContract]
    int Add(int a, int b);
}