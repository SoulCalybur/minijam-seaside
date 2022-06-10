using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
       e_spawn_model = new EnemySpawnerModel(move_offset_);
       c_movement_model = new CameraMovementModel(move_offset_);

       //init callbacks
       e_spawn_model.init_callback();
       c_movement_model.init_callback();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void spawnEnemy(Vector3 spawnposition)
    { 
        GameObject go = Instantiate(low, spawnposition, Quaternion.identity);

        go.GetComponent<EnemyController>().
    }
    
}
