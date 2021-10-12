using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    [SerializeField] private Transform[] alarmButtons;
    [SerializeField] private int maxNotes = 3;

    private GameObject tempLights;
    private GameObject alarm;
    private GameObject gameUI;
    private GameObject gameOverScreen;
    private TextMeshProUGUI infoText;

    private bool computerHacked = false;
    public bool ComputerHacked {
        get {
            return computerHacked;
        }
        set {
            computerHacked = value;
            if(computerHacked) {
                gameUI.transform.Find("CollectedNotes").GetComponent<TextMeshProUGUI>().text = "CEO's Computer Hacked";
            }
        }
    }

    private int collectedNotes;
    public int CollectNote {
        get {
            return collectedNotes;
        }
        set {
            if (collectedNotes <= maxNotes) {
                collectedNotes = value;
                gameUI.transform.Find("CollectedNotes").GetComponent<TextMeshProUGUI>().text = "Code " + collectedNotes + "/" + maxNotes;
            }
        }
    }

    private void Awake() {
        tempLights = GameObject.Find("TempLights");
        alarm = GameObject.Find("Alarm");
        gameUI = GameObject.Find("FPSUI");
        infoText = gameUI.transform.Find("InfoText").GetComponent<TextMeshProUGUI>();
        gameOverScreen = GameObject.Find("GameOverScreen");
    }

    public void setInfoText(string text) {
        infoText.text = text;
    }

    // Start is called before the first frame update
    private void Start() {
        tempLights.SetActive(false);
        alarm.SetActive(false);
        gameUI.SetActive(true);
        CollectNote = 0;
        gameOverScreen.SetActive(false);
        setInfoText("");
    }

    public Transform getClosestAlarm(Transform currentPosition) {
        Transform closest = alarmButtons[0];
        foreach(Transform alarm in alarmButtons) {
            float distance = Vector3.Distance(alarm.position, currentPosition.position);
            if(distance <= Vector3.Distance(closest.position, currentPosition.position)) {
                closest = alarm;
            }
        }
        return closest;
    }

    public bool canUnlock() {
        if(collectedNotes == maxNotes) {
            return true;
        } else {
            return false;
        }
    }

    public void gameOver() {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameUI.SetActive(false);
        gameOverScreen.SetActive(true);
        if(ComputerHacked) {
            gameOverScreen.transform.Find("GameOverText").GetComponent<TextMeshProUGUI>().text = "Victory!";
        }
    }

    public void turnOnAlarm() {
        alarm.SetActive(true);
    }

    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
