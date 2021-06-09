using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    public Button button;

    [SerializeField]
    private int _price;
    public int price
    {
        get => _price;
        set => _price = value;
    }

    [SerializeField]
    private string _companion;
    public string companion
    {
        get => _companion;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnActivate()
    {
        foreach (var comp in ClientUserData.companions)
        {
            if (comp.companion == companion)
            {
                if (button)
                    button.interactable = false;
                return;
            }
        }
    }
}
