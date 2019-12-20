using Ward.Days;

namespace Ward
{
    class Program
    {
        static void Main(string[] args)
        {
            var days = new BaseDay[]
               {
                    //new Day1(), // DONE **
                    //new Day2(), // DONE **
                    //new Day3(), // HALF *
                    //new Day4(), // HALF *
                    //new Day5(), // DONE **
                    //new Day6(), // DONE **
                    //new Day7(), // DONE ** tad ugly
                    //new Day8(), // DONE ** 
                    new Day9(),
               };

            foreach (var day in days)
            {
                day.Run();
            }
        }
    }
}
