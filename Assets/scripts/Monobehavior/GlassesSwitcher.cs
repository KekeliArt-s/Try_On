using System.Collections;

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using System.Collections.Generic;

public class GlassesSwitcher : MonoBehaviour
{
    public List<GameObject> glassesPrefabs;  // assign in Inspector
    public Transform attachmentPoint; // place on face prefab where glasses are attached
    private GameObject currentGlasses;

    public void SetGlasses(int index)
    {
        if (currentGlasses != null)
            Destroy(currentGlasses);

        currentGlasses = Instantiate(glassesPrefabs[index], attachmentPoint);
    }
}


