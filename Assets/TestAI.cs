using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    public float distance = 2.0f; // D��manlar�n di�er d��manlara olan en yak�n mesafesi
    public float attackDistance = 5.0f; // D��manlar�n oyuncuya veya di�er hedeflere sald�rmaya ba�layacak mesafesi

    private NavMeshAgent agent;
    private Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindNearestTarget();
    }

    void Update()
    {
        // Hedefe do�ru hareket et
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    Transform FindNearestTarget()
    {
        // T�m d��manlar� bul
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy_AI");

        Transform nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        // En yak�n hedefi bul
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < nearestDistance && distanceToEnemy > distance)
            {
                nearestDistance = distanceToEnemy;
                nearestTarget = enemy.transform;
            }
        }

        // En yak�n hedef oyuncu mu yoksa ba�ka bir d��man m�?
        if (nearestTarget == null || nearestDistance > attackDistance)
        {
            nearestTarget = FindPlayer();
        }

        return nearestTarget;
    }

    Transform FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            return player.transform;
        }

        return null;
    }

    void OnCollisionEnter(Collision collision)
    {
        // E�er �arp��ma di�er bir d��manla olduysa, dur
        if (collision.gameObject.tag == "Enemy_AI")
        {
            agent.isStopped = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // E�er di�er d��mandan uzakla��yorsa, hareketine devam et
        if (collision.gameObject.tag == "Enemy_AI")
        {
            agent.isStopped = false;
        }
    }
}
