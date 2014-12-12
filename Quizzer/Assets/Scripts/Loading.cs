using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System;

public class Loading : MonoBehaviour {

    private bool loaded = false, loading = false, categories = false;
    private string getQuestionsXMLURL = "http://hazlett206.ddns.net/QuestionManager/GetQuestionsXML.php",
        getCategoriesURL = "http://hazlett206.ddns.net/QuestionManager/GetCategories.php";
    private string name = "QuestionManager";
    private string message = "";
	void Start () {
        
	}
	
	void Update () {
	    if (loaded && categories)
        {
            Application.LoadLevel("Menu");
        }
	}
    void OnGUI()
    {
        if (loading)
        {
            GUI.Label(new Rect(Screen.width * 0.5f - 50.0f, 0, 100.0f, 50.0f), "...LOADING...");
        }
        else
        {
            GUILayout.Label(message);
            GUI.Label(new Rect(0, Screen.height * 0.2f - 50.0f, Screen.width * 0.5f, 50.0f), "CLASS ROOM NAME");
            name = GUI.TextField(new Rect(0, Screen.height * 0.2f, Screen.width * 0.5f, Screen.height * 0.1f), name);
            if (GUI.Button(new Rect(0, Screen.height * 0.3f, Screen.width * 0.5f, Screen.height * 0.1f), "BEGIN"))
            {
                loading = true;
                getQuestionsXMLURL = "http://hazlett206.ddns.net/" + name + "/GetQuestionsXML.php";
                getCategoriesURL = "http://hazlett206.ddns.net/" + name + "/GetCategories.php";
                StartCoroutine(Load());
                StartCoroutine(LoadCategories());
            }
        }
    }

    private IEnumerator LoadCategories()
    {
        Debug.Log("Loading");
        WWW www = new WWW(getCategoriesURL);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("Successful list download: " + www.text);
            Questions.Instance.SetCategories(DeserializeToCategories(www.text));
        }
        else
        {
            message = "COULD NOT LOAD THE CATEGORIES";
            Questions.Instance.SetCategories(DeserializeToCategories("-"));
            Debug.Log("Error downloading: " + www.error);
        }
        categories = true;
    }
    private IEnumerator Load()
    {
        Debug.Log("Loading");
        WWW www = new WWW(getQuestionsXMLURL);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("Successful list download: " + www.text);
            Questions.Instance.Initialize(DeserializeToQuestions(www.text));
            loaded = true;
        }
        else
        {
            loading = false;
            message = "COULD NOT LOAD THAT CLASS ROOM";
            Debug.Log("Error downloading: " + www.error);
        }
    }

    private List<Question> DeserializeToQuestions(string text)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            List<Question> obj = new List<Question>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Question>));
            XmlReader reader = new XmlNodeReader(doc);

            obj = serializer.Deserialize(reader) as List<Question>;

            return obj;
        }
        catch (Exception)
        {
            return new List<Question>();
        }
    }
    private string[] DeserializeToCategories(string text)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            XmlSerializer serializer = new XmlSerializer(typeof(string[]));
            XmlReader reader = new XmlNodeReader(doc);

            string[] obj = serializer.Deserialize(reader) as string[];

            return obj;
        }
        catch (Exception)
        {
            return new string[] { "Category 1", "Category 2", "Category 3", "Category 4", "Category 5", "Category 6" };
        }
    }
}
