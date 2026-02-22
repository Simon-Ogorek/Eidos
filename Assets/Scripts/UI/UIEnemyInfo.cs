using System.Collections.Generic;
using TMPro;
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
    }

    Dictionary<Combatant, EnemyUI> combatantToUIMap;

    [SerializeField]
    GameObject template;
    [SerializeField]
    Transform origin;

    void Start()
    {
        combatantToUIMap = new Dictionary<Combatant, EnemyUI>();
    }

    /// @brief !! Does not update portraits or name!!
    public void SoftUpdateenemyInfo(Combatant enemy)
    {
        EnemyUI ui = combatantToUIMap[enemy];
        ui.healthSlider.value = enemy.health / enemy.maxHealth;
    }

    public void UpdateEnemyInfo(Combatant enemy)
    {
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

        combatantToUIMap.Add(enemy, uiInstance);

        UpdateEnemyInfo(enemy);
    }
}
