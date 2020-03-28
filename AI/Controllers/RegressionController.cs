using MachineLearning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace AI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegressionController : ControllerBase
    {

        [HttpPost]
        [Route("Create")]
        public ActionResult CreateAI(int InputSize)
        {
            try
            {
                if (InputSize <= 0 | InputSize > 10000) return NotFound("Please enter a valid number");
                string newMachineId = Guid.NewGuid().ToString();
                System.IO.File.Create(Directory.GetCurrentDirectory() + @"\Machines\" + newMachineId.ToString() + ".txt").Close();
                System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + @"\Machines\" + newMachineId.ToString() + ".txt", InputSize.ToString());
                System.IO.File.AppendAllText(Directory.GetCurrentDirectory() + @"\MachinesList.txt", newMachineId.ToString() + "," + DateTime.Now.ToString() + "\n");
                return Ok("MACHINE ID " + newMachineId + " was CREATED");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPut]
        [Route("Train")]
        public ActionResult Train(string guid, string DataFilePath)
        {
            try
            {
                Perceptron perceptron = new Perceptron(Convert.ToInt32(System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + @"\Machines\" + guid.ToString() + ".txt")));
                CSVHandler cSVHandler = new CSVHandler();
                var trainSet = cSVHandler.CollectData(DataFilePath);
                //TODO Change Hardcoded values
                perceptron.Train(trainSet, 10000);
                var weights = perceptron.GetWeights();
                string[] values = new string[weights.Length];
                System.IO.File.Delete(System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + @"\Machines\" + guid.ToString() + ".txt"));

                for (int i = 0; i < weights.Length; i++)
                {
                    System.IO.File.AppendAllText(Directory.GetCurrentDirectory() + @"\Machines\" + guid.ToString() + ".txt", weights[i].ToString() + Environment.NewLine);
                }
                return Ok("MACHINE ID " + guid + " was TRAINED");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("Predict")]
        public ActionResult Predict(string ID, string DataFilePath)
        {
            try
            {
                var output = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Machines\" + ID + ".txt");
                Perceptron p = new Perceptron(output.Length - 1);
                string fileName = Directory.GetCurrentDirectory() + @"\Machines\" + ID + ".txt";
                p.ReadWeightsFromFile(fileName);
                var csv = new CSVHandler();
                var datas = csv.CollectData(DataFilePath);
                List<double> results = new List<double>();
                for (int i = 0; i < datas.Length; i++)
                {
                    results.Add(p.Predict(datas[i]));
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}