using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace CellularAutomata
{
    public class Elementary : ProblemVisualization
    {
        private Dictionary<string, char> ruleMap;

        public Elementary()
        {
            Name = "Cellular Automaton: Elementary (1D)";
            Settings.Add("Rule", "90");
            Settings.Add("Cell Size", "3");
            Settings.Add("Pattern Size", "100");
            Settings.Add("Iterations", "100");
        }

        public Dictionary<string, char> RuleMap(int rule)
        {
            var ruleBinaryString = Convert.ToString(rule, 2).PadLeft(8, '0');
            return new Dictionary<string, char>
            {
                {"111", ruleBinaryString[0]},
                {"110", ruleBinaryString[1]},
                {"101", ruleBinaryString[2]},
                {"100", ruleBinaryString[3]},
                {"011", ruleBinaryString[4]},
                {"010", ruleBinaryString[5]},
                {"001", ruleBinaryString[6]},
                {"000", ruleBinaryString[7]},
            };
        }

        public string Evaluate(string pattern)
        {
            var nextPattern = new char[pattern.Length];

            for (var i = 0; i < pattern.Length; i++)
            {
                var l = i > 0 ? pattern[i - 1] : '0';
                var c = pattern[i];
                var r = i < pattern.Length - 1 ? pattern[i + 1] : '0';

                var localPattern = new string(new [] {l, c, r});
                nextPattern[i] = ruleMap[localPattern];
            }

            return new string(nextPattern);
        }

        public override Bitmap Visualization
        {
            get
            {
                var rule = Settings.GetIntValue("Rule");
                var cellSize = Settings.GetIntValue("Cell Size");
                var patternSize = Settings.GetIntValue("Pattern Size");
                var iterations = Settings.GetIntValue("Iterations");

                ruleMap = RuleMap(rule);

                var bitmap = new Bitmap(cellSize * patternSize, cellSize * iterations);
                var rects = new List<Rectangle>();

                var patternArray = Enumerable.Repeat('0', patternSize).ToArray();
                patternArray[patternSize / 2] = '1';
                var pattern = new string(patternArray);
                for (var i = 0; i < iterations; i++)
                {
                    for (var j = 0; j < patternSize; j++)
                    {
                        if (pattern.ToCharArray()[j] == '1')
                        {
                            var x = j * cellSize;
                            var y = i * cellSize;
                            var rect = new Rectangle(x, y, cellSize, cellSize);
                            rects.Add(rect);
                        }
                    }

                    pattern = Evaluate(pattern);
                }

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                    graphics.FillRectangles(Brushes.Black, rects.ToArray());
                }

                return bitmap;
            }
        }
    }
}
