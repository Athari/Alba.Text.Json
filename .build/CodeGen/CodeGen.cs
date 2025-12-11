//using System.Text.Json.Nodes;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;
//using System.Text.Json;
//using System.Text.RegularExpressions;
//using System.Xml.Linq;
//using System.Xml.XPath;
//using Attr = System.Reflection.CustomAttributeData;
//using AttrArg = System.Reflection.CustomAttributeTypedArgument;
//using AttrNamedArg = System.Reflection.CustomAttributeNamedArgument;

//namespace Alba.Text.Json.Dynamic;

//public enum Nullability : byte
//{
//    Unknown = 0,
//    NotNull,
//    Nullable,
//    Unset = byte.MaxValue,
//}

//// ReSharper disable once CanSimplifyDictionaryTryGetValueWithGetValueOrDefault

//file static class Exts
//{
//    public const string SerializationUnreferencedCodeMessage = "JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.";
//    public const string SerializationRequiresDynamicCodeMessage = "JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.";

//    const string NsCompiler = "System.Runtime.CompilerServices";
//    const string NsInterop = "System.Runtime.InteropServices";

//    const byte BUnknown = (byte)Nullability.Unknown;
//    const byte BNotNull = (byte)Nullability.NotNull;
//    const byte BNullable = (byte)Nullability.Nullable;
//    const byte BUnset = (byte)Nullability.Unset;

//    public static string AttrExtension = AttrName<ExtensionAttribute>();
//    public static string AttrNullable = AttrName("NullableAttribute");
//    public static string AttrNullableContext = AttrName("NullableContextAttribute");
//    public static string AttrIn = AttrName<InAttribute>();
//    public static string AttrOut = AttrName<OutAttribute>();
//    public static string AttrParamArray = AttrName<ParamArrayAttribute>();

//    static HashSet<string> Keywords = "abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|volatile|while".Split('|').ToHashSet();

//    public static string Join(this IEnumerable<object?> @this, string sep = ", ") =>
//        string.Join(sep, @this.Where(o => o != null));

//    public static T? GetOrDefault<TKey, T>(this Dictionary<TKey, T> @this, TKey key) where TKey : notnull =>
//        @this.GetOrDefault(key, default!);

//    public static T GetOrDefault<TKey, T>(this Dictionary<TKey, T> @this, TKey key, T def) where TKey : notnull =>
//        @this.TryGetValue(key, out var v) ? v : def;

//    public static string FormatReturn(this MethodInfo m)
//    {
//        return m.ReturnType.Format(m.ReturnTypeCustomAttributes.GetAttrs(), m);
//    }

//    public static string FormatGenericArgs(this MethodInfo m)
//    {
//        return m.ContainsGenericParameters ? $"<{m.GetGenericArguments().Select(Format).Join()}>" : "";
//    }

//    public static string FormatExt(this MethodInfo m)
//    {
//        return m.CustomAttributes.HasAttr(AttrExtension) ? "this " : "";
//    }

//    public static string Format(this ParameterInfo p)
//    {
//        var attrs = p.CustomAttributes.Select(Format).Join();
//        var attrsStr = attrs.Length > 0 ? $"[{attrs}] " : "";
//        var before = p.CustomAttributes.HasAttr(AttrParamArray) ? "params " : "";
//        var type = p.ParameterType.Format(p.CustomAttributes);
//        var after = !p.ParameterType.IsNullable(p.CustomAttributes) && p.IsNullable() ? "?" : "";
//        var name = Id(p.Name!);
//        var def = p.HasDefaultValue ? $" = {p.DefaultValue.FormatConst()}" : "";
//        return $"{attrsStr}{before}{type}{after} {name}{def}";
//    }

//    public static string FormatArg(this ParameterInfo p)
//    {
//        var mods =
//            !p.ParameterType.IsByRef ? "" :
//            p.CustomAttributes.HasAttr(AttrIn) ? "in " :
//            p.CustomAttributes.HasAttr(AttrOut) ? "out " : "ref ";
//        var name = Id(p.Name!);
//        return $"{mods}{name}";
//    }

//    public static string? Format(this Attr a)
//    {
//        var typeName = a.AttributeType.FullName!;
//        if (typeName.StartsWith(NsCompiler) || typeName.StartsWith(NsInterop) || typeName == AttrParamArray || typeName.Contains("__"))
//            return null;
//        var name = a.AttributeType.Name;
//        var shortName = a.AttributeType.Name.Substring(0, name.Length - "Attribute".Length);
//        var args = a.ConstructorArguments.Select(v => v.Value.FormatConst())
//            .Concat(a.NamedArguments.Select(v => $"{v.MemberName} = {v.TypedValue.Value.FormatConst()}"))
//            .Join();
//        return args.Length > 0 ? $"{shortName}({args})" : $"{shortName}";
//    }

