using UnityEngine;

public class Exit : MonoBehaviour {
    private GameManager manager;

    private void Awake() {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseOver() {
        manager.setInfoText("Exit");
        if(Input.GetMouseButtonDown(0)) {
            manager.gameOver();
        }
    }

    private void OnMouseExit() {
        manager.setInfoText("");
    }
}
