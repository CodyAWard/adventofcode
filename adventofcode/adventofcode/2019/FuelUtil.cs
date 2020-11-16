namespace AdventOfCode
{
    public static class FuelUtil
    {
        public static int GetRequiredFuelForModule(Module module)
        {
            int fuel = 0;
            GetRequiredFuelRecursive(module.Mass, ref fuel);
            return fuel;
        }

        private static void GetRequiredFuelRecursive(int mass, ref int fuel)
        {
            int newFuel = mass / 3;
            newFuel -= 2;

            if (newFuel > 0) // we are still adding fuel, and need to recursively add fuel
            {
                fuel += newFuel;
                GetRequiredFuelRecursive(newFuel, ref fuel);
            }
        }
    }
}
