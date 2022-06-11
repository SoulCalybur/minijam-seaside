using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum move_pettern
    {
        UNDEFINED = 0,
        STRAIGHT = 1,
        DIAGONAL = 2,
        JUMPY = 3
    }

    public enum attack_type
    {
        UNDEFINED = 0,
        RANGE = 1,
        MELEE = 2,
    }

    private float speed_;

    private float delta_;

    private float range_cd;

    private Vector3 start_pos;

    private Vector3 next_pos;

    private move_pettern move_behavior = move_pettern.UNDEFINED;

    private attack_type attack_behavior = attack_type.UNDEFINED;
    // Start is called before the first frame update
    void Start()
    {
        delta_ = 0;
        range_cd = 0;
        start_pos = new Vector3();
        change_target_to_move();
    }

    // Update is called once per frame
    void Update()
    {
        process_move_behavior();
    }

    public void set_move_pattern(move_pettern e)
    {
        move_behavior = e;
    }

    public void set_attack_type(attack_type e)
    {
        attack_behavior = e;
    }

    public void init_values(float speed)
    {
        speed_ = speed;
    }
    
    //should be used for melee attack
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
    }

    void shoot_projectile()
    {
        //noch ueberarbeiten 

    }

    void change_target_to_move()
    {
        start_pos = this.transform.position;
        //tile diagonal space 
        //next_pos = start_pos + tds
    }

    void process_move_behavior()
    {
        if (move_behavior != move_pettern.UNDEFINED)
        {
            if (move_behavior == move_pettern.STRAIGHT)
            {
                this.transform.position += Vector3.left * speed_ * Time.deltaTime;
                
                
            }
            else if (move_behavior == move_pettern.DIAGONAL)
            {
                if (delta_ >= 1)
                {
                    delta_ = 0;
                    change_target_to_move();
                }

                delta_ += Time.deltaTime;

                Vector3.Lerp(start_pos, next_pos, delta_);
            }
            else if (move_behavior == move_pettern.JUMPY)
            {
                this.transform.position += Vector3.left * speed_ * Time.deltaTime;

                float result = (Random.value % 100);

                if (result > 70)
                {
                    // Jump
                }
            }
        }
    }

}
