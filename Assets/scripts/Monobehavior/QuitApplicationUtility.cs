/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplicationUtility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
*/

using UnityEngine;

public static class QuitApplicationUtility
{
    public static void MoveAndroidApplicationToBack()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call<bool>("moveTaskToBack", true);
#endif
    }
}
