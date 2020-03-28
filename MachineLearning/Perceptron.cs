using System;
using System.IO;

namespace MachineLearning
{
    public class Perceptron
    {
        private Calculations _calculations;
        private double[] _synapses;
        private double[] _inputs;
        private double _predictedOutput;
        private Random _random;
        private int _networkSize;
        private double _learnRate;

        public Perceptron(int _inputsCount, double learnRate = 0.1, int weightsSeed = 1)
        {
            _learnRate = learnRate;
            _networkSize = _inputsCount + 1;
            _calculations = new Calculations();
            _random = new Random(weightsSeed);
            _inputs = new double[_networkSize];
            _synapses = new double[_networkSize];
            for (int i = 0; i < _networkSize; i++) _synapses[i] = _random.NextDouble();
        }

        public void Populate_inputs(double[] values)
        {
            for (int i = 0; i < values.Length - 1; i++) _inputs[i] = values[i];
            //Assign value 1 to bias neuron
            _inputs[_networkSize - 1] = 1.0;
        }

        private void FeedForward()
        {
            _predictedOutput = _calculations.SigmoidWeightedSum(_inputs, _synapses);
        }

        private void BackPropagate(double expected)
        {
            //calculate delta error for neuron
            var deltaErrorLocal = -(expected - _predictedOutput);
            //calculate delta input
            var deltaNetImput = _predictedOutput * (1 - _predictedOutput);
            for (int i = 0; i < _networkSize; i++)
            {
                //calcualte delta for weight
                var deltaSynapse = _inputs[i];
                //Update
                _synapses[i] = _synapses[i] - (_learnRate * deltaErrorLocal * deltaNetImput * deltaSynapse);
            }
        }

        // Trains the newural network using a dataset
        public void Train(Data[] data, int epochs)
        {
            try
            {
                for (int i = 0; i < epochs; i++)
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        Populate_inputs(data[j].inputs);
                        FeedForward();
                        BackPropagate(data[j].output);
                    }
                }
            }
            catch (Exception ex)
            {
                //TO DO
            }            
        }

        // Returns the predicted output of a trained network based on data input
        public double Predict(Data data)
        {
            try
            {
                Populate_inputs(data.inputs);
                FeedForward();
                return _predictedOutput;
            }
            catch (Exception ex)
            {
                //TO DO
            }
            return -1000;
        }

        public int GetBiggestWeightIndex()
        {
            double max = 0;
            int index = -1;
            for (int i = 0; i < _synapses.Length - 1; i++)
            {
                max = Math.Max(max, _synapses[i]);
                if (max == _synapses[i])
                {
                    index = i;
                }
            }
            return index;
        }

        public double[] GetWeights()
        {
            return this._synapses;
        }

        public void ReadWeightsFromFile(string file)
        {
            var rez = File.ReadAllLines(file);
            for (int i = 0; i < _synapses.Length; i++)
            {
                _synapses[i] = Convert.ToDouble(rez[i]);
            }
        }
    }
}
