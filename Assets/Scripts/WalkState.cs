using UnityEngine;
using UnityEngine.AI;


public class WalkState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int currentIndex;
    private Transform player;


    public float chaseRange = 10f;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Lấy NavMeshAgent từ parent, kiểm tra null
        if (animator.transform.parent != null)
        {
            agent = animator.transform.parent.GetComponent<NavMeshAgent>();
            if (agent == null)
                Debug.LogWarning("NavMeshAgent not found on parent!");
        }
        else
        {
            Debug.LogWarning("Animator has no parent!");
        }


        // Tìm Player, nếu không tìm thấy thì gán null
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            player = null;
            Debug.LogWarning("Player not found in scene!");
        }


        // Lấy WayPoints, kiểm tra tồn tại
        GameObject wpParent = GameObject.FindGameObjectWithTag("WayPoints");
        if (wpParent != null && wpParent.transform.childCount > 0)
        {
            waypoints = new Transform[wpParent.transform.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = wpParent.transform.GetChild(i);
            }
            // Chọn waypoint ngẫu nhiên ban đầu
            currentIndex = Random.Range(0, waypoints.Length);
            if (agent != null)
                agent.SetDestination(waypoints[currentIndex].position);
        }
        else
        {
            Debug.LogWarning("WayPoints parent not found or has no children!");
            waypoints = new Transform[0];
        }
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Kiểm tra agent null
        if (agent == null) return;


        // Kiểm tra player null, nếu null thì Enemy vẫn tuần tra waypoint
        bool canChase = player != null && player;


        if (canChase)
        {
            float distance = Vector3.Distance(player.position, animator.transform.position);


            if (distance < chaseRange)
            {
                animator.SetBool("isChasing", true);
                return; // Nếu chase Player, dừng tuần tra waypoint
            }
        }


        animator.SetBool("isChasing", false);


        // Tuần tra waypoint
        if (waypoints.Length > 0 && agent.remainingDistance < 0.5f)
        {
            currentIndex = Random.Range(0, waypoints.Length);
            agent.SetDestination(waypoints[currentIndex].position);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset flag
        animator.SetBool("isChasing", false);
    }
}
