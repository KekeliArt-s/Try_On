using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARFaceManagerController : MonoBehaviour
{
    public ARFaceManager arFaceManager;

    void Start()
    {
        arFaceManager.requestedMaximumFaceCount = 1;
    }
}

