using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField]
    private int _price;
    public int price
    {
        get => _price;
        set => _price = value;
    }
}
