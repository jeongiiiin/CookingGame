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

        // �÷��̾ ������ ����
        if (distance <= _openDistance)
        {
            // �������� ��������
            _animator.SetBool(Define.IsOpen, true);
        }
        else
        {
            // �������� ��������
            _animator.SetBool(Define.IsOpen, false);
        }

        GameManager.Instance.UpdateIngredientIcons();
    }

    private void OnTriggerStay(Collider other)
    {
        // ��ᰡ �浹�ϰ� �ְ�, �����̽��ٸ� �����ٸ�
        if (other.transform.CompareTag(Define.Ingredient) && Input.GetKey(KeyCode.Space))
        {
            // ��� ����
            Destroy(other.gameObject);

            PlayManager.Instance.HeldIngredient = null;
            PlayManager.Instance.IsHoldingIngredient = false;
            PlayManager.Instance.CanPickUp = true;
        }
    }
}
