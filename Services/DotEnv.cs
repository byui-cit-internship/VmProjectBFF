namespace vmProjectBFF.Services
{
    // Blatently stolen from https://dusted.codes/dotenv-in-dotnet
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}
