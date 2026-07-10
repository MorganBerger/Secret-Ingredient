namespace SomeExtensions
{
    public static class FloatExtensions
    {

        public static int Raw(this float value)
        {
            if (value > 0)
            {
                return 1;
            }
            else if (value < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}