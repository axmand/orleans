namespace GrainImplement.DRL.Method
{
    /// <summary>
    /// basically neural network definition
    /// </summary>
    public interface INeuralNetwork : ISupervisedLearning
    {

        /// <summary>
        /// training batch size
        /// </summary>
        uint BatchSize { get; }

        /// <summary>
        /// train net
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns></returns>
        double Train(float[][] inputs, float[][] outputs);

        /// <summary>
        /// Predict By Model
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        float[] Predict(float[] input);

        /// <summary>
        /// persistence in local dir, default in debug/tmp
        /// </summary>
        string PersistencNative(string modelFilename = null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] PersistenceMemory();
    }
}
