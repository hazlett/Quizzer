using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {

    void OnGUI()
    {
        GUILayout.Label("MAIN MENU");
        if (GUILayout.Button("NEW GAME"))
        {
            Questions.Instance.NewGame();
        }
        if (GUILayout.Button("REFRESH"))
        {
            Questions.Instance.Refresh();
        }
        GUILayout.Label("CLASSROOMS");
        foreach (string classroom in Loading.Instance.Classrooms)
        {
            if (GUILayout.Button(classroom))
            {
                Loading.Instance.LoadQuestions(classroom);
            }
        }
        GUILayout.Label("GAME INVITES");
        foreach (string invite in Questions.Instance.Invites)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(invite))
            {

            }
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
                Questions.Instance.ChangeTurn(game);
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
    }
}
