using System.Collections.Generic;
using UnityEngine;

public class IPipelineBehaviour : MonoBehaviour
{
    // Format: 
    // Key: Name of data
    // Value: Interaction data
    protected Dictionary<string, IInteractionData> data = new Dictionary<string, IInteractionData>();
}
