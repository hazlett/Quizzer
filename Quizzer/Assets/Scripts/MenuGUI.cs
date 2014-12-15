using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {
    private Vector2 gamesScroll = new Vector2();
    private string classroom = "QuestionManager";

    void Start()
    {
        Questions.Instance.Refresh();
        Questions.Instance.Refresh();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("<b>MAIN MENU</b>");
        if (GUILayout.Button("BACK"))
        {
            Application.LoadLevel("Login");
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("NEW GAME"))
        {
            Application.LoadLevel("NewGameMenu");
        }
        if (GUILayout.Button("REFRESH"))
        {
            Questions.Instance.Refresh();
        }
        GUILayout.EndHorizontal();
        //GUILayout.Label("CLASSROOMS");
        //foreach (string room in Loading.Instance.Classrooms)
        //{
        //    if (GUILayout.Button(room))
        //    {
        //        this.classroom = room;
        //    }
        //}
        GUILayout.Label("<b>GAMES</b>");
        gamesScroll = GUILayout.BeginScrollView(gamesScroll);
        GUILayout.Label("GAME INVITES");
        foreach (string invite in Questions.Instance.Invites)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(invite);
            if (GUILayout.Button("ACCEPT"))
            {
                Questions.Instance.AcceptInvite(invite);
            }
            if (GUILayout.Button("DECLINE"))
            {
                Questions.Instance.DeclineInvite(invite);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Label("ACTIVE GAMES");
        GUILayout.Label("MY TURN");
        foreach (Game game in Questions.Instance.MyTurn)
        {
            if (GUILayout.Button(game.ID))
            {
                Questions.Instance.SetGame(game);
                Application.LoadLevel("LoadGame");
            }
        }
        GUILayout.Space(15);
        GUILayout.Label("OPPONENT TURN");
        foreach (Game game in Questions.Instance.OppTurn)
        {
            if (GUILayout.Button(game.ID))
            {

            }
        }
        GUILayout.EndScrollView();
    }
}
