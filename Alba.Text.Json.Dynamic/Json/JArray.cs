using System.Dynamic;
using System.Text.Json.Nodes;
using Alba.Framework;
using Alba.Text.Json.Dynamic.Extensions;

namespace Alba.Text.Json.Dynamic;

/// <summary>Dynamic adapter for <see cref="JsonArray"/> with support for dynamic dispatch via <see cref="IDynamicMetaObjectProvider"/>.</summary>
/// <param name="source">Source <see cref="JsonArray"/>.</param>
/// <param name="options">Options to control the behavior.</param>
public sealed partial class JArray(JsonArray source, JNodeOptions? options = null)
    : JNode<JsonArray>(source, options), IDynamicMetaObjectProvider
{
    private static readonly MethodRef PGetInt = MethodRef.Of((JArray o) => o.Get(0));
    private static readonly MethodRef PGetIndex = MethodRef.Of((JArray o) => o.Get(Index.Start));
    private static readonly MethodRef PSetInt = MethodRef.Of((JArray o) => o.Set(0, MethodKey.GetT(0)));
    private static readonly MethodRef PSetIndex = MethodRef.Of((JArray o) => o.Set(Index.Start, MethodKey.GetT(0)));
    private static readonly MethodRef PAdd = MethodRef.Of((JArray o) => o.Add(MethodKey.GetT(0)));
    private static readonly MethodRef PInsertInt = MethodRef.Of((JArray o) => o.Insert(0, MethodKey.GetT(0)));
    private static readonly MethodRef PInsertIndex = MethodRef.Of((JArray o) => o.Insert(Index.Start, MethodKey.GetT(0)));
    private static readonly MethodRef PRemove = MethodRef.Of((JArray o) => o.Remove(MethodKey.GetT(0)));
    private static readonly MethodRef PRemoveAtInt = MethodRef.Of((JArray o) => o.RemoveAt(0));
    private static readonly MethodRef PRemoveAtIndex = MethodRef.Of((JArray o) => o.RemoveAt(Index.Start));
    private static readonly MethodRef PContains = MethodRef.Of((JArray o) => o.Contains(MethodKey.GetT(0)));
    private static readonly MethodRef PIndexOf = MethodRef.Of((JArray o) => o.IndexOf(MethodKey.GetT(0)));
    private static readonly MethodRef PGetEnumerator = MethodRef.Of((JArray o) => o.GetEnumerator());

    private static readonly PropertyRef PNodeCount = PropertyRef.Of((JsonArray o) => o.Count);
    private static readonly MethodRef PNodeClear = MethodRef.Of((JsonArray o) => o.Clear());

    /// <summary>Gets or sets the value at the specified index. The assigned value is converted to <see cref="JNode"/> using <see cref="ValueTypeExts.ToJsonNode{T}"/>.</summary>
    /// <param name="index">The zero-based index of the value to get or set.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or <paramref name="index"/> is greater than the number of values.</exception>
    public object? this[int index] {
        get => Get(index);
        set => Set(index, value);
    }

    /// <summary>Gets or sets the value at the specified index. The assigned value is converted to <see cref="JNode"/> using <see cref="ValueTypeExts.ToJsonNode{T}"/>.</summary>
    /// <param name="index">The index of the value to get or set, which is either from the beginning or the end of the array.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or <paramref name="index"/> is greater than the number of values.</exception>
    public object? this[Index index] {
        get => Node[index].ToJNodeOrValue(Options);
        set => Node[index] = value.ToJsonNode(Node.Options);
    }

    /// <summary>Gets the number of values contained in the <see cref="JArray"/>.</summary>
    public int Count =>
        Node.Count;

    private object? Get(int index) =>
        Node[index].ToJNodeOrValue(Options);

    private object? Get(Index index) =>
        Node[index].ToJNodeOrValue(Options);

    private void Set<T>(int index, T item) =>
        Node[index] = item.ToJsonNode(Node.Options);

    private void Set<T>(Index index, T item) =>
        Node[index] = item.ToJsonNode(Node.Options);

    /// <summary>Adds a value to the end of the <see cref="JArray"/>. The value is converted to <see cref="JNode"/> using <see cref="ValueTypeExts.ToJsonNode{T}"/>.</summary>
    /// <param name="item">The value to be added to the end of the <see cref="JArray"/>.</param>
    public void Add<T>(T item) =>
        Node.Add(item.ToJsonNode(Node.Options));

    /// <summary>Inserts a value into the <see cref="JArray"/> at the specified index. The value is converted to <see cref="JNode"/> using <see cref="ValueTypeExts.ToJsonNode{T}"/>.</summary>
    /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
    /// <param name="item">The value to insert.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or <paramref name="index"/> is greater than <see cref="Count"/>.</exception>
    public void Insert<T>(int index, T item) =>
        Node.Insert(index, item.ToJsonNode(Node.Options));

    /// <summary>Inserts a value into the <see cref="JArray"/> at the specified index. The value is converted to <see cref="JNode"/> using <see cref="ValueTypeExts.ToJsonNode{T}"/>.</summary>
    /// <param name="index">The index at which <paramref name="item"/> should be inserted, which is either from the beginning or the end of the array.</param>
    /// <param name="item">The value to insert.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or <paramref name="index"/> is greater than <see cref="Count"/>.</exception>
    public void Insert<T>(Index index, T item) =>
        Node.Insert(index.GetOffset(Node.Count), item.ToJsonNode(Node.Options));

    /// <summary>Removes the first occurrence of a specific value from the <see cref="JArray"/>. The value is searched using <see cref="JNodeOptions.SearchEquality"/> comparison kind.</summary>
    /// <param name="item">The value to remove from the <see cref="JArray"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="item"/> is successfully removed; otherwise, <see langword="false"/>.</returns>
    public bool Remove<T>(T item) =>
        Node.Remove(item, Options);

    /// <summary>Removes the value at the specified index of the <see cref="JArray"/>.</summary>
    /// <param name="index">The zero-based index of the value to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or <paramref name="index"/> is greater than <see cref="Count"/>.</exception>
    public void RemoveAt(int index) =>
        Node.RemoveAt(index);

    /// <summary>Removes the value at the specified index of the <see cref="JArray"/>.</summary>
    /// <param name="index">The index of the value to remove, which is either from the beginning or the end of the array.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or <paramref name="index"/> is greater than <see cref="Count"/>.</exception>
    public void RemoveAt(Index index) =>
        Node.RemoveAt(index.GetOffset(Node.Count));

    /// <summary>Removes all values from the <see cref="JArray"/>.</summary>
    public void Clear() =>
        Node.Clear();

    /// <summary>Determines whether a value is in the <see cref="JArray"/>. The value is searched using <see cref="JNodeOptions.SearchEquality"/> comparison kind.</summary>
    /// <param name="item">The object to locate in the <see cref="JArray"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="JArray"/>; otherwise, <see langword="false"/>.</returns>
    public bool Contains<T>(T item) =>
        Node.Contains(item, Options);

    /// <summary>The object to locate in the <see cref="JArray"/>. The value is searched using <see cref="JNodeOptions.SearchEquality"/> comparison kind.</summary>
    /// <param name="item">The <see cref="JNode"/> to locate in the <see cref="JArray"/>.</param>
    /// <returns>The index of item if found in the list; otherwise, -1.</returns>
    public int IndexOf<T>(T item) =>
        Node.IndexOf(item, Options);

    /// <summary>Creates a new instance of the <see cref="JArray"/>. All children nodes are recursively cloned.</summary>
    /// <returns>A new cloned instance of the current node.</returns>
    public new JArray Clone() =>
        (JArray)base.Clone();

    /// <summary>Returns an enumerator that iterates through the <see cref="JArray"/>.</summary>
    /// <returns>An <see cref="IEnumerator{Object}"/> for the <see cref="JArray"/>.</returns>
    public IEnumerator<object?> GetEnumerator() =>
        Node.Select(n => n.ToJNodeOrValue(Options)).GetEnumerator();

    dobject IDynamicMetaObjectProvider.GetMetaObject(E expression) =>
        new MetaJArray(expression, this);

    private sealed class MetaJArray(E expression, JArray dynamicValue)
        : MetaJNode<JArray>(expression, dynamicValue)
    {
        public override dobject BindGetIndex(GetIndexBinder binder, dobject[] indexes) =>
            CallSelfMethod(SelectIndexMethod(indexes, 1, PGetInt, PGetIndex),
                indexes.SelectExpressions());

        public override dobject BindSetIndex(SetIndexBinder binder, dobject[] indexes, dobject value) =>
            CallSelfMethod(SelectIndexMethod(indexes, 1, PSetInt, PSetIndex),
                [ indexes[0].Expression, value.TypedExpression ], [ value.LimitType ]);

        public override dobject BindDeleteIndex(DeleteIndexBinder binder, dobject[] indexes) =>
            CallSelfMethod(SelectIndexMethod(indexes, 1, PRemoveAtInt, PRemoveAtIndex),
                indexes.SelectExpressions());

        public override dobject BindInvokeMember(InvokeMemberBinder binder, dobject[] args) =>
            binder.Name switch {
                nameof(Count) =>
                    ExprNode().EProperty(PNodeCount.Getter.Method).ToDObject(Value.Node.Count),
                nameof(Add) =>
                    CallSelfMethod(PAdd, args.SelectTypedExpressions(), args.SelectTypes()),
                nameof(Insert) =>
                    CallSelfMethod(
                        SelectIndexMethod(args, 2, PInsertInt, PInsertIndex),
                        [ args[0].Expression, args[1].TypedExpression ], args.SelectType(1)),
                nameof(Remove) =>
                    CallSelfMethod(PRemove, args.SelectTypedExpressions(), args.SelectTypes()),
                nameof(RemoveAt) =>
                    CallSelfMethod(
                        SelectIndexMethod(args, 1, PRemoveAtInt, PRemoveAtIndex),
                        args.SelectExpressions()),
                nameof(Contains) =>
                    CallSelfMethod(PContains, args.SelectTypedExpressions(), args.SelectTypes()),
                nameof(IndexOf) =>
                    CallSelfMethod(PIndexOf, args.SelectTypedExpressions(), args.SelectTypes()),
                nameof(Clear) =>
                    CallNodeMethod(PNodeClear, [ ]),
                nameof(GetEnumerator) =>
                    CallSelfMethod(PGetEnumerator, [ ]),
              #if JSON10_0_OR_GREATER
                nameof(RemoveAll) =>
                    CallSelfMethod(PRemoveAll, args.SelectExpressions()),
                nameof(RemoveRange) =>
                    CallNodeMethod(PNodeRemoveRange, args.SelectExpressions()),
              #endif
                _ =>
                    base.BindInvokeMember(binder, args),
            };

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var count = Value.Node.Count;
            for (int i = 0; i < count; i++)
                yield return $"{i}";
        }

        private static MethodRef SelectIndexMethod(dobject[] indexes, int count,
            MethodRef intMethod, MethodRef indexMethod) =>
            Ensure.Count(indexes, count) switch {
                [ var index, .. ] => index switch {
                    { HasValue: true, Value: var v } => v switch {
                        int => intMethod,
                        Index => indexMethod,
                        _ => throw IndexArgumentType(),
                    },
                    { LimitType: var t } =>
                        t == typeof(int) ? intMethod :
                        t == typeof(Index) ? indexMethod :
                        throw IndexArgumentType(),
                },
                _ => throw new ArgumentException("One index expected.", nameof(indexes)),
            };

        [SuppressMessage("ReSharper", "NotResolvedInText")]
        private static ArgumentException IndexArgumentType() => new("Argument index must be Int32 or Index.", "index");
    }
}