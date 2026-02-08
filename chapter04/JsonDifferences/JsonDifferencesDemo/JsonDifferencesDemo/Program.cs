var zoo = new Zoo()
{
    Animals =
    [
        new Dog() { Name = "Fido", Breed = "Golden Retriever" },
        new Cat() { Name = "Whiskers", FluffinessLevel = 10 }
    ]
};

Console.WriteLine("Newtonsoft.Json result");
Console.WriteLine("===");
Console.WriteLine(
    Newtonsoft.Json.JsonConvert.SerializeObject(zoo, 
        Newtonsoft.Json.Formatting.Indented));
Console.WriteLine();
Console.WriteLine();

Console.WriteLine("System.Text.Json result");
Console.WriteLine("===");
Console.WriteLine(
    System.Text.Json.JsonSerializer.Serialize(zoo, 
        new System.Text.Json.JsonSerializerOptions() { WriteIndented = true }));
Console.WriteLine();
Console.WriteLine();

public class Zoo
{
    public Animal[] Animals { get; set; }
}

// Without adding these attributes, System.Text.Json will
// only serialize the Name property from the base class
//[System.Text.Json.Serialization.JsonDerivedType(typeof(Dog))]
//[System.Text.Json.Serialization.JsonDerivedType(typeof(Cat))]
public abstract class Animal
{
    public string Name { get; set; }
}
public class Dog : Animal
{
    public string Breed { get; set; }
}
public class Cat : Animal
{
    public int FluffinessLevel { get; set; }
}