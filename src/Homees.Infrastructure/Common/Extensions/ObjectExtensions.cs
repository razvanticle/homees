namespace Homees.Infrastructure.Persistence;

public static class ObjectExtensions
{
    public static string GetTypeName(this object obj)
    {
        var type = obj.GetType();
        
        return type.FullName!;
    }
}