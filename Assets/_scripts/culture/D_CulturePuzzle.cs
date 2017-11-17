using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CulturePuzzle : MonoBehaviour
{
    public D_CultureCoin mActiveCoin;

    public List<D_CulturePiece> pPieceSelection = new List<D_CulturePiece>();

    public List<D_CulturePiece> mSelection = new List<D_CulturePiece>();
     
    public bool IsNewPieceAvailable()
    {
        return false;
    }
}
