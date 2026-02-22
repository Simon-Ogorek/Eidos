using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    [SerializeField]
    Slider healthSlider;
    [SerializeField]
    Slider manaSlider;
    [SerializeField]
    RawImage portraitUI;

    /// @brief !! Does not update portraits !!
    public void SoftUpdatePlayerInfo(Combatant player)
    {
        healthSlider.value = player.health / player.maxHealth;
        manaSlider.value = player.mana / player.maxMana;
    }

    public void UpdatePlayerInfo(Combatant player)
    {
        healthSlider.value = player.health / player.maxHealth;
        manaSlider.value = player.mana / player.maxMana;
        portraitUI.texture = player.portrait;
    }
}
