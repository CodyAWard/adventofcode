using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode.Days
{
    public class Day6 : BaseDay
    {
        public override int Year() => 2019;
        public override int Day() => 6;

        private class Orbit
        {
            public string Key;
            public Orbit Parent;
            public List<Orbit> Children;
        }

        public override string GetSolution()
        {
            var input = GenerateInput();

            List<Orbit> orbits = new List<Orbit>();

            foreach (var data in input)
            {
                var sides = data.Split(')');

                var root = GetOrCreateOrbit(sides[0], orbits);
                if(root.Parent == null && !orbits.Contains(root)) orbits.Add(root);

                var moon = GetOrCreateOrbit(sides[1], orbits);
                if (orbits.Contains(moon)) orbits.Remove(moon);

                moon.Parent = root;
                root.Children.Add(moon);
            }

            var santa = GetOrbit("SAN", orbits);
            var you = GetOrbit("YOU", orbits);
            var santaParents = GetParents(santa);
            var youParents = GetParents(you);

            for (int i = 0; i < youParents.Length; i++)
            {
                for (int j = 0; j < santaParents.Length; j++)
                {
                    if (youParents[i].Key == santaParents[j].Key) return (i + j).ToString();
                }
            }
            return "error";

            //CountOrbits(orbits, ref total); //part 1

            //return $"{total} orbits";
        }

        private Orbit[] GetParents(Orbit orbit)
        {
            List<Orbit> parents = new List<Orbit>();
            var par = orbit.Parent;
            while (par != null)
            {
                parents.Add(par);
                par = par.Parent;
            }
            return parents.ToArray();
        }

        private void CountOrbits(List<Orbit> orbits, ref int count)
        {
            foreach (var orbit in orbits)
            {
                var parent = orbit.Parent;
                while (parent != null)
                {
                    count++;
                    parent = parent.Parent;
                }

                if (orbit.Children.Count > 0)
                {
                    CountOrbits(orbit.Children, ref count);
                }
            }
        }

        private Orbit GetOrCreateOrbit(string key, List<Orbit> orbits)
        {
            var orb = GetOrbit(key, orbits);
            if (orb != null)
                return orb;

            return new Orbit() { Key = key, Children = new List<Orbit>() };
        }

        private Orbit GetOrbit(string key, List<Orbit> orbits)
        {
            foreach (var orbit in orbits)
            {
                if (orbit.Key == key) return orbit;
                else
                {
                    var orb = GetOrbit(key, orbit.Children);
                    if (orb != null) return orb;
                }
            }
            return null;
        }

        private string[] GenerateInput()
        {
            var input = new List<string>(); 
            var line = string.Empty;
            StreamReader file = new StreamReader($@"..\..\Data\Day{Day()}Input.txt");
            while ((line = file.ReadLine()) != null)
            {
                input.Add(line);
            }

            file.Close();

            return input.ToArray();
        }
    }
}