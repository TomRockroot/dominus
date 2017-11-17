using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public static class D_Dice {

   

    public static int RollDie(EDieType dieType)
    {
        switch(dieType)
        {
            case EDieType.DT_None:
                return Mathf.CeilToInt(Random.Range(0.01f, 3.99f))-2;
            case EDieType.DT_D4:
               return Mathf.CeilToInt( Random.Range(0.01f, 3.99f));
            case EDieType.DT_D6:
                return Mathf.CeilToInt(Random.Range(0.01f, 5.99f));
            case EDieType.DT_D8:
                return Mathf.CeilToInt(Random.Range(0.01f, 7.99f));
            case EDieType.DT_D10:
                return Mathf.CeilToInt(Random.Range(0.01f, 9.99f));
            case EDieType.DT_D12:
                return Mathf.CeilToInt(Random.Range(0.01f, 11.99f));
        }
        return 0;
    }

    public static int DieTypeToInt(EDieType dieType)
    {
        switch(dieType)
        {
            case EDieType.DT_None:
                return 0;
            case EDieType.DT_D4:
                return 4;
            case EDieType.DT_D6:
                return 6;
            case EDieType.DT_D8:
                return 8;
            case EDieType.DT_D10:
                return 10;
            case EDieType.DT_D12:
                return 12;
        }

        return 0;
    }
}
