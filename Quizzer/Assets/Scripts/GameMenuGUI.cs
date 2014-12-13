using UnityEngine;
using System.Collections;

public class GameMenuGUI : MonoBehaviour {

	void Start () {
	    
	}
	
	void Update () {
	
	}
    void OnGUI()
    {
        foreach (Question q in Questions.Instance.allQuestions)
        {
            GUILayout.Label(q.QuestionText);
        }
    }
}
