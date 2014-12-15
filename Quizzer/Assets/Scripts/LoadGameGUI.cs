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
        GUILayout.Label("<b>LOADING GAME</b>");
    }
	

}
