using System.Text.Json.Serialization;

namespace Chapter02_SourceGenerators;

[JsonSerializable(typeof(CustomerModel))]
[JsonSerializable(typeof(ProductModel))]
partial class GeneratedJsonContext : JsonSerializerContext
{
}

public class CustomerModel
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
}

public class ProductModel
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }
	public int Stock { get; set; }
}