using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEO : MonoBehaviour {
    private GameManager manager;
    private bool collected = false;

    private void Awake() {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseOver() {
        manager.setInfoText("Use CEO's Computer");
        if(Input.GetMouseButtonDown(0) && !collected) {
            manager.ComputerHacked = true;
        }
    }

    private void OnMouseExit() {
        manager.setInfoText("");
    }
}
