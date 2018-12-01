using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Ship> controllableShips;
    private int controlIndex = 0;

    public static GameManager Instance { get; private set; }
    public Ship CurrentControllable { get { return controllableShips[controlIndex]; } }

    public Ship GetNextControllable()
    {
        controlIndex++;
        if (controlIndex > controllableShips.Count - 1) controlIndex = 0;
        return controllableShips[controlIndex];
    }

    private void FindAllControllables()
    {
        controllableShips = new List<Ship>(GameObject.FindObjectsOfType<Ship>());
    }

    private void Start()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this)
        {
            Debug.LogError("Instance of GameManager already exists!");
            Destroy(gameObject);
        }

        FindAllControllables();
    }
}
