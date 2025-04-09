using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// * Beef 스크립트 주석 참고

public class GroundBeef : MonoBehaviour
{
    // private
    GameObject _bakeCabinet;
    Transform _centralPoint;
    Slider _bakeTimer;
    float _pickUpDistance = 2.5f;
    float _timer = 2f;
    bool _canPickUp = true;

    // public
    public GameObject BakedGroundBeef;
    public GameObject BakeEffect;
    public AudioSource BakeSound;

    void Start()
    {
        _bakeCabinet = GameObject.FindGameObjectWithTag(Define.BakeCabinet);
        _centralPoint = _bakeCabinet.transform.Find(Define.CenterPoint);
        _bakeTimer = GameObject.Find("BakeTimer")?.GetComponent<Slider>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _bakeCabinet.transform.position);

        if (distance <= _pickUpDistance)
        {
            if (Input.GetKeyDown(KeyCode.Space) && PlayManager.Instance.IsHoldingIngredient && _canPickUp)
            {
                if (_centralPoint != null && _centralPoint.childCount == 0)
                {
                    DropGroundBeef();
                    ChangeGroundBeef();
                }

                _canPickUp = true;
            }
        }
    }

    void DropGroundBeef()
    {
        Instantiate(BakeEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

        transform.SetParent(_centralPoint);
        transform.position = _centralPoint.position;
        transform.rotation = _centralPoint.rotation;

        PlayManager.Instance.HeldIngredient = null;
        PlayManager.Instance.IsHoldingIngredient = false;
    }

    void ChangeGroundBeef()
    {
        _canPickUp = false;

        StartCoroutine(CoChangeGroundBeef());
    }

    IEnumerator CoChangeGroundBeef()
    {
        BakeSound.PlayOneShot(BakeSound.clip);

        float timer = _timer;
        _bakeTimer.value = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime;
            _bakeTimer.value = 1 - (elapsedTime / timer);
            yield return null;
        }

        Destroy(gameObject);

        GameObject bakedGroundBeef = Instantiate(BakedGroundBeef, _centralPoint.transform.position, Quaternion.identity);

        bakedGroundBeef.transform.SetParent(_centralPoint);
    }
}
