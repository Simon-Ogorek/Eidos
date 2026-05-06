using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.InputSystem;
using System.Collections;

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
    private GameObject ThoughtUI;

    [SerializeField]
    private GameObject SpeechUI;

    [SerializeField]
    private GameObject NotificationUI;

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

    //UI before Tessamark is receieved
    [SerializeField]
    private UITextDisplay ThoughtBox;

    [SerializeField]
    private UITextDisplay SpeechBox;

    [SerializeField]
    private UITextDisplay QuestBox;

    [SerializeField]
    private UITextDisplay NotificationBox;

    [SerializeField]
    private UITextDisplay DayBox;

    [SerializeField]
    private UITransition Fade;

    public TMP_Text hits;
    public TMP_Text strikes;

    bool usingController = false;

    bool notif = false;

    bool canCloseNotif = false;

    bool quest = false;

    public static UIController Instance { get; private set; }

    public Combatant playerCombatant;

    public PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        current_state = UIState.Exploring;
    }

    void Start()
    {
        AdventureUI.SetActive(false);
        BattleUI.SetActive(false);
        DialogueUI.SetActive(false);
        ThoughtUI.SetActive(false);
        SpeechUI.SetActive(false);
        BaseballUI.SetActive(false);
        playerMovement = playerCombatant.GetComponent<PlayerMovement>();
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
                AudioController.Instance.BattlePlayMoveUp();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || (usingController && Gamepad.current.leftShoulder.wasPressedThisFrame))
            {
                MovePanel.ChangeMove(false);
                AudioController.Instance.BattlePlayMoveDown();
            }


            // TODO : Make controller binds
            // Change the targeted combatant upwards relative to the Enemy UI
            if (current_state == UIState.Battle_Selecting_Target && Input.GetKeyDown(KeyCode.R))
            {
                playerCombatant.target = EnemyPanel.ChangeTargetUp();
                AudioController.Instance.BattlePlaySelectUp();
            }

            // TODO : Make controller binds
            // Change the targeted combatant downwards relative to the Enemy UI
            if (current_state == UIState.Battle_Selecting_Target && Input.GetKeyDown(KeyCode.F))
            {
                playerCombatant.target = EnemyPanel.ChangeTargetDown();
                AudioController.Instance.BattlePlaySelectDown();
            }

            if ((current_state == UIState.Battle_Selecting_Target && Input.GetKeyDown(KeyCode.Return)) || (current_state == UIState.Battle_Selecting_Target && usingController && Gamepad.current.rightTrigger.wasPressedThisFrame))
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                MovePanel.DoSelectedMove();
            }


            // TODO : Make controller binds
            // Change the targeted combatant upwards relative to the Enemy UI
            if (current_state == UIState.Battle_Selecting_Target && Input.GetKeyDown(KeyCode.R))
            {
                playerCombatant.target = EnemyPanel.ChangeTargetUp();
            }

            // TODO : Make controller binds
            // Change the targeted combatant downwards relative to the Enemy UI
            if (current_state == UIState.Battle_Selecting_Target && Input.GetKeyDown(KeyCode.F))
            {
                playerCombatant.target = EnemyPanel.ChangeTargetDown();
            }

            if ((current_state == UIState.Battle_Selecting_Target && Input.GetKeyDown(KeyCode.Return)) || (current_state == UIState.Battle_Selecting_Target && usingController && Gamepad.current.rightTrigger.wasPressedThisFrame))
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                MovePanel.DoSelectedMove();
            }

            if ((current_state == UIState.Battle && Input.GetKeyDown(KeyCode.Return)) || (current_state == UIState.Battle && usingController && Gamepad.current.rightTrigger.wasPressedThisFrame))
            {
                Time.timeScale = 0.05f;
                Time.fixedDeltaTime *= Time.timeScale;
                current_state = UIState.Battle_Selecting_Target;
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
          //Hide Notification
            if (Input.GetKeyDown(KeyCode.I) && notif)
            {
            if (!canCloseNotif)
            {
                canCloseNotif = true;
                return;
            }
                if(NotificationUI.activeSelf){
                    NotificationUI.SetActive(false);
                    playerMovement.cantMove = false;
                    notif = false;
                    canCloseNotif = false;
                    if(quest)
                    {
                        SetQuestTitle(NotificationBox.header.text);
                        SetQuestObjective(NotificationBox.textBox.text);
                        quest = false;
                    }
                }
            }
    }

    public Combatant ResetTarget()
    {
        EnemyPanel.Reset();
        return EnemyPanel.ChangeTargetUp();
    }

    public void SetState(UIState newState)
    {
        current_state = newState;

        if (current_state == UIState.Battle)
        {
            AudioController.Instance.PlayCombatMusic();
            AdventureUI.SetActive(false);
            BattleUI.SetActive(true);
            BaseballUI.SetActive(false);
            MovePanel.UpdateMoveSelection(playerCombatant);
            PlayerPanel.UpdatePlayerInfo(playerCombatant);
        }
        else if (current_state == UIState.Exploring)
        {
            AudioController.Instance.PlayAdventureMusic();
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
        if (combatant.isEnemy && !EnemyPanel.CheckIfEnemyInfoExists(combatant))
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
        current_state = UIState.Battle;
        MovePanel.setCooldownFalse();
    }

    public void OpenDialogue(string dialogue, string name, string type = "Dialogue")
    {
        Debug.Log(name + "dialogue started");
        if(type == "Dialogue"){
        DialogueUI.SetActive(true);
        DialogueBox.SetText(dialogue);
        DialogueBox.SetHeader(name);
        }
        if(type == "Thought"){
        Debug.Log("Thought Happened");
        ThoughtUI.SetActive(true);
        ThoughtBox.SetText(dialogue);
        ThoughtBox.SetHeader(name);
        }
        if(type == "Speech"){
        SpeechUI.SetActive(true);
        SpeechBox.SetText(dialogue);
        SpeechBox.SetHeader(name);
        }
    }

    public void EndDialogue()
    {
        Debug.Log("Thought Ended");
        DialogueUI.SetActive(false);
        ThoughtUI.SetActive(false);
        SpeechUI.SetActive(false);
    }

    public void SetQuestTitle(string title)
    {
        QuestBox.SetHeader(title);
    }

    public void SetQuestObjective(string objective)
    {
        QuestBox.SetText(objective);
    }

    public void SetDay(string day)
    {
        DayBox.SetText(day);
    }

    public void SetHit(float hit)
    {
        hits.SetText(hit.ToString());
    }

    public void SetStrike(float strike)
    {
        strikes.SetText(strike.ToString());
    }

    public void NotificationPop(string notifDetails, string notifHeader = "", bool Quest = false, bool fromDialogue = true)
    {
        AudioController.Instance.PlayPopUp();
        if(!fromDialogue)
            canCloseNotif = true;
        else
            canCloseNotif = false;
        if(Quest)
            quest = true;

        NotificationBox.SetHeader(notifHeader);
        NotificationBox.SetText(notifDetails);
        NotificationUI.SetActive(true);
        playerMovement.cantMove = true;
        notif = true;
    }

    public void hideBattleUI()
    {
        if (current_state == UIState.Exploring || current_state == UIState.Baseball)
        {
            Debug.LogWarning("Trying to end a battle in a non battle state");
            return;
        }

        current_state = UIState.Exploring;

        AdventureUI.SetActive(true);
        BattleUI.SetActive(false);
        DialogueUI.SetActive(false);
        BaseballUI.SetActive(false);
    }

    public void FadeOut()
    {
        StartCoroutine(Fade.FadeOut());
    }

    public void FadeIn()
    {
        StartCoroutine(Fade.FadeIn());
    }
}
