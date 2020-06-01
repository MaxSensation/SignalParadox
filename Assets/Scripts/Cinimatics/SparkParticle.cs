//Main author: Maximiliam Rosén

using UnityEngine;

public class SparkParticle : MonoBehaviour
{
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    private ParticleSystem sparkParticle;
    private AudioSource sparkSound;
    private float nextTime, currentTime;

    private void Awake()
    {
        sparkParticle = GetComponent<ParticleSystem>();
        sparkSound = GetComponent<AudioSource>();
        nextTime = Random.Range(minTime, maxTime);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > nextTime)
            Spark();
    }

    private void Spark()
    {
        sparkParticle.Play();
        sparkSound.Play();
        nextTime = Random.Range(minTime, maxTime);
        currentTime = 0;
    }
}