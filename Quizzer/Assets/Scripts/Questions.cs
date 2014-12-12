using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Questions : MonoBehaviour {
    private static Questions instance;
    public static Questions Instance { get { return instance; } }
    private List<Question> allQuestions;
    private List<Question>[] categories;
    private string[] category;
    public string[] Category { get { return category; } }
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
        allQuestions = new List<Question>();
        categories = new List<Question>[] { new List<Question>(), new List<Question>(), new List<Question>(), new List<Question>(), new List<Question>(), new List<Question>() };
	}
	
	void Update () {
	
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
}
