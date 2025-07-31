using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Alba.Text.Json.Dynamic;

public class JNodeConverter(JNodeOptions? options = null) : JsonConverter<JNode?>
{
    public JNodeOptions? Options { get; } = options ?? JNodeOptions.Default;

    private static readonly JsonConverter<JsonNode?> NodeConverter = JsonMetadataServices.JsonNodeConverter;

    public override JNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        NodeConverter.Read(ref reader, typeToConvert, options).ToDynamic(Options);

    public override void Write(Utf8JsonWriter writer, JNode? value, JsonSerializerOptions options) =>
        NodeConverter.Write(writer, value?.NodeUntyped, options);
}