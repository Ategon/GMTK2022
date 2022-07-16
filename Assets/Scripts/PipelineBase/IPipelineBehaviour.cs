using System.Collections.Generic;
using UnityEngine;

// Holds all the IInteractionData, IHandler, IInteractionDataGenerator
public class IPipelineBehaviour : MonoBehaviour
{
    // Format: 
    // Key: Name of data
    // Value: Interaction data
    protected Dictionary<string, IInteractionData> data = new Dictionary<string, IInteractionData>();
}
