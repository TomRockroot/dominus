using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface D_ICrafter
{
    List<D_Recipe> GetRecipes();
    List<D_Item> GetInventory();
    List<D_Item> GetSimilarItems(D_Item blueprint);

    Transform GetTransform();
    D_IInventory AsInventory();

    void Craft(D_Item result, D_Item first, D_Item second);
}
