using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using obs_test.Domain.Models;

namespace obs_test.Application.Extensions;

public static class JsonFileUtils
{
    private static readonly JsonSerializerOptions Options =
        new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

    public static void SimpleWrite(object obj, string fileName)
    {
        var jsonString = JsonSerializer.Serialize(obj, Options);
        File.WriteAllText(fileName, jsonString);
    }

    public static void PrettyWrite(object obj, string fileName)
    {
        var options = new JsonSerializerOptions(Options)
        {
            WriteIndented = true
        };
        var jsonString = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(fileName, jsonString);
    }

    public static void WriteDynamicJsonObject(JsonObject jsonObj, string fileName)
    {
        using var fileStream = File.Create(fileName);
        using var utf8JsonWriter = new Utf8JsonWriter(fileStream);
        jsonObj.WriteTo(utf8JsonWriter);
    }

    public static InputData? ReadJsonObject(string inputFilePath)
    {
        var inputJson = File.ReadAllText(inputFilePath);
        return JsonSerializer.Deserialize<InputData>(inputJson, Options);
    }

    public static async Task<InputData?> ReadJsonObjectAsync(IFormFile inputJsonFile)
    {
        await using var inputStream = inputJsonFile.OpenReadStream();
        var inputJson = await new StreamReader(inputStream).ReadToEndAsync();
        return JsonSerializer.Deserialize<InputData>(inputJson, Options);
    }
}