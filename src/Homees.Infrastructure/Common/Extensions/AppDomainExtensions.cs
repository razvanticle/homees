namespace Homees.Infrastructure.Persistence;

public static class AppDomainExtensions
{
    public static Type? GetTypeByName(this AppDomain appDomain, string name)
    {
        return appDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes().Where(x => x.FullName == name || x.Name == name))
            .FirstOrDefault();
    }
}