using CommandLine;

namespace NoSecret.Scanner.Core.Entities.Argument;

public class ArgumentEntity
{
    [Option("predict", Required = false, Default = false)]
    public bool Predict { get; set; }

    [Option("train", Required = false, Default = false)]
    public bool Train { get; set; }

    [Option("model-path", Required = false, Default = "")]
    public string ModelPath { get; set; }

    //traint
    [Option("train-data-file-path", Required = false, Default = "")]
    public string TrainDataFilePath { get; set; }

    [Option("train-data-db-category", Required = false, Default = "")]
    public string TrainDataDBCategory { get; set; }

    // predict
    [Option("sample-data-file-path", Required = false, Default = "")]
    public string SampleDataFilePath { get; set; }

    //predict
    [Option("sample-data-db-category", Required = false, Default = "")]
    public string SampleDataDBCategory { get; set; }
}