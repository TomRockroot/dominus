using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CulturePiece : MonoBehaviour
{
    public D_CultureCoin mAvailableCoin;

    public List<D_CultureSocket> pSocketSelection = new List<D_CultureSocket>();

    public List<D_CultureSocket> mSockets;

    public bool IsCoinAvailable()
    {
        return false;
    }
}
