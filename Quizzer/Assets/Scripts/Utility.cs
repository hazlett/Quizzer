using UnityEngine;
using System.Collections;

public class Utility
{
    internal static Resolution STARTINGRESOLUTION = Screen.currentResolution;
    internal static Vector3 GUIPOSITION = Vector3.zero;
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_BLACKBERRY
    internal static float SCREENWIDTH = Screen.currentResolution.width;
    internal static float SCREENHEIGHT = Screen.currentResolution.height;
#else
    internal static float SCREENHEIGHT = Screen.height;
    internal static float SCREENWIDTH = Screen.width;
#endif



}
