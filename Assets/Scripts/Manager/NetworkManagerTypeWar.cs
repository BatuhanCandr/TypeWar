using System;
using Mirror;
using UnityEngine;



    // Custom NetworkManager that simply assigns the correct racket positions when
    // spawning players. The built in RoundRobin spawn method wouldn't work after
    // someone reconnects (both players would be on the same side).
    [AddComponentMenu("")]
    public class NetworkManagerTypeWar : NetworkManager
    {
        public Transform leftRacketSpawn;
        public Transform rightRacketSpawn;
        private GameObject player;
        GameObject bullet;

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // add player at correct spawn position
            Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
             player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);
          GameManager.Instance.GetPlayer(player.GetComponent<CastleController>());
        }
        


        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            // destroy ball
            if (bullet != null)
                NetworkServer.Destroy(bullet);

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }
    }

