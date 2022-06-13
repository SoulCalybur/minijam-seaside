using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.SceneManagement;

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

        public GameObject sandpile;

        private List<Vector3> blocked_position_gridspace;
        //private

        private int move_offset_ = 10;

        private float move_speed_ = 0.2f;

        private float spawn_delta_ = 0.0f;

        private int max_enemys_simultaneously = 1;

        private float spawn_cooldown = 6f;

        private float tile_x = 1f;

        private float tile_y = 1f;

        private float game_time = 0.0f;

        private EnemySpawnerModel e_spawn_model;

        private CameraMovementModel c_movement_model;

        public Grid grid;

        public GameObject tilemap;

        public GameObject water;

        public GameObject camera;

        public bool change_zone = false;

       private Vector3 current_water_pos;
    
       private Vector3 next_water_pos;
      
       private Vector3 current_zone_pos;
       
       private Vector3 next_zone_pos;
       
       private Vector3 current_camera_pos;
       
       private Vector3 next_camera_pos;

       private float max_x_factor = 6;

       private float min_x_factor = 1;

       private float top_max = 3.5f;

       private float bottom_max = -0.5f;

       private int sand_piles = 0;

       private float sand_delta_ = 0;

       private float sand_cooldown = 2;

       private bool can_spawn = false;

       private int game_irgendwas = 0;

       private int more_movement = 0;

        public GameObject GameOverScreen;

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
           if(!change_zone)
           game_time += Time.deltaTime;
           
           else
           {
               game_time += Time.deltaTime;
           
               if (game_time <= 1.0f)
               {
                   water.transform.position = Vector3.Lerp(current_water_pos, next_water_pos, game_time);
               }
               else if (game_time > 1.0f && game_time <=2.1f)
               {
                   camera.transform.position = Vector3.Lerp(current_camera_pos, next_camera_pos, game_time -1);
               }
               else
               {
                   tilemap.transform.position -= Vector3.right;
                   this.transform.position -= Vector3.right;
                   change_zone = false;
               }
           }
           
           if (game_time > 30.0f)
           {
               game_time = 0.0f;

               game_irgendwas++;
               
               if (game_irgendwas == 0 || game_irgendwas == 3)
                   max_enemys_simultaneously++;
               else if (game_irgendwas == 1 || game_irgendwas == 4)
                   more_movement++;
               else if((game_irgendwas % 2) == 1)
               {
                   spawn_cooldown -= 0.15f;
               }
               else
               {
                   move_speed_ += 0.05f;
               }
           
               current_water_pos = water.transform.position;
               next_water_pos = current_water_pos;
               next_water_pos.x -= 1;

               current_camera_pos = camera.transform.position;
               next_camera_pos = current_camera_pos;
               next_camera_pos.x -= 1;
               
               change_zone = true;
           }

            spawn_delta_ += Time.deltaTime;

            if (spawn_delta_ > spawn_cooldown)
            {
                spawn_delta_ = 0.0f;
                
                for(int index = 0; index < max_enemys_simultaneously; index++)
                    spwn_cb(EnemySpawnerModel.spawner_function.SPAWNLOW);
                e_spawn_model.reset_set();
            }

            sand_delta_ += Time.deltaTime;

            if (sand_delta_ >= sand_cooldown)
            {
                sand_delta_ = 0.0f;
                if (sand_piles < 3)
                {
                    Vector3 sandpos = new Vector3();

                    for(int tryit = 0; tryit < 200; tryit++)
                    {
                        sandpos.x = this.transform.position.x - ((int) (Random.value * 5) + 1) * tile_x;
                        sandpos.y = this.transform.position.y + ((int) (Random.value * 5));

                        

                        can_spawn = true;

                        for (int index = 0; index < blocked_position_gridspace.Count; index++)
                        {
                            if ((Vector3.Distance(sandpos, blocked_position_gridspace[index]) < 0.05f))
                            {
                                can_spawn = false;
                                break;
                            }
                        }
                        
                        if (blocked_position_gridspace.Count == 0 || can_spawn)
                        {
                            blocked_position_gridspace.Add(sandpos);
                            Instantiate(sandpile, sandpos, Quaternion.identity);
                            sand_piles++;
                            break;
                        }
                    }
                }
            }
        }

        public void spawn_enemy(Vector3 spawnposition, 
            EnemySpawnerModel.spawner_function e)
        {
            GameObject go = Instantiate(low, spawnposition, Quaternion.identity);
            EnemyController ec = go.GetComponent<EnemyController>();
        
            ec.init_values(move_speed_);

            if(more_movement == 0)
                ec.set_move_pattern(EnemyController.move_pettern.STRAIGHT);
            if (more_movement == 1)
            {
                if(Random.value > 0.5f)
                    ec.set_move_pattern(EnemyController.move_pettern.STRAIGHT);
                else
                    ec.set_move_pattern(EnemyController.move_pettern.DIAGONAL);
            }
            if (more_movement == 2)
            {
                float r = Random.value;
                if (r >= 0.25f && r <= 0.5f)
                    ec.set_move_pattern(EnemyController.move_pettern.STRAIGHT);
                else if(r > 0.5f)
                    ec.set_move_pattern(EnemyController.move_pettern.DIAGONAL);
                else
                    ec.set_move_pattern(EnemyController.move_pettern.JUMPY);
            }
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

        public void dec_sand_pile_counter()
        {
            if(sand_piles > 0)
                sand_piles--;
        }

        public void showGameOver() {
            if(GameOverScreen.gameObject.activeSelf == false) {
                GameOverScreen.gameObject.SetActive(true);
            }
        }
    }
}
