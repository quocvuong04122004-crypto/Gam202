using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class AttributesManager : MonoBehaviour
{


    public int health; // Máu hiện tại của nhân vật


    public int attack;// Sát thương cơ bản


    [Header("ENEMY Die")]
    public bool isDead = false;


    // =======================
    // CHỈ SỐ CHÍ MẠNG (CRITICAL)
    // =======================


    // Hệ số sát thương khi crit
    // Ví dụ: 1.5 = tăng 150% damage
    public float critDamage = 1.5f;


    // Tỉ lệ crit
    // 0.5 = 50% ra crit
    public float critChance = 0.5f;


    // =======================
    // BIẾN LIÊN QUAN HEALTH BAR
    // =======================


    // Slider đại diện cho thanh máu
    private Slider healthSlider;


    // Canvas chứa thanh máu (World Space)
    private Transform healthCanvas;


    // Camera chính để thanh máu luôn quay về màn hình
    private Camera mainCamera;


    // =======================
    // HÀM START – CHẠY 1 LẦN KHI OBJECT SINH RA
    // =======================
    void Start()
    {
        // Lấy camera chính trong scene
        mainCamera = Camera.main;


        // Nếu object này KHÔNG PHẢI Enemy → bỏ qua
        if (!CompareTag("Enemy")) return;


        // =======================
        // TÌM CANVAS THEO TÊN
        // (KHÔNG DÙNG GetChild(index) để tránh lỗi)
        // =======================
        healthCanvas = transform.Find("Canvas");


        // Nếu không tìm thấy Canvas → báo lỗi
        if (healthCanvas == null)
        {
            Debug.LogError(" Không tìm thấy Canvas trong Enemy: " + gameObject.name);
            return;
        }


        // =======================
        // TÌM HEALTH BAR (SLIDER)
        // =======================
        Transform bar = healthCanvas.Find("HealthBar");


        // Nếu không tìm thấy HealthBar → báo lỗi
        if (bar == null)
        {
            Debug.LogError("Không tìm thấy HealthBar trong Canvas: " + gameObject.name);
            return;
        }


        // Lấy component Slider từ HealthBar
        healthSlider = bar.GetComponent<Slider>();


        // Nếu HealthBar không có Slider → báo lỗi
        if (healthSlider == null)
        {
            Debug.LogError("HealthBar không có Slider component");
            return;
        }


        // Gán giá trị max cho thanh máu
        healthSlider.maxValue = health;


        // Gán giá trị ban đầu
        healthSlider.value = health;
    }


    // =======================
    // UPDATE – CHẠY MỖI FRAME
    // =======================
    void Update()
    {
        // Nếu có Canvas và Camera
        if (healthCanvas != null && mainCamera != null)
        {
            // Làm cho thanh máu LUÔN QUAY VỀ CAMERA
            // => Enemy xoay hướng nào thì máu vẫn nhìn thẳng
            healthCanvas.LookAt(
                healthCanvas.position + mainCamera.transform.forward
            );
        }
    }


    // =======================
    // HÀM NHẬN SÁT THƯƠNG
    // =======================
    public void TakeDamage(int amount)
    {
        // Trừ máu theo damage nhận vào
        health -= amount;


        // Nếu không phải Enemy thì không xử lý UI
            if (CompareTag("Enemy"))
            {
                if (healthSlider != null)
                    healthSlider.value = health;
            }




        // Nếu máu <= 0 → chết
        if (health <= 0)
        {
            if (CompareTag("Player"))
            {
                PlayerDie();
                return;
            }
            if (CompareTag("Enemy"))
            {
                EnemyDie();
                return;
            }
        }
    }


    // =======================
    // HÀM ENEMY CHẾT
    // =======================

    public GameObject gemPrefab;


    void EnemyDie()
    {
        if (isDead) return;
        isDead = true;


        Debug.Log(gameObject.name + " Dead");


        // 0. TẮT AI (RẤT QUAN TRỌNG)
        EnemyController enemyController = GetComponent<EnemyController>();
        if (enemyController != null)
            enemyController.enabled = false;


        // 1. TẮT NAVMESH AGENT
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }


        // 2. TẮT CHARACTER CONTROLLER
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;


        // 3. TẮT COLLIDER
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;


        // 4. BẬT ANIMATION CHẾT
        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.applyRootMotion = false;
            animator.SetBool("isDead", true);
        }


        // 5. SPAWN GEM
        if (gemPrefab != null)
        {
            GameObject gem = Instantiate(
                gemPrefab,
                transform.position + Vector3.up * 0.5f,
                Quaternion.identity
            );
            gem.SetActive(true);
        }


        // 6. HỦY ENEMY
        Destroy(gameObject, 2f);
    }


    // =======================
    // HÀM GÂY SÁT THƯƠNG
    // =======================
    public void DealDamage(GameObject target)
    {
        // Lấy AttributesManager của đối tượng bị đánh
        AttributesManager atm = target.GetComponent<AttributesManager>();


        // Nếu target không có AttributesManager → thoát
        if (atm == null) return;


        // Damage ban đầu = attack cơ bản
        float totalDamage = attack;


        // Random kiểm tra crit
        if (Random.Range(0f, 1f) < critChance)
        {
            // Nếu crit → nhân damage
            totalDamage *= critDamage;


            Debug.Log("Critical Hit!");
        }


        // Gây damage cho target (ép float → int)
        atm.TakeDamage((int)totalDamage);
    }
    public void PlayerDie()
    {
        Debug.Log("Player Dead");


        // Tắt controller
        var cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;


        // Tắt movement script khác
        var scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
        {
            if (s != this) s.enabled = false;
        }


        // Play animation chết
        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetTrigger("Dead");


        // Tắt NavMeshAgent an toàn
        var agent = GetComponent<NavMeshAgent>();
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }


        // Destroy object sau 3s
        Destroy(gameObject, 1f);
    }



}
