using CNTK;

using System;
using System.Linq;

namespace GrainImplement.DRL.Utils
{
    public partial class NP
    {
        public static class Keras
        {
            public static class Layers
            {

                #region 私有方法

                /// <summary>
                /// 
                /// </summary>
                /// <param name="input">输入变量</param>
                /// <param name="convolution_map_size">卷积核参数</param>
                /// <param name="device"></param>
                /// <param name="use_padding"></param>
                /// <param name="use_bias"></param>
                /// <param name="strides"></param>
                /// <param name="activation"></param>
                /// <param name="outputName"></param>
                /// <returns></returns>
                static Function Convolution(
                    Variable input,
                    int[] convolution_map_size,
                    DeviceDescriptor device,
                    bool use_padding,
                    bool use_bias,
                    int[] strides,
                    Func<Variable, string, Function> activation = null,
                    string outputName = "")
                {
                    //构造 Y=W*X+B
                    Parameter W = new Parameter(
                        NDShape.CreateNDShape(convolution_map_size),
                        DataType.Float,
                        CNTKLib.GlorotUniformInitializer(
                            CNTKLib.DefaultParamInitScale,
                            CNTKLib.SentinelValueForInferParamInitRank,
                            CNTKLib.SentinelValueForInferParamInitRank,
                            1),
                        device,
                        outputName + "_W");
                    //构造Y=W*X
                    Function y = CNTKLib.Convolution(
                        W,
                        input,
                        strides,
                        new BoolVector(new bool[] { true }), //sharing
                        new BoolVector(new bool[] { use_padding }));
                    //应用偏差项
                    if (use_bias)
                    {
                        int num_output_channels = convolution_map_size[convolution_map_size.Length - 1];
                        int[] b_shape = Concat(Enumerable.Repeat(1, convolution_map_size.Length - 2).ToArray(), new int[] { num_output_channels });
                        Parameter b = new Parameter(
                            b_shape,
                            DataType.Float,
                            CNTKLib.GlorotUniformInitializer(
                            CNTKLib.DefaultParamInitScale,
                            CNTKLib.SentinelValueForInferParamInitRank,
                            CNTKLib.SentinelValueForInferParamInitRank,
                            1),
                            device,
                            outputName + "_b");
                        y = CNTKLib.Plus(y, b);
                    }
                    //应用激活函数
                    if (activation != null)
                    {
                        y = activation(y, outputName);
                    }
                    //输出
                    return y;
                }

                /// <summary>
                /// 全连接层
                /// https://github.com/microsoft/CNTK/blob/e9396480025b9ca457d26b6f33dd07c474c6aa04/Examples/TrainingCSharp/Common/TestHelper.cs#L52
                /// https://github.com/axmand/deep-learning-with-csharp-and-cntk/blob/2cc585d593a9f3a9f2065b8f25fcc24efd04ee17/DeepLearning/Util.cs#L752
                /// </summary>
                /// <param name="input"></param>
                /// <param name="outputDim">输出维度</param>
                /// <param name="device"></param>
                /// <param name="outputName"></param>
                /// <returns></returns>
                static Function FullyConnectedLinearLayer(
                    Variable input,
                    int outputDim,
                    DeviceDescriptor device,
                    string outputName = "")
                {
                    Parameter w = new Parameter(
                        new int[] { outputDim, NDShape.InferredDimension },
                        DataType.Float,
                        CNTKLib.GlorotUniformInitializer(
                            CNTKLib.DefaultParamInitScale,
                            CNTKLib.SentinelValueForInferParamInitRank,
                            CNTKLib.SentinelValueForInferParamInitRank,
                            1),
                        device,
                        outputName + "_timesParam"
                        );
                    Parameter b = new Parameter(
                        new int[] { outputDim },
                        DataType.Float,
                        CNTKLib.GlorotUniformInitializer(
                            CNTKLib.DefaultParamInitScale,
                            CNTKLib.SentinelValueForInferParamInitRank,
                            CNTKLib.SentinelValueForInferParamInitRank,
                            1),
                        device,
                        outputName + "_plusParam");
                    Function t = CNTKLib.Times(
                        w,
                        input,
                        1 /* output dimension */,
                        0 /* CNTK should infer the input dimensions */);
                    Function y = CNTKLib.Plus(b, t, outputName);
                    return y;
                }

                #endregion

                /// <summary>
                /// 
                /// </summary>
                /// <param name="input">输入变量</param>
                /// <param name="num_output_channels">输出通道</param>
                /// <param name="filter_shape">二维数组，表示 MxN 卷积核</param>
                /// <param name="device"></param>
                /// <param name="activation">激活函数</param>
                /// <param name="use_padding">使用Padding填充数值</param>
                /// <param name="use_bias">使用偏差项，即 +b</param>
                /// <param name="strides">采样间隔，默认是1</param>
                /// <param name="outputName"></param>
                /// <returns></returns>
                public static Function ConvolutionLayer(
                    Variable input,
                    int num_output_channels,
                    int[] filter_shape,
                    DeviceDescriptor device,
                    Func<Variable, string, Function> activation,
                    bool use_padding = true,
                    bool use_bias = true,
                    int[] strides = null,
                    string outputName = "")
                {
                    //采样步长默认是1
                    strides = strides ?? (new int[] { 1 });
                    //卷积核参数 NDShape.InferredDimension
                    int[] convolution_map_size = new int[] { filter_shape[0], filter_shape[1], filter_shape[2], num_output_channels };
                    return Convolution(input, convolution_map_size, device, use_padding, use_bias, strides, activation, outputName);
                }

                /// <summary>
                /// 带有激活函数的全连接层
                /// </summary>
                /// <param name="input"></param>
                /// <param name="outputDim"></param>
                /// <param name="device"></param>
                /// <param name="activation"></param>
                /// <param name="outputName"></param>
                /// <returns></returns>
                public static Function Dense(
                    Variable input,
                    int outputDim,
                    DeviceDescriptor device,
                    Func<Variable, Function> activation = null,
                    string outputName = "")
                {
                    if (input.Shape.Rank != 1)
                    {
                        int reshapeDim = input.Shape.Dimensions.Aggregate((d1, d2) => d1 * d2);
                        input = CNTKLib.Reshape(input, new int[] { reshapeDim });
                    }
                    Function fc = FullyConnectedLinearLayer(input, outputDim, device, outputName);
                    fc = activation != null ? activation(fc) : fc;
                    return fc;
                }
                //
            }
        }
    }
}
