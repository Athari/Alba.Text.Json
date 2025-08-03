using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Alba.Text.Json.Dynamic;

Trace.Listeners.Add(new ConsoleTraceListener());

var values = new object?[] {
    TimeSpan.FromDays(1, 2),
    new byte[] { 1, 2, 3 },
    new sbyte[] { 1, 2, 3 },
    DateTime.UtcNow,
    DateTimeOffset.UtcNow,
    Guid.CreateVersion7(),
    new Version(10, 11, 12, 13),
    new Uri("urn:foo:bar"),
    (Half)20,
    (Int128)2000000,
    TimeOnly.FromTimeSpan(TimeSpan.FromHours(1.2)),
    DateOnly.FromDayNumber(3),
    JsonSerializer.Deserialize<JsonNode>("[ 2, 3 ]"),
    JsonSerializer.Deserialize<JsonArray>("[ 2, 3 ]"),
    JsonSerializer.Deserialize<JsonElement>("20"),
    JsonDocument.Parse("[ 2, 3, 4 ]"),
    'a', "b", 2, 3m,
    (9, 8, "c"),
    Tuple.Create(11, 10, "d"),
    new { a = 5, b = 6 },
    (c: 7, d: 8),
}.Select(o => new VS(
    o: o,
    v: Attempt(() => JsonValue.Create(o)),
    d: Attempt(() => (JsonValue)JsonValue.Create((dynamic)o!)),
    s: Attempt(() => JsonSerializer.SerializeToNode(o))
)).ToList();

//values.ForEach(WriteLine);

var jo = JsonNode.Parse("""
    {
      "abc": 1,
      "abcd": 2,
      "Foo": "Bar",
      "Pos1": { "x": 10, "y": 20 },
      "Pos2": { "x": 50, "y": 60 },
      "Pos3": { "x": 50, "y": 60 },
      "Arr": [ 2, 3, 4 ],
    }
    """,
    new() {
        PropertyNameCaseInsensitive = true,
    }, new() {
        AllowTrailingCommas = true,
    })!;
dynamic json = new JObject((JsonObject)jo, new() {
    IsCaseSensitive = false,
});

WriteLine($"abc = {json.abc}");
WriteLine($"abcd = {json.abcd}");
WriteLine($"foo = {json.Foo}");
WriteLine($"Pos1.x = {json.Pos1.x}");
WriteLine($"(int)Pos1.x + (int)Pos2.x = {(int)json.Pos1.x + (int)json.Pos2.x}");
WriteLine($"(float)Pos1.y / (float)Pos2.y = {(float)json.Pos1.x / (float)json.Pos2.x}");
WriteLine($"Pos1.x + Pos2.x = {json.Pos1.x + json.Pos2.x}");
WriteLine($"Pos1 == Pos2 = {json.Pos1 == json.Pos2}");
WriteLine($"Pos2 == Pos3 = {json.Pos2 == json.Pos3}");
WriteLine($"Arr[1] = {json.Arr[1]}");

json.Arr[1]++;
json.Arr[1] += 10;
json.Arr[^1]++;
json.Arr[0]++;
json.Arr.RemoveAt(^1);
json.Arr.RemoveAt(0);
json.Hi = "Hi";
json["Hi2"] = "Hi2";
//json.Hi3 = new { Hi3 = 3 };
//json.Hi4 = KeyValuePair.Create(99, "_99");
//json.Hi4 = new DictionaryEntry(99, "_99");
//json.Hi4.Key--;
json.World = 100m;
json.Pos1.Clear();
json.Pos1.Add("null", null);
json.Pos2.Add("null", null);
json.Pos2.Add("10", 10);
json.Pos4 = json.Pos3;
json.Points = new object[] { json.Pos1, json.Pos2, json.Pos1 };
json.Points.Remove(json.Points[0]);
WriteLine($"{json.Hi} {json.World}!");
WriteLine(jo.ToJsonString(new() { WriteIndented = true }));

WriteLine("Done!");
//ReadLine();

//dynamic o = 1;
//o.ͱtree = 1;
//o.λtree = 1;
//o.ψtree = 1;
//o.ϕtree = 1;
//o.ϵtree = 1;
//o.Єtree = 1;
//o.єtree = 1;
//o.Эtree = 1;
//o.эtree = 1;
//o.ᑌᑎᑐᑕ = 1;
//o.ᕫᕬᕭᕮ = 1;
//o.ᐯᐱᐳᐸ = 1;
//o.ᘮᘯᘰᘳ = 1;
//o.ᙀᙁᙂᙅ = 1;
//o.ⴲⴱⵁⵔⵕⵙⵝⵠⵦⴷⴸⴹⴺ = 1;
//o.ᴥtree = 1;
//o.ⳬtree = 1;
//o.Ꙭtree = 1;
//o.〱tree = 1;
//o.〳tree = 1;
//o.〵tree = 1;
//o.ーtree = 1;
//o.ꘌtree = 1;

object? Attempt(Func<object?> f)
{
    try {
        return f();
    }
    catch {
        return "<ERROR>";
    }
}

namespace Alba.Text.Json.Dynamic
{
    record VS(object? o, object? v, object? d, object? s)
    {
        public override string ToString()
        {
            return $"{S(o)} => {S(v)} {S(d)} {S(s)}";
            static string S(object? o) => ReferenceEquals(o, "<ERROR>") ? $"{o}" : $"[{o?.GetType().Name ?? ""}] {o}";
        }
    }
}