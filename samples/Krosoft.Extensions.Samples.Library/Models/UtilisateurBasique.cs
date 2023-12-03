namespace Krosoft.Extensions.Samples.Library.Models;

public class UtilisateurBasique : IEquatable<UtilisateurBasique>
{
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public DateTime DateModification { get; set; }

    public bool Equals(UtilisateurBasique? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Nom == other.Nom && Prenom == other.Prenom;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((UtilisateurBasique)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Nom != null
                        ? Nom.GetHashCode()
                        : 0) *
                    397) ^
                   (Prenom != null
                       ? Prenom.GetHashCode()
                       : 0);
        }
    }
}