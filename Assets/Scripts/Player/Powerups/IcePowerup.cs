using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePowerup : MonoBehaviour
{
    public float radius;

    public LayerMask layerMask;

    public int numOfEnemiesToTarget;

    public GameObject iceProjectile;

    public PowerupSettings powerupSettings;

    [System.Serializable]
    // TODO(GnoxNahte): Refactor / rename this
    class EnemyPairs
    {
        public GameObject gameObject;
        public Vector3 pos;
        public float dist;

        public EnemyPairs(GameObject gameObject, Vector3 pos, float dist)
        {
            this.pos = pos;
            this.gameObject = gameObject;
            this.dist = dist;
        }
    }

    EnemyPairs[] targetedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        targetedEnemies = new EnemyPairs[numOfEnemiesToTarget];

        List<Collider> colliders = Camera.main.transform.GetComponentInChildren<StoreColliders>().colliders;

        for (int i = 0; i < colliders.Count; ++i)
        {
            if (colliders[i] == null || colliders[i].GetComponent<Enemy>() == null)
                continue;

            float dist = (colliders[i].transform.position - transform.position).sqrMagnitude;
            for (int j = 0; j < targetedEnemies.Length; j++)
            {
                if (targetedEnemies[j] == null)
                {
                    targetedEnemies[j] = new EnemyPairs(colliders[i].gameObject, colliders[i].transform.position, dist);
                    break;
                }
                else
                {
                    if (dist < targetedEnemies[j].dist)
                    {
                        targetedEnemies[j].dist = dist;
                        targetedEnemies[j].gameObject = colliders[i].gameObject;
                    }
                }
            }
        }

        StartCoroutine(SpawnProjectiles());
    }

    IEnumerator SpawnProjectiles()
    {
        for (int i = 0; i < numOfEnemiesToTarget; i++)
        {
            if (targetedEnemies[i] == null)
            {
                if (targetedEnemies[0] != null)
                {
                    GameObject projectileObj2 = Instantiate(iceProjectile, transform.position, Quaternion.identity);
                    projectileObj2.GetComponent<IceProjectile>().Init(
                        powerupSettings.damage * powerupSettings.floatMultiplier,
                        targetedEnemies[0].pos - transform.position,
                        powerupSettings.knockbackForce);
                }

                continue;
            }

            GameObject projectileObj = Instantiate(iceProjectile, transform.position, Quaternion.identity);
            projectileObj.GetComponent<IceProjectile>().Init(
                powerupSettings.damage * powerupSettings.floatMultiplier,
                targetedEnemies[i].pos - transform.position,
                powerupSettings.knockbackForce);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
