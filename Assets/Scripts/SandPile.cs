using System.Collections;
using System.Collections.Generic;
using Assets.Code;
using UnityEngine;

public class SandPile : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "water")
        {
            GameModel.Instance.dec_sand_pile_counter();
            Destroy(this.gameObject);
        }
    }
}
