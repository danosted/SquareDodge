namespace Assets.Code.Utilities
{
    public class MathUtil
    {
        public static float MapInputFromInputRangeToOutputRange(float input, float input_start, float input_end, float output_start, float output_end)
        {
            var slope = 1f * (output_end - output_start) / (input_end - input_start);
            var output = output_start + slope * (input - input_start);
            return output;
        }
    }
}
