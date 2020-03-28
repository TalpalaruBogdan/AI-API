using System;

namespace MachineLearning
{
    public class Calculations
    {
        public double Sigmoid(double value)
        {
            return (1 / (1 + Math.Pow(Math.E, -value)));
        }

        public double ReLU(double value)
        {
            return Math.Max(0, value);
        }

        public double Step(double value)
        {
            return value < 0 ? 0 : 1;
        }

        public double SigmoidWeightedSum(double[] values1, double[] values2)
        {
            double output = 0;
            for (int i = 0; i < values1.Length; i++)
            {
                output += (values1[i] * values2[i]);
            }
            return Sigmoid(output);
        }

        public double CostFunction(double expected, double predicted)
        {
            return (1 / 2 * (expected - predicted));
        }

    }
}
