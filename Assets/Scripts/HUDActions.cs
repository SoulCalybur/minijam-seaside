using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDActions : MonoBehaviour
{

    public GameObject[] actions;


    public void SetImages(int numAvailable) {

        for (int i = 0; i < actions.Length; i++) {
            if(i < numAvailable) {
                actions[i].GetComponent<Animator>().SetBool("isAvailable", true);
            } else {
                actions[i].GetComponent<Animator>().SetBool("isAvailable", false);

            }
        }

    }

}
