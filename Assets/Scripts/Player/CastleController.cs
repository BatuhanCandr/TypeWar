using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

    public class CastleController : NetworkBehaviour
    {
        
        GameObject bullet;
        
        [Client]
        void Start()
        {
           
        }

       
        void Update()
        {
        }
        [Command]
        public void SpawnBullets()
        {
            if (authority)
            {
                bullet = Instantiate(GameManager.Instance.networkManagerTypeWar.spawnPrefabs.Find(prefab => prefab.name == "Sphere"));
                NetworkServer.Spawn(bullet);
            }
            
        }
     
    }


