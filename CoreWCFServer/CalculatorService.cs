using CoreWCF;
using Microsoft.AspNetCore.Authorization;

namespace CoreWCFServer;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class CalculatorService : ICalculatorService
{
	[Authorize(policy: "TestPolicy")]
	public int Add(int a, int b)
	{
		return a + b;
	}
}