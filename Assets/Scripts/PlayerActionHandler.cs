using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionHandler : MonoBehaviour
{
    public bool inAction = false;

    public GameObject objInContact;
    public Animator animator;

    public Grid grid;
    public GameObject placementSpot;

    public GameObject towerPrefab;


    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("OnTriggerEnter2D");
        Debug.Log(collision);

        SandPile pile = collision.gameObject.GetComponent<SandPile>();
        objInContact = collision.gameObject;

        if (pile) {
            Debug.Log("OnTriggerEnter2D pile");
            Debug.Log(pile);
        }


    }

    private void OnTriggerExit2D(Collider2D collision) {
        objInContact = null;
        animator.SetBool("inAction", false);
        Debug.Log("OnTriggerExit2D");

    }

    public void DoAction() {
        Debug.Log("Do Action");


        Instantiate(towerPrefab, placementSpot.transform.position, Quaternion.identity);

        if (objInContact) {
            inAction = true;
            animator.SetBool("inAction", true);
            Debug.Log(objInContact.tag);



            StartCoroutine(Dig(3f));
        }


    }

    IEnumerator Dig(float timer) {

        Debug.Log("Dig!");
        float countdown = timer;

        while (countdown > 0f) {
            countdown -= Time.deltaTime;
            Debug.Log(countdown);
            yield return new WaitForSeconds(Time.deltaTime);
        }


    }

    private void Update() {

        Vector3Int cellPos = grid.WorldToCell(this.transform.position);
        Vector3 spotPos = grid.GetCellCenterWorld(cellPos);

        Debug.Log(cellPos);
        Debug.Log(spotPos);
        placementSpot.transform.position = spotPos;

    }
}
