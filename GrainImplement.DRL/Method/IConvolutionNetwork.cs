namespace GrainImplement.DRL.Method
{
    /// <summary>
    /// deep convolution neural network
    /// expecially suitable for deep feature extract
    /// </summary>
    public interface IConvolutionNetwork : INeuralNetwork
    {
        /// <summary>
        /// remove softmax, convert it to Extract Feature Network
        /// </summary>
        void ConvertToExtractNetwork();

        /// <summary>
        /// predicts
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        float[][] Predicts(float[][] inputs);
    }
}
