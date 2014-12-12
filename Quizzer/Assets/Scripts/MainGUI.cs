using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainGUI : MonoBehaviour {

    private float timer, maxTime = 20.0f, timeRemaining;
    private bool questionAsked, advance;
    private Question asked;
    private List<string> answers;
    private string message = "";
    private int correct;
    private bool yourTurn;
	void Start () {
        yourTurn = true;
        correct = 0;
        asked = null;
        timer = 0;
        questionAsked = false;
        advance = false;
	}
	
	void Update () {
	    if (questionAsked)
        {
            timer += Time.deltaTime;
            if (timer > maxTime)
            {
                timer = 0;
                questionAsked = false;
                asked = null;
                advance = true;
                message = "OUT OF TIME";
            }
            else
            {
                timeRemaining = maxTime - timer;
            }
        }
	}
    void OnGUI()
    {
        if (questionAsked)
            GUILayout.Box(Questions.Instance.Category[int.Parse(asked.Category)]);
        GUILayout.BeginHorizontal();
        GUILayout.Label("CORRECT: " + correct);
        if (questionAsked)
            GUILayout.Label("TIME TO ANSWER: " + timeRemaining.ToString("F0") + " SECONDS");
        GUILayout.EndHorizontal();
        GUILayout.Label(message);
        if (advance)
        {
            if (GUI.Button(new Rect(0, Screen.height * 0.2f, Screen.width * 0.5f, Screen.height * 0.25f), "CONTINUE"))
            {
                Application.LoadLevel("Menu");
            }
        }
        else if (!questionAsked)
        {
            if (GUI.Button(new Rect(0, Screen.height * 0.2f, Screen.width * 0.5f, Screen.height * 0.25f), "TAKE A SPIN"))
            {
                Spin();
            }
        }
        else
        {
            GUILayout.Space(15.0f);
            GUILayout.Box("<b>" + asked.QuestionText + "</b>");
            GUILayout.Space(20.0f);
            float height = 0.2f;
            foreach (string answer in answers)
            {
                if (GUI.Button(new Rect(0, Screen.height * height, Screen.width * 0.5f, Screen.height * 0.15f), answer))
                {
                    questionAsked = false;
                    timer = 0;
                    if (answer.Equals(asked.CorrectAnswer))
                    {
                        message = "CORRECT ANSWER";
                        correct++;
                    }
                    else
                    {
                        message = "WRONG ANSWER";
                        yourTurn = false;
                        advance = true;
                    }
                }
                height += 0.2f;
            }
        }
    }
    private void Spin()
    {
        try
        {
            message = "SPINNING";
            int random = Random.Range(0, 6);
            asked = Questions.Instance.RetrieveQuestion(random);
            answers = new List<string>();
            answers.Add(asked.CorrectAnswer);
            foreach (string answer in asked.WrongAnswers)
            {
                answers.Add(answer);
            }
            answers.Sort();
            questionAsked = true;
            message = "ANSWER QUESTION";
        }
        catch 
        {
            Spin();
        }
    }
}
