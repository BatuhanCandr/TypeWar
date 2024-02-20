using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TowerController : NetworkBehaviour
{
    [Header("Player Classes")]
    public List<TurretController> turretControllers;
    public TargetFinderController targetFinderController;

    public int towerHealth;
    private int currentTurretIndex = 0; 

    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            // Set the player controller in the GameManager
            GameManager.Instance.playerController = this;
        }
    }

    // Client-side method to initiate shooting
    [Client]
    public void Shoot()
    {
        if (turretControllers.Count > 0)
        {
            if (turretControllers[currentTurretIndex].gameObject.activeInHierarchy)
            {
                turretControllers[currentTurretIndex].BulletSpawn(); 
            }
            else
            {
                // Find the next active turret
                for (int i = 1; i < turretControllers.Count; i++)
                {
                    int nextTurretIndex = (currentTurretIndex + i) % turretControllers.Count;
                    if (turretControllers[nextTurretIndex].gameObject.activeInHierarchy)
                    {
                        // Set the index to the next active turret
                        currentTurretIndex = nextTurretIndex;
                        turretControllers[currentTurretIndex].BulletSpawn();
                        break;
                    }
                }
            }

            // Move to the next turret index, and loop back to 0 if needed
            currentTurretIndex = (currentTurretIndex + 1) % turretControllers.Count;
        }
    }
    
    // Method to handle taking damage
    public void TakeDamage(int _damage)
    {
        towerHealth -= _damage;

        if (towerHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Triggered when the tower is hit by a bullet
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            PlayHitFx(other.gameObject);
            Debug.Log("HITTTT");
            TakeDamage(other.GetComponent<BulletController>().damage);
        }
    }

    // Play hit particle effect
    public void PlayHitFx(GameObject bullets)
    {
        GameObject hitFx = Instantiate(GameManager.Instance.networkManagerTypeWar.spawnPrefabs.Find(prefab => prefab.name == "ExplosionFireballSharpFire"));
        hitFx.transform.position= bullets.transform.position;

        NetworkServer.Spawn(hitFx);
    }
}
