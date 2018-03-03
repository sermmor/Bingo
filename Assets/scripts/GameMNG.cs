using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMNG : MonoBehaviour
{
    public const float DEFAULT_VALUE_SECONDS_TO_NEW_BALL = 0.4f;
    public const float MIN_VALUE_SECONDS_TO_NEW_BALL = 0.1f;
    public const float MAX_VALUE_SECONDS_TO_NEW_BALL = 1000f;
    const int MIN_VALUE_NUMBER_OF_BALLS = 1;
    public const int MAX_VALUE_NUMBER_OF_BALLS = 30;
    const int MIN_VALUE_NUMBER_OF_CARDS = 1;
    public const int MAX_VALUE_NUMBER_OF_CARDS = 4;

    const int CreditCostPerGame = 45;
    const int CreditAtTheBegin = 200;
    const int BetStep = 20;
    const int PointsLine = 50;
    const int PointsM = 200;
    const int PointsBingo = 500;
    
    [Header("Game Config")]
    [Range(MIN_VALUE_SECONDS_TO_NEW_BALL, MAX_VALUE_SECONDS_TO_NEW_BALL)]
    public float SecondsToNewBall = DEFAULT_VALUE_SECONDS_TO_NEW_BALL;
    [Range(MIN_VALUE_NUMBER_OF_BALLS, MAX_VALUE_NUMBER_OF_BALLS)]
    public int NumberOfBalls = MAX_VALUE_NUMBER_OF_BALLS;
    [Range(MIN_VALUE_NUMBER_OF_CARDS, MAX_VALUE_NUMBER_OF_CARDS)]
    public int NumberOfCards = MAX_VALUE_NUMBER_OF_CARDS;

    [Header("Properties")]
    public CardsGUI CardGUI;
    public BallGUI BallGUI;
    public Text CreditsText, ScoreText, BetText;
    public Button IncreaseBetButton, DecreaseBetButton;
    public GameObject YouLosePanel;
    public bool TestingModeEnabled = false;
    
    bool _onStart, _isFirstRound;
    int _numberOfBallsExtracted, _score, _credits, _bet;
    
    #region SINGLETON
    static GameMNG _instance = null;
    public static GameMNG Instance
    {
        get { return _instance; }
    }
    #endregion

    void Awake()
    {
        _instance = this;
    }

    void Start ()
    {
        _onStart = false;
        _isFirstRound = true;
        _credits = CreditAtTheBegin;
        _score = _bet = 0;

        CreditsText.text = CreditAtTheBegin.ToString();
        ScoreText.text = BetText.text = "0";
        YouLosePanel.SetActive(false);
    }

    int FromPointToCredit(float points, float betValue)
    {
        return Mathf.RoundToInt(betValue * points / 50);
    }

    void UpdateCostCredits(int newCost)
    {
        _credits -= newCost;
        CreditsText.text = _credits.ToString();
    }

    void UpdateIngressCredits(int newIngresss)
    {
        _credits += newIngresss;
        CreditsText.text = _credits.ToString();
    }

    public void StartGame()
    {
        if (_onStart || _credits < CreditCostPerGame)
            return;

        UpdateCostCredits(CreditCostPerGame);

        // Initialize game.
        _onStart = true;
        _score = 0;
        _numberOfBallsExtracted = 0;

        ScoreText.text = "0";
        BallGUI.ResetBalls();
        if (!_isFirstRound)
            CardGUI.ResetCards();
        _isFirstRound = false;

        // Disable bet buttons.
        IncreaseBetButton.interactable = false;
        DecreaseBetButton.interactable = false;

        // Start game.
        StartCoroutine(DoGame());
    }
    
    bool CanPlayAnotherGame()
    {
        return _credits >= CreditCostPerGame;
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void PostGame()
    {
        // Update decrease the bet and increase credits wins with bet.
        UpdateCostCredits(_bet);
        UpdateIngressCredits(FromPointToCredit(_score, _bet));

        if (CanPlayAnotherGame())
        {
            // Enable bet buttons.
            IncreaseBetButton.interactable = true;
            DecreaseBetButton.interactable = true;

            _bet = 0;
            BetText.text = "0";

            // Mark game like finished.
            _onStart = false;
        }
        else
        {
            YouLosePanel.SetActive(true);
            // If player lost all credit, return to main menu.
            Invoke("ReturnToMainMenu", 3.0f);
        }
    }

    IEnumerator DoGame()
    {
        int newBall, numberOfNewLines, numberOfNewM, numberOfNewBingo;

        while (_numberOfBallsExtracted < NumberOfBalls)
        {
            yield return new WaitForSeconds(SecondsToNewBall);
            
            // Extract a ball and add the ball in the scene.
            newBall = BallGUI.GetNewBall();

            // Put "X" in the number of the Card.
            CardGUI.MarkNumberInCard(newBall);

            // Check if Line (in Cards).
            numberOfNewLines = CardGUI.CheckLine();
            if (numberOfNewLines > 0)
            {
                _score += (PointsLine * numberOfNewLines);
                ScoreText.text = _score.ToString();
                Debug.Log("Total points: " + _score);
                numberOfNewLines = 0;
            }
            
            // Check if M (in Cards).
            numberOfNewM = CardGUI.CheckM();
            if (numberOfNewM > 0)
            {
                _score += (PointsM * numberOfNewM);
                ScoreText.text = _score.ToString();
                Debug.Log("Total points: " + _score);
            }

            // Check if Bingo (in Cards).
            numberOfNewBingo = CardGUI.CheckBingo();
            if (numberOfNewBingo > 0)
            {
                _score += (PointsBingo * numberOfNewBingo);
                ScoreText.text = _score.ToString();
                Debug.Log("Total points: " + _score);
            }

            _numberOfBallsExtracted++;
        }

        PostGame();
    }

    public void IncreaseBet()
    {
        int toBet = _bet + BetStep;
        int limitToBet = _credits - CreditCostPerGame;
        if (toBet > 1000)
            _bet = 1000;
        else if (toBet <= limitToBet)
            _bet = toBet;

        BetText.text = _bet.ToString();
    }

    public void DecreaseBet()
    {
        if (_bet < BetStep)
            _bet = 0;
        else
            _bet -= BetStep;

        BetText.text = _bet.ToString();
    }

    public void ResetCards()
    {
        CardGUI.ResetCards();
    }
}
