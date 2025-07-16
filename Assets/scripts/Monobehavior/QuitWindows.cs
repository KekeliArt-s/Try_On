using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitWindows : MonoBehaviour
{

    public void Start()
    {
        {
            Input.backButtonLeavesApp = true;
        }
        // Optionally, you can add any initialization code here
    }
    public void QuitGame()
    {
       // if (Input.GetKey("escape"))
        {
            //Application.Quit();
            QuitApplicationUtility.MoveAndroidApplicationToBack();
        }
    }
}
