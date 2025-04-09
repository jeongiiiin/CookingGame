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
        // : timescale이 0일 때 (일시정지 상황일 때) 움직이도록 하기 위해 사용함
        transform.Translate(Vector3.up * _speed * Time.unscaledDeltaTime);

        StartCoroutine(CoMoveUp());
    }

    IEnumerator CoMoveUp()
    {
        yield return new WaitForSeconds(3f);
        
        Destroy(gameObject);
    }
}
