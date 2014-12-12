using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
    public Color color;
    public Texture2D background;
	void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ApplyBackground();
    }
    void ApplyBackground()
    {
        camera.backgroundColor = color;
    }

}
