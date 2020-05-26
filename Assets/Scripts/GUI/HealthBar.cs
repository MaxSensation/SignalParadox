//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Color onColor, offColor;
    private Image[] healthBarBlocks;
    private void Awake()
    {
        HealthSystem.onInitEvent += UpdateGui;
        HealthSystem.onPlayerTakeDamageEvent += UpdateGui;
        healthBarBlocks = transform.GetComponentsInChildren<Image>();
    }

    private void OnDestroy()
    {
        HealthSystem.onInitEvent -= UpdateGui;
        HealthSystem.onPlayerTakeDamageEvent -= UpdateGui;
    }

    private void UpdateGui(int currentHealth)
    {
        for (var i = 0; i < healthBarBlocks.Length; i++) 
            healthBarBlocks[i].color = i < currentHealth ? onColor : offColor;
    }
}
