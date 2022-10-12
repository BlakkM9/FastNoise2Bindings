namespace FastNoise2Bindings.Internal
{
    internal class Member
    {
        internal string Name { get; }
        internal MemberType Type { get; }
        internal int Index { get; }

        private readonly Dictionary<string, int>? _enumNames;


        /// <summary>
        /// Constructor for enum members
        /// </summary>
        internal Member(string name, MemberType type, int index, Dictionary<string, int> enumNames)
        {
            Name = name;
            Type = type;
            Index = index;
            _enumNames = enumNames;
        }


        /// <summary>
        /// Constructor for non enum members.
        /// </summary>
        internal Member(string name, MemberType type, int index)
        {
            Name = name;
            Type = type;
            Index = index;
            _enumNames = null;
        }


        internal bool TypGetEnumIndex(string enumValue, out int enumIndex)
            => _enumNames?.TryGetValue(Metadata.FormatLookup(enumValue), out enumIndex)
            ?? throw new ArgumentException(Name + " cannot be set to an enum value");
    }
}