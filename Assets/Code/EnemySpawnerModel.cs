using UnityEngine;
using System.Collections.Generic;

namespace Assets.Code
{
    public class EnemySpawnerModel
    {
        // Start is called before the first frame update

        private Vector3 base_position_;
       
        private SortedSet<int> av_positions;

        private int last_spawn_position;
        //used for offsetcalc
        private int lane_count = 5;
        private float tile_size = 0.5f;
        
        public enum spawner_function
        {
            UPDATEPOSITION = 0,
            SPAWNLOW = 1,
            SPAWNMEDIUM = 2,
            SPAWNMAX = 3
        }


        public EnemySpawnerModel(float offset,Vector3 base_position)
        {
            tile_size = offset;
            base_position_ = base_position;
            av_positions = new SortedSet<int>();
        }
        //spawner callback
        void update(spawner_function e)
        {
            if(spawner_function.UPDATEPOSITION == e)
                base_position_ += Vector3.left * tile_size;
            else 
            {
                //randomize startposition on y axis
                int lane;
                while (true)
                {
                    lane =(int) (Random.value * lane_count);
                    if (!av_positions.Contains(lane))
                        break;
                }

                av_positions.Add(lane);

                Vector3 spawnpos = base_position_ + 
                                   Vector3.up * (tile_size * lane);

                if (GameModel.Instance != null)
                    GameModel.Instance.spawn_enemy(spawnpos, e);
            }
        }
        public void init_callback()
        {
            GameModel.spwn_cb += update;
        }

        public void remove_callback()
        {
            GameModel.spwn_cb -= update;
        }
        public void reset_set()
        {
            av_positions.Clear();
        }
    }
}
