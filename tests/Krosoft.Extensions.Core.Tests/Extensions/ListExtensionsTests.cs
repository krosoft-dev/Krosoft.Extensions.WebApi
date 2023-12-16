using System.Collections.ObjectModel;
using Krosoft.Extensions.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class ListExtensionsTests
{
    private readonly string[] _data = { "apple", "orange", "banana" };

    [TestMethod]
    public void AddRange_Integer_CheckContains()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        list.AddRange(new List<int> { 3, 4, 5 }, true);

        // Assert
        Check.That(list).ContainsExactly(1, 2, 3, 4, 5);
    }

    [TestMethod]
    public void AddRange_Integer_NotCheckContains()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 8, 9, 8, 9 };

        // Act
        list.AddRange(new List<int> { 3, 4, 5 }, false);

        // Assert
        Check.That(list).ContainsExactly(1, 2, 3, 8, 9, 8, 9, 3, 4, 5);
    }

    [TestMethod]
    public void AddRange_String_CheckContains()
    {
        // Arrange
        var list = new List<string> { "apple", "aaa", "orange", "bbbb" };

        // Act
        list.AddRange(new List<string> { "orange", "banana", "bbbb" }, true);

        // Assert
        Check.That(list).ContainsExactly("apple", "aaa", "orange", "bbbb", "banana");
    }

    [TestMethod]
    public void ToDictionary_ShouldReturnCorrectDictionary()
    {
        // Act
        var result = _data.ToDictionary(x => x.Length, true);

        // Assert
        Check.That(result).IsEqualTo(new Dictionary<int, string> { { 5, "apple" }, { 6, "orange" } });
    }

    [TestMethod]
    public void ToDictionaryWithModifier_ShouldReturnCorrectDictionary()
    {
        Check.ThatCode(() => new List<string> { "apple", "orange", "apple", "orange", "banana" }.ToDictionary(x => x.Length.ToString(), x => x.ToUpper(), false))
             .Throws<ArgumentException>()
             .WithMessage("An item with the same key has already been added. Key: 5");
    }

    [TestMethod]
    public void ToDictionaryWithModifier_ShouldReturnCorrectDictionary_Distinct()
    {
        // Act
        var result = new List<string> { "apple", "orange", "apple", "orange", "banana" }.ToDictionary(x => x.Length.ToString(), x => x.ToUpper(), true);

        // Assert
        Check.That(result).ContainsExactly(new Dictionary<string, string> { { "5", "apple" }, { "6", "orange" } });
    }

    [TestMethod]
    public void ToReadOnlyDictionary_ShouldReturnCorrectReadOnlyDictionary()
    {
        Check.ThatCode(() => _data.ToReadOnlyDictionary(x => x.Length, false))
             .Throws<ArgumentException>()
             .WithMessage("An item with the same key has already been added. Key: 6");
    }

    [TestMethod]
    public void ToReadOnlyDictionary_ShouldReturnCorrectReadOnlyDictionary_Distinct()
    {
        // Act
        var result = _data.ToReadOnlyDictionary(x => x.Length, true);

        // Assert
        Check.That(result).IsEqualTo(new ReadOnlyDictionary<int, string>(new Dictionary<int, string> { { 5, "apple" }, { 6, "orange" } }));
    }
}