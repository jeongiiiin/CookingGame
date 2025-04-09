using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    // private
    GameManager _gameManager;

    // public
    public List<Image> IngredientIcons = new List<Image>();

    void Start()
    {
        _gameManager = GameManager.Instance;

        if (!_gameManager.IsExistCorrectOrder)
        {
            _gameManager.GenerateCorrectOrder();
        }

        UpdateOrderUI();
    }

    public void UpdateOrderUI()
    {
        for (int i = 0; i < _gameManager.CorrectOrder.Count; i++)
        {
            Sprite ingredientIcon = GetIngredientIcon(_gameManager.CorrectOrder[i]);

            if (ingredientIcon != null && i < IngredientIcons.Count)
            {
                IngredientIcons[i].sprite = ingredientIcon;
                IngredientIcons[i].gameObject.SetActive(true);
            }
            else if (i < IngredientIcons.Count)
            {
                IngredientIcons[i].gameObject.SetActive(false);
            }
        }
    }

    Sprite GetIngredientIcon(string ingredientName)
    {
        foreach (var ingredient in _gameManager.AllIngredient)
        {
            if (ingredient == ingredientName)
            {
                return GetIconByIngredientName(ingredientName);
            }
        }

        return null;
    }

    Sprite GetIconByIngredientName(string ingredientName)
    {
        switch (ingredientName)
        {
            case Define.Beef:
                return Resources.Load<Sprite>(Define.BeefIconPath);
            case Define.BurgerBun_B:
                return Resources.Load<Sprite>(Define.BurgerBun_BIconPath);
            case Define.BurgerBun_T:
                return Resources.Load<Sprite>(Define.BurgerBun_TIconPath);
            case Define.Cheese:
                return Resources.Load<Sprite>(Define.CheeseIconPath);
            case Define.GroundBeef:
                return Resources.Load<Sprite>(Define.GroundBeefIconPath);
            case Define.Lettuce:
                return Resources.Load<Sprite>(Define.LettuceIconPath);
            case Define.Tomato:
                return Resources.Load<Sprite>(Define.TomatoIconPath);
            default:
                return null;
        }

    }
}
