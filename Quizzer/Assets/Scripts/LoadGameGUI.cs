using UnityEngine;
using System.Collections;

public class LoadGameGUI : MonoBehaviour {

    void Start()
    {
        Debug.Log("Loading game with classroom: " + Questions.Instance.CurrentGame.Classroom);
        Loading.Instance.LoadQuestions(Questions.Instance.CurrentGame.Classroom);
    }
    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Utility.SCREENHEIGHT, Screen.height / Utility.SCREENWIDTH, 1)); 
        GUILayout.Label("<b>LOADING GAME</b>");
    }
	

}
