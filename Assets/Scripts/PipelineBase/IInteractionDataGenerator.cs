// Interface for data generation
public interface IInteractionDataGenerator
{
    class GenerationParams { }

    IInteractionData GenerateData(GenerationParams parameters, ref IInteractionData data);
}
