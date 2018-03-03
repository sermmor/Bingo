using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BallGUI : MonoBehaviour
{
    public Sprite[] BallSprites;

    List<SpriteRenderer> _allSRBalls;
    List<int> _allRandomBalls;
    
    void Start ()
    {
        _allSRBalls = new List<SpriteRenderer>();
        GrabReferences();
    }

    void GrabReferences()
    {
        // Disable all balls (all balls > NumberOfBalls) and grab all it's SpriteRenderer references in _allSRBalls.
        int i = 0;
        string iterIndexBall;
        Transform currentBallTransform;
        do
        {
            iterIndexBall = i > 9 ? i.ToString() : ("0" + i);
            currentBallTransform = transform.Find("bolap" + iterIndexBall);

            if (currentBallTransform != null)
            {
                _allSRBalls.Add(currentBallTransform.GetComponent<SpriteRenderer>());
                currentBallTransform.gameObject.SetActive(false);
            }
            i++;

        } while (currentBallTransform != null);
    }

    public void ResetBalls()
    {
        foreach (var srBall in _allSRBalls)
        {
            srBall.gameObject.SetActive(false);
        }

        CreateBalls();
    }

    void CreateBalls()
    {
        if (GameMNG.Instance.TestingModeEnabled)
        {
            _allRandomBalls = new List<int>();
            TestingUtils.PrepareBallsTest(_allSRBalls, _allRandomBalls, BallSprites);
            return;
        }

        List<int> allRangeNumbers = new List<int>(Enumerable.Range(1, 60).ToArray());
        _allRandomBalls = new List<int>();

        int nextRandomIndex, nextRandom;
        for (int i = 0; i < GameMNG.Instance.NumberOfBalls; i++)
        {
            nextRandomIndex = Random.Range(0, allRangeNumbers.Count);
            nextRandom = allRangeNumbers[nextRandomIndex];
            _allRandomBalls.Add(nextRandom);
            _allSRBalls[i].sprite = BallSprites[nextRandom - 1];
            allRangeNumbers.RemoveAt(nextRandomIndex);
        }
    }

    int PopBall()
    {
        if (_allRandomBalls.Count < 0)
            throw new System.ArgumentOutOfRangeException("There isn't more balls.");

        int ball = _allRandomBalls[0];

        _allRandomBalls.RemoveAt(0);
        
        return ball;
    }

    /// <summary>
    /// Put a new ball in the Ball Box.
    /// </summary>
    /// <returns> Number of the ball. </returns>
    public int GetNewBall()
    {
        // Enable ball.
        int indexToExtract = GameMNG.Instance.NumberOfBalls - _allRandomBalls.Count;
        _allSRBalls[indexToExtract].gameObject.SetActive(true);

        // Pop ball of _allRandomBalls counter and return it.
        return PopBall();
    }
}
