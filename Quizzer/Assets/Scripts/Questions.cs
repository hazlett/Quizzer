﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Serialization;
public class Questions : MonoBehaviour {
    private static Questions instance;
    public static Questions Instance { get { return instance; } }
    internal List<Question> allQuestions;
    private List<Question>[] categories;
    private List<Game> games = new List<Game>();
    private List<Game> myTurn = new List<Game>();
    private List<Game> oppTurn = new List<Game>();
    public List<Game> Games { get { return games; } }
    public List<Game> MyTurn { get { return myTurn; } }
    public List<Game> OppTurn { get { return oppTurn; } }
    private User user;
    public User CurrentUser { get { return user; } }
    private string[] category;
    public string[] Category { get { return category; } }
    private List<string> invites;
    public List<string> Invites { get { return invites; } }
    private List<Game> gameInvites;
    private int refreshing;
    private Game currentGame;
    public Game CurrentGame { get { return currentGame; } }

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
	void Start () {
        refreshing = 0;
        invites = new List<string>();
        allQuestions = new List<Question>();
        categories = new List<Question>[] { new List<Question>(), new List<Question>(), new List<Question>(), new List<Question>(), new List<Question>(), new List<Question>() };
	}
	
    internal void SetUser(User user)
    {
        this.user = user;
        Refresh();
    }
    internal void SetGame(Game game)
    {
        this.currentGame = game;
    }
    private void GetUserGames()
    {
        games = new List<Game>();
        myTurn = new List<Game>();
        oppTurn = new List<Game>();
        foreach (string id in user.Games)
        {
            StartCoroutine(AddGameFromServer(id));
        }
    }
    internal void Refresh()
    {
        if (refreshing == 0)
        {
            Debug.Log("Refreshing");
            CheckInvites();
            GetUserGames();
        }
    }


    private IEnumerator AddGameFromServer(string id)
    {
        refreshing++;
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        WWW www = new WWW("http://hazlett206.ddns.net/Quizzer/GetGame.php", form);
        yield return www;
        if (www.error == null)
        {
            Game game = DeserializeToGame(www.text);
            if ((game != null) && (!games.Contains(game)))
            {
                games.Add(game);
                CheckTurn(game);
                Debug.Log("Added game: " + www.text);
            }
            else
            {
                user.Games.Remove(id);
                StartCoroutine(SaveUser(user));
                Debug.Log("Failed to add game");

            }
        }
        else
        {
            Debug.Log("Adding game error: " + www.error);
        }
        refreshing--;
    }
    private void CheckTurn(Game game)
    {
        switch (game.Turn)
        {
            case "1":
                if (user.Name == game.Player1)
                {
                    if (!myTurn.Contains(game))
                    {
                        Debug.Log("Game: " + game.ID);
                        myTurn.Add(game);
                    }
                }
                else
                {
                    if (!oppTurn.Contains(game))
                    {
                        Debug.Log("Game: " + game.ID);
                        oppTurn.Add(game);
                    }
                }
                break;
            case "2":
                if (user.Name == game.Player2)
                {
                    if (!myTurn.Contains(game))
                    {
                        Debug.Log("Game: " + game.ID);
                        myTurn.Add(game);
                    }
                }
                else
                {
                    if (!oppTurn.Contains(game))
                    {
                        Debug.Log("Game: " + game.ID);
                        oppTurn.Add(game);
                    }
                }
                break;
            default:
                try
                {
                    games.Remove(game);
                    user.Games.Remove(game.ID);
                }
                catch { }
                Debug.Log("Unknown turn on CheckTurn: " + game.Turn);
                break;
        }
    }
    private Game DeserializeToGame(string text)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            Game obj = new Game();
            XmlSerializer serializer = new XmlSerializer(typeof(Game));
            XmlReader reader = new XmlNodeReader(doc);

            obj = serializer.Deserialize(reader) as Game;

