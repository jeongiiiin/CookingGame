using UnityEngine;

public class PlayManager : MonoBehaviour
{
    #region 인스턴스 생성
    public static PlayManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    // private
    float _pickUpDistance = 3f; // 상호작용 가능 거리

    // public
    public GameObject Player; // 플레이어
    public GameObject HeldPlate; // 들고 있는 접시
    public GameObject HeldIngredient; // 들고 있는 재료
    public Transform Hand; // 플레이어 손 위치
    public bool CanPickUp = true; // 들기 가능 여부
    public bool IsHoldingIngredient = false; // 재료 들고 있는지 여부

    private void Update()
    {
        // 접시 관리
        // 들고 있는 접시가 없다면
        if (HeldPlate == null)
        {
            // 접시 들기
            PickUpPlate();
        }
        // 접시를 들고 있다면
        else
        {
            // 스페이스바를 눌렀을 때
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 접시 놓기 (제출하기)
                DropPlate();
                DropPlateFin();
            }

            // 들고 있는 재료가 있다면
            if (HeldIngredient != null)
            {
                // 접시에 재료 쌓기
                StackIngredientOnPlate();
            }
        }

        // 재료 관리
        // 들고 있는 재료가 없다면
        if (HeldIngredient == null)
        {
            // 재료 들기
            PickUpIngredient();
        }
        // 상호작용이 가능하고, 스페이스바를 눌렀고, 재료를 들고 있다면
        else if (CanPickUp && Input.GetKeyDown(KeyCode.Space) && IsHoldingIngredient)
        {
            // 재료 놓기
            DropIngredient();
        }
        // 스페이스바를 떼면
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            CanPickUp = true;
        }

        // * 재료의 자식 관계가 해제되는 오류가 발생하여 추가함
        // 들고 있는 접시가 있을 때
        if (HeldPlate != null)
        {
            // 재료를 들고 있고, 재료가 접시 중심점 자식이 아니라면
            if (HeldIngredient != null && HeldIngredient.transform.parent != HeldPlate.transform.Find(Define.StackPosition))
            {
                // 재료의 부모를 접시 중심점으로 설정
                HeldIngredient.transform.SetParent(HeldPlate.transform.Find(Define.StackPosition));
            }
        }
    }

    // 가까운 캐비넷 찾기
    GameObject FindClosestCabinet()
    {
        GameObject closestCabinet = null;
        float closestDistance = _pickUpDistance;
        GameObject[] cabinets = GameObject.FindGameObjectsWithTag(Define.Cabinet);

        foreach (GameObject cabinet in cabinets)
        {
            float distance = Vector3.Distance(Player.transform.position, cabinet.transform.position);
            if (distance < closestDistance)
            {
                closestCabinet = cabinet;
                closestDistance = distance;
            }
        }

        return closestCabinet;
    }

    // 가까운 제출 캐비넷 찾기
    GameObject FindClosestExitCabinet()
    {
        GameObject closestExitCabinet = null;
        float closestDistance = _pickUpDistance;
        GameObject[] exitCabinets = GameObject.FindGameObjectsWithTag(Define.ExitCabinet);

        foreach (GameObject exitCabinet in exitCabinets)
        {
            float distance = Vector3.Distance(Player.transform.position, exitCabinet.transform.position);

            if (distance < closestDistance)
            {
                closestExitCabinet = exitCabinet;
                closestDistance = distance;
            }
        }

        return closestExitCabinet;
    }

    // 접시 드는 함수
    public void PickUpPlate()
    {
        GameObject plate = FindClosestPlate();

        if (plate != null && Vector3.Distance(Player.transform.position, plate.transform.position) <= _pickUpDistance)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HeldPlate = plate;
                // 플레이어 손의 자식으로 설정
                HeldPlate.transform.SetParent(Hand);
                HeldPlate.transform.localPosition = Vector3.zero;
                HeldPlate.transform.localRotation = Quaternion.identity;

                // 재료 아이콘 업데이트
                GameManager.Instance.UpdateIngredientIcons();
            }
        }
    }

    // 가까운 접시 찾기
    GameObject FindClosestPlate()
    {
        GameObject closestPlate = null;
        float closestDistance = _pickUpDistance;
        GameObject[] plates = GameObject.FindGameObjectsWithTag(Define.Plate);

        foreach (GameObject plate in plates)
        {
            float distance = Vector3.Distance(Player.transform.position, plate.transform.position);
            if (distance < closestDistance)
            {
                closestPlate = plate;
                closestDistance = distance;
            }
        }

        return closestPlate;
    }

    // 접시 내려놓는 함수
    public void DropPlate()
    {
        if (HeldPlate != null)
        {
            // * 재료의 자식 관계가 해제되는 오류가 발생하여 추가함
            if (HeldIngredient != null && HeldIngredient.transform.parent != HeldPlate.transform.Find(Define.StackPosition))
            {
                HeldIngredient.transform.SetParent(HeldPlate.transform.Find(Define.StackPosition));
            }

            GameObject closestCabinet = FindClosestCabinet();

            if (closestCabinet != null)
            {
                Transform centralPoint = closestCabinet.transform.Find(Define.CenterPoint);

                // 캐비넷 위에 물건이 없을 때만 배치
                if (centralPoint != null && centralPoint.childCount == 0)
                {
                    HeldPlate.transform.SetParent(centralPoint);
                    HeldPlate.transform.position = centralPoint.position;
                    HeldPlate.transform.rotation = centralPoint.rotation;

                    HeldIngredient = null;
                    HeldPlate = null;
                    IsHoldingIngredient = false;
                    CanPickUp = true;

                    // 재료 아이콘 업데이트
                    GameManager.Instance.UpdateIngredientIcons();
                }
            }
        }
    }

    // 완료된 접시 내려놓는 함수
    public void DropPlateFin()
    {
        if (HeldPlate != null)
        {
            GameObject closestExitCabinet = FindClosestExitCabinet();

            if (closestExitCabinet != null)
            {
                GameManager.Instance.PlaySubmitSound();

                Transform centralPoint = closestExitCabinet.transform.Find("CenterPoint");

                if (centralPoint != null & centralPoint.childCount == 0)
                {
                    HeldPlate.transform.SetParent(centralPoint);
                    HeldPlate.transform.position = centralPoint.position;
                    HeldPlate.transform.rotation = centralPoint.rotation;

                    HeldPlate = null;
                    IsHoldingIngredient = false;
                    CanPickUp = true;

                    GameManager.Instance.UpdateIngredientIcons();
                }
            }
        }
    }

    // 접시에 쌓는 함수
    public void StackIngredientOnPlate()
    {
        // 재료를 들고 있고, 잡을 수 없고, 접시를 들고 있지 않으면 나가기
        if (HeldIngredient == null || !CanPickUp || HeldPlate == null) return;


        Transform plateStackPosition = HeldPlate.transform.Find("StackPosition");

        if (plateStackPosition == null) return;

        int stackCount = plateStackPosition.childCount;

        // 접시 위에 4개의 재료만 올릴 수 있도록 함
        if (stackCount <= 4)
        {
            // BurgerBun_T는 항상 맨 위에 오도록 함
            if (HeldIngredient.name.Contains(Define.BurgerBun_T))
            {
                HeldIngredient.transform.SetParent(plateStackPosition);
                HeldIngredient.transform.localPosition = new Vector3(0, 0.02f * 4, 0);
                HeldIngredient.transform.localRotation = Quaternion.identity;
            }
            else
            {
                float stackHeight = 0.02f;

                HeldIngredient.transform.SetParent(plateStackPosition);
                HeldIngredient.transform.localPosition = new Vector3(0, stackHeight * stackCount, 0);
                HeldIngredient.transform.localRotation = Quaternion.identity;
            }

            GameManager.Instance.UpdateIngredientIcons();

            HeldIngredient = null;
            IsHoldingIngredient = false;
            CanPickUp = true;
        }
        else
        {
            // * 의도하지 않은 재료가 자식 관계로 추가되는 오류를 해결하기 위함
            for (int i = 4; i < stackCount; i++)
            {
                Transform child = plateStackPosition.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }

    // 재료 드는 함수
    public void PickUpIngredient()
    {
        GameObject ingredient = FindClosestIngredient();
        Transform plateStackPosition;

        if (ingredient != null && Vector3.Distance(Player.transform.position, ingredient.transform.position) <= _pickUpDistance)
        {
            if (Input.GetKeyDown(KeyCode.Space) && HeldPlate == null)
            {
                HeldIngredient = ingredient;
                HeldIngredient.transform.SetParent(Hand);
                HeldIngredient.transform.localPosition = Vector3.zero;
                HeldIngredient.transform.localRotation = Quaternion.identity;
                IsHoldingIngredient = true;
                CanPickUp = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && HeldPlate != null)
            {
                if (HeldPlate != null)
                {
                    plateStackPosition = HeldPlate.transform.Find(Define.StackPosition);

                    HeldIngredient = ingredient;
                    HeldIngredient.transform.SetParent(plateStackPosition);
                    HeldIngredient.transform.localPosition = Vector3.zero;
                    HeldIngredient.transform.localRotation = Quaternion.identity;
                    IsHoldingIngredient = true;

                    // 아이콘 업데이트
                    GameManager.Instance.UpdateIngredientIcons();
                }
            }
        }
    }

    // 가까운 재료 찾기
    GameObject FindClosestIngredient()
    {
        GameObject closestIngredient = null;
        float closestDistance = _pickUpDistance;

        GameObject[] Ingredients = GameObject.FindGameObjectsWithTag(Define.Ingredient);

        foreach (GameObject ingredient in Ingredients)
        {
            float distance = Vector3.Distance(Player.transform.position, ingredient.transform.position);

            if (distance < closestDistance)
            {
                closestIngredient = ingredient;
                closestDistance = distance;
            }
        }

        return closestIngredient;
    }

    // 재료 내려놓기
    public void DropIngredient()
    {
        if (HeldPlate != null)
        {
            if (HeldIngredient != null && HeldIngredient.transform.parent != HeldPlate.transform.Find(Define.StackPosition))
            {
                HeldIngredient.transform.SetParent(HeldPlate.transform.Find(Define.StackPosition));
            }
        }

        if (IsHoldingIngredient || HeldIngredient != null)
        {
            GameObject closestCabinet = FindClosestCabinet();

            if (closestCabinet != null)
            {
                Transform centralPoint = closestCabinet.transform.Find(Define.CenterPoint);

                // 캐비넷 위에 물건이 없을 때만 배치
                if (centralPoint != null && centralPoint.childCount == 0 && HeldPlate == null)
                {
                    foreach (Transform child in Hand)
                    {
                        child.SetParent(centralPoint);
                        child.position = centralPoint.position;
                        child.rotation = centralPoint.rotation;
                    }

                    HeldIngredient = null;
                    IsHoldingIngredient = false;
                    CanPickUp = true;
                }
            }
        }
    }
}
