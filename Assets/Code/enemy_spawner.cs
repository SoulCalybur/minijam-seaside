using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerModel : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 base_position_;
    private int offset_;
    private bool[] canSpawn;
    private int top_offset_ = 1;
    private int bottom_offset_ = 1;

    public enum spawner_function
    {
        UPDATEPOSITION = 0,
        SPAWNLOW = 1,
        SPAWNMEDIUM = 2,
        SPAWNMAX = 3
    }


    public EnemySpawnerModel(int offset)
    {
        offset_ = offset;
    }

    void Start()
    {
        base_position_ = new Vector3(0, 0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawner callback
    void update(spawner_function e)
    {

        switch (e)
        {
            case spawner_function.SPAWNLOW:
                break;

            case spawner_function.SPAWNMAX:

                break;
            case spawner_function.SPAWNMEDIUM:

                break;
            case spawner_function.UPDATEPOSITION:

                base_position_ += Vector3.left * offset_;
                break;
           
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
