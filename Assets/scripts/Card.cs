using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public const int CardSize = 15;
    const int NumRow = 3;
    const int NumCols = 5;

    readonly Transform _transformThisCard;

    #region Bingo Prizes
    readonly int[][] _positionsMPrize = new []{new[]{0,0}, new []{1,0}, new []{2,0}, new []{0,1}, new []{1,2},
        new []{0,3}, new []{0,4}, new []{1,4}, new []{2,4}};
    bool[] _linesPrize;
    bool _isMPrize;
    bool _isBingo;
    #endregion

    CardCell[][] _allCells;

    public Card(Transform transform, Sprite[] cardNumbers, Sprite[] markedCardNumbers)
    {
        _transformThisCard = transform;
        CardCell.CardNumbers = cardNumbers;
        CardCell.MarkedCardNumbers = markedCardNumbers;

        GrabReferences();
    }

    void GrabReferences()
    {
        string nameItem;

        _linesPrize = new bool[NumRow];
        _allCells = new CardCell[NumRow][];
        
        /* Search all SpriteRenderer references for each cellNum of this card table
         * for _allSpriteInCards, and prepare _allNumbersInCards. */
        for (int i = 0; i < NumRow; i++)
        {
            _allCells[i] = new CardCell[NumCols];
            for (int j = 0; j < NumCols; j++)
            {
                nameItem = "c[" + i + "," + j + "]/cellNum";
                _allCells[i][j] = new CardCell(_transformThisCard.Find(nameItem).GetComponent<SpriteRenderer>());
            }
        }
    }

    void ResetPrizes()
    {
        _linesPrize = new bool[NumRow];
        _isMPrize = false;
        _isBingo = false;
    }

    /// <summary>
    /// Set new randoms values to the card.
    /// </summary>
    /// <param name="allRandomNumbers"> Array with random numbers, length should be equals to CardSize.</param>
    public void ResetCard(int[] allRandomNumbers)
    {
        if (allRandomNumbers.Length != CardSize)
            throw new System.ArgumentOutOfRangeException("Cards don't have a size of " + CardSize + ".");
        
        int indexNextRandom = 0;
        
        ResetPrizes();
        
        /* Set in order each random number in allRandomNumbers to _allNumbersInCards and _allSpriteInCards.
         * Set references for _fromRandomNumberToIndexRowCols. */
        for (int i = 0; i < NumRow; i++)
        {
            for (int j = 0; j < NumCols; j++)
            {
                _allCells[i][j].ResetCell(allRandomNumbers[indexNextRandom]);
                indexNextRandom++;
            }
        }
    }

    /// <summary>
    /// Mark all the cell of this card with the ball number.
    /// </summary>
    /// <param name="ballNumber">Number to mark in card.</param>
    public void Mark(int ballNumber)
    {
        for (int i = 0; i < NumRow; i++)
        {
            for (int j = 0; j < NumCols; j++)
            {
                if (!_allCells[i][j].IsCellMarked() && _allCells[i][j].CellValue == ballNumber)
                {
                    _allCells[i][j].MarkCell();
                }
            }
        }
    }

    #region Check line prize methods
    bool CheckThisLine(int rowIndex)
    {
        bool isLineMarked = true;
        
        for (int j = 0; j < NumCols; j++)
        {
            isLineMarked = _allCells[rowIndex][j].IsCellMarked();
            if (!isLineMarked)
                break;
        }

        return isLineMarked;
    }

    void PaintLine(int rowIndex)
    {
        for (int j = 0; j < NumCols; j++)
        {
            _allCells[rowIndex][j].PaintCell();
        }
    }

    public int CheckLine()
    {
        int numOfLines = 0;

        for (int i = 0; i < NumRow; i++)
        {
            if (_linesPrize[i])
                continue; // It's impossible do more than one horizontal line in the same line.
            
            if (CheckThisLine(i))
            {
                _linesPrize[i] = true;
                PaintLine(i);
                numOfLines++;
            }

        }

        return numOfLines;
    }
    #endregion

    #region Check M Prize methods
    void PaintM()
    {
        foreach (var index in _positionsMPrize)
            _allCells[index[0]][index[1]].PaintCell();
    }

    public bool CheckM()
    {
        if (_isMPrize)
            return false; // It's impossible do more than one M in the same card.

        _isMPrize = true;
        foreach (var index in _positionsMPrize)
        {
            if (!_allCells[index[0]][index[1]].IsCellMarked())
            {
                _isMPrize = false;
                break;
            }
        }

        if (_isMPrize)
            PaintM();

        return _isMPrize;
    }
    #endregion

    #region Check Bingo prize methods
    void PaintBingo()
    {
        for (int i = 0; i < NumRow; i++)
        {
            for (int j = 0; j < NumCols; j++)
            {
                _allCells[i][j].PaintCell();
            }
        }
    }

    public bool CheckBingo()
    {
        if (_isBingo)
            return false; // It's impossible do more than one bingo in the same card.
        
        // 1º test: If all lines of the card are red => BINGO.
        _isBingo = true;
        for (int i = 0; i < NumRow; i++)
        {
            if (!_linesPrize[i])
            {
                _isBingo = false;
                break;
            }
        }
        
        if (!_isBingo)
        {
            // 2º test: Check all cells. If all cells are marked => BINGO.
            _isBingo = true;
            for (int i = 0; i < NumRow; i++)
            {
                for (int j = 0; j < NumCols; j++)
                {
                    if (!_allCells[i][j].IsCellMarked())
                    {
                        _isBingo = false;
                        break;
                    }
                }
            }

            if (_isBingo)
                PaintBingo();
        }

        
        return _isBingo;
    }
    #endregion

}
