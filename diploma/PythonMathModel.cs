﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Python.Runtime;
using System.IO;
using System.Net;
using System.Globalization;

namespace Diploma {
    class PythonMathModel {
        private List<double> _startConcentration = new();
        private List<double> _aValues = new();
        private List<double> _eValues = new();
        private double _temperature;
        private string _method;
        //private List<double> _reactionSpeed = new();
        private double _contactTime;
        private const int COUNT_OF_ELEMENTS = 23;
        private const int COUNT_OF_SPEED = 21;

        public PythonMathModel(List<double> startConcentration, List<double> aValues, List<double> eValues, double temperature,  double contactTime, string method) {
            _startConcentration = startConcentration;
            _aValues = aValues;
            _eValues = eValues;
            _temperature = temperature;
           // _reactionSpeed = reactionSpeed;
            _contactTime = contactTime;
            _method = method;
        }



        public List<List<double>> RunScript() {
            string scriptName = @"..\..\..\ImportantFiles\math_model.py";
            Runtime.PythonDLL = @"python\python39.dll";
            PythonEngine.Initialize();
            List<List<double>> concentation = new();
            try {
                using (Py.GIL()) {

                    using (var scope = Py.CreateScope()) {
                        var scriptFileName = scriptName;
                        var compiledFile = PythonEngine.Compile(File.ReadAllText(scriptFileName), scriptFileName);

                        scope.Execute(compiledFile); 

                        var concentrationListPy = new PyList();
                        for (int i = 0; i < COUNT_OF_ELEMENTS; i++) {
                            concentrationListPy.Append(new PyFloat(_startConcentration[i]));
                        }

                        var aListPy = new PyList();
                        for (int i = 0; i < COUNT_OF_SPEED; i++) {
                            aListPy.Append(new PyFloat(_aValues[i]));
                        }

                        var eListPy = new PyList();
                        for (int i = 0; i < COUNT_OF_SPEED; i++) {
                            eListPy.Append(new PyFloat(_eValues[i]));
                        }

                        var temperaturePy = new PyFloat(_temperature);

                        var contactTimePy = new PyFloat(_contactTime);

                        var methodPy = new PyString(_method);

                        NumberFormatInfo format = new NumberFormatInfo();
                        format.NumberGroupSeparator = ".";

                        var result = scope.InvokeMethod("calculate_math_model", new PyObject[] { concentrationListPy, aListPy, eListPy, temperaturePy, contactTimePy, methodPy }).ToString();
                        dynamic concentrationsJson = JsonConvert.DeserializeObject(result);


                        foreach (var conc in concentrationsJson) {
                            string currentDataString = conc.Value.ToString();
    

                            currentDataString = currentDataString.Substring(5, currentDataString.Length - 8);
                            string[] currentDataArray = currentDataString.Split(",\r\n");
                            List<double> concList = new();
                            foreach (string s in currentDataArray)
                            {
                               // s.Replace('.', ',');
                                double parsedS = double.Parse(s, format);
                                if (parsedS < 0) {
                                    parsedS = 0;
                                }
                                concList.Add(parsedS);
                            }

                            concentation.Add(concList);
                        }
                        

                    }
                }

            } finally {
                PythonEngine.Shutdown();
            }

            return concentation;


        }
    }
}
