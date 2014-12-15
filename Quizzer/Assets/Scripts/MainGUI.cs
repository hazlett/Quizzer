using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainGUI : MonoBehaviour {

    private float timer, maxTime = 25.0f, timeRemaining;
    private bool questionAsked, advance;
    private Question asked;
    private List<string> answers;
    private string message = "";
    private int correct;
    private string totals;
    private string player;
    private bool yourTurn;
    private int attempt;
    private bool crowning, attemptingCrown;
	void Start () {
        attempt = -1;
        attemptingCrown = false;
        crowning = false;
        yourTurn = true;
        if (Questions.Instance.CurrentGame.Turn == "1")
        {
            correct = int.Parse(Questions.Instance.CurrentGame.Player1Correct);
            totals = Questions.Instance.CurrentGame.Player1Totals;
        }
        else if (Questions.Instance.CurrentGame.Turn == "2")
        {
            correct = int.Parse(Questions.Instance.CurrentGame.Player2Correct);
            totals = Questions.Instance.CurrentGame.Player2Totals;
        }
        else
        {
            correct = 0;
            totals = "000000";
        }
        asked = null;
        timer = 0;
        questionAsked = false;
        advance = false;
	}
	
	void Update () {
        if (CheckWinState())
        {
            Application.LoadLevel("ChangeTurns");
        }
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
    private bool CheckWinState()
    {
        if (totals == Game.WinState)
            return true;
        else
            return false;
    }
    void OnGUI()
    {
        GUILayout.Label("<b>ROUND: </b>" + Questions.Instance.CurrentGame.Round);
        if ((crowning) && (!attemptingCrown))
        {
            DrawCrowning();
        }
        else
        {
            DrawGameplay();
        }
        
    }
    private void DrawCrowning()
    {
        GUILayout.Label("TURN: " + Questions.Instance.CurrentGame.Turn);
        GUILayout.Label("P1: " + Questions.Instance.CurrentGame.Player1 + " " + Questions.Instance.CurrentGame.Player1Totals);
        GUILayout.Label("P2: " + Questions.Instance.CurrentGame.Player2 + " " + Questions.Instance.CurrentGame.Player2Totals);
        if (Questions.Instance.CurrentGame.Turn == "1")
        {
            GUILayout.Label("Player1Crowning");
            for (int i = 0; i < Questions.Instance.CurrentGame.Player1Totals.Length; i++)
            {
                if (Questions.Instance.CurrentGame.Player1Totals[i] == '0')
                {
                    if (GUILayout.Button(Questions.Instance.Category[i]))
                    {
                        AskCrowning(i);
                    }
                }
                else if (Questions.Instance.CurrentGame.Player1Totals[i] == '1')
                {
                    GUILayout.Box(Questions.Instance.Category[i]);
                }
            }
        }
        else if (Questions.Instance.CurrentGame.Turn == "2")
        {
            GUILayout.Label("Player2Crowning");
            for (int i = 0; i < Questions.Instance.CurrentGame.Player2Totals.Length; i++)
            {
                if (Questions.Instance.CurrentGame.Player2Totals[i] == '0')
                {
                    if (GUILayout.Button(Questions.Instance.Category[i]))
                    {
                        AskCrowning(i);
                    }
                }
                else if (Questions.Instance.CurrentGame.Player2Totals[i] == '1')
                {
                    GUILayout.Box(Questions.Instance.Category[i]);
                }
            }
        }
        else
        {
            Debug.Log("Error Crowning. Turn unknown: " + Questions.Instance.CurrentGame.Turn);
            crowning = false;
        }
    }
    private void DrawGameplay()
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
                if (Questions.Instance.CurrentGame.Turn == "1")
                {
                    Questions.Instance.CurrentGame.Player1Correct = correct.ToString();
                }
                else if (Questions.Instance.CurrentGame.Turn == "2")
                {
                    Questions.Instance.CurrentGame.Player2Correct = correct.ToString();
                }

                Application.LoadLevel("ChangeTurns");
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
                        if ((attemptingCrown) && (attempt != -1))
                        {
                            if (Questions.Instance.CurrentGame.Turn == "1")
                            {
                                Debug.Log("Saving turn 1");
                                Debug.Log("Attempt: " + attempt);
                                char[] temp = Questions.Instance.CurrentGame.Player1Totals.ToCharArray();
                                temp[attempt] = '1';
                                totals = new string(temp);
                                Debug.Log("TOTALS: " + totals);
                                Questions.Instance.CurrentGame.Player1Totals = totals;
                            }
                            else if (Questions.Instance.CurrentGame.Turn == "2")
                            {
                                Debug.Log("Saving turn 2");
                                Debug.Log("Attempt: " + attempt);
                                char[] temp = Questions.Instance.CurrentGame.Player2Totals.ToCharArray();
                                temp[attempt] = '1';
                                totals = new string(temp);
                                Debug.Log("TOTALS: " + totals);
                                Questions.Instance.CurrentGame.Player2Totals = totals;
                            }
                            attempt = -1;
                            correct = 0;
                        }
                        if (correct == 3)
                        {
                            yourTurn = true;
                            crowning = true;
                            attemptingCrown = false;
                        }
                        if (correct > 3)
                        {
                            correct = 0;
                        }
                    }
                    else
                    {
                        if (attemptingCrown)
                        {
                            correct = 0;
                        }
                        message = "WRONG ANSWER";
                        yourTurn = false;
                        crowning = false;
                        attempt = -1;
                        attemptingCrown = false;
                        advance = true;
                    }
                }
                height += 0.2f;
            }
        }
    }
    private void AskCrowning(int crown)
    {
        Debug.Log("SetAsked crowning");
        attempt = crown;
        SetAsked(crown);
        attemptingCrown = true;
    }
    private void Spin()
    {
        try
        {
            message = "SPINNING";
            int random = Random.Range(0, 7);
            if (random == 6)
            {
                Debug.Log("Choose category (rolled a 6)");
                correct = 3;
                crowning = true;
                attemptingCrown = false;
                //Choose category
            }
            else if (random == 7)
            {
                Debug.Log("Special (rolled a 7)");
                //special
            }
            else
            {
                Debug.Log("SetAsked random");
                SetAsked(random);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error spinning: " + e.Message);
            Spin();
        }
    }
    private void SetAsked(int i)
    {
        asked = Questions.Instance.RetrieveQuestion(i);
        Debug.Log("SetAsked (i): " + i + " | " + "Asked: " + asked.ID);
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
}
