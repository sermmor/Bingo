using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestingUtils
{
    public static List<int> AllTestCardList = new List<int>(new [] {1, 59, 49, 51, 34, 18, 3, 43, 39, 40, 2, 55, 16,
        5, 57, 4, 29, 30, 6, 29, 31, 10, 32, 9, 33, 17, 18, 20, 13, 30, 11, 12, 35, 36, 15, 38, 39, 41, 8, 42,
        43, 7, 44, 45, 14, 47, 48, 50, 52, 16, 53, 17, 54, 56, 46, 58, 18, 19, 21, 60});
    
    static readonly List<int> AllBallTestList = new List<int>(new [] {4, 29, 30, 6, 21, 31, 10, 32, 9, 33, 17, 18, 20, 13,
        2, 1, 59, 43, 51, 34, 40, 57, 38, 41, 8, 42, 58, 60, 50, 39});

    public static void PrepareBallsTest(List<SpriteRenderer> allSRBalls, List<int> allRandomBalls, 
        Sprite[] ballSprites)
    {
        int nextTest;
        for (int i = 0; i < GameMNG.Instance.NumberOfBalls; i++)
        {
            nextTest = AllBallTestList[i];
            allRandomBalls.Add(nextTest);
            allSRBalls[i].sprite = ballSprites[nextTest - 1];
        }
    }
}
