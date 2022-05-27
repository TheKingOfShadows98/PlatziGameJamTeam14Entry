using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniGame1 : MonoBehaviour
{
    [Header("Public Values")]
    public UnityEvent OnWinGame;
    [field: SerializeField, Range(0f, 1f)] public float stability { get; private set; } = 0.5f;
    public UnityEvent OnLosseGame;

    [Header("Components")]
    [SerializeField] private GameObject xMark;
    [SerializeField] private GameObject yMark;
    [SerializeField] private GameObject centerMark;
    [SerializeField] private GameObject requiredMark;

    [Header("Settings")]
    [SerializeField] private float miniGameTime = 30f;
    [SerializeField] private Vector2 markerSize;
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;
    [SerializeField] private float sensivity = 0.001f;
    [SerializeField] private float dropGainRate = 0.02f;


    private bool active = true;
    private Vector2 requiredPoint;
    private Vector2 values = new Vector2(0,0);
    private Timer gameTimer = new Timer();
    private Timer changeTimer = new Timer(2.5f);


    void Start()
    {
        gameTimer.Cd = miniGameTime;
        gameTimer.Start();
        centerMark.transform.localScale = new Vector3(markerSize.x, markerSize.y, centerMark.transform.localScale.z);
    }


    void changeRequiredPointPosition()
    {
        requiredPoint = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
    }


    bool inRange()
    {
        return Vector2.Distance(requiredPoint, values) < markerSize.x;
    }


    void MoveStability()
    {
        stability = inRange() ? stability += dropGainRate * Time.deltaTime : stability -= dropGainRate * Time.deltaTime;
        stability = Mathf.Clamp(stability, 0f, 1f);
    }


    void MovePlayer()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * sensivity * Time.deltaTime;
        values += input;
        values.x = Mathf.Clamp(values.x, 0, 1);
        values.y = Mathf.Clamp(values.y, 0, 1);
        Vector2 posi = min + values * (max - min);
        centerMark.transform.localPosition = posi;
        xMark.transform.localPosition = new Vector3(posi.x, xMark.transform.localPosition.y, 0);
        yMark.transform.localPosition = new Vector3(yMark.transform.localPosition.x, posi.y, 0);
    }


    void MoveRequiredMarker()
    {

        Vector3 pos = Vector3.Lerp(requiredMark.transform.localPosition, min + requiredPoint * (max - min), 0.5f * Time.deltaTime);
        requiredMark.transform.localPosition = pos;
    }


    void Update()
    {
        if (!active) return;

        if (stability <= 0f)
        {
            OnLosseGame.Invoke();
            active = false;
        }

        if (gameTimer.IsEnd())
        {
            OnWinGame.Invoke();
            active = false;
        }

        if (changeTimer.IsAndRestart()) changeRequiredPointPosition();

        MoveStability();
        MovePlayer();
        MoveRequiredMarker();
    }
}
