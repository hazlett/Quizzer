using UnityEngine;
using System.Collections;

public class Utility
{
#if UNITY_WEBPLAYER
    internal static float SCREENHEIGHT = 1920f;
    internal static float SCREENWIDTH = 1080f;
#elif UNITY_ANDROID
    internal static float SCREENHEIGHT = 1020f;
    internal static float SCREENWIDTH = 768f;
#endif

}
