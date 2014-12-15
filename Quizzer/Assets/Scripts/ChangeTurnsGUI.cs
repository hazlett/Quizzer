using UnityEngine;
using System.Collections;

public class ChangeTurnsGUI : MonoBehaviour {

    void Start()
    {
        Questions.Instance.ChangeTurn();
    }

    void OnGUI()
    {
        GUILayout.Label("<b>SENDING MOVE</b>");
    }
}
