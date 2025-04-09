using UnityEngine;

public class TrashCan : MonoBehaviour
{
    // private
    Animator _animator;
    Transform Player;
    float _openDistance = 2.5f;

    // property
    public bool IsOpen
    {
        get { return _animator.GetBool(Define.IsOpen); }
        set { _animator.SetBool(Define.IsOpen, value); }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag(Define.Player).transform;
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.position);

        // 플레이어가 가까이 오면
        if (distance <= _openDistance)
        {
            // 쓰레기통 열리도록
            _animator.SetBool(Define.IsOpen, true);
        }
        else
        {
            // 쓰레기통 닫히도록
            _animator.SetBool(Define.IsOpen, false);
        }

        GameManager.Instance.UpdateIngredientIcons();
    }

    private void OnTriggerStay(Collider other)
    {
        // 재료가 충돌하고 있고, 스페이스바를 눌렀다면
        if (other.transform.CompareTag(Define.Ingredient) && Input.GetKey(KeyCode.Space))
        {
            // 재료 제거
            Destroy(other.gameObject);

            PlayManager.Instance.HeldIngredient = null;
            PlayManager.Instance.IsHoldingIngredient = false;
            PlayManager.Instance.CanPickUp = true;
        }
    }
}
