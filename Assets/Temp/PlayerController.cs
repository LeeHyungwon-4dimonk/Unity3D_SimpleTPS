using UnityEngine;

namespace LHW_Test
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Movement �׽�Ʈ������ ������ Ŭ�����Դϴ�.
        /// Controller �����Ͻô� �в��� Movement ȣ�� ���� �޼��� ���� �����ø�
        /// �ش� ������ �����ϼŵ� �˴ϴ�.
        /// </summary>
        public PlayerMovement _movement;
        public PlayerStatus _status;

        private void Update()
        {
            MoveTest();

            _status.IsAming.Value = Input.GetKey(KeyCode.Mouse1);
        }

        /// <summary>
        /// �Ʒ� �޼��忡 ���� �ҽ��ڵ�� ���� ������� ����մϴ�.
        /// </summary>
        public void MoveTest()
        {
            // (ȸ�� ���� ��)�¿� ȸ���� ���� ���� ��ȯ
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if(_status.IsAming.Value) moveSpeed = _status.WalkSpeed;
            else moveSpeed = _status.RunSpeed;

            Vector3 moveDir = _movement.SetMove(moveSpeed);
            _status.IsMoving.Value = (moveDir != Vector3.zero);

            // ��ü�� ȸ��
            Vector3 avatarDir;
            if (_status.IsAming.Value) avatarDir = camRotateDir;
            else avatarDir = moveDir;

            _movement.SetAvatarRotation(avatarDir);
        }
    }
}
