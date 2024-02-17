using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopElement> _shopElements = new List<ShopElement>();

    private void Awake()
    {
        PlayerPrefs.SetInt("Element0", 1);
        foreach (var item in _shopElements)
        {
            item.Initialize(this);
        }
        DeselectAllWithin(_shopElements[PlayerPrefs.GetInt("LastSelected", 0)]);
        _shopElements[PlayerPrefs.GetInt("LastSelected", 0)].OnShopElementSelected();
    }
    public void OnShopElementSelected(ShopElement shopElement)
    {
        DeselectAllWithin(shopElement);
        shopElement.OnShopElementSelected();
    }
    private void DeselectAllWithin(ShopElement shopElement)
    {
        foreach (var element in _shopElements)
        {
            if (shopElement != element)
                element.OnShopElementDeselected();
        }
    }

}
