namespace Library
{
    public static class MathHelper
    {
        public static bool Approximately(float a, float b, float threshold = 0.0001f)
        {
            return (a < b ? b - a : a - b) <= threshold;
        }

        public static int GetPowerOfTwoSize(int n)
        {
            if (n < 2)
            {
                return 2;
            }
            n--;
            n = n | (n >> 1);
            n = n | (n >> 2);
            n = n | (n >> 4);
            n = n | (n >> 8);
            n = n | (n >> 16);
            return n + 1;
        }
    }
}