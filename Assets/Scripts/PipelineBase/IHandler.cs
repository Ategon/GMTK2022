using System.Collections.Generic;

public interface IHandler
{
    // Similar to MonoBehaviour.Start()
    public void Init(Dictionary<string, IInteractionData> data);
    
    // Similar to MonoBehaviour.Update()
    public void Handle();
}
