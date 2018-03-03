using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Initialize all Cards.
/// </summary>
public class CardsGUI : MonoBehaviour
{
    public GameObject[] Cards;
    public Sprite[] CardNumbers;
    public Sprite[] MarkedCardNumbers;
    
    Card[] _allCards;
    int _totalNumbers;
    
    void Start ()
    {
        GrabReferences();
        SettingReferences();
	}

    void GrabReferences()
    {
        // Grab all "Card n" references.
        _totalNumbers = GameMNG.Instance.NumberOfCards * Card.CardSize; // Each card has Card.CardSize random numbers.
        _allCards = new Card[GameMNG.Instance.NumberOfCards];

        for (int i = 0; i < Cards.Length; i++)
        {
            if (i < GameMNG.Instance.NumberOfCards)
            {
                Cards[i].SetActive(true);
                _allCards[i] = new Card(Cards[i].transform, CardNumbers, MarkedCardNumbers);
            }
            else
                Cards[i].SetActive(false);
        }

    }
    

    List<int> CreateRandomIntArray(int size)
    {
        List<int> allRandom = new List<int>();

        for (int i = 0; i < size; i++)
        {
            allRandom.Add(Random.Range(1, size + 1));
        }

        return allRandom;
    }

    void SettingReferences()
    {
        List<int> allRandomNumbers = GameMNG.Instance.TestingModeEnabled 
            ? TestingUtils.AllTestCardList 
            : CreateRandomIntArray(60);

        // Distribute all random numbers on all the Cards.
        int start = 0;
        foreach (var card in _allCards)
        {
            card.ResetCard(allRandomNumbers.GetRange(start, Card.CardSize).ToArray());
            start += Card.CardSize;
        }
    }

    void ResetCardsShowed()
    {
        if (GameMNG.Instance.NumberOfCards == _allCards.Length)
            return;

        GrabReferences();
    }

    public void ResetCards()
    {
        ResetCardsShowed();
        SettingReferences();
    }

    public void MarkNumberInCard(int ballNumber)
    {
        foreach (var card in _allCards)
        {
            card.Mark(ballNumber);
        }
    }

    public int CheckLine()
    {
        int numOfLines = 0;

        foreach (var card in _allCards)
        {
            numOfLines += card.CheckLine();
        }
        
        return numOfLines;
    }
    
    public int CheckM()
    {
        int numOfM = 0;
        bool isM;

        foreach (var card in _allCards)
        {
            isM = card.CheckM();
            if (isM)
                numOfM++;
        }
        
        return numOfM;
    }
    
    public int CheckBingo()
    {
        int numOfBingo = 0;
        bool isBingo;

        foreach (var card in _allCards)
        {
            isBingo = card.CheckBingo();
            if (isBingo)
                numOfBingo++;
        }
        
        return numOfBingo;
    }
}

