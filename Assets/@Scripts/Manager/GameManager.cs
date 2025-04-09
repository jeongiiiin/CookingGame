using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region 인스턴스 생성
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // 전체 재료 리스트 생성
        AllIngredient = new List<string> { Define.Beef, Define.BurgerBun_B, Define.BurgerBun_T, Define.Cheese, Define.GroundBeef, Define.Lettuce, Define.Tomato };
    }
    #endregion

    // private
    TMP_Text _coinText;
    List<GameObject> _ingredientIcons = new List<GameObject>(); // 재료 아이콘 리스트

    // public
    public List<string> AllIngredient = new List<string>(); // 전체 재료 리스트
    public List<string> CorrectOrder; // 정답 리스트
    public Canvas Menu; // 메뉴창
    public Order order; // 주문서
    public GameObject IngredientIconPrefab; // 재료 아이콘 프리팹
    public Transform IconParent; // 재료 아이콘 위치
    public AudioSource BGM;
    public AudioSource SubmitSound;
    public AudioSource EndGameSound;
    public bool IsExistCorrectOrder = false; // 정답 리스트 존재 여부
    public bool IsPause = false; // 일시정지 여부 (메뉴창 클릭 여부)
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
        // 정답 리스트가 존재하지 않는다면
        if (!IsExistCorrectOrder)
        {
            // 정답 리스트 생성
            GenerateCorrectOrder();
        }

        // 코인 출력
        // * 천 단위로 콤마 찍기
        string coinText = string.Format("{0:n0}", Coin);
        _coinText.text = $"{coinText}";

        // Esc키를 눌렀다면
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 메뉴창이 열려있다면 닫고
            if (IsPause)
            {
                MenuClose();
            }
            // 닫혀있다면 열기
            else
            {
                MenuOpen();
            }
        }
    }

    // 정답 리스트 생성
    public void GenerateCorrectOrder()
    {
        // 정답 리스트에 BurgerBun_B, T는 반드시 포함되도록 함
        List<string> randomIngredients = new List<string>(AllIngredient);
        randomIngredients.Remove(Define.BurgerBun_B);
        randomIngredients.Remove(Define.BurgerBun_T);

        System.Random rand = new System.Random();
        List<string> selectedIngredients = new List<string>();

        selectedIngredients.Add(Define.BurgerBun_T);
        selectedIngredients.Add(Define.BurgerBun_B);

        // BurgerBun_T, B를 제외한 재료 두 가지 설정
        for (int i = 0; i < 2; i++)
        {
            int randomIndex = rand.Next(randomIngredients.Count);
            selectedIngredients.Add(randomIngredients[randomIndex]);
            randomIngredients.RemoveAt(randomIndex);
        }

        CorrectOrder = selectedIngredients;

        IsExistCorrectOrder = true;

        // 주문서 출력
        if (order != null)
        {
            order.UpdateOrderUI();
        }
    }

    void MenuOpen()
    {
        IsPause = true;
        // 일시정지
        Time.timeScale = 0;
        Menu.gameObject.SetActive(true);
    }

    void MenuClose()
    {
        IsPause = false;
        Menu.gameObject.SetActive(false);
        // 일시정지 해제
        Time.timeScale = 1f;
    }

    // 재료 아이콘 업데이트하는 함수
    public void UpdateIngredientIcons()
    {
        // 재료 아이콘 초기화
        foreach (var icon in _ingredientIcons)
        {
            Destroy(icon);
        }

        _ingredientIcons.Clear();

        // 들고 있는 접시가 없으면 나가기
        if (PlayManager.Instance.HeldPlate == null) return;

        // 접시 중심점
        Transform stackPosition = PlayManager.Instance.HeldPlate.transform.Find(Define.StackPosition);

        if (stackPosition != null)
        {
            // 자식 수를 세고
            int stackCount = stackPosition.childCount;

            // 둘 중 더 작은 값 반환
            int maxIcons = Mathf.Min(stackCount, 4);
            int iconsPerRow = 2;  // 한 줄에 2개
            float iconSpacing = 1.0f;  // 간격
            float horizontalOffset = iconSpacing * (iconsPerRow - 1) / 2;  // 중앙 정렬

            // 아이콘이 항상 화면에 잘보이도록 하기 위함
            IconParent.rotation = Quaternion.Euler(60, 0, 0);

            for (int i = 0; i < maxIcons; i++)
            {
                Transform ingredientTransform = stackPosition.GetChild(i);
                // 프리팹의 이름에 붙는 (Clone)을 제거함 (정확한 재료 이름으로 비교하기 위함)
                string ingredientName = ingredientTransform.name.Replace("(Clone)", "");

                // 아이콘 출력
                GameObject ingredientIcon = Instantiate(IngredientIconPrefab, IconParent);
                // 이름으로 아이콘 연결
                ingredientIcon.GetComponent<Image>().sprite = GetIngredientIconSprite(ingredientName);

                // 아이콘의 가로/세로 위치 계산
                int row = i / iconsPerRow;  // 몇 번째 줄인지 계산
                int column = i % iconsPerRow;  // 현재 줄에서 몇 번째 아이콘인지 계산

                // 아이콘 위치 조정 : 오른쪽으로 조금씩 이동, 새로운 아이콘은 중앙에 위치
                float xPos = column * iconSpacing - horizontalOffset;
                float yPos = -row * iconSpacing;  // 한 줄이 끝날 때마다 아래로 내려가도록 함

                ingredientIcon.transform.localPosition = new Vector3(xPos, yPos, 0);
                ingredientIcon.transform.localRotation = Quaternion.identity;

                // 아이콘 목록에 추가
                _ingredientIcons.Add(ingredientIcon);
            }
        }
    }


    // 이름으로 아이콘 이미지 연결하는 함수
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
            // 위에 있는 재료가 아닌 재료 (익지 않은, 썰지 않은 재료) 를 들면 X 아이콘 기본 출력
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
