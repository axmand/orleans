using CNTK;

using GrainImplement.DRL.Method.ReinforcementLearning;
using GrainImplement.DRL.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GrainImplement.DRL.Method.SupervisedLearning
{
    /// <summary>
    /// fully channel net with 10 layers
    /// 3x3 + 3x3 + pooling layer
    /// 3x3 + 3x3 + pooling layer
    /// 1 dense layer
    /// </summary>
    public class FCN7 : IConvolutionNetwork, ISupportNet
    {
        /// <summary>
        /// 
        /// </summary>
        public uint BatchSize { get; } = 96;

        /// <summary>
        /// store traind epochs
        /// </summary>
        int traindEpochs = 0;

        /// <summary>
        /// trainer function
        /// </summary>
        Trainer trainer;

        /// <summary>
        /// model output
        /// </summary>
        Function classifierOutput;

        /// <summary>
        /// 
        /// </summary>
        Variable inputVariable;

        /// <summary>
        /// 
        /// </summary>
        Variable outputVariable;

        /// <summary>
        /// 
        /// </summary>
        readonly DeviceDescriptor device;

        /// <summary>
        /// create model by w,h,c,outputClassNum
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="c"></param>
        /// <param name="outputClassNum"></param>
        /// <param name="deviceName"></param>
        public FCN7(int w, int h, int c, int outputClassNum, string deviceName)
        {
            device = NP.CNTKHelper.GetDeviceByName(deviceName);
            //input output variable
            int[] inputDim = new int[] { w, h, c };
            int[] outputDim = new int[] { outputClassNum };
            inputVariable = Variable.InputVariable(NDShape.CreateNDShape(inputDim), DataType.Float, "inputVariable");
            outputVariable = Variable.InputVariable(NDShape.CreateNDShape(outputDim), DataType.Float, "labelVariable");
            //build model
            classifierOutput = CreateFullyChannelNetwork(inputVariable, c, outputClassNum);
            Function loss = CNTKLib.SquaredError(classifierOutput, outputVariable);
            Function pred = CNTKLib.ClassificationError(classifierOutput, outputVariable);
            //adam leaner
            ParameterVector parameterVector = new ParameterVector(classifierOutput.Parameters().ToList());
            TrainingParameterScheduleDouble learningRateSchedule = new TrainingParameterScheduleDouble(0.00178125, BatchSize);
            TrainingParameterScheduleDouble momentumRateSchedule = new TrainingParameterScheduleDouble(0.9, BatchSize);
            Learner leaner = CNTKLib.AdamLearner(parameterVector, learningRateSchedule, momentumRateSchedule, true);
            //构造leaner方法
            trainer = Trainer.CreateTrainer(classifierOutput, loss, pred, new List<Learner>() { leaner });
            //TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.00178125, BatchSize); //0.00178125
            //TrainingParameterScheduleDouble momentumTimeConstant = CNTKLib.MomentumAsTimeConstantSchedule(256);
            //IList<Learner> parameterLearners = new List<Learner>() { Learner.MomentumSGDLearner(classifierOutput.Parameters(), learningRatePerSample, momentumTimeConstant, true) };
            //trainer = Trainer.CreateTrainer(classifierOutput, loss, pred, parameterLearners);
        }

        /// <summary>
        /// create from saved model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="deviceName"></param>
        public FCN7(byte[] modelBuffer, string deviceName)
        {
            device = NP.CNTKHelper.GetDeviceByName(deviceName);
            Function model = Function.Load(modelBuffer, device);
            inputVariable = model.Inputs.First(v => v.Name == "inputVariable");
            outputVariable = Variable.InputVariable(model.Output.Shape, DataType.Float, "labelVariable");
            classifierOutput = model;
            Function loss = CNTKLib.SquaredError(classifierOutput, outputVariable);
            Function pred = CNTKLib.ClassificationError(classifierOutput, outputVariable);
            //adam leaner
            ParameterVector parameterVector = new ParameterVector(classifierOutput.Parameters().ToList());
            TrainingParameterScheduleDouble learningRateSchedule = new TrainingParameterScheduleDouble(0.00178125, BatchSize);
            TrainingParameterScheduleDouble momentumRateSchedule = new TrainingParameterScheduleDouble(0.9, BatchSize);
            Learner leaner = CNTKLib.AdamLearner(parameterVector, learningRateSchedule, momentumRateSchedule, true);
            //构造leaner方法
            trainer = Trainer.CreateTrainer(classifierOutput, loss, pred, new List<Learner>() { leaner });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuffer"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static FCN7 Load(byte[] modelBuffer, string deviceName)
        {
            return new FCN7(modelBuffer, deviceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputChannel"></param>
        /// <param name="outputClassNum"></param>
        /// <returns></returns>
        private Function CreateFullyChannelNetwork(Variable input, int inputChannel, int outputClassNum)
        {
            int[] channels = new int[] { inputChannel, inputChannel << 1, inputChannel << 2, inputChannel << 2, inputChannel };
            //3x3xc+3x3x(2c)+maxpooling
            Function network = NP.Keras.Layers.ConvolutionLayer(input, channels[1], new int[] { 3, 3, channels[0] }, device, CNTKLib.ReLU, strides: new int[] { 1, 1, channels[0] });
            network = NP.Keras.Layers.ConvolutionLayer(network, channels[2], new int[] { 3, 3, channels[1] }, device, CNTKLib.ReLU, strides: new int[] { 1, 1, channels[1] });
            network = CNTKLib.Pooling(network, PoolingType.Max, new int[] { 2, 2, 2 }, new int[] { 1 }, new bool[] { true });
            network = CNTKLib.Dropout(network, 0.2f);
            //3x3x(2c)+3x3x(4c)+maxpooling
            network = NP.Keras.Layers.ConvolutionLayer(network, channels[3], new int[] { 3, 3, channels[2] }, device, CNTKLib.ReLU, strides: new int[] { 1, 1, channels[2] });
            network = NP.Keras.Layers.ConvolutionLayer(network, channels[4], new int[] { 3, 3, channels[3] }, device, CNTKLib.ReLU, strides: new int[] { 1, 1, channels[3] });
            network = CNTKLib.Pooling(network, PoolingType.Max, new int[] { 2, 2, 2 }, new int[] { 1 }, new bool[] { true });
            network = CNTKLib.Dropout(network, 0.2f);
            //dense
            //return NP.CNTKHelper.Dense(network, outputClassNum, device, CNTKLib.Softmax, "ouput");
            return NP.CNTKHelper.Dense(network, outputClassNum, device, CNTKLib.SELU, "ouput");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns></returns>
        public double Train(float[][] inputs, float[][] outputs)
        {
            //ensure that data is destroyed after use
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, NP.ToOneDimensional(inputs), device))
            using (Value outputsValue = Value.CreateBatch(outputVariable.Shape, NP.ToOneDimensional(outputs), device))
            {
                traindEpochs++;
                var miniBatch = new Dictionary<Variable, Value>() { { inputVariable, inputsValue }, { outputVariable, outputsValue } };
                trainer.TrainMinibatch(miniBatch, false, device);
                return trainer.PreviousMinibatchEvaluationAverage();
            }
        }

        public void ConvertToExtractNetwork()
        {
            throw new NotImplementedException();
        }

        public byte[] PersistenceMemory()
        {
            byte[] modelBytes = classifierOutput.Save();
            return modelBytes;
        }

        public string PersistencNative(string modelFilename = null)
        {
            byte[] bytes = PersistenceMemory();
            Stream modelStream = new MemoryStream(bytes);
            using (FileStream fileStream = File.Create(modelFilename))
                modelStream.CopyTo(fileStream);
            return modelFilename;
            //modelFilename = modelFilename ?? string.Format(@"{0}\tmp\{1}_{2}_{3}_{4}_{5}_{6}.net", Directory.GetCurrentDirectory(), DateTime.Now.ToFileTimeUtc(), inputVariable.Shape[0], inputVariable.Shape[1], inputVariable.Shape[2], traindEpochs, typeof(FCN7).Name);
            //classifierOutput.Save(modelFilename);
            //return modelFilename;
        }

        /// <summary>
        ///  forward calcute, same as batch size equals one
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public float[] Predict(float[] input)
        {
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, input, device))
            {
                var inputDict = new Dictionary<Variable, Value>() { { inputVariable, inputsValue } };
                var outputDict = new Dictionary<Variable, Value>() { { classifierOutput.Output, null } };
                classifierOutput.Evaluate(inputDict, outputDict, device);
                IList<IList<float>> prdicts = outputDict[classifierOutput.Output].GetDenseData<float>(classifierOutput.Output);
                float[] result = prdicts[0].ToArray();
                ////}{debug fix Resize error
                //outputDict[classifierOutput.Output].Erase();
                //outputDict[classifierOutput.Output].Dispose();
                //inputDict[inputVariable].Erase();
                //inputDict[inputVariable].Dispose();
                return result;
            }
        }

        public float[][] Predicts(float[][] inputs)
        {
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, NP.ToOneDimensional(inputs), device))
            {
                var inputDict = new Dictionary<Variable, Value>() { { inputVariable, inputsValue } };
                var outputDict = new Dictionary<Variable, Value>() { { classifierOutput.Output, null } };
                classifierOutput.Evaluate(inputDict, outputDict, device);
                var prdict = outputDict[classifierOutput.Output].GetDenseData<float>(classifierOutput.Output);
                float[][] outputs = new float[inputs.Length][];
                for (int i = 0; i < inputs.Length; i++)
                    outputs[i] = prdict[i].ToArray();
                return outputs;
            }
        }

        public void Accept(ISupportNet sourceNet)
        {
            //convert to bytes 
            byte[] bytes = sourceNet.PersistenceMemory();
            //read model and set parameters
            classifierOutput = Function.Load(bytes, device);
            inputVariable = classifierOutput.Inputs.First(v => v.Name == "inputVariable");
            outputVariable = Variable.InputVariable(classifierOutput.Output.Shape, DataType.Float, "labelVariable");
            var trainingLoss = CNTKLib.SquaredError(classifierOutput, outputVariable);
            var prediction = CNTKLib.SquaredError(classifierOutput, outputVariable);
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.00178125, 1); //0.00178125
            TrainingParameterScheduleDouble momentumTimeConstant = CNTKLib.MomentumAsTimeConstantSchedule(256);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.MomentumSGDLearner(classifierOutput.Parameters(), learningRatePerSample, momentumTimeConstant, true) };
            trainer = Trainer.CreateTrainer(classifierOutput, trainingLoss, prediction, parameterLearners);
        }
    }
}
