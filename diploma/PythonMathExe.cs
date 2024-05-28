using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diploma {
    internal class PythonMathExe {

        private List<double> _startConcentration = new();
        private List<double> _aValues = new();
        private List<double> _eValues = new();
        private List<List<int>> _matrix = new();
        private double _temperature;
        private double _contactTime;
        private List<List<double>> _finalConcentrations = new();

        public PythonMathExe(List<double> startConcentration, List<double> aValues, List<double> eValues, double temperature, double contactTime, List<List<int>> matrix) {
            _startConcentration = startConcentration;
            _aValues = aValues;
            _eValues = eValues;
            _temperature = temperature;
            _contactTime = contactTime;
            _matrix = matrix;
        }

        private List<string> MakeArgs() {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ".";
            List<string> args = new();
            args.Add("-c");
            foreach (var conc in _startConcentration) {
                args.Add(conc.ToString());
                
            }

            args.Add("-m");
            foreach (var a in _aValues) {
                args.Add(a.ToString());
            }

            args.Add("-e");
            foreach (var e in _eValues) {
                args.Add(e.ToString());
            }

            args.Add("-temp");
            args.Add(_temperature.ToString());

            args.Add("-time");
            args.Add(_contactTime.ToString());

            args.Add("-matrix");
            foreach (var row in _matrix) {
                foreach (var el in row) {
                    args.Add(el.ToString());
                }

            }

            return args;
        }

        public List<List<double>> GetPythonOutput() {
            return _finalConcentrations;
        }

        public void PythonWork() {
            List<string> args = MakeArgs();

            DateTime startTime = DateTime.Now;
            var process = new Process();

            process.StartInfo.FileName = @"..\..\..\ImportantFiles\math_model_python.exe";
            //process.StartInfo.FileName = @"C:\Users\lyolya\source\repos\Diploma\diploma\ImportantFiles\math_model_python.exe";

            process.StartInfo.Arguments = String.Join(" ", args);

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            try {
                process.Start();
            } catch (Exception) {
                MessageBox.Show("Не найден файл программы!");
            }

            string YF = string.Empty;
            YF = process.StandardOutput.ReadLine();
            //process.StandardInput.Write(Keys.Enter);
         // process.WaitForExit();
            //resultsForTestFact.Add(YF);
            DateTime endTime = DateTime.Now;
            TimeSpan elapsed = endTime - startTime;
            //calcTimes.Add(elapsed.TotalMilliseconds);
            //return YF;

            GetConcentrationsFromJson();

        }

        private List<List<double>> GetConcentrationsFromJson() {
            string result = File.ReadAllText("concentrations.json");
            List<List<double>> concentations = new();
            dynamic concentrationsJson = JsonConvert.DeserializeObject(result);
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ".";

            foreach (var conc in concentrationsJson) {
                string currentDataString = conc.Value.ToString();


                currentDataString = currentDataString.Substring(5, currentDataString.Length - 8);
                string[] currentDataArray = currentDataString.Split(",\r\n");
                List<double> concList = new();
                foreach (string s in currentDataArray) {
                    // s.Replace('.', ',');
                    double parsedS = double.Parse(s, format);
                    if (parsedS < 0) {
                        parsedS = 0;
                    }
                    concList.Add(parsedS);
                }

                _finalConcentrations.Add(concList);
            }

            return concentations;
        }
    }
}
