using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.InputSystem;

/// <summary>
/// Controls all in-game aspects about the UI
/// </summary>
public class UIController : MonoBehaviour
{
    public enum UIState
    {
        Exploring,
        Battle,
        Battle_Selecting_Target,
        Baseball
    }
    UIState current_state;

    [Header("UI Containers")]
    [SerializeField]
    private GameObject AdventureUI;
    [SerializeField]
    private GameObject BattleUI;
    [SerializeField]
    private GameObject DialogueUI;

    [SerializeField]
    private GameObject BaseballUI;

    [Header("Battle UI Panels")]
    [SerializeField]
    private UIEnemyInfo EnemyPanel;
    [SerializeField]
    private UIPlayerInfo PlayerPanel;
    [SerializeField]
    private UIMoveInfo MovePanel;
    
    [SerializeField]
    private UITextDisplay DialogueBox;

    [SerializeField]
    private UITextDisplay QuestBox;

    public TMP_Text hits;
    public TMP_Text strikes;

    bool usingController = false;

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
        DialogueUI.SetActive(false);
        BaseballUI.SetActive(false);
    }

    void Update()
    {
        if(Gamepad.current!=null){
            usingController = true;
        }
        else if(Gamepad.current==null)
        {
            usingController = false;
        }

        if (current_state == UIState.Battle || current_state == UIState.Battle_Selecting_Target)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || (usingController && Gamepad.current.rightShoulder.wasPressedThisFrame))
            {
                MovePanel.ChangeMove(true);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || (usingController && Gamepad.current.leftShoulder.wasPressedThisFrame))
            {
                MovePanel.ChangeMove(false);
            }

            if ((current_state == UIState.Battle && Input.GetKeyDown(KeyCode.Return)) || (current_state == UIState.Battle && usingController && Gamepad.current.rightTrigger.wasPressedThisFrame))
            {
                Time.timeScale = 0.02f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                current_state = UIState.Battle_Selecting_Target;
                
                
            }
            if ((current_state == UIState.Battle_Selecting_Target && Input.GetKeyDown(KeyCode.Return)) || (current_state == UIState.Battle_Selecting_Target && usingController && Gamepad.current.rightTrigger.wasPressedThisFrame))
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
            BaseballUI.SetActive(false);
            MovePanel.UpdateMoveSelection(playerCombatant);
            PlayerPanel.UpdatePlayerInfo(playerCombatant);
        }
        else if (current_state == UIState.Exploring)
        {
            AdventureUI.SetActive(true);
            BattleUI.SetActive(false);
            BaseballUI.SetActive(false);
        }
        else if (current_state == UIState.Baseball)
        {
            AdventureUI.SetActive(false);
            BattleUI.SetActive(false);
            BaseballUI.SetActive(true);
        }
        
    }

    public void AddToEnemyPanel(Combatant combatant)
    {
        if (combatant.isEnemy)
        {
            EnemyPanel.AddEnemyInfo(combatant);
        }
    }
    public void RemoveFromEnemyPanel(Combatant combatant)
    {
        EnemyPanel.RemoveEnemyInfo(combatant);
    }

    public void startCooldown()
    {
        MovePanel.setCooldownTrue();
    }

    public void endCooldown()
    {
        MovePanel.setCooldownFalse();
    }

    public void OpenDialogue(string dialogue, string name)
    {
        Debug.Log(name + "dialogue started");
        DialogueUI.SetActive(true);
        DialogueBox.SetText(dialogue);
        DialogueBox.SetHeader(name);
    }

    public void EndDialogue()
    {
        DialogueUI.SetActive(false);
    }

    public void SetQuest(string quest)
    {
        QuestBox.SetText(quest);
    }

    public void SetHit(float hit)
    {
        hits.SetText(hit.ToString());
    }

    public void SetStrike(float strike)
    {
        strikes.SetText(strike.ToString());
    }
}
