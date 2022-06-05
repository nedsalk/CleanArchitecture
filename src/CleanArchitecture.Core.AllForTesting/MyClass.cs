using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Core.AllForTesting;

public struct PersonId
{
}

public partial class MyClass : Entity
{
    public MyClass()
    {
        
    }
    private string Id { get; set; }
    public string PublicProp { get; private set; }

    public void AssignToId(string s)
    {
        Id = s;

        PublicProp = s;
    }
}