using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Diploma {
    public class Kinetic {
        private List<string> _reactions = new();
        private List<string> _components = new();
        public Kinetic(List<string> reactions) {
            _reactions = reactions;
            for (int i = 0; i < _reactions.Count; i++) {
                _reactions[i] = _reactions[i].Replace(" ", "");
            }
        }

        public List<string> GetComponents() {
            return _components;
        }

        private string Remove(string inp) {
            for (int i = 0; i < inp.Length; i++)
                if (char.IsLetter(inp[i]) || inp[i] == '[') {
                    return inp.Substring(i);
                }
            return inp;
        }

        public List<string> GetAllElements() {
            List<string> elements = new();

            foreach (string reaction in _reactions) {
                var splittedReaction = reaction.Split("->");

                foreach (string parts in splittedReaction) {

                    if (parts.Contains("+")) {
                        var splittedAgain = parts.Split("+");

                        foreach (string el in splittedAgain) {

                            string removedNumbers = Remove(el);
                            if (!elements.Contains(removedNumbers)) {
                                elements.Add(removedNumbers);
                            }
                        }

                    } else {
                        string removedNumbers = Remove(parts);
                        if (!elements.Contains(removedNumbers)) {
                            elements.Add(removedNumbers);
                        }
                    }

                }

            }

            _components = elements;

            return elements;
        }


        private int GetNumber(string input) {
            return int.Parse(Regex.Match(input, @"\d+").Value);
        }

        public List<Dictionary<string, int>> GetMatrix() {
            var allElements = GetAllElements();


            List<Dictionary<string, int>> matrix = new();

            foreach (string reaction in _reactions) {
                Dictionary<string, int> elements = new();

                foreach (string comp in allElements) {
                    elements[comp] = 0;
                }

                var splittedReaction = reaction.Split("->");

                for (int i = 0; i < splittedReaction.Length; i++) {
                    if (splittedReaction[i].Contains("+")) {
                        var splittedAgain = splittedReaction[i].Split("+");

                        foreach (string el in splittedAgain) {

                            if (allElements.Contains(el)) {
                                elements[el] = (i == 0) ? -1 : 1;
                            } else {
                                int koeff = GetNumber(el);
                                string removedNumbers = Remove(el);
                                elements[removedNumbers] = (i == 0) ? (-1) * koeff : koeff;

                            }
                            //string removedNumbers = Remove(el);

                        }

                    } else {
                        if (allElements.Contains(splittedReaction[i])) {
                            elements[splittedReaction[i]] = (i == 0) ? -1 : 1;
                        } else {
                            int koeff = GetNumber(splittedReaction[i]);
                            string removedNumbers = Remove(splittedReaction[i]);
                            elements[removedNumbers] = (i == 0) ? (-1) * koeff : koeff;

                        }

                    }
                }

                matrix.Add(elements);

            }

            return matrix;
        }

        public List<List<int>> GetBiMatrix() {
            var matrix = GetMatrix();
            List<List<int>> biMatrix = new();

            foreach (var dict in matrix) {
                List<int> row = new List<int>();
    
                foreach (var key in dict.Keys) {
                    row.Add(dict[key]);
                }
                biMatrix.Add(row);

            }

            return biMatrix;
        }
    }
}
