using Microsoft.ML.Data;

namespace NoSecret.AI.Entity.Output;

public class OutputEntity
{
    [ColumnName("PredictedLabel")] public bool Prediction { get; set; }

    public float Probability { get; set; }

    public float Score { get; set; }
}