using UnityEngine;
using UnityEngine.AI;


public class RunState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;


    public float stopChaseDistance = 15f;
    public float attackDistance = 2.5f;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Lấy NavMeshAgent từ parent (Enemy)
        agent = animator.transform.parent.GetComponent<NavMeshAgent>();


        // Tìm Player, nếu không tìm thấy thì player = null
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


        agent.speed = 5f;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || player == null)
            return;


        //  CHỐNG LỖI SETDESTINATION
        if (!agent.isOnNavMesh)
            return;


        float distance = Vector3.Distance(
            animator.transform.root.position,
            player.position
        );


        agent.SetDestination(player.position);


        bool chase = distance <= stopChaseDistance;
        bool attack = distance <= attackDistance;


        if (animator.GetBool("isChasing") != chase)
            animator.SetBool("isChasing", chase);


        if (animator.GetBool("isAttacking") != attack)
            animator.SetBool("isAttacking", attack);
    }
}
