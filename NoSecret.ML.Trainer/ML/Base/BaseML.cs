using Microsoft.ML;

namespace NoSecret.AI.ML.Base;

public class BaseML
{
    protected readonly MLContext MlContext;

    protected BaseML()
    {
        MlContext = new MLContext();
    }
}