//    public static string FormatConst(this object? o, bool raw = false)
//    {
//        return o switch {
//            SerializationUnreferencedCodeMessage when !raw => nameof(SerializationUnreferencedCodeMessage),
//            SerializationRequiresDynamicCodeMessage when !raw => nameof(SerializationRequiresDynamicCodeMessage),
//            { } v => JsonSerializer.Serialize(v),
//            null => "default",
//        };
//    }

//    static string Format<T>(this T t) where T : Type => t.Format(null);
//    static string Format(this Type t, IEnumerable<Attr>? attrs = null, MemberInfo? owner = null)
//    {
//        bool isNullable = t.IsValueType && t.IsNullable();
//        if (isNullable)
//            t = Nullable.GetUnderlyingType(t)!;
//        var args = t.IsGenericType ? $"<{t.GenericTypeArguments.Select(Format).Join()}>" : "";
//        var before =
//            !t.IsByRef ? "" :
//            attrs.HasAttr(AttrIn) ? "in " :
//            attrs.HasAttr(AttrOut) ? "out " : "ref ";
//        var after = isNullable || t.IsNullable(attrs, owner) ? "?" : "";
//        var cleanName = t.Name.Split('`')[0].TrimEnd('&');
//        var name = cleanName switch {
//            "String" => "string",
//            "Object" => "object",
//            "Char" => "char",
//            "Byte" => "byte",
//            {} s => s,
//        };
//        return $"{before}{name}{args}{after}";
//    }

//    static string Id(string s) => Keywords.Contains(s) ? $"@{s}" : s;

//    static string AttrName<T>() => typeof(T).FullName!;
//    static string AttrName(string s) => $"{NsCompiler}.{s}";

//    public static Attr? GetAttr(this IEnumerable<Attr>? attrs, string fullName) =>
//        attrs?.FirstOrDefault(a => a.AttributeType.FullName == fullName);

//    public static bool HasAttr(this IEnumerable<Attr>? attrs, string fullName) =>
//        attrs?.GetAttr(fullName) != null;

//    public static bool IsNullable(this ParameterInfo p) =>
//        p.ParameterType.IsNullable(p.CustomAttributes, p.Member);

//    public static bool IsNullable(this Type type, IEnumerable<Attr>? attrs = null, MemberInfo? owner = null)
//    {
//        if (type.IsByRef || (attrs?.HasAttr(AttrExtension) ?? false))
//            return false;
//        if (type.IsValueType)
//            return Nullable.GetUnderlyingType(type) != null;

//        if (attrs.GetAttr(AttrNullable) is { ConstructorArguments: [ var arg ] }) {
//            if (arg.Value switch {
//                    byte b => b,
//                    IList<byte> and [ var b0, .. ] => b0,
//                    _ => (byte?)null,
//                } is { } state)
//                return state != BNotNull;
//        }

//        for (var t = owner; t != null; t = t.DeclaringType)
//            if (t.CustomAttributes.GetAttr(AttrNullableContext) is { ConstructorArguments: [ { Value: byte state } ] })
//                return state != BNotNull;

//        return false;
//    }

//    static Attr[] GetAttrs(this ICustomAttributeProvider @this) =>
//        @this.GetCustomAttributes(false).Select(Attr (a) => new RefAttr(a)).ToArray();

//    sealed class RefAttr : Attr
//    {
//        public RefAttr(object a)
//        {
//            var type = a.GetType();
//            if (type.FullName == AttrNullable) {
//                var flagsField = type.GetField("NullableFlags", BindingFlags.Public | BindingFlags.Instance)!;
//                Constructor = type.GetConstructor(new[] { typeof(byte[]) })!;
//                ConstructorArguments.Add(new(typeof(byte[]), flagsField.GetValue(a)));
//            }
//            else if (type.FullName == AttrNullableContext) {
//                var flagField = type.GetField("Flag", BindingFlags.Public | BindingFlags.Instance)!;
//                Constructor = type.GetConstructor(new[] { typeof(byte) })!;
//                ConstructorArguments.Add(new(typeof(byte), flagField.GetValue(a)));
//            }
//        }

//        public override ConstructorInfo Constructor { get; } = null!;
//        public override IList<AttrArg> ConstructorArguments { get; } = new List<AttrArg>();
//        public override IList<AttrNamedArg> NamedArguments { get; } = new List<AttrNamedArg>();
//    }
//}

//// ReSharper disable ConvertTypeCheckPatternToNullCheck

//class XmlDoc
//{
//    private Dictionary<string, XElement> _members = new();
//    private HashSet<Type> _types = new();
//    private HashSet<string> _docFiles = new();

