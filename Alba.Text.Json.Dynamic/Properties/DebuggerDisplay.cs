using System.Diagnostics;
using System.Dynamic;

// ReSharper disable CheckNamespace

[assembly: DebuggerDisplay("{LimitType.Name,nq} {Value}.{Expression}", Type = T.Short, Target = typeof(dobject))]

[assembly: DebuggerDisplay($"{B.Op}", Type = T.Short, Target = typeof(BinaryOperationBinder))]
[assembly: DebuggerDisplay("""{Explicit ? "explicit" : "implicit"} {Type}""", Type = T.Short, Target = typeof(ConvertBinder))]
[assembly: DebuggerDisplay($".ctor {B.ArgsFn}", Type = T.Short, Target = typeof(CreateInstanceBinder))]
[assembly: DebuggerDisplay($"delete {B.ArgsIdx}", Type = T.Short, Target = typeof(DeleteIndexBinder))]
[assembly: DebuggerDisplay($"delete {B.Name}", Type = T.Short, Target = typeof(DeleteMemberBinder))]
[assembly: DebuggerDisplay($"{B.ArgsIdx}", Type = T.Short, Target = typeof(GetIndexBinder))]
[assembly: DebuggerDisplay($"{B.Name}", Type = T.Short, Target = typeof(GetMemberBinder))]
[assembly: DebuggerDisplay($"{B.ArgsFn}", Type = T.Short, Target = typeof(InvokeBinder))]
[assembly: DebuggerDisplay($"{B.Name}{B.ArgsFn}", Type = T.Short, Target = typeof(InvokeMemberBinder))]
[assembly: DebuggerDisplay($"{B.ArgsIdx}=", Type = T.Short, Target = typeof(SetIndexBinder))]
[assembly: DebuggerDisplay($"{B.Name}=", Type = T.Short, Target = typeof(SetMemberBinder))]
[assembly: DebuggerDisplay($"{B.Op}", Type = T.Short, Target = typeof(UnaryOperationBinder))]

file static class T
{
    public const string Short = "{GetType().Name,nq}";

    //void f(UnaryOperationBinder b) => b.
}

file static class B
{
    public const string Name = ".{Name,nq}";
    public const string Op = "{Operation}";
    public const string ArgsFn = "({CallInfo.ArgumentNames})";
    public const string ArgsIdx = "[{CallInfo.ArgumentNames}]";
}