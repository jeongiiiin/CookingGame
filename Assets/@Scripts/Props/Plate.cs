using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Plate : MonoBehaviour
{
    #region 인스턴스 생성
    public static Plate Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    // private
    Vector3 _initialPosition;
    Vector3 _initialScale;

    // public
    public TextMeshPro CorrectText;

    private void Start()
    {
        _initialPosition = transform.position;
        _initialScale = transform.localScale;
    }

    private void Update()
    {
        SubmitPlate();
    }

    // 접시 제출하는 함수 (접시 제거 및 재생성)
    void SubmitPlate()
    {
        GameObject[] exitCabinets = GameObject.FindGameObjectsWithTag(Define.ExitCabinet);

        foreach (GameObject exitCabinet in exitCabinets)
        {
            Transform centerPoint = exitCabinet.transform.Find(Define.CenterPoint);

            if (centerPoint != null && transform.IsChildOf(centerPoint))
            {
                if (PlayManager.Instance.HeldIngredient != null && PlayManager.Instance.HeldIngredient.transform.parent != gameObject.transform.Find(Define.StackPosition))
                {
                    PlayManager.Instance.HeldIngredient.transform.SetParent(gameObject.transform.Find(Define.StackPosition));
                }

                // 정답이라면
                if (CheckPlateOrder())
                {
                    GameManager.Instance.Coin += 500;
                    CorrectText.text = "+500";
                    CorrectText.color = new Color32(78, 173, 255, 255);
                    Instantiate(CorrectText, transform.position, Quaternion.Euler(60,0,0));
                }
                // 오답이라면
                else
                {
                    GameManager.Instance.Coin -= 100;
                    CorrectText.text = "-100";
                    CorrectText.color = new Color32(255, 71, 96, 255);
                    Instantiate(CorrectText, transform.position, Quaternion.Euler(60,0,0));
                }

                GameManager.Instance.IsExistCorrectOrder = false;
                DestroyIngredient(gameObject);
                Destroy(gameObject);

                if (PlayManager.Instance.HeldIngredient != null)
                {
                    PlayManager.Instance.HeldIngredient.transform.SetParent(null);
                    PlayManager.Instance.HeldIngredient = null;
                    PlayManager.Instance.IsHoldingIngredient = false;
                }

                GameObject newPlate = Instantiate(gameObject, _initialPosition, Quaternion.identity);
                newPlate.transform.localScale = _initialScale;

                DestroyIngredient(newPlate);

                Plate newPlateScript = newPlate.GetComponent<Plate>();
                newPlateScript.enabled = true;

                break;
            }
        }
    }

    // 정답 확인 (플레이트에 담긴 재료 확인)
    public bool CheckPlateOrder()
    {
        Transform stackPosition = transform.Find(Define.StackPosition);

        if (stackPosition != null)
        {
            List<string> currentOrder = new List<string>();

            foreach (Transform child in stackPosition)
            {
                string ingredientName = child.name.Replace("(Clone)", "");
                currentOrder.Add(ingredientName);
            }

            // 누락된 재료가 있으면 오답
            foreach (string ingredient in GameManager.Instance.CorrectOrder)
            {
                if (!currentOrder.Contains(ingredient))
                {
                    return false;
                }
            }

            // 추가된 재료가 있으면 오답
            foreach (string ingredient in currentOrder)
            {
                if (!GameManager.Instance.CorrectOrder.Contains(ingredient))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    // 재생성될 때 재료 삭제하는 함수
    void DestroyIngredient(GameObject plate)
    {
        Transform stackPosition = plate.transform.Find(Define.StackPosition);

        if (stackPosition != null)
        {
            foreach (Transform child in stackPosition)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
