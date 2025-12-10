using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Alba.Text.Json.Dynamic;

/// <summary><see cref="JsonConverter"/> for <see cref="JNode"/>.</summary>
/// <param name="options">Options to control the behavior.</param>
public sealed class JNodeConverter(JNodeOptions? options) : JsonConverter<JNode?>
{
    [SuppressMessage("ReSharper", "RedundantSuppressNullableWarningExpression", Justification = "Nullability fixed in JSON 8.0+")]
    private static readonly JsonConverter<JsonNode?> NodeConverter = JsonMetadataServices.JsonNodeConverter!;

    /// <summary>Options to control the behavior.</summary>
    public JNodeOptions? Options { get; } = options ?? JNodeOptions.Default;

    /// <summary>Create new <see cref="JNodeConverter"/> with default <see cref="JNodeOptions"/>.</summary>
    public JNodeConverter() : this(null) { }

    /// <inheritdoc/>
    public override JNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        NodeConverter.Read(ref reader, typeToConvert, options).ToDynamic(Options);

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, JNode? value, JsonSerializerOptions options) =>
        NodeConverter.Write(writer, value?.NodeUntyped, options);
}