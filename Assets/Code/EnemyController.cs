using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private float delta_ = 0.0f;

    private float top_max = 2.25f;

    private float bottom_max = -4.25f;

    private float range_cd;

    private bool is_jumping = false;

    private Vector3 start_pos;

    private Vector3 next_pos;

    private Vector3 move_direction;

    private move_pettern move_behavior = move_pettern.UNDEFINED;

    private attack_type attack_behavior = attack_type.UNDEFINED;
    // Start is called before the first frame update
 
    // Update is called once per frame
    void Update()
    {
        process_move_behavior();
    }

    public void set_move_pattern(move_pettern e)
    {
        move_behavior = e;

        if (e == move_pettern.DIAGONAL)
        {
            move_direction = new Vector3();
            move_direction.x = -1;
            move_direction.y = (Random.value > 0.5) ? -1 : 1;
            change_target_to_move();
        }
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
        if (col.collider.tag == "Projectile")
        {
            Debug.Log("destroy it");
            Destroy(col.collider.gameObject);
            Destroy(this.gameObject);
        }
    }

    void shoot_projectile()
    {


        //noch ueberarbeiten 

    }

    void change_target_to_move()
    {
        start_pos = this.transform.position;
        //tile diagonal space 
        if (next_pos.y <= bottom_max || next_pos.y >= top_max)
            move_direction.y = -move_direction.y;
        next_pos = start_pos + move_direction * 0.5f;
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
                Debug.Log("start " + start_pos + " end " + next_pos);
                this.transform.position = Vector3.Lerp(start_pos, next_pos, delta_);
            }
            else if (move_behavior == move_pettern.JUMPY)
            {
                if (Random.value > 0.7f)
                {
                    is_jumping = true;
                }

                if (is_jumping)
                {
                    if (delta_ >= 1)
                    {
                        delta_ = 0;
                        is_jumping = false;
                    }
                    else
                    { 
                        delta_ += Time.deltaTime;

                        float a, b, c;
                        a = c = 0.25f;
                        b = 1;

                        float ab = ((1f - delta_) * a) + (b * delta_);
                        float ac = ((1f - delta_) * a) + (c * delta_);
                        float abac = ((1f - delta_) * ab) + (ac * delta_);

                        transform.localScale = abac * Vector3.one;
                    }
                }
                else
                {
                    this.transform.position += Vector3.left * speed_ * Time.deltaTime;
                }
            }
        }
    }

}
