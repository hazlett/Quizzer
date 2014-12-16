using UnityEngine;
using System.Collections;

public class Utility
{
#if UNITY_ANDROID
    internal static float SCREENHEIGHT = 1020f;
    internal static float SCREENWIDTH = 768f;
#else
    internal static float SCREENHEIGHT = 1920f;
    internal static float SCREENWIDTH = 1080f;
#endif

}
