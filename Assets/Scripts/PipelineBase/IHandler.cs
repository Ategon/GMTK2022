using System.Collections.Generic;

// Classes that inherit this should only write data to IInteractionData
public interface IHandler
{
    // Similar to MonoBehaviour.Start()
    public void Init(Dictionary<string, IInteractionData> data);
    
    // Similar to MonoBehaviour.Update()
    public void Handle();
}
