using obs_test.Domain.Enums;

namespace obs_test.Domain.Validators
{
    internal class SampleParser
    {
        private static readonly Dictionary<string, Samples> SamplesMappings = new()
        {
        { "Fe", Samples.Ferrum },
        { "Se", Samples.Selenium },
        { "W", Samples.Water },
        { "Si", Samples.Silicon },
        { "Zn", Samples.Zinc },
        { "Obs", Samples.Obstacle },
        };

        public static bool TryParse(string sampleText, out Samples Sample) => SamplesMappings.TryGetValue(sampleText, out Sample);
    }
}
