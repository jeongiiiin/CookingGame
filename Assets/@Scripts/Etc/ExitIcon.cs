using UnityEngine;

public class ExitIcon : MonoBehaviour
{
    // private
    MeshRenderer _meshRenderer;
    float _speed = 1f;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // x축 기준으로 이미지 루프
        Vector2 newOffset = _meshRenderer.material.mainTextureOffset;
        newOffset.x += _speed * -1 * Time.deltaTime;

        _meshRenderer.material.mainTextureOffset = newOffset;
    }
}
