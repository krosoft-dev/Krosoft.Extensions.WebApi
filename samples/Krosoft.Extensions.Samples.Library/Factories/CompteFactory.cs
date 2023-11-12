using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Samples.Library.Factories;

public static class CompteFactory
{
    public static Task<IEnumerable<Compte>> ToCompteAsync(IEnumerable<string> strings)
    {
        var comptes = new List<Compte>();
        foreach (var s in strings)
        {
            comptes.Add(new Compte
            {
                Name = s
            });
        }

        return Task.FromResult(comptes.AsEnumerable());
    }
}