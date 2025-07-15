using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitWindows : MonoBehaviour
{
    public void QuitGame()
    {
       // if (Input.GetKey("escape"))
        {
            //Application.Quit();
            QuitApplicationUtility.MoveAndroidApplicationToBack();
        }
    }
}
