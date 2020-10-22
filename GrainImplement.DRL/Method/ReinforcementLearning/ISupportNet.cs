namespace GrainImplement.DRL.Method.ReinforcementLearning
{
    /// <summary>
    /// support dqn training and apply
    /// </summary>
    public interface ISupportNet : INeuralNetwork
    {
        /// <summary>
        /// copy sourceNet parameters to this Net
        /// </summary>
        /// <param name="sourceNet"></param>
        void Accept(ISupportNet sourceNet);
    }
}
