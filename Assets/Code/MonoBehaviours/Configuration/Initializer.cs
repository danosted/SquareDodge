namespace Assets.Code.MonoBehaviours.Configuration
{
    using UnityEngine;
    using IoC;
    using GameLogic;
    using DataAccess;

    public class Initializer : MonoBehaviour
    {
        public GlobalConfiguration Config;
        private IoC Container { get; set; }
        private PrefabManager PrefabManager { get; set; }

        /// <summary>
        /// Master awake - no other awake methods should be used
        /// </summary>
        void Awake()
        {
            // Initialize IoC container
            Container = new IoC(Config);
            PrefabManager = Container.Resolve<PrefabManager>();
            
            // Initialize game...
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Initialize Game
            var control = Container.Resolve<FlowLogic>();
            control.StartGameFlow();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Container.Resolve<FlowLogic>().GameOver();
            }
            if (!PrefabManager.GetConfiguration().param_game_over) return;
            if (Input.GetMouseButtonDown(0))
            {
                Container.Resolve<FlowLogic>().RestartGame();
            }
        }
    }
}
