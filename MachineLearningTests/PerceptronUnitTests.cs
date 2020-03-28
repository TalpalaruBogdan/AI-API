using MachineLearning;
using NUnit.Framework;
using System.Diagnostics;

namespace MachineLearningTests
{
    public class PerceptronUnitTests
    {
        Data d1, d2, d3, d4, testData;
        Data[] datas;
        Perceptron p;

        [SetUp]
        public void Setup()
        {
            d1 = new Data();
            d1.inputs = new double[3] { 1, 1, 0 };
            d1.output = 1;

            d2 = new Data();
            d2.inputs = new double[3] { 1, 0, 1 };
            d2.output = 1;

            d3 = new Data();
            d3.inputs = new double[3] { 0, 0, 1 };
            d3.output = 0;

            d4 = new Data();
            d4.inputs = new double[3] { 0, 1, 1 };
            d4.output = 0;

            datas = new Data[4];
            datas[0] = d1;
            datas[1] = d2;
            datas[2] = d3;
            datas[3] = d4;

            p = new Perceptron(3, 0.5);
            p.Train(datas, 5000);
        }

        [TestCase(1)]
        [TestCase(99)]
        [TestCase(10000)]
        public void IfFirstElementIsZero_WithAnySeed_IShouldReceiveAScore_Below_0_02(int seed)
        {
            p = new Perceptron(3, 0.5, seed);
            p.Train(datas, 10000);
            testData = new Data();
            testData.inputs = new double[3] { 0, 0, 0 };
            testData.output = 0;
            System.Console.WriteLine("Result is: " + p.Predict(testData));
            Assert.True(p.Predict(testData) < 0.02);
        }

        [TestCase(1)]
        [TestCase(99)]
        [TestCase(10000)]
        public void IfFirstElementIsOne_WithAnySeed_IShouldReceiveAScore_Above_0_98(int seed)
        {
            p = new Perceptron(3, 0.5, seed);
            p.Train(datas, 10000);
            testData = new Data();
            testData.inputs = new double[3] { 1, 0, 0 };
            testData.output = 0;
            System.Console.WriteLine("Result is: " + p.Predict(testData));
            Assert.True(p.Predict(testData) > 0.98);
        }

        [TestCase(1, 0, 0, 1, 0)]
        [TestCase(0, 1, 0, 1, 1)]
        [TestCase(0, 0, 1, 1, 2)]
        public void NetworkLearnsCorrectlyWeights(int a, int b, int c, int d, int e)
        {
            p = new Perceptron(3, 0.5);
            Data[] datas = new Data[1];
            datas[0] = new Data { inputs = new double[3] { a, b, c }, output = d };
            p.Train(datas, 10000);
            Assert.True(p.GetBiggestWeightIndex() == e);
        }

        [TestCase(10000)]
        [TestCase(100000)]
        public void SmallDataset_Learning_Iterations_ShouldBeVeryFast(int iterations)
        {
            p = new Perceptron(3, 0.5);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            p.Train(datas, iterations);
            sw.Stop();
            System.Console.WriteLine("Result is: " + sw.ElapsedMilliseconds);
            if (iterations < 10001 ) Assert.True(sw.ElapsedMilliseconds <= 5);
            else Assert.True(sw.ElapsedMilliseconds <= 50);
        }


        [TestCase(10000, 10, 10000)]
        [TestCase(100000, 10, 10000)]
        [TestCase(10000, 10, 100000)]
        [TestCase(100000, 10, 100000)]
        public void HugeNetwork_Trains_Correctly(int inputs, int indexHighlight, int epochs)
        {
            p = new Perceptron(inputs, 0.5);
            Data d = new Data();
            d.inputs = new double[inputs];
            for (int i = 0; i < inputs; i++) d.inputs[i] = 0;
            d.inputs[indexHighlight] = 1;
            d.output = 1;
            Data[] datas = new Data[1];
            datas[0] = d;
            p.Train(datas, epochs);
            int index = p.GetBiggestWeightIndex();
            Assert.True(index == indexHighlight);
        }
    }
}