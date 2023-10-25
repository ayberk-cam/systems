using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    public enum FadeType
    {
        Single,
        Whole,
        Parent,
        Multiple
    }

    public FadeType fadeType;
    public bool faded;
    FadeCheck fadeCheck;
    public GameObject[] otherObjects;

    public Material originalMaterial;

    [SerializeField] Material transparentMaterial;

    private void Awake()
    {
        originalMaterial = gameObject.GetComponent<Renderer>().material;
    }

    void Start()
    {
        fadeCheck = WorldManager.instance.GetCurrentCar().GetComponent<FadeCheck>();
    }

    void Update()
    {
        if (fadeType == FadeType.Single)
        {
            if (fadeCheck.objectHit == gameObject)
            {
                if (!faded)
                {
                    gameObject.GetComponent<Renderer>().sharedMaterial = transparentMaterial;
                    for (int i = 0; i < otherObjects.Length; i++)
                    {
                        if(otherObjects[i] != null)
                        {
                            otherObjects[i].GetComponent<Renderer>().sharedMaterial = transparentMaterial;
                        }
                    }
                    faded = true;
                }
            }
            else
            {
                if (faded)
                {
                    gameObject.GetComponent<Renderer>().sharedMaterial = originalMaterial;
                    for (int i = 0; i < otherObjects.Length; i++)
                    {
                        if(otherObjects[i] != null)
                        {
                            otherObjects[i].GetComponent<Renderer>().sharedMaterial = otherObjects[i].GetComponent<ObjectFade>().originalMaterial;
                            otherObjects[i].GetComponent<ObjectFade>().faded = false;
                        }
                    }
                    faded = false;
                }
            }
        }
    }
}
