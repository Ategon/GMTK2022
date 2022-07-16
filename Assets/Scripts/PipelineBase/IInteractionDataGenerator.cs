// Interface for data generation
public interface IInteractionDataGenerator
{
    public class GenerationParams { }

    public IInteractionData GenerateData(GenerationParams parameters, ref IInteractionData data);
}
