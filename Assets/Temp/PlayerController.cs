using UnityEngine;

namespace LHW_Test
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Movement 테스트용으로 구현한 클래스입니다.
        /// Controller 구현하시는 분께서 Movement 호출 관련 메서드 정리 끝나시면
        /// 해당 파일은 삭제하셔도 됩니다.
        /// </summary>
        public PlayerMovement _movement;
        public PlayerStatus _status;

        private void Update()
        {
            MoveTest();

            _status.IsAming.Value = Input.GetKey(KeyCode.Mouse1);
        }

        /// <summary>
        /// 아래 메서드에 적힌 소스코드와 같은 방식으로 사용합니다.
        /// </summary>
        public void MoveTest()
        {
            // (회전 수행 후)좌우 회전에 대한 벡터 반환
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if(_status.IsAming.Value) moveSpeed = _status.WalkSpeed;
            else moveSpeed = _status.RunSpeed;

            Vector3 moveDir = _movement.SetMove(moveSpeed);
            _status.IsMoving.Value = (moveDir != Vector3.zero);

            // 몸체의 회전
            Vector3 avatarDir;
            if (_status.IsAming.Value) avatarDir = camRotateDir;
            else avatarDir = moveDir;

            _movement.SetAvatarRotation(avatarDir);
        }
    }
}
