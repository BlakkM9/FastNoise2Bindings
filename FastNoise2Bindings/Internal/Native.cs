using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastNoise2Bindings.Internal
{
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
}
