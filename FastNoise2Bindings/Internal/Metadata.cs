using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FastNoise2Bindings.Internal
{
    public static class Metadata
    {
        private static readonly Dictionary<string, int> _metadataNameLookup;
        private static readonly NodeMetadata[] _nodeMetadata;


        #region Initialization

        static Metadata()
        {
            var metadataCount = Native.fnGetMetadataCount();

            _nodeMetadata = new NodeMetadata[metadataCount];
            _metadataNameLookup = new Dictionary<string, int>(metadataCount);

            // Collect metadata for all FastNoise node classes
            for (var id = 0; id < metadataCount; id++)
            {
                //var metadata = new NodeMetadata();

                var metadataName = FormatLookup(Marshal.PtrToStringAnsi(Native.fnGetMetadataName(id)));
                //Console.WriteLine(id + " - " + metadata.name);
                _metadataNameLookup.Add(metadataName, id);

                var variableCount = Native.fnGetMetadataVariableCount(id);
                var nodeLookupCount = Native.fnGetMetadataNodeLookupCount(id);
                var hybridCount = Native.fnGetMetadataHybridCount(id);
                var metadataMember = new Dictionary<string, Member>(variableCount + nodeLookupCount + hybridCount);

                // Init variables
                for (var variableIdx = 0; variableIdx < variableCount; variableIdx++)
                {
                    var name = FormatLookup(Marshal.PtrToStringAnsi(Native.fnGetMetadataVariableName(id, variableIdx)));
                    var type = (MemberType)Native.fnGetMetadataVariableType(id, variableIdx);
                    var index = variableIdx;

                    name = FormatDimensionMember(name, Native.fnGetMetadataVariableDimensionIdx(id, variableIdx));


                    // Get enum names
                    if (type == MemberType.Enum)
                    {
                        var enumCount = Native.fnGetMetadataEnumCount(id, variableIdx);
                        var enumNames = new Dictionary<string, int>(enumCount);

                        for (var enumIdx = 0; enumIdx < enumCount; enumIdx++)
                        {
                            enumNames.Add(FormatLookup(Marshal.PtrToStringAnsi(Native.fnGetMetadataEnumName(id, variableIdx, enumIdx))), enumIdx);
                        }

                        metadataMember.Add(name, new Member(name, type, index, enumNames));
                    }
                    else
                    {
                        metadataMember.Add(name, new Member(name, type, index));
                    }
                }

                // Init node lookups
                for (var nodeLookupIdx = 0; nodeLookupIdx < nodeLookupCount; nodeLookupIdx++)
                {
                    var name = FormatLookup(Marshal.PtrToStringAnsi(Native.fnGetMetadataNodeLookupName(id, nodeLookupIdx)));
                    var type = MemberType.NodeLookup;
                    var index = nodeLookupIdx;

                    name = FormatDimensionMember(name, Native.fnGetMetadataNodeLookupDimensionIdx(id, nodeLookupIdx));

                    metadataMember.Add(name, new Member(name, type, index));

                }

                // Init hybrids
                for (var hybridIdx = 0; hybridIdx < hybridCount; hybridIdx++)
                {
                    var name = FormatLookup(Marshal.PtrToStringAnsi(Native.fnGetMetadataHybridName(id, hybridIdx)));
                    var type = MemberType.Hybrid;
                    var index = hybridIdx;

                    name = FormatDimensionMember(name, Native.fnGetMetadataHybridDimensionIdx(id, hybridIdx));


                    metadataMember.Add(name, new Member(name, type, index));

                }

                _nodeMetadata[id] = new NodeMetadata(id, metadataName, metadataMember);
            }
        }

        #endregion


        #region Metadata access

        internal static bool TryGetMember(NoiseNode node, string memberName, [NotNullWhen(true)] out Member? member)
            => _nodeMetadata[node.MetadataId].TryGetMember(FormatLookup(memberName), out member);


        internal static bool TryGetMetadataId(string metadataName, out int metadataId)
            => _metadataNameLookup.TryGetValue(FormatLookup(metadataName), out metadataId);

        #endregion


        #region Utils

        // Append dimension char where neccessary 
        private static string FormatDimensionMember(string name, int dimIdx)
        {
            if (dimIdx >= 0)
            {
                var dimSuffix = new char[] { 'x', 'y', 'z', 'w' };
                name += dimSuffix[dimIdx];
            }
            return name;
        }


        // Ignores spaces and caps, harder to mistype strings
        public static string FormatLookup(string? s)
        {
            return s?.Replace(" ", "").ToLower() ?? string.Empty;
        }

        #endregion
    }
}
