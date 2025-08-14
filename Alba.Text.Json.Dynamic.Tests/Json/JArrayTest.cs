using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic.Tests;

[TestOf(typeof(JArray))]
public class JArrayTest
{
    [Test]
    public void ReturnsValueByIndex()
    {
        dynamic d = new JArray(new(1, "str", Guid.Empty));
        ((object?)d[0]).Should().Be(1);
        ((object?)d[1]).Should().Be("str");
        ((object?)d[2]).Should().Be(Guid.Empty);
    }

    [Test]
    public void ReturnsValueOfJValueByIndex()
    {
        dynamic d = new JArray(new(JsonValue.Create(1), JsonValue.Create("str"), JsonValue.Create(Guid.Empty)));
        ((object?)d[0]).Should().Be(1);
        ((object?)d[1]).Should().Be("str");
        ((object?)d[2]).Should().Be(Guid.Empty);
    }

  #if NET5_0_OR_GREATER
    [Test]
    public void ReturnsValueOfJValueByIndexNet5()
    {
        dynamic d = new JArray(new(JsonValue.Create((Half)2)));
        ((object?)d[0]).Should().Be((Half)2);
    }
  #endif

  #if NET7_0_OR_GREATER
    [Test]
    public void ReturnsValueOfJValueByIndexNet7()
    {
        dynamic d = new JArray(new(JsonValue.Create(new Int128(20, 30))));
        ((object?)d[0]).Should().Be(new Int128(20, 30));
    }
  #endif
}