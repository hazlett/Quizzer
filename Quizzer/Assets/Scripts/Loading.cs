using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System;

public class Loading : MonoBehaviour {

    private static Loading instance;
    public static Loading Instance { get { return instance; } }
    private bool loaded = false, loading = false, categories = false;
    private string getQuestionsXMLURL = "http://hazlett206.ddns.net/QuestionManager/GetQuestionsXML.php",
        getCategoriesURL = "http://hazlett206.ddns.net/QuestionManager/GetCategories.php";
    private string message = "";
    private List<string> classrooms;
    public List<string> Classrooms { get { return classrooms; } }
	void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
	
	void Update () {
	    if (loaded && categories)
        {
            loading = false;
            loaded = false;
            categories = false;
            Application.LoadLevel("Main");
        }
	}
    void OnGUI()
    {
        GUILayout.Label(message);
        if (loading)
        {
            GUI.Label(new Rect(Screen.width * 0.5f - 50.0f, 0, 100.0f, 50.0f), "...LOADING...");
        }
    }
    public void LoadClassrooms()
    {
        StartCoroutine(GetClassrooms());
    }
    internal void SetClassrooms(List<string> classrooms)
    {
        this.classrooms = classrooms;
    }
    private IEnumerator GetClassrooms()
    {
        SetClassrooms((new List<string>() { "QuestionManager" }));
        yield return null;
    }
    public void LoadQuestions(string name)
    {
        if (name == "" || name == null)
        {
            name = "QuestionManager";
        }
        loading = true;
        getQuestionsXMLURL = "http://hazlett206.ddns.net/" + name + "/GetQuestionsXML.php";
        getCategoriesURL = "http://hazlett206.ddns.net/" + name + "/GetCategories.php";
        StartCoroutine(Load());
        StartCoroutine(LoadCategories());
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
