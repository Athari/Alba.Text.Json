Alba.Text.Json
==============

Extensions for System.Text.Json.

Alba.Text.Json.Dynamic
----------------------

Operating on `JsonNode` using `dynamic` type, as if it's JavaScript.

<img src=".nuget/Logo512.png" width=256 align=left style="margin: 0 20px 0 0">

```cs
dynamic json = JNode.Parse("""
    { "Hello": { "World": [ 1, 2, 3 ] } }
    """);

json.Hello.World[2]++.

Console.WriteLine(json.Hello.World[2]);
```

<br clear=all>

### System.Text.Json version compatibility

When using **System.Text.Json** of the same version as .NET, reference **Alba.Text.Json.Dynamic**, otherwise reference highest compatible version of **Alba.Text.Json.Dynamic.STJ#**.

Using the latest version of System.Text.Json is recommended not only because it provides more functionality, but also because newer versions include significant performance improvements.

* **Alba.Text.Json.Dynamic**: non-vulnerable bundled version:
   * .NET 6 & 7: System.Text.Json 6.0.10+
   * .NET 8: System.Text.Json 8.0.5+
   * .NET 9+: System.Text.Json 9.0.0+
   * .NET Standard 2.0 & 2.1, .NET Framework 4.6.2: 9.0.0+
* **Alba.Text.Json.Dynamic.STJ6**: System.Text.Json 6.0.0+
* **Alba.Text.Json.Dynamic.STJ8**: System.Text.Json 8.0.0+
* **Alba.Text.Json.Dynamic.STJ9**: System.Text.Json 9.0.0+

> [!CAUTION]
> Version-specific **Alba.Text.Json.Dynamic.STJ#** packages are not restricted to non-vulnerable versions of System.Text.Json. Make sure to reference an up-to-date version if you find that kind of thing important.