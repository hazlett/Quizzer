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
        GUI.matrix = Matrix4x4.TRS(Utility.GUIPOSITION, Quaternion.identity, new Vector3(Screen.width / Utility.SCREENWIDTH, Screen.height / Utility.SCREENHEIGHT, 1));

        GUILayout.BeginHorizontal();
        GUILayout.Label("<b>MAIN MENU</b> Welcome " + Questions.Instance.CurrentUser.Name);
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

        GUILayout.Label("<b>ACTIVE GAMES</b>");
        GUILayout.Label("MY TURN");
        foreach (Game game in Questions.Instance.MyTurn)
        {
            if (game.Player1 == Questions.Instance.CurrentUser.Name)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("GAME WITH: " + game.Player2))
                {
                    Questions.Instance.SetGame(game);
                    Application.LoadLevel("LoadGame");
                }
                if (GUILayout.Button("REMOVE"))
                {
                    Questions.Instance.DeleteGame(game);
                }
                GUILayout.EndHorizontal();
            }
            else if (game.Player2 == Questions.Instance.CurrentUser.Name)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("GAME WITH: " + game.Player1))
                {
                    Questions.Instance.SetGame(game);
                    Application.LoadLevel("LoadGame");
                }
                if (GUILayout.Button("REMOVE"))
                {
                    Questions.Instance.DeleteGame(game);
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.Space(15);
        GUILayout.Label("OPPONENT TURN");
        foreach (Game game in Questions.Instance.OppTurn)
        {
            if (game.Player1 == Questions.Instance.CurrentUser.Name)
                GUILayout.Box("GAME WITH: " + game.Player2);
            else if (game.Player2 == Questions.Instance.CurrentUser.Name)
                GUILayout.Box("GAME WITH: " + game.Player1);
        }
        GUILayout.Label("<b>PREVIOUS GAMES</b>");
        foreach (Game game in Questions.Instance.OldGames)
        {
            if (game.Player1 == Questions.Instance.CurrentUser.Name)
            {
                if (game.Turn == "-1")
                {
                    GUILayout.Label("YOU BEAT " + game.Player2);
                }
                else if (game.Turn == "-2")
                {
                    GUILayout.Label("YOU LOST TO " + game.Player2);
                }
                else
                {
                    GUILayout.Label("YOU TIED " + game.Player2);
                }
            }
            else if (game.Player2 == Questions.Instance.CurrentUser.Name)
            {
                if (game.Turn == "-2")
                {
                    GUILayout.Label("YOU BEAT " + game.Player1);
                }
                else if (game.Turn == "-1")
                {
                    GUILayout.Label("YOU LOST TO " + game.Player1);
                }
                else
                {
                    GUILayout.Label("YOU TIED " + game.Player1);
                }
            }
        }
        GUILayout.EndScrollView();
    }
}