            return obj;
        }
        catch (Exception)
        {
            return null;
        }
    }
    public void NewGame(string opponent, string classroom)
    {
        Debug.Log("USER: " + user.Name);
        StartCoroutine(CreateGame(new Game(user.Name, opponent, classroom)));
    }
    private IEnumerator CreateGame(Game game)
    {
        WWWForm form = new WWWForm();
        form.AddField("content", game.ToXml());
        WWW www = new WWW("http://hazlett206.ddns.net/Quizzer/CreateGameID.php", form);
        yield return www;

        if (www.error == null)
        {
            Debug.Log("Created game. ID: " + www.text);
            Debug.Log("P1: " + game.Player1 + " | P2: " + game.Player2);
            game.ID = www.text.Replace("\n", "");
            Debug.Log("Game id: " + game.ID);
            game.Turn = "2";
            WWWForm form2 = new WWWForm();
            form2.AddField("id", game.ID);
            form2.AddField("content", game.ToXml());
            WWW www2 = new WWW("http://hazlett206.ddns.net/Quizzer/CreateGame.php", form2);
            yield return www2;
            if (www2.error == null)
            {
                Debug.Log("Created game with ID: " + www2.text.Replace("\n", ""));
            }
            else
            {
                Debug.Log("Creating game with ID ERROR: " + www2.error);
            }
            //Send invite after your move.
            StartCoroutine(SendGameInvite(www.text.Replace("\n", ""), game.Player2));
            //Add game on game creation and save user.
            user.Games.Add(www.text.Replace("\n", ""));
            StartCoroutine(SaveUser(user));
        }
        else
        {
            Debug.Log("Failed to create game: " + www.error);
        }
    }
    private IEnumerator SendGameInvite(string gameID, string playerID)
    {
        WWWForm form = new WWWForm();
        form.AddField("gameid", gameID);
        form.AddField("playerid", playerID);
        Debug.Log("playerid: " + playerID);
        WWW www = new WWW("http://hazlett206.ddns.net/Quizzer/SendInvite.php", form);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("Game invite sent: " + www.text);
        }
        else
        {
            Debug.Log("Invite error: " + www.error);
        }
        Application.LoadLevel("Menu");
    }
    public void CheckInvites()
    {
        StartCoroutine(GetGameInvites());
    }
    private IEnumerator GetGameInvites()
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", user.Name);
        WWW www = new WWW("http://hazlett206.ddns.net/Quizzer/CheckInvites.php", form);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("Got invites: " + www.text);
            invites = DeserializeToList(www.text);
            if (invites == null)
            {
                invites = new List<string>();
            }
            foreach (string s in invites)
            {
                Debug.Log("Invite: " + s);
            }
        }
        else
        {
            Debug.Log("Getting invites error: " + www.error);
        }
    }
    internal List<string> DeserializeToList(string text)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(text);
        XmlSerializer xmls = new XmlSerializer(typeof(List<string>));
        XmlReader reader = new XmlNodeReader(doc);
        List<string> list = xmls.Deserialize(reader) as List<string>;
        if (list == null)
        {
            list = new List<string>();
        }
        return list;
    }
    public void SetCategories(string[] categories)
    {
        this.category = categories;
    }
    public void Initialize(List<Question> questions)
    {
        allQuestions = questions;
        Sort();
    }
    public Question RetrieveQuestion(int category)
    {
        int random = UnityEngine.Random.Range(0, categories[category].Count);
        Debug.Log("Category: " + category + " | Random: " + random);
        return categories[category][random];
    }
    private void Sort()
    {
        StartCoroutine(SortQuestions());
    }

    private IEnumerator SortQuestions()
    {
        foreach (Question question in allQuestions)
        {
            categories[int.Parse(question.Category)].Add(question);
        }

        yield return null;
    }

    internal void AcceptInvite(string invite)
    {
        invites = new List<string>();
        StartCoroutine(AcceptGameInvite(invite));
    }

    internal void ChangeTurn()
    {
        switch (currentGame.Turn)
        {
            case "0":
                currentGame.Turn = "1";
                break;
            case "1":
                currentGame.Turn = "2";
                break;
            case "2":
                currentGame.Turn = "1";
                break;
            default:
                games.Remove(currentGame);
                user.Games.Remove(currentGame.ID);
                Debug.Log("Unknown turn");
                break;
        }
        StartCoroutine(SaveGame());
    }

    private IEnumerator SaveGame()
    {
        WWWForm form2 = new WWWForm();
        form2.AddField("id", currentGame.ID);
        form2.AddField("content", currentGame.ToXml());
        WWW www2 = new WWW("http://hazlett206.ddns.net/Quizzer/SaveGame.php", form2);
        yield return www2;
        if (www2.error == null)
        {
            Debug.Log("Created game with ID: " + www2.text.Replace("\n", ""));
        }
        else
        {
            Debug.Log("Creating game with ID ERROR: " + www2.error);
        }
        Refresh();
        Application.LoadLevel("Menu");
    }
    private IEnumerator AcceptGameInvite(string invite)
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", user.Name);
        form.AddField("gameid", invite);
        WWW www = new WWW("http://hazlett206.ddns.net/Quizzer/AcceptInvite.php", form);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("Accept successful: " + www.text);

            Game game = DeserializeToGame(www.text);
            if (game != null)
            {
                games.Add(game);
                user.Games.Add(invite);
                Debug.Log("Game: " + game.Player1);
                Debug.Log("Invite: " + invite);
                StartCoroutine(SaveUser(user));
            }
            else
            {
                Debug.Log("Null Game");
            }
            CheckInvites();
        }
        else
        {
            Debug.Log("Error accepting: " + www.error);
        }
        Refresh();
    }
    private IEnumerator DeclineGameInvite(string invite)
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", user.Name);
        form.AddField("gameid", invite);
        WWW www = new WWW("http://hazlett206.ddns.net/Quizzer/DeclineInvite.php", form);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("Decline successful");
            CheckInvites();
        }
        else
        {
            Debug.Log("Error declining: " + www.error);
        }
    }
    internal void DeclineInvite(string invite)
    {
        StartCoroutine(DeclineGameInvite(invite));
    }
    private IEnumerator SaveUser(User user)
    {
        Debug.Log("Save user: " + user.ToXml());
        WWWForm form = new WWWForm();
        form.AddField("userid", user.Name);
        form.AddField("content", user.ToXml());
        WWW www = new WWW("http://hazlett206.ddns.net/Quizzer/SaveUser.php", form);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("User saved: " + www.text);
        }
        else
        {
            Debug.Log("Create user error: " + www.error);
        }
    }


}
