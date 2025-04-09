using UnityEngine;

public class PlayManager : MonoBehaviour
{
    #region �ν��Ͻ� ����
    public static PlayManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    // private
    float _pickUpDistance = 3f; // ��ȣ�ۿ� ���� �Ÿ�

    // public
    public GameObject Player; // �÷��̾�
    public GameObject HeldPlate; // ��� �ִ� ����
    public GameObject HeldIngredient; // ��� �ִ� ���
    public Transform Hand; // �÷��̾� �� ��ġ
    public bool CanPickUp = true; // ��� ���� ����
    public bool IsHoldingIngredient = false; // ��� ��� �ִ��� ����

    private void Update()
    {
        // ���� ����
        // ��� �ִ� ���ð� ���ٸ�
        if (HeldPlate == null)
        {
            // ���� ���
            PickUpPlate();
        }
        // ���ø� ��� �ִٸ�
        else
        {
            // �����̽��ٸ� ������ ��
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ���� ���� (�����ϱ�)
                DropPlate();
                DropPlateFin();
            }

            // ��� �ִ� ��ᰡ �ִٸ�
            if (HeldIngredient != null)
            {
                // ���ÿ� ��� �ױ�
                StackIngredientOnPlate();
            }
        }

        // ��� ����
        // ��� �ִ� ��ᰡ ���ٸ�
        if (HeldIngredient == null)
        {
            // ��� ���
            PickUpIngredient();
        }
        // ��ȣ�ۿ��� �����ϰ�, �����̽��ٸ� ������, ��Ḧ ��� �ִٸ�
        else if (CanPickUp && Input.GetKeyDown(KeyCode.Space) && IsHoldingIngredient)
        {
            // ��� ����
            DropIngredient();
        }
        // �����̽��ٸ� ����
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            CanPickUp = true;
        }

        // * ����� �ڽ� ���谡 �����Ǵ� ������ �߻��Ͽ� �߰���
        // ��� �ִ� ���ð� ���� ��
        if (HeldPlate != null)
        {
            // ��Ḧ ��� �ְ�, ��ᰡ ���� �߽��� �ڽ��� �ƴ϶��
            if (HeldIngredient != null && HeldIngredient.transform.parent != HeldPlate.transform.Find(Define.StackPosition))
            {
                // ����� �θ� ���� �߽������� ����
                HeldIngredient.transform.SetParent(HeldPlate.transform.Find(Define.StackPosition));
            }
        }
    }

    // ����� ĳ��� ã��
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

    // ����� ���� ĳ��� ã��
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

    // ���� ��� �Լ�
    public void PickUpPlate()
    {
        GameObject plate = FindClosestPlate();

        if (plate != null && Vector3.Distance(Player.transform.position, plate.transform.position) <= _pickUpDistance)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HeldPlate = plate;
                // �÷��̾� ���� �ڽ����� ����
                HeldPlate.transform.SetParent(Hand);
                HeldPlate.transform.localPosition = Vector3.zero;
                HeldPlate.transform.localRotation = Quaternion.identity;

                // ��� ������ ������Ʈ
                GameManager.Instance.UpdateIngredientIcons();
            }
        }
    }

    // ����� ���� ã��
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

    // ���� �������� �Լ�
    public void DropPlate()
    {
        if (HeldPlate != null)
        {
            // * ����� �ڽ� ���谡 �����Ǵ� ������ �߻��Ͽ� �߰���
            if (HeldIngredient != null && HeldIngredient.transform.parent != HeldPlate.transform.Find(Define.StackPosition))
            {
                HeldIngredient.transform.SetParent(HeldPlate.transform.Find(Define.StackPosition));
            }

            GameObject closestCabinet = FindClosestCabinet();

            if (closestCabinet != null)
            {
                Transform centralPoint = closestCabinet.transform.Find(Define.CenterPoint);

                // ĳ��� ���� ������ ���� ���� ��ġ
                if (centralPoint != null && centralPoint.childCount == 0)
                {
                    HeldPlate.transform.SetParent(centralPoint);
                    HeldPlate.transform.position = centralPoint.position;
                    HeldPlate.transform.rotation = centralPoint.rotation;

                    HeldIngredient = null;
                    HeldPlate = null;
                    IsHoldingIngredient = false;
                    CanPickUp = true;

                    // ��� ������ ������Ʈ
                    GameManager.Instance.UpdateIngredientIcons();
                }
            }
        }
    }

    // �Ϸ�� ���� �������� �Լ�
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

    // ���ÿ� �״� �Լ�
    public void StackIngredientOnPlate()
    {
        // ��Ḧ ��� �ְ�, ���� �� ����, ���ø� ��� ���� ������ ������
        if (HeldIngredient == null || !CanPickUp || HeldPlate == null) return;


        Transform plateStackPosition = HeldPlate.transform.Find("StackPosition");

        if (plateStackPosition == null) return;

        int stackCount = plateStackPosition.childCount;

        // ���� ���� 4���� ��Ḹ �ø� �� �ֵ��� ��
        if (stackCount <= 4)
        {
            // BurgerBun_T�� �׻� �� ���� ������ ��
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
            // * �ǵ����� ���� ��ᰡ �ڽ� ����� �߰��Ǵ� ������ �ذ��ϱ� ����
            for (int i = 4; i < stackCount; i++)
            {
                Transform child = plateStackPosition.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }

    // ��� ��� �Լ�
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

                    // ������ ������Ʈ
                    GameManager.Instance.UpdateIngredientIcons();
                }
            }
        }
    }

    // ����� ��� ã��
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

    // ��� ��������
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

                // ĳ��� ���� ������ ���� ���� ��ġ
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
