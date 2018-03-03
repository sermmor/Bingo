using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCell {
    
    public static Sprite[] CardNumbers;
    public static Sprite[] MarkedCardNumbers;
    
    SpriteRenderer _sprite; // NumRow x NumCols
    int _value; // NumRow x NumCols

    public int CellValue {
        get { return Mathf.Abs(_value); }
    }

    public CardCell(SpriteRenderer srCell)
    {
        _sprite = srCell;
    }

    public void ResetCell(int cellValue)
    {
        _value = cellValue;
        _sprite.sprite = CardNumbers[cellValue - 1];
        _sprite.color = Color.white;
    }

    public void MarkCell()
    {
        // Mark a cell => Put in negative the cell.
        int currentNumber = _value;
        _value = -currentNumber;
        _sprite.sprite = CardCell.MarkedCardNumbers[currentNumber - 1];
    }

    public bool IsCellMarked()
    {
        return _value < 0;
    }

    public void PaintCell()
    {
        int currentNumber = CellValue;
        _sprite.sprite = CardNumbers[currentNumber - 1];
        _sprite.color = Color.red;
    }
}
