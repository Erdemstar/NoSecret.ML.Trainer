using Microsoft.ML.Data;

namespace NoSecret.AI.Entity.Input;

public class InputEntity
{
    [LoadColumn(0)] public bool Label { get; set; }

    //[LoadColumn(1)] public bool Entropy { get; set; }
    [LoadColumn(1)] public string Secret { get; set; }
}