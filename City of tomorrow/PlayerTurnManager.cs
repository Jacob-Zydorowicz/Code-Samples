/*
 * CIS 450 Programming design patterns
 * Jacob Welch, Jacob Zydorowicz
 * City of Tomorrow
 * Manages player turns
 * Last Updated: October first 2023
 */
#region imported namespaces
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
#endregion

public class PlayerTurnManager : MonoBehaviour
{
    Header["Turn Manager"]
    #region Fields
    [SerializeField] int maxTurns = 50;
    [SerializeField] float baseCO2rate = .1f;
    [SerializeField] float CO2RateOfIncrease = .4f;
    private static int turn;
    private static Subject sb;
    private static PlayerTurnManager Instance;
    #endregion

    #region Functions
    private void Awake()
    {
        Instance = this;
        turn = 1;
        sb = FindObjectOfType<Subject>();
    }

    /// <summary>
    /// Calls for an event to take place once per frame.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Debug.Log("next turn");
            NextTurn();
        }
    }

    public void NextTurn()
    {
        if (Time.timeScale != 0)
        {
            EconManager.AddMoney(100);
            turn++;
            CO2Manager.UpdateCO2((baseCO2rate + CO2RateOfIncrease * (turn - 1)));
            sb.UpdateTurn(turn);
            foreach (Building building in GameObject.FindObjectsOfType<Building>())
            {
                building.TurnEffect();
            }
            CO2Manager.TurnEnd();
            if (turn >= maxTurns)
                GameObject.FindObjectOfType<ExtraMenusController>().Win();
        }

        PlayerBuildController.ResetCommands();

        if (turn >= maxTurns)
            GameObject.FindObjectOfType<ExtraMenusController>().Win();
    }
    #endregion
}
