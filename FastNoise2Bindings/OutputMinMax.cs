using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastNoise2Bindings
{
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
}
