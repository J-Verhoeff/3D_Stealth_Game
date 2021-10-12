using UnityEngine;

public class Keycode : MonoBehaviour {
    private GameManager manager;
    private bool collected = false;

    private void Awake() {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseOver() {
        //Debug.Log("Entered");
        manager.setInfoText("Use Computer");
        if (Input.GetMouseButtonDown(0) && !collected) {
            //Debug.Log("Clicked");
            manager.CollectNote += 1;
            collected = true;
        }
    }

    private void OnMouseExit() {
        manager.setInfoText("");
    }
}
