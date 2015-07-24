using UnityEngine;
using System.Collections;

public class GameMenuGUI : MonoBehaviour {

	void Start () {
	    
	}
	
	void Update () {
	
	}
    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Utility.GUIPOSITION, Quaternion.identity, new Vector3(Screen.width / Utility.SCREENWIDTH, Screen.height / Utility.SCREENHEIGHT, 1)); 
        foreach (Question q in Questions.Instance.allQuestions)
        {
            GUILayout.Label(q.QuestionText);
        }
    }
}
