using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class NetworkManagerTypeWar : NetworkManager
{
    [Header("Spawn Pos")]
    public Transform leftRacketSpawn;
    public Transform rightRacketSpawn;

    [Header("Player")]
    public TurretController turretController;

    public GameObject object1;
    public GameObject object2;

    // Method to start the server
    public void StartServer()
    {
        StartHost();
        object1.SetActive(false);
        object2.SetActive(false);
    }

    // Method to connect to the server
    public void ConnectToServer()
    {
        networkAddress = "192.168.1.126";
        StartClient();
        object2.SetActive(false);
        object1.SetActive(false);
    }

    // Override the method to add a player on the server
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Add the player at the correct spawn position
        Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;

        // Check if the connection ID is 1
        if (conn.connectionId == 1)
        {
            // If so, rotate the player object 180 degrees around the y-axis
            start.rotation *= Quaternion.Euler(0f, 180f, 0f);
        }

        // Instantiate the player object and add it to the server
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);

        // Get the TurretController component from the player
        turretController = player.GetComponent<TurretController>();
    }
}