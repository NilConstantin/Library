namespace Library
{
    public static class ArrayHelper
    {
        public static void SwapElements<T>(ref T[] array, int indexFrom, int indexTo)
        {
            var tmp = array[indexFrom];
            array[indexFrom] = array[indexTo];
            array[indexTo] = tmp;
        }
    }
}
