using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSwingable : MonoBehaviour
{
    PlayerController pCon;
    public void OnPlayerAttach(PlayerController pCon)
    {
        this.pCon = pCon;
        SetLayerRecursively(gameObject.transform, "RopeUnhittable");
    }

    public void OnPlayerDetach()
    {
        StartCoroutine(DelayedLayerChange());
    }

    public void SetAsCollidable()
    {
        
    }

    IEnumerator DelayedLayerChange()
    {
        yield return new WaitForSeconds(0.6f);
        SetLayerRecursively(gameObject.transform, "Rope");
    }

    void SetLayerRecursively(Transform root, string layerName)
    {
        // Set the layer of the current GameObject
        root.gameObject.layer = LayerMask.NameToLayer(layerName);

        // Iterate through each child and set their layers recursively
        foreach (Transform child in root)
        {
            SetLayerRecursively(child, layerName);
        }
    }
}
