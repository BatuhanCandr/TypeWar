using System.Collections;
using Mirror;
using UnityEngine;

public class TargetFinderController : NetworkBehaviour
{
    public float targetingDistance = 1000000;
    
    [SyncVar(hook = nameof(OnCastleNameChanged))]
    private string castleName = "mycastle";

    [SyncVar] public TowerController targetPlayer;

    private bool waitingForPlayer = true;

    void Start()
    {
        InitializeCastleSettings();
        StartCoroutine(AutoFindTarget());
    }

    void InitializeCastleSettings()
    {
        if (castleName == "Player0")
        {
            foreach (var turretController in GameManager.Instance.playerController.turretControllers)
            {
                turretController.healthBar.localRotation = Quaternion.Euler(45, 0, 0);
            }
        }
        else if (castleName == "Player1")
        {
            GameManager.Instance.ChangeCamera();
            GameManager.Instance.typeManager._wordDisplay.rectTransform.localRotation = Quaternion.Euler(0, 0, -180);
        }

        if (isServer)
        {
            castleName = "Player" + connectionToClient.connectionId;
        }
    }

    void OnCastleNameChanged(string oldName, string newName)
    {
        gameObject.name = newName;
    }

    [Client]
    IEnumerator AutoFindTarget()
    {
        while (waitingForPlayer)
        {
            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, targetingDistance))
            {
                TowerController tower = hit.collider.GetComponent<TowerController>();

                if (tower != null && tower.gameObject != gameObject && tower.gameObject != gameObject.transform.parent)
                {
                    SetTarget(tower);
                    yield break;
                }
            }

            yield return null;
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        // Player's movement controls go here

        // Pressing the Space key triggers target finding
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFindTarget();
        }
    }

    [Command(requiresAuthority = false)]
    void CmdFindTarget()
    {
        if (!waitingForPlayer)
        {
            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, targetingDistance))
            {
                TowerController tower = hit.collider.GetComponent<TowerController>();

                if (tower != null && tower.gameObject != gameObject && tower.gameObject != gameObject.transform.parent)
                {
                    SetTarget(tower);
                }
            }
        }
    }

    [ClientRpc]
    void RpcSetTarget(TowerController target)
    {
        targetPlayer = target;
        Debug.Log("Target set: " + targetPlayer.name);
    }

    [Command(requiresAuthority = false)]
    void SetTarget(TowerController newTarget)
    {
        targetPlayer = newTarget;
        RpcSetTarget(newTarget);
    }
}
