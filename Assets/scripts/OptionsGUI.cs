using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsGUI : MonoBehaviour
{
    public GameObject GameGUI;
    public GameObject PanelGameInfo;

    InputField _inputNumCards, _inputNumBalls, _inputSecondsToNewBall;
    Slider _sliderNumCards, _sliderNumBalls;
	
	void Start ()
	{
	    DisableAllGame();
	    GrabReferences();
	    SettingReferences();
	}

    void GrabReferences()
    {
        _inputNumCards = transform.Find("InputNumCards").GetComponent<InputField>();
        _inputNumBalls = transform.Find("InputNumBalls").GetComponent<InputField>();
        _inputSecondsToNewBall = transform.Find("InputSecondsToNewBall").GetComponent<InputField>();

        _sliderNumCards = transform.Find("SliderNumCards").GetComponent<Slider>();
        _sliderNumBalls = transform.Find("SliderNumBalls").GetComponent<Slider>();
    }

    void SettingReferences()
    {
        _inputNumCards.text = GameMNG.MAX_VALUE_NUMBER_OF_CARDS.ToString();
        _sliderNumCards.value = GameMNG.MAX_VALUE_NUMBER_OF_CARDS;

        _inputNumBalls.text = GameMNG.MAX_VALUE_NUMBER_OF_BALLS.ToString();
        _sliderNumBalls.value = GameMNG.MAX_VALUE_NUMBER_OF_BALLS;

        _inputSecondsToNewBall.text = GameMNG.DEFAULT_VALUE_SECONDS_TO_NEW_BALL.ToString();
    }

    void DisableAllGame()
    {
        GameGUI.SetActive(false);
        PanelGameInfo.SetActive(false);
    }

    void EnableAllGame()
    {
        GameGUI.SetActive(true);
        PanelGameInfo.SetActive(true);
    }

    void SetGameInfo()
    {
        GameMNG.Instance.NumberOfCards = (int) _sliderNumCards.value;
        GameMNG.Instance.NumberOfBalls = (int) _sliderNumBalls.value;
        
        // Validate textSecondsToBall (set new value in GameMNG.Instance.SecondsToNewBall).
        string textSecondsToBall = _inputSecondsToNewBall.text;

        if (textSecondsToBall == "-")
            GameMNG.Instance.SecondsToNewBall = GameMNG.DEFAULT_VALUE_SECONDS_TO_NEW_BALL;
        else
        {
            float secondsToBall = float.Parse(textSecondsToBall);
            if (secondsToBall < GameMNG.MIN_VALUE_SECONDS_TO_NEW_BALL)
                GameMNG.Instance.SecondsToNewBall = GameMNG.DEFAULT_VALUE_SECONDS_TO_NEW_BALL;
            else if (secondsToBall > GameMNG.MAX_VALUE_SECONDS_TO_NEW_BALL)
                GameMNG.Instance.SecondsToNewBall = GameMNG.DEFAULT_VALUE_SECONDS_TO_NEW_BALL;
            else
                GameMNG.Instance.SecondsToNewBall = secondsToBall;
        }
    }

    #region Action Events
    
    public void PlayGame()
    {
        SetGameInfo();
        EnableAllGame();
        gameObject.SetActive(false);
    }

    public void LinkNumCardsInputToSlider()
    {
        _inputNumCards.text = _sliderNumCards.value.ToString();
    }

    public void LinkNumBallsInputToSlider()
    {
        _inputNumBalls.text = _sliderNumBalls.value.ToString();
    }

    #endregion

}
