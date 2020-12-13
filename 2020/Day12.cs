using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.MMXX
{
    public class Day12 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();

            return
                PartA(data) + "\n" +
                PartB(data);
        }

        public class Instruction
        {
            public string Command;
            public int Value;

            public Instruction(string data)
            {
                Command = data.Substring(0, 1);
                Value = int.Parse(data.Substring(1));
            }
        }

        private string PartA(IEnumerable<string> data)
        {
            int x = 0, y = 0;
            var direction = "E";

            foreach (var line in data)
            {
                var instruction = new Instruction(line);
                switch (instruction.Command)
                {
                    case "N":
                        y += instruction.Value;
                        break;
                    case "S":
                        y -= instruction.Value;
                        break;
                    case "E":
                        x += instruction.Value;
                        break;
                    case "W":
                        x -= instruction.Value;
                        break;
                    case "F":
                        switch (direction)
                        {
                            case "N":
                                y += instruction.Value;
                                break;
                            case "S":
                                y -= instruction.Value;
                                break;
                            case "E":
                                x += instruction.Value;
                                break;
                            case "W":
                                x -= instruction.Value;
                                break;
                        }
                        break;
                    case "L":
                        switch (instruction.Value)
                        {
                            case 0:
                                break;
                            case 90:
                                direction = Turn270(direction);
                                break;
                            case 180:
                                direction = Turn180(direction);
                                break;
                            case 270:
                                direction = Turn90(direction);
                                break;
                            case 360:
                                break;
                            default:
                                throw new InvalidOperationException(instruction.Command + " : " + instruction.Value);
                        }
                        break;
                    case "R":
                        switch (instruction.Value)
                        {
                            case 0:
                                break;
                            case 90:
                                direction = Turn90(direction);
                                break;
                            case 180:
                                direction = Turn180(direction);
                                break;
                            case 270:
                                direction = Turn270(direction);
                                break;
                            case 360:
                                break;
                            default:
                                throw new InvalidOperationException(instruction.Command + " : " + instruction.Value);
                        }
                        break;
                    default:
                        throw new InvalidOperationException(instruction.Command);
                }
            }

            return $"{x} {y} = {(Math.Abs(x) + Math.Abs(y))}";
        }

        private string PartB(IEnumerable<string> data)
        {
            int x = 0, y = 0;
            Vector2 waypoint = new Vector2(10, 1);

            foreach (var line in data)
            {
                var instruction = new Instruction(line);
                switch (instruction.Command)
                {
                    case "N":
                        waypoint.Y += instruction.Value;
                        break;
                    case "S":
                        waypoint.Y -= instruction.Value;
                        break;
                    case "E":
                        waypoint.X += instruction.Value;
                        break;
                    case "W":
                        waypoint.X -= instruction.Value;
                        break;
                    case "F":
                        x += (int)waypoint.X * instruction.Value;
                        y += (int)waypoint.Y * instruction.Value;
                        break;
                    case "L":
                        var leftTurns = instruction.Value / 90f;
                        for (int i = 0; i < leftTurns; i++)
                        {
                            var w = waypoint;
                            waypoint.X = -w.Y;
                            waypoint.Y = w.X;
                        }
                        break;

                    case "R":
                        var rightTurns = instruction.Value / 90f;
                        for (int i = 0; i < rightTurns; i++)
                        {
                            var w = waypoint;
                            waypoint.X = w.Y;
                            waypoint.Y = -w.X;
                        }
                        break;
                    default:
                        throw new InvalidOperationException(instruction.Command);
                }
            }

            return $"{x} {y} = {(Math.Abs(x) + Math.Abs(y))}";
        }

        private static string Turn90(string direction)
        {
            if (direction == "N") direction = "E";
            else if (direction == "E") direction = "S";
            else if (direction == "S") direction = "W";
            else if (direction == "W") direction = "N";
            return direction;
        }

        private static string Turn180(string direction)
        {
            if (direction == "N") direction = "S";
            else if (direction == "E") direction = "W";
            else if (direction == "S") direction = "N";
            else if (direction == "W") direction = "E";
            return direction;
        }

        private static string Turn270(string direction)
        {
            if (direction == "N") direction = "W";
            else if (direction == "E") direction = "N";
            else if (direction == "S") direction = "E";
            else if (direction == "W") direction = "S";
            return direction;
        }
    }
}