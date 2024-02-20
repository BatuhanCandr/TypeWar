using Mirror;
using Mirror.Examples.MultipleAdditiveScenes;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : NetworkBehaviour
{
    [Header("Class References")] 
    [SerializeField] internal TypeManager typeManager;
    [SerializeField] internal NetworkManagerTypeWar networkManagerTypeWar;
    [SerializeField] internal TowerController playerController;
    public Transform cameraTransform;
    public Vector3 cameraPos;
    public Vector3 cameraRot;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern: Ensure there is only one instance of GameManager
        Instance = this;
    }
    
    // Method to change the camera position and rotation
    public void ChangeCamera()
    {
        // Set the camera's position and rotation based on predefined values
        cameraTransform.position = cameraPos;
        cameraTransform.eulerAngles = cameraRot;
    }
}