using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SoundEmitter : MonoBehaviour
{
    private SphereCollider _soundSphere;
    private void Awake()
    {
        _soundSphere = GetComponent<SphereCollider>();
        PlayerController.PlayerController.onSoundLevelChanged += SetCurrentSoundLevel;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerController.onSoundLevelChanged -= SetCurrentSoundLevel;
    }

    private void SetCurrentSoundLevel(float soundStrengh)
    {
        _soundSphere.radius = soundStrengh;
    }
}
