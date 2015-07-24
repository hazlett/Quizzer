using UnityEngine;
using System.Collections;

public class ChangeTurnsGUI : MonoBehaviour {

    void Start()
    {
    }
    void Update()
    {
        if (Questions.Instance.advance)
        {
            Questions.Instance.advance = false;
            Application.LoadLevel("Menu");
        }
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Utility.GUIPOSITION, Quaternion.identity, new Vector3(Screen.width / Utility.SCREENWIDTH, Screen.height / Utility.SCREENHEIGHT, 1)); 
        GUILayout.Label("<b>SENDING MOVE</b>");
    }
}
