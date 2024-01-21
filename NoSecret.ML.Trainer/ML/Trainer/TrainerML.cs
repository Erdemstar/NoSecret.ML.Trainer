using Microsoft.ML;
using NoSecret.AI.Entity.Input;
using NoSecret.AI.ML.Base;

namespace NoSecret.AI.ML.Trainer;

public class TrainerML : BaseML
{
    public bool Trainer(IEnumerable<InputEntity> inputEntityTrainData, string modelPath)
    {
        try
        {
            // Load data from inputEntityData variable
            var trainingDataView = MlContext.Data.LoadFromEnumerable(inputEntityTrainData);

            // // List reading data
            //ListModelContent(trainingDataView);

            var dataSplit = MlContext.Data.TrainTestSplit(trainingDataView);

            //
            var dataProcessPipeline =
                MlContext.Transforms.CopyColumns("Label", nameof(InputEntity.Label))
                    .Append(MlContext.Transforms.Text.FeaturizeText("Features", nameof(InputEntity.Secret)));

            //
            var sdcaRegressionTrainer = MlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName:nameof(InputEntity.Label),
                featureColumnName:"Features");
            
            //maximumNumberOfIterations: 500,
            //l2Regularization: 0.01F);

            //
            var trainingPipeline = dataProcessPipeline.Append(sdcaRegressionTrainer);

            //
            var trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);

            //
            var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

            // Save model
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, modelPath);

            //
            var modelMetrics = MlContext.BinaryClassification.Evaluate(
                testSetTransform);

            Console.WriteLine($"Area Under Curve: {modelMetrics.AreaUnderRocCurve:P2}{Environment.NewLine}" +
                              $"Area Under Precision Recall Curve: {modelMetrics.AreaUnderPrecisionRecallCurve:P2}{Environment.NewLine}" +
                              $"Accuracy: {modelMetrics.Accuracy:P2}{Environment.NewLine}" +
                              $"F1Score: {modelMetrics.F1Score:P2}{Environment.NewLine}" +
                              $"Positive Recall: {modelMetrics.PositiveRecall:#.##}{Environment.NewLine}" +
                              $"Negative Recall: {modelMetrics.NegativeRecall:#.##}{Environment.NewLine}");

            return true;
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
            return false;
        }
    }

    public void ListModelContent(IDataView data)
    {
        var preview = data.Preview();
        foreach (var column in preview.ColumnView)
            Console.WriteLine($"Column '{column.Column.Name}', Type: {column.Column.Type}");

        var rowCount = preview.RowView.Length;
        var columnCount = preview.RowView[0].Values.Length;
        for (var i = 0; i < rowCount; i++)
        {
            for (var j = 0; j < columnCount; j++) Console.Write($"{preview.RowView[i].Values[j]}, ");
            Console.WriteLine();
        }
    }
}