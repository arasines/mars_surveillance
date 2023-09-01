using obs_test.Domain.Enums;

namespace obs_test.Domain.Validators;

internal class TerrainTypeParser
{
    public static bool TryParse(string sampleText, out TerrainType sample) => Enum.TryParse(sampleText, out sample);
}