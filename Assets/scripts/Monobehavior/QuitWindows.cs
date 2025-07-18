using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitWindows : MonoBehaviour
{
    public GameObject quitDialog; // Assign in Inspector

    public void Start()
    {
        {
            Input.backButtonLeavesApp = true;
            if (quitDialog != null)
                quitDialog.SetActive(false); // Hide dialog at start
        }
        // Optionally, you can add any initialization code here
    }

    // Call this method to show the quit dialog
    public void ShowQuitDialog()
    {
        if (quitDialog != null)
            quitDialog.SetActive(true);
    }
    // Call this method to hide the quit dialog
    public void QuitGame()
    {
       // if (Input.GetKey("escape"))
        {
            //Application.Quit();
            QuitApplicationUtility.MoveAndroidApplicationToBack();
        }
    }
    // Called when "Cancel" button is pressed
    public void CancelQuit()
    {
        if (quitDialog != null)
            quitDialog.SetActive(false);
    }
}
