using System.Diagnostics.CodeAnalysis;

namespace FastNoise2Bindings.Internal
{
    internal class NodeMetadata
    {
        private readonly int _id;
        private readonly string _name;
        private readonly Dictionary<string, Member> _members;


        internal NodeMetadata(int id, string name, Dictionary<string, Member> members)
        {
            _id = id;
            _name = name;
            _members = members;
        }


        internal bool TryGetMember(string memberName, [NotNullWhen(true)] out Member? member)
            => _members.TryGetValue(memberName, out member);
    }
}