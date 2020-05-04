namespace Engine.GIS.GTools
{
    /// <summary>
    /// 百分比裁剪工具
    /// </summary>
    public interface IRasterPrecentClipTool : IRasterBandTool
    {
        /// <summary>
        /// apply percent clip
        /// </summary>
        /// <param name="percentNum"></param>
        void ApplyPercentClipStretch(double percentNum = 0.02);
    }
}
