using UnityEngine;
using System.Collections;

public class ChangeTurnsGUI : MonoBehaviour {

    void Start()
    {
        Questions.Instance.ChangeTurn();
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Utility.SCREENWIDTH, Screen.height / Utility.SCREENHEIGHT, 1)); 
        GUILayout.Label("<b>SENDING MOVE</b>");
    }
}
