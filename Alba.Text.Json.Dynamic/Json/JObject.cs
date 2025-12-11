using System.Dynamic;
using System.Text.Json.Nodes;
using Alba.Text.Json.Dynamic.Extensions;

namespace Alba.Text.Json.Dynamic;

/// <summary>Dynamic adapter for <see cref="JsonObject"/> with support for dynamic dispatch via <see cref="IDynamicMetaObjectProvider"/>.</summary>
/// <param name="source">Source <see cref="JsonObject"/>.</param>
/// <param name="options">Options to control the behavior.</param>
[SuppressMessage("Naming", "CA1710: Identifiers should have correct suffix", Justification = "This is not a collection")]
public sealed partial class JObject(JsonObject source, JNodeOptions? options = null)
    : JNode<JsonObject>(source, options), IDynamicMetaObjectProvider
{
    private static readonly MethodRef PGet = MethodRef.Of((JObject o) => o.Get(""));
    private static readonly MethodRef PSet = MethodRef.Of((JObject o) => o.Set("", MethodKey.GetT(0)));
    private static readonly MethodRef PAdd = MethodRef.Of((JObject o) => o.Add("", MethodKey.GetT(0)));
    private static readonly MethodRef PGetEnumerator = MethodRef.Of((JObject o) => o.GetEnumerator());

    private static readonly PropertyRef PNodeCount = PropertyRef.Of((JsonObject o) => o.Count);
    private static readonly MethodRef PNodeClear = MethodRef.Of((JsonObject o) => o.Clear());
    private static readonly MethodRef PNodeContainsKey = MethodRef.Of((JsonObject o) => o.ContainsKey(""));
    private static readonly MethodRef PNodeRemove = MethodRef.Of((JsonObject o) => o.Remove(""));

    /// <summary>Gets or sets the value of the specified property. If the property is not found, <see langword="null"/> is returned. The assigned value is converted to <see cref="JNode"/> using <see cref="ValueTypeExts.ToJsonNode{T}"/>.</summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is <see langword="null"/>.</exception>
    public object? this[string propertyName] {
        get => Get(propertyName);
        set => Set(propertyName, value);
    }

    /// <summary>Gets the number of elements contained in <see cref="JsonObject"/>.</summary>
    public int Count =>
        Node.Count;

    private object? Get(string propertyName) =>
        Node[propertyName].ToJNodeOrValue(Options);

    /// <summary>Returns the value of a property with the specified name.</summary>
    /// <param name="propertyName">The name of the property to return.</param>
    /// <param name="value">The value of the property with the specified name.</param>
    /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is <see langword="null"/>.</exception>
    /// <returns><see langword="true"/> if a property with the specified name was found; otherwise, <see langword="false"/>.</returns>
    public bool TryGet(string propertyName, out object? value)
    {
        var ret = Node.TryGetPropertyValue(propertyName, out var node);
        value = ret ? node.ToJNodeOrValue(Options) : null;
        return ret;
    }

    private void Set<T>(string propertyName, T value) =>
        Node[propertyName] = value.ToJsonNode(Node.Options);

    /// <summary>Adds an element with the provided property name and value to the <see cref="JObject"/>. The value is converted to <see cref="JNode"/> using <see cref="ValueTypeExts.ToJsonNode{T}"/>.</summary>
    /// <param name="propertyName">The property name of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="propertyName"/>is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">An element with the same property name already exists in the <see cref="JObject"/>.</exception>
    public void Add<T>(string propertyName, T value) =>
        Node.Add(propertyName, value.ToJsonNode(Node.Options));

    /// <summary>Removes the element with the specified property name from the <see cref="JObject"/>.</summary>
    /// <param name="propertyName">The property name of the element to remove.</param>
    /// <returns><see langword="true"/> if the element is successfully removed; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is <see langword="null"/>.</exception>
    public bool Remove(string propertyName) =>
        Node.Remove(propertyName);

    /// <summary>Removes all elements from the <see cref="JObject"/>.</summary>
    public void Clear() =>
        Node.Clear();

    /// <summary>Determines whether the <see cref="JObject"/> contains an element with the specified property name.</summary>
    /// <param name="propertyName">The property name to locate in the <see cref="JObject"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="JObject"/> contains an element with the specified property name; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is <see langword="null"/>.</exception>
    public bool ContainsKey(string propertyName) =>
        Node.ContainsKey(propertyName);

    /// <summary>Creates a new instance of the <see cref="JObject"/>. All children nodes are recursively cloned.</summary>
    /// <returns>A new cloned instance of the current node.</returns>
    public new JObject Clone() =>
        (JObject)base.Clone();

    /// <summary>Returns an enumerator that iterates through the <see cref="JObject"/>.</summary>
    /// <returns>An enumerator that iterates through the <see cref="JObject"/>.</returns>
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() =>
        Node.Select(p => new KeyValuePair<string, object?>(p.Key, p.Value.ToJNodeOrValue(Options)))
            .GetEnumerator();

    dobject IDynamicMetaObjectProvider.GetMetaObject(E expression) =>
        new MetaJObject(expression, this);

    private sealed class MetaJObject(E expression, JObject dynamicValue)
        : MetaJNode<JObject>(expression, dynamicValue)
    {
        public override dobject BindGetIndex(GetIndexBinder binder, dobject[] indexes) =>
            CallSelfMethod(PGet, [ indexes.Single().Expression ]);

        public override dobject BindSetIndex(SetIndexBinder binder, dobject[] indexes, dobject value) =>
            CallSelfMethod(PSet, [ indexes.Single().Expression, value.TypedExpression ], [ value.LimitType ]);

        public override dobject BindDeleteIndex(DeleteIndexBinder binder, dobject[] indexes) =>
            CallNodeMethod(PNodeRemove, [ indexes.Single().Expression ]);

        public override dobject BindGetMember(GetMemberBinder binder) =>
            CallSelfMethod(PGet, [ binder.Name.EConst() ]);

        public override dobject BindSetMember(SetMemberBinder binder, dobject value) =>
            CallSelfMethod(PSet, [ binder.Name.EConst(), value.TypedExpression ], [ value.LimitType ]);

        public override dobject BindDeleteMember(DeleteMemberBinder binder) =>
            CallNodeMethod(PNodeRemove, [ binder.Name.EConst() ]);

        public override dobject BindInvokeMember(InvokeMemberBinder binder, dobject[] args) =>
            binder.Name switch {
                nameof(Count) =>
                    ExprNode().EProperty(PNodeCount.Getter.Method).ToDObject(Value.Node.Count),
                nameof(Add) =>
                    CallSelfMethod(PAdd, args.SelectTypedExpressions(), args.SelectType(1)),
                nameof(Remove) =>
                    CallNodeMethod(PNodeRemove, args.SelectExpressions()),
                nameof(Clear) =>
                    CallNodeMethod(PNodeClear, [ ]),
                nameof(ContainsKey) =>
                    CallNodeMethod(PNodeContainsKey, args.SelectExpressions()),
                nameof(GetEnumerator) =>
                    CallSelfMethod(PGetEnumerator, [ ]),
                _ =>
                    base.BindInvokeMember(binder, args),
            };

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach ((string key, JsonNode? _) in Value.Node)
                yield return key;
        }
    }
}