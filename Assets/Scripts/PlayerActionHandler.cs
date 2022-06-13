using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;
public class PlayerActionHandler : MonoBehaviour
{
    public bool inAction = false;

    public GameObject pileInContact;
    public Animator animator;

    public Grid grid;
    public GameObject placementSpot;

    public GameObject towerPrefab;

    public int availableActions = 3;
    public const int MAX_ACTIONS = 3;

    public HUDActions hudActions;
    

    private void OnTriggerEnter2D(Collider2D collision) {

        SandPile pile = collision.gameObject.GetComponent<SandPile>();

        if (pile) {
            pileInContact = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        pileInContact = null;
    }

    public void DoAction() {
        Debug.Log("Do Action");
        Debug.Log(pileInContact);

        if (pileInContact) {
            // trigger Action?
            animator.SetTrigger("DigPile");
            animator.speed = 1;
            Destroy(pileInContact);
            GameModel.Instance.dec_sand_pile_counter();
            AddAction();

        } else {
            if (CanPlaceTower()) {
                GameModel.Instance.spawn_grid_object(placementSpot.transform.position, towerPrefab);

                ActionPerformed();
            }
        }
    }


    void AddAction() {
        availableActions++;
        if (availableActions > MAX_ACTIONS) availableActions = MAX_ACTIONS;
        hudActions.SetImages(availableActions);

    }

    bool CanPlaceTower() {
        if(availableActions > 0) {
            return true;
        }
        return false;
    }

    void ActionPerformed() {
        if (availableActions > 0) {
            availableActions--;

            hudActions.SetImages(availableActions);
        }
    }

    private void Update() {

        Vector3Int cellPos = grid.WorldToCell(this.transform.position);
        Vector3 spotPos = grid.GetCellCenterWorld(cellPos);

        // Debug.Log(cellPos);
        // Debug.Log(spotPos);
        placementSpot.transform.position = spotPos;

    }
}
