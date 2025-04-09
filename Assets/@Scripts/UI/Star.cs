using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    // private
    Image _star;

    // public
    public Sprite Star_01;
    public Sprite Star_02;
    public Sprite Star_03;
    public Sprite Star_04;

    void Start()
    {
        _star = GetComponent<Image>();
    }

    void Update()
    {
        if (GameManager.Instance.Coin < 500)
        {
            _star.sprite = Star_01;
        }
        if (GameManager.Instance.Coin >= 500)
        {
            _star.sprite = Star_02;
        }
        if (GameManager.Instance.Coin >= 1000)
        {
            _star.sprite = Star_03;
        }
        if (GameManager.Instance.Coin >= 1500)
        {
            _star.sprite = Star_04;
        }
    }
}
