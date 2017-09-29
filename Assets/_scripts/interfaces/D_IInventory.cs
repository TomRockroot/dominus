using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface D_IInventory
{
    void RemoveFromInventory(D_Item item);
    void AddToInventory(D_Item item);
    void DropInventory();
}
