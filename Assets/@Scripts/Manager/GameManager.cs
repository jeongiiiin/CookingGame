using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region �ν��Ͻ� ����
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // ��ü ��� ����Ʈ ����
        AllIngredient = new List<string> { Define.Beef, Define.BurgerBun_B, Define.BurgerBun_T, Define.Cheese, Define.GroundBeef, Define.Lettuce, Define.Tomato };
    }
    #endregion

    // private
    TMP_Text _coinText;
    List<GameObject> _ingredientIcons = new List<GameObject>(); // ��� ������ ����Ʈ

    // public
    public List<string> AllIngredient = new List<string>(); // ��ü ��� ����Ʈ
    public List<string> CorrectOrder; // ���� ����Ʈ
    public Canvas Menu; // �޴�â
    public Order order; // �ֹ���
    public GameObject IngredientIconPrefab; // ��� ������ ������
    public Transform IconParent; // ��� ������ ��ġ
    public AudioSource BGM;
    public AudioSource SubmitSound;
    public AudioSource EndGameSound;
    public bool IsExistCorrectOrder = false; // ���� ����Ʈ ���� ����
    public bool IsPause = false; // �Ͻ����� ���� (�޴�â Ŭ�� ����)
    public int Coin = 0;

    private void Start()
    {
        if (BGM != null)
        {
            BGM.Play();
        }

        _coinText = GameObject.Find("CoinText").GetComponent<TMP_Text>();
    }

    void Update()
    {
        // ���� ����Ʈ�� �������� �ʴ´ٸ�
        if (!IsExistCorrectOrder)
        {
            // ���� ����Ʈ ����
            GenerateCorrectOrder();
        }

        // ���� ���
        // * õ ������ �޸� ���
        string coinText = string.Format("{0:n0}", Coin);
        _coinText.text = $"{coinText}";

        // EscŰ�� �����ٸ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �޴�â�� �����ִٸ� �ݰ�
            if (IsPause)
            {
                MenuClose();
            }
            // �����ִٸ� ����
            else
            {
                MenuOpen();
            }
        }
    }

    // ���� ����Ʈ ����
    public void GenerateCorrectOrder()
    {
        // ���� ����Ʈ�� BurgerBun_B, T�� �ݵ�� ���Եǵ��� ��
        List<string> randomIngredients = new List<string>(AllIngredient);
        randomIngredients.Remove(Define.BurgerBun_B);
        randomIngredients.Remove(Define.BurgerBun_T);

        System.Random rand = new System.Random();
        List<string> selectedIngredients = new List<string>();

        selectedIngredients.Add(Define.BurgerBun_T);
        selectedIngredients.Add(Define.BurgerBun_B);

        // BurgerBun_T, B�� ������ ��� �� ���� ����
        for (int i = 0; i < 2; i++)
        {
            int randomIndex = rand.Next(randomIngredients.Count);
            selectedIngredients.Add(randomIngredients[randomIndex]);
            randomIngredients.RemoveAt(randomIndex);
        }

        CorrectOrder = selectedIngredients;

        IsExistCorrectOrder = true;

        // �ֹ��� ���
        if (order != null)
        {
            order.UpdateOrderUI();
        }
    }

    void MenuOpen()
    {
        IsPause = true;
        // �Ͻ�����
        Time.timeScale = 0;
        Menu.gameObject.SetActive(true);
    }

    void MenuClose()
    {
        IsPause = false;
        Menu.gameObject.SetActive(false);
        // �Ͻ����� ����
        Time.timeScale = 1f;
    }

    // ��� ������ ������Ʈ�ϴ� �Լ�
    public void UpdateIngredientIcons()
    {
        // ��� ������ �ʱ�ȭ
        foreach (var icon in _ingredientIcons)
        {
            Destroy(icon);
        }

        _ingredientIcons.Clear();

        // ��� �ִ� ���ð� ������ ������
        if (PlayManager.Instance.HeldPlate == null) return;

        // ���� �߽���
        Transform stackPosition = PlayManager.Instance.HeldPlate.transform.Find(Define.StackPosition);

        if (stackPosition != null)
        {
            // �ڽ� ���� ����
            int stackCount = stackPosition.childCount;

            // �� �� �� ���� �� ��ȯ
            int maxIcons = Mathf.Min(stackCount, 4);
            int iconsPerRow = 2;  // �� �ٿ� 2��
            float iconSpacing = 1.0f;  // ����
            float horizontalOffset = iconSpacing * (iconsPerRow - 1) / 2;  // �߾� ����

            // �������� �׻� ȭ�鿡 �ߺ��̵��� �ϱ� ����
            IconParent.rotation = Quaternion.Euler(60, 0, 0);

            for (int i = 0; i < maxIcons; i++)
            {
                Transform ingredientTransform = stackPosition.GetChild(i);
                // �������� �̸��� �ٴ� (Clone)�� ������ (��Ȯ�� ��� �̸����� ���ϱ� ����)
                string ingredientName = ingredientTransform.name.Replace("(Clone)", "");

                // ������ ���
                GameObject ingredientIcon = Instantiate(IngredientIconPrefab, IconParent);
                // �̸����� ������ ����
                ingredientIcon.GetComponent<Image>().sprite = GetIngredientIconSprite(ingredientName);

                // �������� ����/���� ��ġ ���
                int row = i / iconsPerRow;  // �� ��° ������ ���
                int column = i % iconsPerRow;  // ���� �ٿ��� �� ��° ���������� ���

                // ������ ��ġ ���� : ���������� ���ݾ� �̵�, ���ο� �������� �߾ӿ� ��ġ
                float xPos = column * iconSpacing - horizontalOffset;
                float yPos = -row * iconSpacing;  // �� ���� ���� ������ �Ʒ��� ���������� ��

                ingredientIcon.transform.localPosition = new Vector3(xPos, yPos, 0);
                ingredientIcon.transform.localRotation = Quaternion.identity;

                // ������ ��Ͽ� �߰�
                _ingredientIcons.Add(ingredientIcon);
            }
        }
    }


    // �̸����� ������ �̹��� �����ϴ� �Լ�
    Sprite GetIngredientIconSprite(string ingredientName)
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
            // ���� �ִ� ��ᰡ �ƴ� ��� (���� ����, ���� ���� ���) �� ��� X ������ �⺻ ���
            default:
                return Resources.Load<Sprite>(Define.XIconPath);
        }
    }

    public void EndBGM()
    {
        if (BGM != null)
        {
            BGM.Stop();
        }
    }

    public void PlaySubmitSound()
    {
        SubmitSound.Play();
    }

    public void PlayEndGameSound()
    {
        EndGameSound.Play();
    }
}
