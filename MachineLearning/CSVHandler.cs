using System;
using System.IO;

namespace MachineLearning
{
    public class CSVHandler
    {

        public Data[] CollectData(string filePath)
        {
            Data[] datas;
            var readData = File.ReadAllLines(filePath);
            datas = new Data[readData.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                var results = readData[i].Split(',');
                datas[i] = new Data();
                datas[i].inputs = new double[results.Length - 1];
                for (int j = 0; j < results.Length - 1; j++)
                {
                    datas[i].inputs[j] = Convert.ToDouble(results[j]);
                }
                try
                {
                    datas[i].output = Convert.ToDouble(results[results.Length - 1]);

                }
                catch (Exception exc)
                {
                    //TO DO FOR NO OUTPUT
                }
            }

            return datas;

        }
    }
}