//    public XmlDoc LoadType<T>() => LoadType(typeof(T));
//    public XmlDoc LoadType(Type type)
//    {
//        if (!_types.Add(type))
//            return this;
//        var version = type.Assembly.GetName().Version!;
//        var versionPrefix = $"{version.Major}.{version.Minor}";
//        var dotnetPacksDir = @"C:\Program Files\dotnet\packs";
//        var packName = "Microsoft.NETCore.App.Ref";
//        var versionDir = Directory.EnumerateDirectories(Path.Combine(dotnetPacksDir, packName), $"{versionPrefix}.*").OrderBy(i => i).Last();
//        var refDir = Directory.EnumerateDirectories(Path.Combine(versionDir, "ref"), $"net{versionPrefix}").OrderBy(i => i).Last();
//        var docFileName = Path.ChangeExtension(type.Module.Name, ".xml");
//        if (!_docFiles.Add(docFileName))
//            return this;
//        using var file = File.OpenText(Path.Combine(refDir, docFileName));
//        var doc = XDocument.Load(file);
//        foreach (var el in doc.XPathSelectElements("/doc/members/member"))
//            _members[el.Attribute("name")!.Value] = el;
//        return this;
//    }

//    public string FormatDoc(XElement el, int indent = 0, char indentChar = ' ')
//    {
//        var indentStr = new string(indentChar, indent);
//        return el.Elements().Select(i => Regex.Replace($"{i}", @"\s+", " ")).Join("\n")
//            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
//            .Select(s => $"{indentStr}/// {s}")
//            .Join(Environment.NewLine);
//    }

//    public XElement? GetDoc(MethodInfo m) =>
//        _members.GetOrDefault(GetXmlNameMethod(method: m));

//    string GetDocStr(Type type, bool isParam, Map map)
//    {
//        if (type.IsGenericParameter) {
//            var methodIndex = map.Methods.GetOrDefault(type.Name, -1);
//            return methodIndex != -1 ? "``" + methodIndex : "`" + map.Types[type.Name];
//        }
//        else if (type.HasElementType) {
//            string elementTypeString = GetDocStr(type.GetElementType()!, isParam, map);
//            switch (type) {
//                case Type when type.IsPointer:
//                    return elementTypeString + "*";
//                case Type when type.IsByRef:
//                    return elementTypeString + "@";
//                case Type when type.IsArray:
//                    int rank = type.GetArrayRank();
//                    return elementTypeString + (rank > 1 ? $"[{Enumerable.Repeat("0:", rank).Join(",")}]" : "[]");
//                default:
//                    throw new InvalidOperationException();
//            }
//        }
//        else {
//            string prefix = type.IsNested ? GetDocStr(type.DeclaringType!, isParam, map) : type.Namespace!;
//            string typeName = isParam ? Regex.Replace(type.Name, @"`\d+", "") : type.Name;
//            string genericArgs = type.IsGenericType && isParam
//                ? "{" + type.GetGenericArguments().Select(a => GetDocStr(a, isParam, map)).Join(",") + "}" : "";
//            return $"{prefix}.{typeName}{genericArgs}";
//        }
//    }

//    public string GetXmlNameMethod(MethodInfo? method = null, ConstructorInfo? ctor = null)
//    {
//        if (method is { DeclaringType.IsGenericType: true })
//            method = method.DeclaringType.GetGenericTypeDefinition()
//                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
//                .First(x => x.MetadataToken == method.MetadataToken);

//        MethodBase methodBase = method ?? (MethodBase?)ctor!;
//        var map = new Map(
//            Types: GenericArgsToMap(methodBase.DeclaringType!.GetGenericArguments()),
//            Methods: GenericArgsToMap(method?.GetGenericArguments()));
//        var pars = methodBase.GetParameters();

//        string typeName = GetDocStr(methodBase.DeclaringType, isParam: false, map);
//        string methodName = ctor != null ? "#ctor" : methodBase.Name;
//        string genericArgs = map.Methods.Count > 0 ? "``" + map.Methods.Count : "";
//        string paramsStr = pars.Length > 0 ? "(" + pars.Select(x => GetDocStr(x.ParameterType, isParam: true, map)).Join(",") + ")" : "";
//        string key = $"M:{typeName}.{methodName}{genericArgs}{paramsStr}";
//        if (method != null && methodBase.Name is "op_Implicit" or "op_Explicit")
//            key += "~" + GetDocStr(method.ReturnType, isParam: true, map);
//        return key;

//        Dictionary<string, int> GenericArgsToMap(Type[]? types) =>
//            types?.Select((t, i) => (t.Name, Index: i)).ToDictionary(p => p.Name, p => p.Index) ?? new();
//    }

//    record Map(Dictionary<string, int> Types, Dictionary<string, int> Methods);
//}