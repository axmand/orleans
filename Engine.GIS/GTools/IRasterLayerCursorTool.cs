namespace Engine.GIS.GTools
{
    /// <summary>
    /// pick normal value at posiont (x,y) in eachlayer
    /// </summary>
    public interface IRasterLayerCursorTool : IRasterLayerTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        float[] PickRawValue(int x, int y);

        /// <summary>
        /// pick normalized value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        float[] PickNormalValue(int x, int y);

        /// <summary>
        /// pick normalized with range
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        float[] PickRagneNormalValue(int x, int y, int row = 5, int col = 5);
    }
}
