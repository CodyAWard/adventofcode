using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day8 : BaseDay
    {
        public override int Year() => 2019;
        public override int Day() => 8;

        public class Row
        {
            public int[] Data { get; set; }
        }

        public class Layer
        {
            public Row[] Rows;

            public int CountDigit(int digit)
            {
                int total = 0;
                foreach (var row in Rows)
                {
                    foreach (var data in row.Data)
                    {
                        if (data == digit) total++;
                    }
                }
                return total;
            }
        }

        enum Colors
        {
            Black = 0,
            White = 1,
            Transparent = 2,
        }

        public override string GetSolution()
        {
            var input = GenerateInput();

            var width = 25;
            var height = 6;

            int layerSize = width * height;
            var layerCount = input.Length / layerSize;

            var layers = new Layer[layerCount];

            for (int l = 0; l < layerCount; l++)
            {
                var layer = new Layer
                {
                    Rows = new Row[height]
                };
                layers[l] = layer;

                for (int y = 0; y < height; y++)
                {
                    var row = new Row
                    {
                        Data = new int[width]
                    };
                    layer.Rows[y] = row;

                    for (int x = 0; x < width; x++)
                    {
                        int index = x + (width * y);
                        index += (layerSize * l);

                        row.Data[x] = input[index];
                    }
                }
            }

            var image =  new Colors[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    image[x, y] = Colors.Transparent;
                }
            }

            foreach (var layer in layers)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var curCol = image[x, y];
                        if (curCol != Colors.Transparent) continue;

                        image[x, y] = (Colors)layer.Rows[y].Data[x];
                    }
                }
            }

            for (int y = 0; y < height; y++)
            {
                string line = "";
                for (int x = 0; x < width; x++)
                {
                    var col = image[x, y];
                    if (col == Colors.White) line += "*";
                    else line += " ";
                }
                Debug.WriteLine(line);
            }

            //int leastZeros = int.MaxValue; //part 1
            //Layer leastZeroLayer = null;
            //foreach (var layer in layers)
            //{
            //    var count = layer.CountDigit(0);
            //    if (count < leastZeros)
            //    {
            //        leastZeros = count;
            //        leastZeroLayer = layer;
            //    }
            //}

            //int ones = leastZeroLayer.CountDigit(1);
            //int twos = leastZeroLayer.CountDigit(2);

            return "";
        }
        
        private int[] GenerateInput()
        {
            var input = new List<int>();
            var line = string.Empty;
            StreamReader file = new StreamReader($@"..\..\Data\Day{Day()}Input.txt");
            var data = file.ReadToEnd();
            foreach (var character in data)
            {
                var value = int.Parse(character.ToString());
                input.Add(value);
            }

            file.Close();

            return input.ToArray();
        }
    }
}