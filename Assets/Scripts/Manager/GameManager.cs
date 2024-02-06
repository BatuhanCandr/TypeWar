using UnityEngine;


    public class GameManager : MonoBehaviour
    {
        [Header("Class References")] 
        [SerializeField] internal TypeManager typeManager;
        [SerializeField] internal NetworkManagerTypeWar networkManagerTypeWar;
        [SerializeField] internal CastleController castleController;
        public static GameManager Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void GetPlayer(CastleController _castleController)
        {
            this.castleController = _castleController;
        }
    }


