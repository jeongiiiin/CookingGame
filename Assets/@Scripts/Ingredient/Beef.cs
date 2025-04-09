using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Beef : MonoBehaviour
{
    // private
    GameObject _bakeCabinet; // �������� ĳ���
    Transform _centralPoint; // ĳ��� �߽���
    Slider _bakeTimer; // ���� �ð� Ÿ�̸�
    float _pickUpDistance = 2.5f; // ��ȣ�ۿ� ���� �Ÿ�
    float _timer = 2f; // ���� �ð�
    bool _canPickUp = true; // ��� ���� ����

    // public
    public GameObject BakedBeef; // ������ ��� (���� �ð� �� ������ ��ü ����)
    public GameObject BakeEffect; // ���� �� ����Ʈ
    public AudioSource BakeSound;

    void Start()
    {
        _bakeCabinet = GameObject.FindGameObjectWithTag(Define.BakeCabinet);
        _centralPoint = _bakeCabinet.transform.Find(Define.CenterPoint);
        _bakeTimer = GameObject.Find("BakeTimer")?.GetComponent<Slider>();
    }

    void Update()
    {
        // Beef�� �������� ĳ��� ��ġ Ȯ��
        float distance = Vector3.Distance(transform.position, _bakeCabinet.transform.position);

        // ������ (��ȣ�ۿ� ���� �Ÿ� �ȿ� ���԰�)
        if (distance <= _pickUpDistance)
        {
            // �����̽��ٸ� ������, �÷��̾ ��Ḧ ��� �ְ�, ��ȣ�ۿ��� �����ϴٸ�
            if (Input.GetKeyDown(KeyCode.Space) && PlayManager.Instance.IsHoldingIngredient && _canPickUp)
            {
                // ĳ��� �߽����� ���� �ƴϰ�, ĳ��� �ڽ��� ���ٸ�
                if (_centralPoint != null && _centralPoint.childCount == 0)
                {
                    // Beef ��������
                    DropBeef();
                    // Beef ����
                    ChangeBeef();
                }

                _canPickUp = true;
            }
        }
    }

    // Beef �������� �Լ�
    void DropBeef()
    {
        // ���������� ���� ����Ʈ �߻�
        Instantiate(BakeEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

        // ĳ��� �߽��� �ڽ����� ���� (Beef ��ġ, ���� ĳ��� �߽������� ����)
        transform.SetParent(_centralPoint);
        transform.position = _centralPoint.position;
        transform.rotation = _centralPoint.rotation;

        // �÷��̾ ��Ḧ ���� ���� ���·� ����
        PlayManager.Instance.HeldIngredient = null;
        PlayManager.Instance.IsHoldingIngredient = false;
    }

    // Beef ���� �Լ�
    void ChangeBeef()
    {
        // ���� �߿��� �� �� ������ ������ üũ
        _canPickUp = false;

        StartCoroutine(CoChangeBeef());
    }

    // ���� Beef�� ������ ����
    IEnumerator CoChangeBeef()
    {
        BakeSound.PlayOneShot(BakeSound.clip);

        // Ÿ�̸� ó��
        float timer = _timer; // ���� �ð�
        _bakeTimer.value = 1f;
        float elapsedTime = 0f; // ���� �ð�

        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime; // �� �����Ӹ��� ����
            _bakeTimer.value = 1 - (elapsedTime / timer); // ���� ����ؼ� �����̴� ����
            yield return null;
        }

        Destroy(gameObject); // �ð� ���� �� ���� Beef ����

        // ���� Beef�� ������ ����
        GameObject bakedGroundBeef = Instantiate(BakedBeef, _centralPoint.transform.position, Quaternion.identity);

        // ���� Beef�� �������� ĳ��� �߽��� �ڽ����� ����
        bakedGroundBeef.transform.SetParent(_centralPoint);
    }
}
