using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSpawner : MonoBehaviour
{
    public GameObject boyArtifact;
    public GameObject girlArtifact;
    public GameObject momArtifact;

    [Space]

    public Transform[] easy;
    public Transform[] medium;
    public Transform[] hard;

    [Space]

    public string mySeed = "NSEED";

    [Space]

    [SerializeField] private bool _createNewSeed = true;

    private void Start()
    {
        if (_createNewSeed) mySeed = GenerateSeed();

        SpawnArtifactsFromSeed(mySeed);
    }

    private Transform[] GetLocationArrayFromIndex(int difficulty)
    {
        return difficulty switch
        {
            0 => easy,
            1 => medium,
            2 => hard,
            _ => null
        };
    }

    private string GenerateIndividualSeed(int difficulty)
    {
        Transform[] locations = GetLocationArrayFromIndex(difficulty);
        int locationIndex = Random.Range(0, locations.Length);

        return difficulty + "" + locationIndex;
    }

    private void SpawnIndividualArtifact(int artifactIndex, string seed)
    {
        GameObject artifact = artifactIndex == 0 ? boyArtifact : (artifactIndex == 1) ? girlArtifact : momArtifact;

        int difficulty = int.Parse("" + seed[0 + artifactIndex * 2]);
        int locationIndex = int.Parse("" + seed[1 + artifactIndex * 2]);

        Vector3 position = GetLocationArrayFromIndex(difficulty)[locationIndex].position;

        Instantiate(artifact, transform).transform.position = position;
    }

    public string GenerateSeed()
    {
        int boyDifficulty = Random.Range(0, 3);
        string boySeed = GenerateIndividualSeed(boyDifficulty);

        int girlDifficulty = Random.Range(0, 3);
        while (girlDifficulty == boyDifficulty)
        {
            girlDifficulty = Random.Range(0, 3);
        }
        string girlSeed = GenerateIndividualSeed(girlDifficulty);

        int momDifficulty = Random.Range(0, 3);
        while (momDifficulty == boyDifficulty || momDifficulty == girlDifficulty)
        {
            momDifficulty = Random.Range(0, 3);
        }
        string momSeed = GenerateIndividualSeed(momDifficulty);

        return boySeed + girlSeed + momSeed;
    }

    public void SpawnArtifactsFromSeed(string seed)
    {
        SpawnIndividualArtifact(0, seed);
        SpawnIndividualArtifact(1, seed);
        SpawnIndividualArtifact(2, seed);
    }
}
