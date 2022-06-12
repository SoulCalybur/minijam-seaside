using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] move;
    public Sprite[] dead;

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

    private float animation_delta_ = 0.0f;

    private float top_max = 4;

    private float bottom_max = -6;

    public float range_to_next_tile_center = 1f;

    private float jump_range = 0;

    private float jump_cooldown = 10.0f;

    private float jump_delta = 0.0f;

    private bool is_jumping = false;

    private bool can_jump = false;

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
        next_pos = start_pos + move_direction * range_to_next_tile_center;
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
                this.transform.position = Vector3.Lerp(start_pos, next_pos, delta_);
            }
            else if (move_behavior == move_pettern.JUMPY)
            {
                if (can_jump && Random.value > 0.9f && !is_jumping)
                {
                    is_jumping = true;

                    jump_delta = 0.0f;
                    can_jump = false;

                    start_pos = transform.position;

                    next_pos = transform.position;

                    jump_range = 2 * range_to_next_tile_center;

                    if (Random.value > 0.5f)
                    {
                        if (transform.position.y + jump_range * range_to_next_tile_center > top_max && is_jumpposition_free(jump_range,Vector3.down))
                            next_pos.y -= jump_range;
                        else if(is_jumpposition_free(jump_range + 2, Vector3.up))
                            next_pos.y += jump_range;
                        else
                        {
                            is_jumping = false;
                            can_jump = true;
                        }
                    }
                    else
                    {
                        if (transform.position.y - jump_range * range_to_next_tile_center > top_max && is_jumpposition_free(jump_range, Vector3.up))
                            next_pos.y += jump_range;
                        else if(is_jumpposition_free(jump_range, Vector3.down))
                            next_pos.y -= jump_range;
                        else
                        {
                            is_jumping = false;
                            can_jump = true;
                        }
                    }
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

                        transform.position = Vector3.Lerp(start_pos, next_pos, delta_);

                    }
                }
                else
                {
                    if (jump_delta > jump_cooldown)
                        can_jump = true;

                    jump_delta += Time.deltaTime;

                    this.transform.position += Vector3.left * speed_ * Time.deltaTime;
                }
            }
        }
    }

    bool is_jumpposition_free(float length, Vector3 dir)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir, length);

        for(int index = 0; index < hit.Length;index++)
            if (hit[index].collider.tag == "enemy" &&
                Vector3.Distance(transform.position, hit[index].transform.position) >=length)
                return false;

        return true;
    }
}
