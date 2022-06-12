using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Code
{




    public class BaseTower : MonoBehaviour
    {
        float cooldown = 1f;
        private float delta_ = 0.0f;

        private bool can_shoot = true;

        public GameObject projectile;

        // Update is called once per frame
        void Update()
        {

            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector3.right);
            // Does the ray intersect any objects excluding the player layer
            
            for(int index = 0; index < hit.Length;index++)
            
            if (hit[index].collider.tag == "Enemy")
            {
                if (can_shoot)
                {
                    can_shoot = false;
                    Instantiate(projectile, transform.position + Vector3.right * 0.25f, Quaternion.identity);
                }
                else
                {
                    if (delta_ >= cooldown)
                    {
                        delta_ = 0.0f;
                        can_shoot = true;
                    }
                    else
                        delta_ += Time.deltaTime;
                }

                break;
            }
        }

        void FixedUpdate()
        {
            // Bit shift the index of the layer (8) to get a bit mask
            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.

            
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.tag == "Enemy")
            {
                GameModel.Instance.remove_object_from_grid(this.transform.position);
                Destroy(col.collider.gameObject);
                Destroy(this.gameObject);
            }
            
        }
    }

//collision
}