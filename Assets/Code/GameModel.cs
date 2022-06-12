using UnityEngine;

namespace Assets.Code
{
    public class GameModel : MonoBehaviour
    {
    
        //public
        public static GameModel Instance { get; private set; }
    
        public delegate void callback();

        public static callback callbackFct;

        public delegate void spawner_callback(EnemySpawnerModel.spawner_function e);

        public static spawner_callback spwn_cb;

        public GameObject low;


        //private

        private int move_offset_ = 10;

        private float move_speed_ = 0.5f;

        private float spawn_delta_ = 0.0f;

        private int max_enemys_simultaneously = 4;

        private EnemySpawnerModel e_spawn_model;

        private CameraMovementModel c_movement_model;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            //create models
            e_spawn_model = new EnemySpawnerModel(move_offset_,this.transform.position);
            c_movement_model = new CameraMovementModel(move_offset_);

            //init callbacks
            e_spawn_model.init_callback();
            c_movement_model.init_callback();
        }

        // Update is called once per frame
        void Update()
        {
            spawn_delta_ += Time.deltaTime;

            if (spawn_delta_ > 1f)
            {
                spawn_delta_ = 0.0f;
                
                for(int index = 0; index < max_enemys_simultaneously; index++)
                    spwn_cb(EnemySpawnerModel.spawner_function.SPAWNLOW);
                e_spawn_model.reset_set();
            }
        }

        public void spawnEnemy(Vector3 spawnposition, 
            EnemySpawnerModel.spawner_function e)
        {
            GameObject go = Instantiate(low, spawnposition, Quaternion.identity);
            EnemyController ec = go.GetComponent<EnemyController>();
        
            ec.init_values(move_speed_);
            ec.set_move_pattern(EnemyController.move_pettern.DIAGONAL);
        }
    }
}
