using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    DiceAttackSettings diceSettings;
    float remainingLifetime;
    PowerupSettings[] equippedPowerups;

    ObjectPool dicePool; // To return itself to the pool 

    Rigidbody rb;

    struct DiceFace
    {
        public int numberRolled; // number of pips / dots on that face
        public Vector3 normal;

        public DiceFace(int numberRolled, Vector3 normal)
        {
            this.numberRolled = numberRolled;
            this.normal = normal;
        }
    }

    DiceFace[] diceFaces;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        diceFaces = new DiceFace[6];
        diceFaces[0] = new DiceFace(1, Vector3.up);
        diceFaces[1] = new DiceFace(2, Vector3.right);
        diceFaces[2] = new DiceFace(3, Vector3.back);
        diceFaces[3] = new DiceFace(4, Vector3.forward);
        diceFaces[4] = new DiceFace(5, Vector3.left);
        diceFaces[5] = new DiceFace(6, Vector3.down);
    }

    // Reset the die
    private void OnDisable()
    {
        remainingLifetime = -1f;

        // Reset rigidbody

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Init(DiceAttackSettings _diceSettings, PowerupSettings[] _equippedPowerups, ObjectPool _dicePool, Vector3 throwDirection)
    {
        diceSettings = _diceSettings;
        equippedPowerups = _equippedPowerups;

        remainingLifetime = diceSettings.Lifetime;

        dicePool = _dicePool;
        
        rb.AddForce(throwDirection * diceSettings.Speed, ForceMode.VelocityChange);
        rb.angularVelocity = Random.onUnitSphere * Random.Range(diceSettings.SpinSpeedRange.x, diceSettings.SpinSpeedRange.y);
    }

    private void Update()
    {
        remainingLifetime -= Time.deltaTime;

        rb.angularVelocity -= rb.angularVelocity * diceSettings.SpinSlowDown * Time.deltaTime;

        if (remainingLifetime < 0f)
        {
            dicePool.Release(this.gameObject);
        }
    }

    // TODO (GnoxNahte): Change to comparing the up vector 
    private int GetRolledNumber()
    {
        Vector3 diceUp = transform.up;

        int numRolled = 0;
        float numRolledDotProduct =  -2f; // Lowest dot product should be -1

        foreach (DiceFace diceFace in diceFaces)
        {
            float currDotProduct = Vector3.Dot(diceUp, diceFace.normal);
            if (currDotProduct > numRolledDotProduct)
            {
                numRolled = diceFace.numberRolled;
                numRolledDotProduct = currDotProduct;
            }
        }
        
        // Shouldn't happen, just checking
        if (numRolled == 0)
        {
            Debug.LogError("Dice number rolled == 0. Should be between 1-6");
            numRolled = 1;
        }

        return numRolled;
    }

    private void SpawnPowerup(int numberRolled)
    {
        PowerupSettings powerupSetting = equippedPowerups[numberRolled - 1];

        if (powerupSetting == null || !powerupSetting.ifEnabled)
            return;

        Vector3 spawnPos = transform.position;
        spawnPos.y = 0.01f;

        // TODO (GnoxNahte): Replace with pool
        GameObject powerupObj = GameObject.Instantiate(powerupSetting.powerupPrefab, spawnPos, Quaternion.identity);
        PowerupGameObject powerup = powerupObj.GetComponent<PowerupGameObject>();
        powerup.Init(powerupSetting);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            RouletteEnemy script = other.GetComponent<RouletteEnemy>();
            if (script != null && script.summoning)
            {
                rb.velocity = new Vector3();
                Vector3 dir = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
                if (dir.x < 1) dir.x = 1;
                if (dir.z < 1) dir.z = 1;
                dir.Normalize();
                rb.AddForce(dir * diceSettings.Speed, ForceMode.VelocityChange);
            }
            else
            {
                enemy.TakeDamage(diceSettings.AttackDamge);

                int chosenSide = GetRolledNumber();
                SpawnPowerup(chosenSide);

                dicePool.Release(this.gameObject);
            }
        }
    }
}
