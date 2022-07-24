using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFaces : MonoBehaviour
{
    [Tooltip("Use this material when there's no effect equipped on that face")]
    [SerializeField] Material unequippedDiceFace;
    [SerializeField] DiceFacePair[] diceFaces;

    [SerializeField] MeshRenderer[] diceFaceQuads;

    Dictionary<PowerupType, Material> diceFaces_Dictionary;

    [System.Serializable]
    public class DiceFacePair
    {
        public PowerupType powerupType;
        public Material material;
    }

    private void Awake()
    {
        diceFaces_Dictionary = new Dictionary<PowerupType, Material>();

        foreach (DiceFacePair diceFace in diceFaces)
            diceFaces_Dictionary.Add(diceFace.powerupType, diceFace.material);
    }

    public void SetDiceFaces(PowerupSettings[] powerupSettings)
    {
        if (diceFaceQuads.Length != powerupSettings.Length)
            Debug.LogError("Number of dice faces != number of powerups. Empty faces should be passed in as null");

        for (int i = 0; i < diceFaceQuads.Length; i++)
        {


            if (powerupSettings[i] == null)
                diceFaceQuads[i].material = unequippedDiceFace;
            else
            {

                if (diceFaceQuads == null)
                    print("Error");
                if (diceFaceQuads[i] == null)
                    print("Error");
                if (diceFaces_Dictionary == null)
                    print("Error");
                if (powerupSettings == null)
                    print("Error");
                if (powerupSettings[i] == null)
                    print("Error");
                if (diceFaces_Dictionary[powerupSettings[i].powerupType] == null)
                    print("Error");

                diceFaceQuads[i].material = diceFaces_Dictionary[powerupSettings[i].powerupType];
            }
        }
    }
}
