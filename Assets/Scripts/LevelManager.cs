using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class LevelManager : MonoBehaviour
{
    public int totalScore = 4;
    public int score;
    public Image displayIcon;
    public Canvas canvas;
    public Transform scoreBoardPosition;

    private List<GameObject> scoreList;

    private void Awake()
    {
        scoreList = new List<GameObject>();
        score = totalScore;
    }
    private void Start()
    {
        RectTransform rt = (RectTransform)displayIcon.transform;

        for (int i = 1; i <= totalScore; i++) {
            float rowOffset = rt.rect.width * i;
            Vector3 position = new Vector3(transform.position.x - rowOffset, transform.position.y, transform.position.z);
            Image img = Instantiate(displayIcon, position, Quaternion.identity);
            img.transform.parent = canvas.transform;
            scoreList.Add(img.gameObject);
        }
    }

    public void UpdateScore() {
        Destroy(scoreList.Last());
        if (scoreList.Any())
        {
            scoreList.RemoveAt(scoreList.Count - 1);
        }
        score--;
    }
}
