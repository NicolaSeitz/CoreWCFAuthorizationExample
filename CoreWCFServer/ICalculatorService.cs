using CoreWCF;

namespace CoreWCFServer;

[ServiceContract]
public interface ICalculatorService
{
	[OperationContract]
	int Add(int a, int b);
}