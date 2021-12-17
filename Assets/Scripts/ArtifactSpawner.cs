using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSpawner : MonoBehaviour
{
    public GameObject boyArtifact;
    public GameObject girlArtifact;
    public GameObject momArtifact;

    [Space]

    public Zone[] zones;

    [Space]

    public string mySeed = "NSEED";

    [Space]

    [SerializeField] private bool _createNewSeed = true;

    [SerializeField] private DataManager dataManager;

    private void Start()
    {
        if (dataManager == null)
            dataManager = FindObjectOfType<DataManager>();

        mySeed = dataManager.GetProgress().seed;
        _createNewSeed = mySeed.Length != 6;

        if (_createNewSeed) mySeed = GenerateSeed();

        try
        {
            SpawnArtifactsFromSeed(mySeed);
        }
        catch
        {
            Debug.LogWarning("Invalid seed, generating new seed");

            mySeed = GenerateSeed();
            SpawnArtifactsFromSeed(mySeed);
        }
    }

    private void OnApplicationQuit()
    {
        ResetSeed();
    }

    private string GenerateIndividualSeed(int zoneIndex)
    {
        Transform[] locations = zones[zoneIndex].locations;
        int locationIndex = Random.Range(0, locations.Length);

        return zoneIndex + "" + locationIndex;
    }

    private void SpawnIndividualArtifact(int artifactIndex, string seed)
    {
        try
        {
            GameObject artifact = artifactIndex == 0 ? boyArtifact : (artifactIndex == 1) ? girlArtifact : momArtifact;

            int zoneIndex = int.Parse("" + seed[0 + artifactIndex * 2]);
            int locationIndex = int.Parse("" + seed[1 + artifactIndex * 2]);

            Vector3 position = zones[zoneIndex].locations[locationIndex].position;
            Vector3 rotation = zones[zoneIndex].locations[locationIndex].eulerAngles;

            GameObject spawnedArtifact = Instantiate(artifact, transform);
            spawnedArtifact.transform.position = position;
            spawnedArtifact.transform.eulerAngles += rotation;
        }
        catch
        {
            Debug.LogError($"{seed} is an invalid seed");
            throw;
        }
    }

    private int GetRandomZoneIndex()
    {
        return Random.Range(0, zones.Length);
    }

    public string GenerateSeed()
    {
        int boyZone = GetRandomZoneIndex();
        string boySeed = GenerateIndividualSeed(boyZone);

        int girlZone = GetRandomZoneIndex();
        while (girlZone == boyZone)
        {
            girlZone = GetRandomZoneIndex();
        }
        string girlSeed = GenerateIndividualSeed(girlZone);

        int momZone = GetRandomZoneIndex();
        while (momZone == boyZone || momZone == girlZone)
        {
            momZone = GetRandomZoneIndex();
        }
        string momSeed = GenerateIndividualSeed(momZone);

        mySeed = boySeed + girlSeed + momSeed;
        dataManager.SetSeed(mySeed);

        return mySeed;
    }

    public void SpawnArtifactsFromSeed(string seed)
    {
        SpawnIndividualArtifact(0, seed);
        SpawnIndividualArtifact(1, seed);
        SpawnIndividualArtifact(2, seed);
    }

    public void ResetSeed()
    {
        dataManager.SetSeed("NSEED");
    }
}

[System.Serializable]
public class Zone
{
    public Transform[] locations;
}
