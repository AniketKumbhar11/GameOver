using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
[System.Serializable]
public struct CardData
{
    public int CardID;
    public string CardName;
    public Sprite CardSprite;
    public bool IsOpen;
}
[System.Serializable]
public class GameData
{
    public int MatchCount;
    public int TurnCount;
    public int CurrentCount;
    public int Rows;
    public int Columns;
    public List<CardData> CardDataList;
    public List<int> RandomList;
}

public class GameManager : MonoBehaviour
{
    public int Rows;
    public int Columns;
    public DynamicGridGenerator gridGenerator;
    public List<CardData> MainCards = new List<CardData>();
    public List<CardData> CardDataList = new List<CardData>();

    public Transform CardParent;
    public static Action<int,Image> OnFlipComplete;
    public List<int> randomList;
    int currentCount = 0;
    int card1 = -1;
    int card2 = -1;
    [SerializeField] private TMP_Text matchText;
    [SerializeField] private TMP_Text turnText;

    [SerializeField] private TMP_InputField RowText;
    [SerializeField] private TMP_InputField ColumnText;
    [SerializeField] SaveManager saveManager;
    Image cardImage1;
    Image cardImage2;

    public int MatchCount { get; private set; }
    public int TurnCount { get; private set; }

    void OnEnable()
    {
        MatchCount = -1;
        TurnCount=-1;
        OnFlipComplete += FlipComplete;
    }

    private void OnDisable()
    {
        OnFlipComplete -= FlipComplete;
    }

    private void FlipComplete(int _data,Image image)
    {
        if(currentCount%2==0)
        {
            card1=_data;
            print("count ");
            cardImage1 = image;
            CheckingCard();
        }
        else
        {
            card2=_data;
            cardImage2 = image;
        }

        Invoke(nameof(GameStart), 2f);
    }
    private void CheckingCard()
    {
        if (card1 == card2)
        {
           SetMatchText();
        }

        SetTurnText();
        ResetCount();
        Invoke(nameof(CardDistory), 1.5f);

    }
    public void Quit()
    {
        Application.Quit();
    }
    private void CardDistory()
    {
        if (cardImage1 != null)
            cardImage1.enabled = false;
        if (cardImage2 != null)
            cardImage2.enabled = false;
    }
    private void SetMatchText()
    {
        MatchCount++;
        matchText.text=MatchCount.ToString();
    }
    private void SetTurnText()
    {
        TurnCount++;
        turnText.text=TurnCount.ToString(); 
    }
    private void ResetCount()
    {
        card1 = card2 = -1;
    }

    public void AssignRows()
    {
        
        if(RowText.text != null && RowText.text != string.Empty && RowText.text != " " && RowText.text != "" && !string.IsNullOrEmpty(RowText.text))
        {
            Rows=int.Parse(RowText.text);
        }
    }
    public void AssignColumns()
    {
        string data = ColumnText.text;

        if (ColumnText.text != null && ColumnText.text != string.Empty && ColumnText.text != " " && ColumnText.text != "" && !string.IsNullOrEmpty(ColumnText.text))
        {
            Columns = int.Parse(ColumnText.text);
        }
    }
    // Method to save game progress
    public void SaveGameProgress()
    {
        GameData gameData = new GameData
        {
            MatchCount = this.MatchCount,
            TurnCount = this.TurnCount,
            CurrentCount = this.currentCount,
            Rows = this.Rows,
            Columns = this.Columns,
            CardDataList = this.CardDataList,
            RandomList = this.randomList
        };

        saveManager.SaveGame(gameData);
    }

    // Method to load game progress
    public void LoadGameProgress()
    {
        GameData loadedData = saveManager.LoadGame();
        if (loadedData != null)
        {
            // Restore data
            this.MatchCount = loadedData.MatchCount;
            this.TurnCount = loadedData.TurnCount;
            this.currentCount = loadedData.CurrentCount;
            this.Rows = loadedData.Rows;
            this.Columns = loadedData.Columns;
            this.CardDataList = loadedData.CardDataList;
            this.randomList = loadedData.RandomList;

            // Optional: Update UI or grid based on loaded data
        }
        GameInit();
    }

    public void NewGame()
    {
        AssignColumns();
        AssignRows();
        SetMatchText();
        SetTurnText();
        ResetCount();
        GenerateCardDataList();
        gridGenerator.GenerateGrid(CardDataList, Rows,Columns);
        generateList();
        Invoke(nameof(GameStart), 2f);
    }

    private void GameInit()
    {

        SetMatchText();
        SetTurnText();
        ResetCount();
        gridGenerator.GenerateGrid(CardDataList, Rows, Columns);
        Invoke(nameof(GameStart), 2f);
    }
    private void GenerateCardDataList()
    {
        int listLenght= Rows*Columns;
        for (int i = 0; i < listLenght; i++)
        {
            int randNumber= Random.Range(0, MainCards.Count-1);
          //  MainCards[randNumber].IsOpen = false;
            CardDataList.Add(MainCards[randNumber]);
        }


    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void PauseClose()
    {
        Time.timeScale = 1;
    }


    public void GameStart()
    {
        
        if(currentCount==CardDataList.Count)
        {
            print("Game Over ...."!);
            return;
        }
        // int randomNumber=Random.Range(0, CardParent.childCount);
            int currentCard = randomList[currentCount];
            CardParent.transform.GetChild(currentCard).GetComponent<Card>().FlipAndChangeSprite();
        

        CardData cd = CardDataList[currentCard];
        cd.IsOpen = true;
        CardDataList[currentCard] = cd;
        currentCount++;
    }
    private void generateList()
    {
        randomList = new List<int>();
        while (randomList.Count < CardDataList.Count)
        {
            int randomNumber = Random.Range(0, CardDataList.Count);
            if (!randomList.Contains(randomNumber))
            {
                randomList.Add(randomNumber);
            }
        }
    }

}
