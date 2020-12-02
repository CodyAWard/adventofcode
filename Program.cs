using AdventOfCode.Days;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var allDays = CollectDays();
            DisplayHelp(allDays);

            var line = Console.ReadLine();
            while (line != "quit")
            {
                if (line == "help")
                {
                    DisplayHelp(allDays);
                }

                var names = allDays.Select(day => $"{day.Year()}.{day.Day()}").ToArray();
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == line)
                    {
                        Console.WriteLine("Running " + line); 
                        allDays[i].Run();
                    }
                }

                line = Console.ReadLine();
            }
        }

        private static void DisplayHelp(BaseDay[] allDays)
        {
            Console.WriteLine();
            Console.WriteLine("year.day to run a day");
            Console.WriteLine();
            ListDays(allDays);
        }

        private static void ListDays(BaseDay[] allDays)
        {
            Console.WriteLine("== All Days ==");
            foreach (var day in allDays)
            {
                Console.WriteLine($"   Year: {day.Year()} Day: {day.Day()}");
            }
        }

        private static BaseDay[] CollectDays()
        {
            var allDayTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
            {
                return typeof(BaseDay).IsAssignableFrom(t)
                && !t.IsAbstract;
            }).ToArray();

            var allDays = new BaseDay[allDayTypes.Length];
            for (int i = 0; i < allDays.Length; i++)
            {
                allDays[i] = Activator.CreateInstance(allDayTypes[i]) as BaseDay;
            }

            allDays.OrderBy(d => (d.Year() * 100) + d.Day());
            return allDays;
        }
    }
}
