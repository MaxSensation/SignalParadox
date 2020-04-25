using PlayerController;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    private Image[] _healthBarBlocks;
    private void Awake()
    {
        HealthSystem.onPlayerTakeDamageEvent += UpdateGui;
        _healthBarBlocks = transform.GetComponentsInChildren<Image>();
    }

    private void OnDestroy() => HealthSystem.onPlayerTakeDamageEvent -= UpdateGui;

    private void UpdateGui(int currentHealth)
    {
        for (var i = 0; i < _healthBarBlocks.Length; i++) 
            _healthBarBlocks[i].color = i < currentHealth ? onColor : offColor;
    }
}
