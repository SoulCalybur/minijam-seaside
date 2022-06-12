using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] pew;

    private float projectile_speed = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Idle());
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += projectile_speed * Vector3.right * Time.deltaTime;
        //anim.Play()
    }
    IEnumerator Idle()
    {
        int i;
        i = 0;
        while (i < pew.Length)
        {
            spriteRenderer.sprite = pew[i];
            i++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;
        }
        StartCoroutine(Idle());
    }
}
