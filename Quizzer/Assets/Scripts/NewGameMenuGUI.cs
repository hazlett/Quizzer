using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewGameMenuGUI : MonoBehaviour {
    private List<string> users;
    private string classroom = "", opponent = "";
	void Start () {
        users = new List<string>();
        StartCoroutine(LoadUsers());
    }
	
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Utility.GUIPOSITION, Quaternion.identity, new Vector3(Screen.width / Utility.SCREENWIDTH, Screen.height / Utility.SCREENHEIGHT, 1)); 
        if (GUILayout.Button("BACK"))
        {
            Application.LoadLevel("Menu");
        }
        GUILayout.Label("<b>OPPONENT:</b> " + opponent);
        GUILayout.Label("<b>CLASSROOM:</b> " + classroom);
        GUILayout.Label("<b>CLASSROOMS</b>");
        foreach (string room in Loading.Instance.Classrooms)
        {
            if (GUILayout.Button(room))
            {
                classroom = room;
            }
        }
        GUILayout.Label("<b>USERS</b>");
        foreach (string user in users)
        {
            if (user != Questions.Instance.CurrentUser.Name)
            {
                if (GUILayout.Button(user))
                {
                    opponent = user;
                }
            }
        }
        GUILayout.Space(10);
        if (GUILayout.Button("SEND INVITE"))
        {
            if (opponent != "" && classroom != "")
                Questions.Instance.NewGame(opponent, classroom);
        }
    }
    private IEnumerator LoadUsers()
    {
        WWW www = new WWW("http://hazlettdavid.com/Quizzer/GetUsers.php");
        yield return www;
        if (www.error == null)
        {
            users = Questions.Instance.DeserializeToList(www.text);
        }
        else
        {
            Debug.Log("Loading Users Error: " + www.error);
        }
    }

}
