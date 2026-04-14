using UnityEngine;


public class AttackState : StateMachineBehaviour
{
    private Transform player;
    public float attackDistance = 2.5f;


    // Khi trạng thái bắt đầu
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Tìm Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            player = null;
            Debug.LogWarning("Player not found in the scene!");
        }
    }


    // Cập nhật mỗi frame
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Kiểm tra player đã bị destroy chưa
        if (player == null) return;       // biến null
        if (!player) return;              // Unity Object bị Destroy (MissingReference)


        // Tính khoảng cách
        float distance = Vector3.Distance(animator.transform.position, player.position);


        // Nếu Player ở xa, dừng tấn công
        if (distance > attackDistance)
        {
            animator.SetBool("isAttacking", false);
        }
        else
        {
            // Nếu gần, bật tấn công
            animator.SetBool("isAttacking", true);
        }
    }


    // Nếu trạng thái kết thúc
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset trigger/flag tấn công để tránh lỗi animation
        animator.SetBool("isAttacking", false);
    }
}
