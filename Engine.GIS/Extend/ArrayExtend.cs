namespace Engine.GIS.Extend
{
    public static class ArrayExtend
    {
        public static float[] toFloatArray(this byte[] array)
        {
            int length = array.Length;
            float[] dist = new float[length];
            for (int i = 0; i < length; i++)
                dist[i] = (float)array[i];
            return dist;
        }
    }
}
