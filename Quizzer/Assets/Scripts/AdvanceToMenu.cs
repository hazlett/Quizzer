using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System;
public class AdvanceToMenu : MonoBehaviour {
    private bool loaded;
    private string userName = "David";
	void Start () {
        StartCoroutine(GetClassrooms());
	}

    void OnGUI()
    {
        //GUILayout.Label("USER NAME");
        //userName = GUILayout.TextField(userName);
        //if (GUILayout.Button("ADVANCE"))
        //{
        //    StartCoroutine(LoadUser(userName));
        //}
        //if (GUILayout.Button("CREATE USER"))
        //{
        //    StartCoroutine(CreateUser(userName));
        //}
        GUILayout.Label("<b>ALL USERS</b>");
        if (GUILayout.Button("David"))
        {
            StartCoroutine(LoadUser("David"));
        }
        if (GUILayout.Button("John"))
        {
            StartCoroutine(LoadUser("John"));
        }
    }
	
    void Update()
    {
        if (loaded)
        {
            Application.LoadLevel("Menu");
        }
    }
    private IEnumerator LoadUser(string user)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", user);
        WWW www = new WWW("http://hazlett206.ddns.net/Quizzer/LoadUser.php", form);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("User loaded successful: " + www.text);
            Questions.Instance.SetUser(DeserializeToUser(www.text));
            loaded = true;
        }
        else
        {
            Debug.Log("User failed to load: " + www.error);
        }
    }
    private IEnumerator CreateUser(string user)
    {
        User newUser = new User(user);
        WWWForm form = new WWWForm();
        form.AddField("username", user);
        form.AddField("content", newUser.ToXml());
        WWW www = new WWW("http://hazlett206.ddns.net/Quizzer/CreateUser.php", form);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("User create: " + www.text);
            Questions.Instance.SetUser(newUser);
            loaded = true;
        }
        else
        {
            Debug.Log("Create user error: " + www.error);
        }
    }
    private IEnumerator GetClassrooms()
    {
        Loading.Instance.SetClassrooms((new List<string>() { "QuestionManager" }));
        yield return null;
    }
    private User DeserializeToUser(string text)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            User obj = new User();
            XmlSerializer serializer = new XmlSerializer(typeof(User));
            XmlReader reader = new XmlNodeReader(doc);

            obj = serializer.Deserialize(reader) as User;

            return obj;
        }
        catch (Exception)
        {
            return new User();
        }
    }


}
