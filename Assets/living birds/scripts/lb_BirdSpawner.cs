using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class lb_BirdSpawner : MonoBehaviour
{
    public static lb_BirdSpawner instance;
    public lb_BirdController controller;
    public List<DefaultTrackableEventHandlerBirds> watchForBirdSpawns;
    public bool okayToSpawnBird = true;
    public AudioClip createBirdSound;
    


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        foreach(DefaultTrackableEventHandlerBirds dteh in watchForBirdSpawns)
        {
            dteh.spawnBirdCall.AddListener((string birdToSpawn) => SpawnBird(birdToSpawn));
        }

        
    }

    public void SpawnBird(string birdType)
    {
        if (okayToSpawnBird)
        {
            Debug.Log($"[{birdType}] bird spawned");
            this.GetComponent<AudioSource>().PlayOneShot(createBirdSound);
            controller.SpawnSpecificBird(birdType);
            StartCoroutine(waitToSpawnBird());
        }
    }

    public void killAllBirds()
    {
        List<GameObject> birdsToDestroy = new List<GameObject>();
        foreach(GameObject bird in controller.myBirds)
        {
            birdsToDestroy.Add(bird);
        }

        foreach(GameObject bird in birdsToDestroy)
        {
            controller.myBirds.Remove(bird);
            Destroy(bird);
        }
    }

    IEnumerator waitToSpawnBird()
    {
        okayToSpawnBird = false;
        yield return new WaitForSeconds(1.5f);
        okayToSpawnBird = true;
        yield return null;
    }
}
