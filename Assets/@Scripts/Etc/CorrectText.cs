using UnityEngine;

public class CorrectText : MonoBehaviour
{
    // private
    float _speed = 2f;

    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    void Update()
    {
        // ���� ������� �̵��ϴ� ���
        transform.position += new Vector3(_speed * Time.deltaTime, _speed * Time.deltaTime, 0f);
    }
}
