using UnityEngine;

namespace Assets.Code
{
    public class EnemySpawnerModel
    {
        // Start is called before the first frame update

        private Vector3 base_position_;
        private int offset_;
        private bool[] canSpawn;

        private int last_spawn_position;
        //used for offsetcalc
        private int lane_count = 10;
        private float tile_size = 0.5f;

        public enum spawner_function
        {
            UPDATEPOSITION = 0,
            SPAWNLOW = 1,
            SPAWNMEDIUM = 2,
            SPAWNMAX = 3
        }


        public EnemySpawnerModel(int offset,Vector3 base_position)
        {
            offset_ = offset;
            base_position_ = base_position;
        }
        //spawner callback
        void update(spawner_function e)
        {
            if(spawner_function.UPDATEPOSITION == e)
                base_position_ += Vector3.left * offset_;
            else 
            {
                //randomize startposition on y axis

                
                int lane =(int) (Random.value * lane_count);

                

                Vector3 spawnpos = base_position_ + 
                                   Vector3.up * (tile_size * lane);

                if (GameModel.Instance != null)
                    GameModel.Instance.spawnEnemy(spawnpos, e);
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

    }
}
