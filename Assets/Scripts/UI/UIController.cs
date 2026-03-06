using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

/// <summary>
/// Controls all in-game aspects about the UI
/// </summary>
public class UIController : MonoBehaviour
{
    public enum UIState
    {
        Exploring,
        Battle,
        Battle_Selecting_Target
    }
    UIState current_state;

    [Header("UI Containers")]
    [SerializeField]
    private GameObject AdventureUI;
    [SerializeField]
    private GameObject BattleUI;


    [Header("Battle UI Panels")]
    [SerializeField]
    private UIEnemyInfo EnemyPanel;
    [SerializeField]
    private UIPlayerInfo PlayerPanel;
    [SerializeField]
    private UIMoveInfo MovePanel;


    public static UIController Instance { get; private set; }

    public Combatant playerCombatant;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        Instance = this;
        current_state = UIState.Exploring;
    }

    void Start()
    {
        AdventureUI.SetActive(true);
        BattleUI.SetActive(false);
    }

    void Update()
    {
        if (current_state == UIState.Battle)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MovePanel.ChangeMove(true);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MovePanel.ChangeMove(false);
            }

            if (current_state == UIState.Battle && Input.GetKeyDown(KeyCode.Return) )
            {
                Time.timeScale = 0.02f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                
                
            }
            if (current_state == UIState.Battle_Selecting_Target && Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                MovePanel.DoSelectedMove();
            }

            
            foreach (Combatant entity in GameObject.FindObjectsByType<Combatant>(FindObjectsSortMode.None))
            {
                if (!entity.uiOutOfSync)
                    continue;
                
                if (!entity.isEnemy)
                    PlayerPanel.SoftUpdatePlayerInfo(playerCombatant);
                else
                    EnemyPanel.UpdateEnemyInfo(entity);
                
            }
        }
    }

    public void SetState(UIState newState)
    {
        current_state = newState;

        if (current_state == UIState.Battle)
        {
            AdventureUI.SetActive(false);
            BattleUI.SetActive(true);
            MovePanel.UpdateMoveSelection(playerCombatant);
            PlayerPanel.UpdatePlayerInfo(playerCombatant);
        }
        else if (current_state == UIState.Exploring)
        {
            AdventureUI.SetActive(true);
            BattleUI.SetActive(false);
        }
        
    }

    public void AddToEnemyPanel(Combatant combatant)
    {
        if (combatant.isEnemy)
        {
            EnemyPanel.AddEnemyInfo(combatant);
        }
    }

    public void startCooldownTimer(float timer)
    {
        MovePanel.startCooldownTimerMoves(timer);
    }
}
