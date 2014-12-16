using UnityEngine;
using System.Collections;

public class GameMenuGUI : MonoBehaviour {

	void Start () {
	    
	}
	
	void Update () {
	
	}
    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Utility.SCREENHEIGHT, Screen.height / Utility.SCREENWIDTH, 1)); 
        foreach (Question q in Questions.Instance.allQuestions)
        {
            GUILayout.Label(q.QuestionText);
        }
    }
}
