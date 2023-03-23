using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    
    public float EnemyMaxHealth;
    public float EnemyHealth;
    public float EnemyArmor;

    public float EnemyDamage;
    public float EnemyAttackRate;
    public float EnemyRange;

    public float ExpToGive;

    public Transform hpBar;
    public GameObject hpBarHolder;

[Header("Spawn")]
    public int StartingWave;
    [Range(0, 100)] public float ChanceToSpawn;
    public double _weight;

    public GameObject Prefab;

[Header("Navigation")]
    public Transform target;
    NavMeshAgent agent;
    public LayerMask TargetLayers;
    List<float> distances = new List<float>();

    private void Start() {
        agent = GetComponent<NavMeshAgent>();

        EnemyHealth = EnemyMaxHealth;
        StartCoroutine("FindTarget");
    }

    public void SubtractHealth(float amount) {

        if(EnemyHealth - amount > 0) {
            EnemyHealth -= amount;
        } else {
            EnemyHealth = 0;
            Death();
        }
    
        if(!hpBarHolder.activeInHierarchy)
            hpBarHolder.SetActive(true);
        hpBar.localScale = new Vector3((EnemyHealth / EnemyMaxHealth) , hpBar.localScale.y, hpBar.localScale.z);
    }

    void Death() {
        Destroy(gameObject);
    }

    IEnumerator FindTarget() {

        if(target == null) {

            Collider[] targets = Physics.OverlapSphere(transform.position, 100, TargetLayers);

            print(targets.Length);
            
            for (int i = 0; i < targets.Length; i++) {
                float dist = Vector3.Distance(transform.position, targets[i].transform.position);
                distances.Add(dist);
            }

            var min = Mathf.Infinity;

            for (int i = 0; i < distances.Count; i++) {

                if(distances[i] < min){
                    min = distances[i];
                    target = targets[i].transform;
                }
            }

            yield return new WaitForSeconds(.6f);
            StartCoroutine("FindTarget");
        } else {

            StartCoroutine("Attack");
            yield return null;
        }

    }

    IEnumerator Attack() {

        if(target != null) {

            GoToTarget();
            float dist = Vector3.Distance(transform.position, target.position);
            if(dist < EnemyRange) {
                target.GetComponent<FolkBase>().SubtractHealth(EnemyDamage);
                print("Attacking " + target.name);
            }

            yield return new WaitForSeconds(EnemyAttackRate);
            StartCoroutine("Attack"); 
        } else {
            StartCoroutine("FindTarget");
            yield return null;
        }
    }
    
    public void GoToTarget() {

        agent.SetDestination(target.position);
    }

}
