using System.Runtime.InteropServices;
using FastNoise2Bindings.Internal;

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
                case MemberType.Float:
                    if (!Native.fnSetVariableFloat(_nodeHandle, member.Index, value))
                    {
                        throw new ExternalException("Failed to set float value");
                    }
                    break;

                case MemberType.Hybrid:
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

            if (member.Type != MemberType.Int)
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
                case MemberType.NodeLookup:
                    if (!Native.fnSetNodeLookup(_nodeHandle, member.Index, nodeLookup._nodeHandle))
                    {
                        throw new ExternalException("Failed to set node lookup");
                    }
                    break;

                case MemberType.Hybrid:
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
}