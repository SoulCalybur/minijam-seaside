using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Assets.Code
{
    public class GameModel : MonoBehaviour
    {
        //public
        public static GameModel Instance { get; private set; }

        public Transform offset_pos;
    
        public delegate void callback();

        public static callback callbackFct;

        public delegate void spawner_callback(EnemySpawnerModel.spawner_function e);

        public static spawner_callback spwn_cb;

        public GameObject low;

        private List<Vector3> blocked_position_gridspace;
        //private

        private int move_offset_ = 10;

        private float move_speed_ = 0.5f;

        private float spawn_delta_ = 0.0f;

        private int max_enemys_simultaneously = 5;

        private float spawn_cooldown = 5f;

        private float tile_x = 1f;

        private float tile_y = 1f;

        private EnemySpawnerModel e_spawn_model;

        private CameraMovementModel c_movement_model;

        public Grid grid;

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

            tile_x = grid.cellSize.x;
            tile_y = grid.cellSize.y;

            e_spawn_model = new EnemySpawnerModel(tile_y, this.transform.position);
            c_movement_model = new CameraMovementModel(move_offset_);

            //init callbacks
            e_spawn_model.init_callback();
            c_movement_model.init_callback();
            blocked_position_gridspace = new List<Vector3>();
        }

        // Update is called once per frame
        void Update()
        {
            spawn_delta_ += Time.deltaTime;

            if (spawn_delta_ > spawn_cooldown)
            {
                spawn_delta_ = 0.0f;
                
                for(int index = 0; index < max_enemys_simultaneously; index++)
                    spwn_cb(EnemySpawnerModel.spawner_function.SPAWNLOW);
                e_spawn_model.reset_set();
            }
        }

        public void spawn_enemy(Vector3 spawnposition, 
            EnemySpawnerModel.spawner_function e)
        {
            GameObject go = Instantiate(low, spawnposition, Quaternion.identity);
            EnemyController ec = go.GetComponent<EnemyController>();
        
            ec.init_values(move_speed_);
            ec.set_move_pattern(EnemyController.move_pettern.DIAGONAL);
        }

        public void spawn_grid_object(Vector3 spawnposition,GameObject prefab)
        {
            for (int index = 0; index < blocked_position_gridspace.Count; index++)
                if ((Vector3.Distance(spawnposition, blocked_position_gridspace[index]) < 0.05f))
                {
                    return;
                }
            
            blocked_position_gridspace.Add(spawnposition);
            Instantiate(prefab, spawnposition, Quaternion.identity);
        }

        public void remove_object_from_grid(Vector3 position)
        {
            if(blocked_position_gridspace.Contains(position))
            blocked_position_gridspace.Remove(position);
        }
    }
}
