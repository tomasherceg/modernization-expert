namespace Chapter02_Architecture;

public class GreetingService : IGreetingService
{
	public string Greet(string name)
	{
		return $"Hello, {name}!";
	}
}