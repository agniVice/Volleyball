using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    [SerializeField] private GameObject _selectedPanel;
    [SerializeField] private GameObject _buyAvailablePanel;
    [SerializeField] private GameObject _availablePanel;

    [SerializeField] private TextMeshProUGUI _priceText;

    [SerializeField] private int _price;
    [SerializeField] private int _id;

    private ShopManager _shopManager;

    private bool _elementAvailable;
    public void Initialize(ShopManager shopManager)
    { 
        _shopManager = shopManager;
        _priceText.text = _price.ToString();
        _elementAvailable = Convert.ToBoolean(PlayerPrefs.GetInt("Element" + _id, 0));
    }
    public void OnShopElementSelected()
    {
        _selectedPanel.SetActive(true);
        _buyAvailablePanel.SetActive(false);
        _availablePanel.SetActive(false);

        PlayerPrefs.SetInt("LastSelected", _id);
    }
    public void OnShopElementDeselected()
    {
        _selectedPanel.SetActive(false);

        if (_elementAvailable)
        {
            _buyAvailablePanel.SetActive(false);
            _availablePanel.SetActive(true);
        }
        else
        {
            _availablePanel.SetActive(false);
        }
    }
    public void SelectElement()
    {
        if (_elementAvailable)
        {
            _shopManager.OnShopElementSelected(this);
        }
        else
        {
            int coins = PlayerPrefs.GetInt("Coins", 0);
            if (coins >= _price)
            {
                PlayerPrefs.SetInt("Coins", coins -= _price);
                PlayerPrefs.SetInt("Element" + _id, 1);
                _elementAvailable = true;
                    
                FindObjectOfType<MenuUserInterface>().UpdateCoinsText();
                _shopManager.OnShopElementSelected(this);
            }
        }
    }
} 
