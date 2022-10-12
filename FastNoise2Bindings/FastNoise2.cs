using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FastNoise2Bindings
{
    public class NoiseNode : IDisposable
    {
        public int MetadataId => _metadataId;
        private readonly int _metadataId = -1;
        private readonly IntPtr _nodeHandle = IntPtr.Zero;


        public NoiseNode(string metadataName)
        {
            if (!Metadata.TryGetMetadataId(metadataName, out _metadataId))
            {
                throw new ArgumentException("Failed to find metadata name: " + metadataName);
            }

            _nodeHandle = Native.fnNewFromMetadata(_metadataId);
        }


        private NoiseNode(IntPtr nodeHandle)
        {
            _nodeHandle = nodeHandle;
            _metadataId = Native.fnGetMetadataID(nodeHandle);
        }


        public static NoiseNode? FromEncodedNodeTree(string encodedNodeTree)
        {
            var nodeHandle = Native.fnNewFromEncodedNodeTree(encodedNodeTree);

            if (nodeHandle == IntPtr.Zero)
            {
                return null;
            }

            return new NoiseNode(nodeHandle);
        }


        public uint GetSIMDLevel() => Native.fnGetSIMDLevel(_nodeHandle);


        public void Set(string memberName, float value)
        {
            if (!Metadata.TryGetMember(this, memberName, out var member))
            {
                throw new ArgumentException("Failed to find member name: " + memberName);
            }

            switch (member.Type)
            {
                case Member.MemberType.Float:
                    if (!Native.fnSetVariableFloat(_nodeHandle, member.Index, value))
                    {
                        throw new ExternalException("Failed to set float value");
                    }
                    break;

                case Member.MemberType.Hybrid:
                    if (!Native.fnSetHybridFloat(_nodeHandle, member.Index, value))
                    {
                        throw new ExternalException("Failed to set float value");
                    }
                    break;

                default:
                    throw new ArgumentException(memberName + " cannot be set to a float value");
            }
        }


        public void Set(string memberName, int value)
        {
            if (!Metadata.TryGetMember(this, memberName, out var member))
            {
                throw new ArgumentException("Failed to find member name: " + memberName);
            }

            if (member.Type != Member.MemberType.Int)
            {
                throw new ArgumentException(memberName + " cannot be set to an int value");
            }

            if (!Native.fnSetVariableIntEnum(_nodeHandle, member.Index, value))
            {
                throw new ExternalException("Failed to set int value");
            }
        }


        public void Set(string memberName, string enumValue)
        {
            if (!Metadata.TryGetMember(this, memberName, out var member))
            {
                throw new ArgumentException("Failed to find member name: " + memberName);
            }

            if (!member.TypGetEnumIndex(enumValue, out int enumIdx))
            {
                throw new ArgumentException("Failed to find enum value: " + enumValue);
            }

            if (!Native.fnSetVariableIntEnum(_nodeHandle, member.Index, enumIdx))
            {
                throw new ExternalException("Failed to set enum value");
            }
        }


        public void Set(string memberName, NoiseNode nodeLookup)
        {
            if (!Metadata.TryGetMember(this, memberName, out var member))
            {
                throw new ArgumentException("Failed to find member name: " + memberName);
            }

            switch (member.Type)
            {
                case Member.MemberType.NodeLookup:
                    if (!Native.fnSetNodeLookup(_nodeHandle, member.Index, nodeLookup._nodeHandle))
                    {
                        throw new ExternalException("Failed to set node lookup");
                    }
                    break;

                case Member.MemberType.Hybrid:
                    if (!Native.fnSetHybridNodeLookup(_nodeHandle, member.Index, nodeLookup._nodeHandle))
                    {
                        throw new ExternalException("Failed to set node lookup");
                    }
                    break;

                default:
                    throw new ArgumentException(memberName + " cannot be set to a node lookup");
            }
        }


        public OutputMinMax GenUniformGrid2D(float[] noiseOut,
                                       int xStart, int yStart,
                                       int xSize, int ySize,
                                       float frequency, int seed)
        {
            float[] minMax = new float[2];
            _ = Native.fnGenUniformGrid2D(_nodeHandle, noiseOut, xStart, yStart, xSize, ySize, frequency, seed, minMax);
            return new OutputMinMax(minMax);
        }


        public OutputMinMax GenUniformGrid3D(float[] noiseOut,
                                       int xStart, int yStart, int zStart,
                                       int xSize, int ySize, int zSize,
                                       float frequency, int seed)
        {
            float[] minMax = new float[2];
            _ = Native.fnGenUniformGrid3D(_nodeHandle, noiseOut, xStart, yStart, zStart, xSize, ySize, zSize, frequency, seed, minMax);
            return new OutputMinMax(minMax);
        }


        public OutputMinMax GenUniformGrid4D(float[] noiseOut,
                                       int xStart, int yStart, int zStart, int wStart,
                                       int xSize, int ySize, int zSize, int wSize,
                                       float frequency, int seed)
        {
            float[] minMax = new float[2];
            _ = Native.fnGenUniformGrid4D(_nodeHandle, noiseOut, xStart, yStart, zStart, wStart, xSize, ySize, zSize, wSize, frequency, seed, minMax);
            return new OutputMinMax(minMax);
        }


        public OutputMinMax GenTileable2D(float[] noiseOut,
                                       int xSize, int ySize,
                                       float frequency, int seed)
        {
            var minMax = new float[2];
            Native.fnGenTileable2D(_nodeHandle, noiseOut, xSize, ySize, frequency, seed, minMax);
            return new OutputMinMax(minMax);
        }


        public OutputMinMax GenPositionArray2D(float[] noiseOut,
                                             float[] xPosArray, float[] yPosArray,
                                             float xOffset, float yOffset,
                                             int seed)
        {
            var minMax = new float[2];
            Native.fnGenPositionArray2D(_nodeHandle, noiseOut, xPosArray.Length, xPosArray, yPosArray, xOffset, yOffset, seed, minMax);
            return new OutputMinMax(minMax);
        }


        public OutputMinMax GenPositionArray3D(float[] noiseOut,
                                             float[] xPosArray, float[] yPosArray, float[] zPosArray,
                                             float xOffset, float yOffset, float zOffset,
                                             int seed)
        {
            var minMax = new float[2];
            Native.fnGenPositionArray3D(_nodeHandle, noiseOut, xPosArray.Length, xPosArray, yPosArray, zPosArray, xOffset, yOffset, zOffset, seed, minMax);
            return new OutputMinMax(minMax);
        }


        public OutputMinMax GenPositionArray4D(float[] noiseOut,
                                             float[] xPosArray, float[] yPosArray, float[] zPosArray, float[] wPosArray,
                                             float xOffset, float yOffset, float zOffset, float wOffset,
                                             int seed)
        {
            var minMax = new float[2];
            Native.fnGenPositionArray4D(_nodeHandle, noiseOut, xPosArray.Length, xPosArray, yPosArray, zPosArray, wPosArray, xOffset, yOffset, zOffset, wOffset, seed, minMax);
            return new OutputMinMax(minMax);
        }


        public float GenSingle2D(float x, float y, int seed) => Native.fnGenSingle2D(_nodeHandle, x, y, seed);


        public float GenSingle3D(float x, float y, float z, int seed) => Native.fnGenSingle3D(_nodeHandle, x, y, z, seed);


        public float GenSingle4D(float x, float y, float z, float w, int seed) => Native.fnGenSingle4D(_nodeHandle, x, y, z, w, seed);


        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                Native.fnDeleteNodeRef(_nodeHandle);
                _disposedValue = true;
            }
        }

        // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~NoiseNode()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }


    public struct OutputMinMax
    {
        public float min;
        public float max;


        public OutputMinMax(float minValue = float.PositiveInfinity, float maxValue = float.NegativeInfinity)
        {
            min = minValue;
            max = maxValue;
        }


        public OutputMinMax(Span<float> nativeOutputMinMax)
        {
            min = nativeOutputMinMax[0];
            max = nativeOutputMinMax[1];
        }


        public void Merge(OutputMinMax other)
        {
            min = Math.Min(min, other.min);
            max = Math.Max(max, other.max);
        }
    }


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
                    var type = (Member.MemberType)Native.fnGetMetadataVariableType(id, variableIdx);
                    var index = variableIdx;

                    name = FormatDimensionMember(name, Native.fnGetMetadataVariableDimensionIdx(id, variableIdx));


                    // Get enum names
                    if (type == Member.MemberType.Enum)
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
                    var type = Member.MemberType.NodeLookup;
                    var index = nodeLookupIdx;

                    name = FormatDimensionMember(name, Native.fnGetMetadataNodeLookupDimensionIdx(id, nodeLookupIdx));

                    metadataMember.Add(name, new Member(name, type, index));

                }

                // Init hybrids
                for (var hybridIdx = 0; hybridIdx < hybridCount; hybridIdx++)
                {
                    var name = FormatLookup(Marshal.PtrToStringAnsi(Native.fnGetMetadataHybridName(id, hybridIdx)));
                    var type = Member.MemberType.Hybrid;
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


    internal static class Native
    {
        private const string NATIVE_LIB = "FastNoise";


        [DllImport(NATIVE_LIB)]
        internal static extern IntPtr fnNewFromMetadata(int id, uint simdLevel = 0);

        [DllImport(NATIVE_LIB)]
        internal static extern IntPtr fnNewFromEncodedNodeTree([MarshalAs(UnmanagedType.LPStr)] string encodedNodeTree, uint simdLevel = 0);

        [DllImport(NATIVE_LIB)]
        internal static extern void fnDeleteNodeRef(IntPtr nodeHandle);

        [DllImport(NATIVE_LIB)]
        internal static extern uint fnGetSIMDLevel(IntPtr nodeHandle);

        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataID(IntPtr nodeHandle);

        [DllImport(NATIVE_LIB)]
        internal static extern uint fnGenUniformGrid2D(IntPtr nodeHandle, float[] noiseOut,
                                       int xStart, int yStart,
                                       int xSize, int ySize,
                                       float frequency, int seed, float[] outputMinMax);

        [DllImport(NATIVE_LIB)]
        internal static extern uint fnGenUniformGrid3D(IntPtr nodeHandle, float[] noiseOut,
                                       int xStart, int yStart, int zStart,
                                       int xSize, int ySize, int zSize,
                                       float frequency, int seed, float[] outputMinMax);

        [DllImport(NATIVE_LIB)]
        internal static extern uint fnGenUniformGrid4D(IntPtr nodeHandle, float[] noiseOut,
                                       int xStart, int yStart, int zStart, int wStart,
                                       int xSize, int ySize, int zSize, int wSize,
                                       float frequency, int seed, float[] outputMinMax);

        [DllImport(NATIVE_LIB)]
        internal static extern void fnGenTileable2D(IntPtr node, float[] noiseOut,
                                        int xSize, int ySize,
                                        float frequency, int seed, float[] outputMinMax);

        [DllImport(NATIVE_LIB)]
        internal static extern void fnGenPositionArray2D(IntPtr node, float[] noiseOut, int count,
                                             float[] xPosArray, float[] yPosArray,
                                             float xOffset, float yOffset,
                                             int seed, float[] outputMinMax);

        [DllImport(NATIVE_LIB)]
        internal static extern void fnGenPositionArray3D(IntPtr node, float[] noiseOut, int count,
                                             float[] xPosArray, float[] yPosArray, float[] zPosArray,
                                             float xOffset, float yOffset, float zOffset,
                                             int seed, float[] outputMinMax);

        [DllImport(NATIVE_LIB)]
        internal static extern void fnGenPositionArray4D(IntPtr node, float[] noiseOut, int count,
                                             float[] xPosArray, float[] yPosArray, float[] zPosArray, float[] wPosArray,
                                             float xOffset, float yOffset, float zOffset, float wOffset,
                                             int seed, float[] outputMinMax);

        [DllImport(NATIVE_LIB)]
        internal static extern float fnGenSingle2D(IntPtr node, float x, float y, int seed);

        [DllImport(NATIVE_LIB)]
        internal static extern float fnGenSingle3D(IntPtr node, float x, float y, float z, int seed);

        [DllImport(NATIVE_LIB)]
        internal static extern float fnGenSingle4D(IntPtr node, float x, float y, float z, float w, int seed);

        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataCount();

        [DllImport(NATIVE_LIB)]
        internal static extern IntPtr fnGetMetadataName(int id);

        // Variable
        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataVariableCount(int id);

        [DllImport(NATIVE_LIB)]
        internal static extern IntPtr fnGetMetadataVariableName(int id, int variableIndex);

        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataVariableType(int id, int variableIndex);

        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataVariableDimensionIdx(int id, int variableIndex);

        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataEnumCount(int id, int variableIndex);

        [DllImport(NATIVE_LIB)]
        internal static extern IntPtr fnGetMetadataEnumName(int id, int variableIndex, int enumIndex);

        [DllImport(NATIVE_LIB)]
        internal static extern bool fnSetVariableFloat(IntPtr nodeHandle, int variableIndex, float value);

        [DllImport(NATIVE_LIB)]
        internal static extern bool fnSetVariableIntEnum(IntPtr nodeHandle, int variableIndex, int value);

        // Node Lookup
        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataNodeLookupCount(int id);

        [DllImport(NATIVE_LIB)]
        internal static extern IntPtr fnGetMetadataNodeLookupName(int id, int nodeLookupIndex);

        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataNodeLookupDimensionIdx(int id, int nodeLookupIndex);

        [DllImport(NATIVE_LIB)]
        internal static extern bool fnSetNodeLookup(IntPtr nodeHandle, int nodeLookupIndex, IntPtr nodeLookupHandle);

        // Hybrid
        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataHybridCount(int id);

        [DllImport(NATIVE_LIB)]
        internal static extern IntPtr fnGetMetadataHybridName(int id, int nodeLookupIndex);

        [DllImport(NATIVE_LIB)]
        internal static extern int fnGetMetadataHybridDimensionIdx(int id, int nodeLookupIndex);

        [DllImport(NATIVE_LIB)]
        internal static extern bool fnSetHybridNodeLookup(IntPtr nodeHandle, int nodeLookupIndex, IntPtr nodeLookupHandle);

        [DllImport(NATIVE_LIB)]
        internal static extern bool fnSetHybridFloat(IntPtr nodeHandle, int nodeLookupIndex, float value);
    }


    internal class Member
    {
        internal enum MemberType
        {
            Float,
            Int,
            Enum,
            NodeLookup,
            Hybrid,
        }


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
        /// Constructor for non-enum members.
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