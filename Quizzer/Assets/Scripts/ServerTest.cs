using UnityEngine;
using System.Collections;

public class ServerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("SendTest");
	}
    private IEnumerator SendTest()
    {
        
        WWWForm form = new WWWForm();
        form.AddField("function", "Add");
        form.AddField("id", 0);
        form.AddField("questionText", "Question");
        form.AddField("a1", "Answer1");
        form.AddField("a2", "Answer2");
        form.AddField("a3", "Answer3");
        form.AddField("a4", "Answer4");
        form.AddField("correctIndex", 1);
        form.AddField("explanation", "Explain");
        form.AddField("adc", -1);
        form.AddField("support", -1);
        form.AddField("mid", -1);
        form.AddField("top", -1);
        form.AddField("jungle", -1);
        form.AddField("awareness", -1);
        form.AddField("counter", -1);
        form.AddField("version", "5.5");

        WWW www = new WWW("http://hazlettdavid.com/QuestionManager/ParseQuestions.php", form);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
        }
        else
        {
            Debug.Log("Error in SendTest: " + www.error);
        }
    }
}
