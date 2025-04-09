using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// * Beef 스크립트 주석 참고

public class Tomato : MonoBehaviour
{
    // private
    GameObject _cutCabinet;
    Transform _centralPoint;
    Slider _cutTimer;
    float _pickUpDistance = 2.5f;
    float _timer = 2f;
    bool _canPickUp = true;

    // public
    public GameObject SliceTomato;
    public AudioSource CutSound;

    private void Start()
    {
        _cutCabinet = GameObject.FindGameObjectWithTag(Define.CutCabinet);
        _centralPoint = _cutCabinet.transform.Find(Define.CenterPoint);
        _cutTimer = GameObject.Find("CutTimer")?.GetComponent<Slider>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _cutCabinet.transform.position);

        if (distance <= _pickUpDistance)
        {
            if (Input.GetKeyDown(KeyCode.Space) && PlayManager.Instance.IsHoldingIngredient && _canPickUp)
            {
                if (_centralPoint != null && _centralPoint.childCount == 0)
                {
                    DropTomato();
                    ChangeTomato();
                }

                _canPickUp = true;
            }
        }

    }

    void DropTomato()
    {
        transform.SetParent(_centralPoint);
        transform.position = _centralPoint.position;
        transform.rotation = _centralPoint.rotation;

        PlayManager.Instance.HeldIngredient = null;
        PlayManager.Instance.IsHoldingIngredient = false;
    }

    void ChangeTomato()
    {
        _canPickUp = false;

        StartCoroutine(CoChangeTomato());
    }

    IEnumerator CoChangeTomato()
    {
        CutSound.Play();

        float timer = _timer;
        _cutTimer.value = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime;
            _cutTimer.value = 1 - (elapsedTime / timer);
            yield return null;
        }

        Destroy(gameObject);

        GameObject sliceTomato = Instantiate(SliceTomato, _centralPoint.transform.position, Quaternion.identity);

        sliceTomato.transform.SetParent(_centralPoint);
    }
}
