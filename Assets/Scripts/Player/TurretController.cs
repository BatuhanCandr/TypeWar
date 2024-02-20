using DG.Tweening;
using Mirror;
using MoreMountains.Tools;
using UnityEngine;

public class TurretController : NetworkBehaviour
{
    [Header("Turret Stats")]
    [SerializeField] private int turretHealth;

    [SerializeField] private GameObject turretHead;

    public MMProgressBar mmProgressBar;
    public RectTransform healthBar;
    private GameObject bullet;
    public TowerController playerController;
    public Transform bulletSpawnPos;
    public int TurretId;

    [Header("Fx")] public ParticleSystem hitFx;

    // Client-side method to spawn bullets
    [Client]
    public void BulletSpawn()
    {
        CmdSpawnBullets();
    }

    // Command to spawn bullets on the server
    [Command]
    public void CmdSpawnBullets()
    {
        bullet = Instantiate(GameManager.Instance.networkManagerTypeWar.spawnPrefabs.Find(prefab => prefab.name == "Sphere"));
        bullet.transform.position = bulletSpawnPos.position;

        NetworkServer.Spawn(bullet);
        BulletMove();
    }

    // Client-side method to move bullets
    [Client]
    public void BulletMove()
    {
        if (bullet != null)
        {
            TurretController targetTurret = GetTargetTurret();

            if (targetTurret != null && targetTurret.turretHealth > 0)
            {
                bullet.transform.DOJump(targetTurret.transform.position, 3, 1, 1).SetEase(Ease.Linear);
                turretHead.transform.DOPunchPosition(Vector3.back, .2f, 2, 1).SetEase(Ease.OutQuad);
            }
            else
            {
                turretHead.transform.DOPunchPosition(Vector3.back, .2f, 2, 1).SetEase(Ease.OutQuad);;
                turretHead.transform.LookAt(playerController.targetFinderController.targetPlayer.transform.position);
                bullet.transform.DOJump(playerController.targetFinderController.targetPlayer.transform.position, 3, 1, 1).SetEase(Ease.Linear);
            }
        }
    }

    // Client-side method to get the target turret
    [Client]
    private TurretController GetTargetTurret()
    {
        if (playerController != null && playerController.targetFinderController.targetPlayer != null)
        {
            int targetTurretId = Mathf.Clamp(TurretId, 0, playerController.targetFinderController.targetPlayer.turretControllers.Count - 1);
            return playerController.targetFinderController.targetPlayer.turretControllers[targetTurretId];
        }

        return null;
    }

    // Method to handle taking damage
    public void TakeDamage(int _damage)
    {
        turretHealth -= _damage;
        mmProgressBar.Minus10Percent();
        if (turretHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Triggered when the turret is hit by a bullet
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
