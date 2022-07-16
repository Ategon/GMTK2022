// Generates the IInteractionData
public interface IInteractionGenerator
{
    public class GenerationParams { }

    public IInteractionData GenerateData(GenerationParams parameters, ref IInteractionData data);
}
