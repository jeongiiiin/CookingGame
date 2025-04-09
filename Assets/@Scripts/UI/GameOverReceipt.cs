using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverReceipt : MonoBehaviour
{
    // private
    float _speed = 800f;
    
    void Update()
    {
        // * Time.unscaledDeltaTime
        // : timescale�� 0�� �� (�Ͻ����� ��Ȳ�� ��) �����̵��� �ϱ� ���� �����
        transform.Translate(Vector3.up * _speed * Time.unscaledDeltaTime);

        StartCoroutine(CoMoveUp());
    }

    IEnumerator CoMoveUp()
    {
        yield return new WaitForSeconds(3f);
        
        Destroy(gameObject);
    }
}
