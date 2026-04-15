using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyInfo : MonoBehaviour
{
    struct EnemyUI
    {
        public GameObject obj;
        public Slider healthSlider;
        public TMP_Text nameText;
        public RawImage portraitUI;
        public int positionIdx;
    }

    Dictionary<Combatant, EnemyUI> combatantToUIMap;

    [SerializeField]
    GameObject template;
    [SerializeField]
    Transform origin;

    int countOfEnemiesBeingDeleted = 0;

    void Start()
    {
        combatantToUIMap = new Dictionary<Combatant, EnemyUI>();
    }

    /// @brief !!Does not update portraits or name!!
    public void SoftUpdateenemyInfo(Combatant enemy)
    {
        if (!combatantToUIMap.ContainsKey(enemy))
        {
            Debug.LogWarning($"{enemy.name} is not defined in a update function");
            return;
        }
        EnemyUI ui = combatantToUIMap[enemy];
        ui.healthSlider.value = enemy.health / enemy.maxHealth;
    }

    public void UpdateEnemyInfo(Combatant enemy)
    {
        if (!combatantToUIMap.ContainsKey(enemy))
        {
            Debug.LogWarning($"{enemy.name} is not defined in a update function");
            return;
        }
        EnemyUI ui = combatantToUIMap[enemy];
        ui.healthSlider.value = enemy.health / enemy.maxHealth;
        ui.nameText.text = enemy.displayName;
        ui.portraitUI.texture = enemy.portrait;
    }
    public void AddEnemyInfo(Combatant enemy)
    {
        GameObject templateInstance = Instantiate(template,origin);
        templateInstance.transform.localPosition = new Vector3(0, combatantToUIMap.Count * -160, 0);

        EnemyUI uiInstance;
        uiInstance.obj = templateInstance;
        uiInstance.nameText = templateInstance.GetComponentInChildren<TMP_Text>();
        uiInstance.portraitUI = templateInstance.GetComponentInChildren<RawImage>();
        uiInstance.healthSlider = templateInstance.GetComponentInChildren<Slider>();
        uiInstance.positionIdx = combatantToUIMap.Count;

        combatantToUIMap.Add(enemy, uiInstance);

        UpdateEnemyInfo(enemy);
    }

    IEnumerator repositionUIElement(EnemyUI ui)
    {

        Vector3 finalVector = new Vector3(0, ui.positionIdx * -160, 0);
        float yDist = ui.positionIdx * -160 + ui.obj.transform.localPosition.y;
        Debug.Log($"{Vector3.Distance(ui.obj.transform.localPosition, finalVector)} left for repositioning at idx {ui.positionIdx}");
        while (Vector3.Distance(ui.obj.transform.localPosition, finalVector) >= 1)
        {
            if (yDist < 0)
            {
                ui.obj.transform.localPosition = new Vector3(
                    ui.obj.transform.localPosition.x,
                    ui.obj.transform.localPosition.y + 1,
                    ui.obj.transform.localPosition.z);
            }
            else
            {
                ui.obj.transform.localPosition = new Vector3(
                    ui.obj.transform.localPosition.x,
                    ui.obj.transform.localPosition.y - 1,
                    ui.obj.transform.localPosition.z);
            }
            //Debug.Log($"New Position {ui.obj.transform.localPosition}");
            yield return new WaitForEndOfFrame();
        }
    }

    void EnemyInfoUpdateRemainingUIsAfterDeletion(Combatant toExclude)
    {
        int excludedID = combatantToUIMap[toExclude].positionIdx;
        Debug.Log($"{excludedID} is excluded");
        // Thread safety is fucked if this isnt done like this as EnemyInfoOffScreenAndDelete
        // can change the collection mid execution of this function
        var copyOfCombatants = new List<Combatant>(combatantToUIMap.Keys);
        for (int i = 0; i < copyOfCombatants.Count; i++)
        {
            Combatant enemy = copyOfCombatants[i];
            if (enemy == toExclude)
            {
                continue;
            }
            EnemyUI ui = combatantToUIMap[enemy];
            
            if (combatantToUIMap[enemy].positionIdx > excludedID)
            {
                Debug.Log($"{enemy.name} has been pushed up");
                ui.positionIdx--;
                combatantToUIMap[enemy] = ui;
            }
            Debug.Log($"{enemy.name} has an id of {ui.positionIdx}");
            StartCoroutine(repositionUIElement(combatantToUIMap[enemy]));
        }
    }

    IEnumerator EnemyInfoOffScreenAndDelete(Combatant enemy)
    {

        // If multiple enemies died, wait for them to finish their deletion from UI;
        // TODO


        EnemyUI ui = combatantToUIMap[enemy];
        GameObject uiObj = ui.obj;
        CanvasGroup uiGroup = uiObj.GetComponent<CanvasGroup>();

        bool calledToRepositionOthers = false;

        Vector3 finalVector = ui.obj.transform.localPosition;
        finalVector.x += 900;
        float distLeft = Vector3.Distance(uiObj.transform.localPosition, finalVector);
        float totalDist = distLeft;
        Debug.Log($"Starting to move {enemy.name} off screen, dist: {distLeft} ");


        while (distLeft >= 1)
        {
            uiObj.transform.localPosition = Vector3.Slerp(uiObj.transform.localPosition, finalVector, 0.02f);
            uiGroup.alpha = distLeft / totalDist;
            distLeft = Vector3.Distance(uiObj.transform.localPosition, finalVector);
            Debug.Log($"moving {enemy.name} off screen, dist: {distLeft} ");
            
            if (!calledToRepositionOthers && distLeft/totalDist <= 0.5f)
            {
                EnemyInfoUpdateRemainingUIsAfterDeletion(enemy);
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Destroying Enemy UI");
        Destroy(ui.obj);
        combatantToUIMap.Remove(enemy);
        enemy.uiOutOfSync = false;

    }
    
    public void RemoveEnemyInfo(Combatant enemy)
    {
        StartCoroutine(EnemyInfoOffScreenAndDelete(enemy));
        
    }
}
