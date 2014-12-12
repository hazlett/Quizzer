using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnGUI()
    {
        GUILayout.Label("MAIN MENU");
        if (GUI.Button(new Rect(0,100.0f, Screen.width * 0.5f, Screen.height * 0.25f), "TAKE YOUR TURN"))
        {
            Application.LoadLevel("Main");
        }
    }
}
