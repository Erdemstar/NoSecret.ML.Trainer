using CommandLine;
using NoSecret.AI.ML.Data;
using NoSecret.AI.ML.Predicter;
using NoSecret.AI.ML.Trainer;
using NoSecret.Scanner.Core.Entities.Argument;

var banner = """


             ███╗   ██╗ ██████╗ ███████╗███████╗ ██████╗██████╗ ███████╗████████╗███╗   ███╗██╗
             ████╗  ██║██╔═══██╗██╔════╝██╔════╝██╔════╝██╔══██╗██╔════╝╚══██╔══╝████╗ ████║██║
             ██╔██╗ ██║██║   ██║███████╗█████╗  ██║     ██████╔╝█████╗     ██║   ██╔████╔██║██║
             ██║╚██╗██║██║   ██║╚════██║██╔══╝  ██║     ██╔══██╗██╔══╝     ██║   ██║╚██╔╝██║██║
             ██║ ╚████║╚██████╔╝███████║███████╗╚██████╗██║  ██║███████╗   ██║██╗██║ ╚═╝ ██║███████╗
             ╚═╝  ╚═══╝ ╚═════╝ ╚══════╝╚══════╝ ╚═════╝╚═╝  ╚═╝╚══════╝   ╚═╝╚═╝╚═╝     ╚═╝╚══════╝
             
                                                                             
             """;
//Ansi Shadow
//https://manytools.org/hacker-tools/ascii-banner/
Console.WriteLine(banner);

var dataProvider = new Data(
    "mongodb://localhost:27017",
    "NoSecret",
    "Vulnerability"
);

var parse = Parser.Default.ParseArguments<ArgumentEntity>(args).WithNotParsed(errors => { Environment.Exit(-1); });

if (parse.Value.Train)
{
    var mlTrainer = new TrainerML();

    if (parse.Value.TrainDataDBCategory != "")
    {
        var inputData = dataProvider.ReturnDataFromDBByCategory(parse.Value.TrainDataDBCategory);
        var mlTrainerResult = mlTrainer.Trainer(inputData, parse.Value.ModelPath);

        if (mlTrainerResult == false)
        {
            Console.WriteLine("There is problem while model creating process");
            Environment.Exit(1);
        }
    }

    if (parse.Value.TrainDataFilePath != "")
    {
        var inputData = dataProvider.ReturnTrainerDataFromCSV(parse.Value.TrainDataFilePath);
        var mlTrainerResult = mlTrainer.Trainer(inputData, parse.Value.ModelPath);

        if (mlTrainerResult == false)
        {
            Console.WriteLine("There is problem while model creating process");
            Environment.Exit(1);
        }
    }
}

if (parse.Value.Predict)
{
    var mlPredicter = new PredictorML(parse.Value.ModelPath);
    if (parse.Value.SampleDataFilePath != "")
    {
        Console.WriteLine("!!! DATA is read from File !!!");

        var data = dataProvider.ReturnPredictDataFromCSV(parse.Value.SampleDataFilePath);
        if (data == null) Console.WriteLine("CVS File is empy");

        foreach (var VARIABLE in data)
        {
            Console.Write(VARIABLE + " ");
            var result = mlPredicter.Predict(VARIABLE);
            Console.WriteLine(result);
        }
    }
    else if (parse.Value.SampleDataDBCategory != "")
    {
        Console.WriteLine("!!! DATA is read from DB !!!");

        var data = dataProvider.ReturnDataFromDBByCategory(parse.Value.SampleDataDBCategory);
        foreach (var VARIABLE in data)
        {
            Console.Write(VARIABLE.Secret + " ");
            var result = mlPredicter.Predict(VARIABLE.Secret);
            Console.WriteLine(result);
        }
    }
    else
    {
        for (;;)
        {
            Console.Write("Enter the secret : ");
            var data = Console.ReadLine();

            if (data == "break" || data == "") break;

            var result = mlPredicter.Predict(data);

            if (result == null) Console.WriteLine("There is problem while predict process");

            Console.WriteLine(result);
        }
    }
}