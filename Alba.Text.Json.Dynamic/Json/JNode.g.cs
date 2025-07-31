#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Alba.Text.Json.Dynamic;

public partial class JNode
{
    private const string SerializationUnreferencedCodeMessage = "JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.";
    private const string SerializationRequiresDynamicCodeMessage = "JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.";

    /// <summary>Parses one JSON value (including objects or arrays) from the provided reader.</summary>
    /// <param name="reader">The reader to read.</param>
    /// <param name="nodeOptions">Options to control the behavior.</param>
    /// <exception cref="T:System.ArgumentException"> <paramref name="reader" /> is using unsupported options.</exception>
    /// <exception cref="T:System.ArgumentException">The current <paramref name="reader" /> token does not start or represent a value.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">A value could not be read from the reader.</exception>
    /// <returns>The <see cref="T:System.Text.Json.Nodes.JsonNode" /> from the reader, or null if the input represents the null JSON value.</returns>
    public static dynamic? Parse(ref Utf8JsonReader reader, JsonNodeOptions? nodeOptions = default) =>
        JsonNode.Parse(ref reader, nodeOptions).ToDynamic();

    /// <summary>Parses text representing a single JSON value.</summary>
    /// <param name="json">JSON text to parse.</param>
    /// <param name="nodeOptions">Options to control the node behavior after parsing.</param>
    /// <param name="documentOptions">Options to control the document behavior during parsing.</param>
    /// <exception cref="T:System.ArgumentNullException"> <paramref name="json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException"> <paramref name="json" /> does not represent a valid single JSON value.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the JSON value, or null if the input represents the null JSON value.</returns>
    public static dynamic? Parse([StringSyntax("Json")] string json, JsonNodeOptions? nodeOptions = default, JsonDocumentOptions documentOptions = default) =>
        JsonNode.Parse(json, nodeOptions, documentOptions).ToDynamic();

    /// <summary>Parses text representing a single JSON value.</summary>
    /// <param name="utf8Json">JSON text to parse.</param>
    /// <param name="nodeOptions">Options to control the node behavior after parsing.</param>
    /// <param name="documentOptions">Options to control the document behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException"> <paramref name="utf8Json" /> does not represent a valid single JSON value.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the JSON value, or null if the input represents the null JSON value.</returns>
    public static dynamic? Parse(ReadOnlySpan<byte> utf8Json, JsonNodeOptions? nodeOptions = default, JsonDocumentOptions documentOptions = default) =>
        JsonNode.Parse(utf8Json, nodeOptions, documentOptions).ToDynamic();

    /// <summary>Parses a <see cref="T:System.IO.Stream" /> as UTF-8-encoded data representing a single JSON value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />. The Stream will be read to completion.</summary>
    /// <param name="utf8Json">JSON text to parse.</param>
    /// <param name="nodeOptions">Options to control the node behavior after parsing.</param>
    /// <param name="documentOptions">Options to control the document behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException"> <paramref name="utf8Json" /> does not represent a valid single JSON value.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the JSON value, or null if the input represents the null JSON value.</returns>
    public static dynamic? Parse(Stream utf8Json, JsonNodeOptions? nodeOptions = default, JsonDocumentOptions documentOptions = default) =>
        JsonNode.Parse(utf8Json, nodeOptions, documentOptions).ToDynamic();

    /// <summary>Parses a <see cref="T:System.IO.Stream" /> as UTF-8 encoded data representing a single JSON value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />. The stream will be read to completion.</summary>
    /// <param name="utf8Json">The JSON text to parse.</param>
    /// <param name="nodeOptions">Options to control the node behavior after parsing.</param>
    /// <param name="documentOptions">Options to control the document behavior during parsing.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="T:System.Text.Json.JsonException"> <paramref name="utf8Json" /> does not represent a valid single JSON value.</exception>
    /// <exception cref="T:System.OperationCanceledException">The cancellation token was canceled. This exception is stored into the returned task.</exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> to produce either a <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the JSON value, or null if the input represents the null JSON value.</returns>
    public static async Task<dynamic?> ParseAsync(Stream utf8Json, JsonNodeOptions? nodeOptions = default, JsonDocumentOptions documentOptions = default, CancellationToken cancellationToken = default) =>
        (await JsonNode.ParseAsync(utf8Json, nodeOptions, documentOptions, cancellationToken)).ToDynamic();

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode(SerializationUnreferencedCodeMessage)]
    [RequiresDynamicCode(SerializationRequiresDynamicCodeMessage)]
    public static dynamic? From<TValue>(TValue value, JsonSerializerOptions? options = default) =>
        JsonSerializer.SerializeToNode<TValue>(value, options).ToDynamic();

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <exception cref="T:System.ArgumentException"> <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException"> <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the value.</returns>
    [RequiresUnreferencedCode(SerializationUnreferencedCodeMessage)]
    [RequiresDynamicCode(SerializationRequiresDynamicCodeMessage)]
    public static dynamic? From(object? value, Type inputType, JsonSerializerOptions? options = default) =>
        JsonSerializer.SerializeToNode(value, inputType, options).ToDynamic();

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <exception cref="T:System.ArgumentNullException"> <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the value.</returns>
    public static dynamic? From<TValue>(TValue value, JsonTypeInfo<TValue> jsonTypeInfo) =>
        JsonSerializer.SerializeToNode<TValue>(value, jsonTypeInfo).ToDynamic();

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <exception cref="T:System.ArgumentNullException"> <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidCastException"> <paramref name="value" /> does not match the type of <paramref name="jsonTypeInfo" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the value.</returns>
    public static dynamic? From(object? value, JsonTypeInfo jsonTypeInfo) =>
        JsonSerializer.SerializeToNode(value, jsonTypeInfo).ToDynamic();

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <exception cref="T:System.ArgumentNullException"> <paramref name="inputType" /> or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the value.</returns>
    public static dynamic? From(object? value, Type inputType, JsonSerializerContext context) =>
        JsonSerializer.SerializeToNode(value, inputType, context).ToDynamic();

}