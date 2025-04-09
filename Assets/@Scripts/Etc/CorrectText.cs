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
        // 우측 상단으로 이동하는 모션
        transform.position += new Vector3(_speed * Time.deltaTime, _speed * Time.deltaTime, 0f);
    }
}
