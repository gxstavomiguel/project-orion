using UnityEngine;
using System.Linq;

public class TargetLockSystem : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private float lockRange = 15f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int lockMouseButton = 2;

    public Transform CurrentTarget { get; private set; }
    public bool IsLocked => CurrentTarget != null; // NOVO: outros scripts consultam isso

    private EnemyHealthBarUI currentTargetBar;

    private void Update()
    {
        if (Input.GetMouseButtonDown(lockMouseButton))
            ToggleLock();
    }

    public void ToggleLock()
    {
        if (CurrentTarget != null) { Unlock(); return; }

        Transform closest = FindClosestEnemy();
        if (closest != null) Lock(closest);
    }

    private void Lock(Transform target)
    {
        CurrentTarget = target;
        currentTargetBar = target.GetComponentInChildren<EnemyHealthBarUI>();
        currentTargetBar?.SetVisible(true);
    }

    private void Unlock()
    {
        currentTargetBar?.SetVisible(false);
        CurrentTarget = null;
        currentTargetBar = null;
    }

    private Transform FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lockRange, enemyLayer);
        return hits.Select(h => h.transform)
                    .OrderBy(t => Vector3.Distance(transform.position, t.position))
                    .FirstOrDefault();
    }

    private void Update2CheckTargetGone() { } // (não usar, apenas ilustrativo)

    private void LateUpdate()
    {
        if (CurrentTarget == null) return;

        bool targetGone = Vector3.Distance(transform.position, CurrentTarget.position) > lockRange * 1.5f;
        if (targetGone) Unlock();

        // ROTAÇÃO FOI REMOVIDA DAQUI — agora é o PlayerMovement que gira o player,
        // via Rigidbody, pra não conflitar com a física.
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lockRange);
    }
}