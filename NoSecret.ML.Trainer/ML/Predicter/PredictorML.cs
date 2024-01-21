using Microsoft.ML;
using NoSecret.AI.Entity.Input;
using NoSecret.AI.Entity.Output;
using NoSecret.AI.ML.Base;

namespace NoSecret.AI.ML.Predicter;

public class PredictorML : BaseML
{
    private readonly string _modelPath;
    private ITransformer _trainedModel;

    public PredictorML(string modelPath)
    {
        _modelPath = modelPath;
    }

    public string Predict(string inputData)
    {
        try
        {
            using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _trainedModel = MlContext.Model.Load(stream, out _);
            }

            if (_modelPath == null)
            {
                Console.WriteLine("Failed to load model");

                return "";
            }

            var predictionEngine = MlContext.Model.CreatePredictionEngine<InputEntity, OutputEntity>(_trainedModel);

            // Make a prediction
            var prediction = predictionEngine.Predict(new InputEntity { Secret = inputData });

            return prediction.Prediction.ToString();
            //return $"Based on \"{inputData}\", the feedback is predicted to be: {(prediction.Prediction)} at a {prediction.Probability:P0} confidence";
        }
        catch (Exception ex)
        {
            return "";
        }
    }
}