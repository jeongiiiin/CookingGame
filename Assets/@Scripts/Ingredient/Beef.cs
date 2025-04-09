using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Beef : MonoBehaviour
{
    // private
    GameObject _bakeCabinet; // 프라이팬 캐비넷
    Transform _centralPoint; // 캐비넷 중심점
    Slider _bakeTimer; // 굽는 시간 타이머
    float _pickUpDistance = 2.5f; // 상호작용 가능 거리
    float _timer = 2f; // 굽는 시간
    bool _canPickUp = true; // 들기 가능 여부

    // public
    public GameObject BakedBeef; // 구워진 고기 (일정 시간 후 프리팹 교체 위함)
    public GameObject BakeEffect; // 구울 때 이펙트
    public AudioSource BakeSound;

    void Start()
    {
        _bakeCabinet = GameObject.FindGameObjectWithTag(Define.BakeCabinet);
        _centralPoint = _bakeCabinet.transform.Find(Define.CenterPoint);
        _bakeTimer = GameObject.Find("BakeTimer")?.GetComponent<Slider>();
    }

    void Update()
    {
        // Beef와 프라이팬 캐비넷 위치 확인
        float distance = Vector3.Distance(transform.position, _bakeCabinet.transform.position);

        // 가깝고 (상호작용 가능 거리 안에 들어왔고)
        if (distance <= _pickUpDistance)
        {
            // 스페이스바를 눌렀고, 플레이어가 재료를 들고 있고, 상호작용이 가능하다면
            if (Input.GetKeyDown(KeyCode.Space) && PlayManager.Instance.IsHoldingIngredient && _canPickUp)
            {
                // 캐비넷 중심점이 널이 아니고, 캐비넷 자식이 없다면
                if (_centralPoint != null && _centralPoint.childCount == 0)
                {
                    // Beef 내려놓기
                    DropBeef();
                    // Beef 굽기
                    ChangeBeef();
                }

                _canPickUp = true;
            }
        }
    }

    // Beef 내려놓는 함수
    void DropBeef()
    {
        // 내려놓으면 굽는 이펙트 발생
        Instantiate(BakeEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

        // 캐비넷 중심점 자식으로 설정 (Beef 위치, 방향 캐비넷 중심점으로 설정)
        transform.SetParent(_centralPoint);
        transform.position = _centralPoint.position;
        transform.rotation = _centralPoint.rotation;

        // 플레이어가 재료를 들지 않은 상태로 변경
        PlayManager.Instance.HeldIngredient = null;
        PlayManager.Instance.IsHoldingIngredient = false;
    }

    // Beef 굽는 함수
    void ChangeBeef()
    {
        // 굽는 중에는 들 수 없도록 변수로 체크
        _canPickUp = false;

        StartCoroutine(CoChangeBeef());
    }

    // 구운 Beef로 프리팹 변경
    IEnumerator CoChangeBeef()
    {
        BakeSound.PlayOneShot(BakeSound.clip);

        // 타이머 처리
        float timer = _timer; // 굽는 시간
        _bakeTimer.value = 1f;
        float elapsedTime = 0f; // 현재 시간

        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime; // 매 프레임마다 증가
            _bakeTimer.value = 1 - (elapsedTime / timer); // 비율 계산해서 슬라이더 비우기
            yield return null;
        }

        Destroy(gameObject); // 시간 지난 후 기존 Beef 제거

        // 구운 Beef로 프리팹 변경
        GameObject bakedGroundBeef = Instantiate(BakedBeef, _centralPoint.transform.position, Quaternion.identity);

        // 구운 Beef를 프라이팬 캐비넷 중심점 자식으로 설정
        bakedGroundBeef.transform.SetParent(_centralPoint);
    }
}
