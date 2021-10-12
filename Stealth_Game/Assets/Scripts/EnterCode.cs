using UnityEngine;

public class EnterCode : MonoBehaviour {
    private GameManager manager;
    private Transform officeDoor;
    private bool unlocked = false;

    private void Awake() {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        officeDoor = GameObject.FindGameObjectWithTag("CEODoor").transform;
    }

    private void OnMouseOver() {
        manager.setInfoText("Enter Code");
        if(Input.GetMouseButtonDown(0) && manager.canUnlock() && !unlocked) {
            officeDoor.localEulerAngles = new Vector3(0f, 90f, 0f);
        }
    }

    private void OnMouseExit() {
        manager.setInfoText("");
    }
}
