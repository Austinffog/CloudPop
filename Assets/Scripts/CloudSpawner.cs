using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    #region Special Clouds
    public GameObject[] specialClouds;
    public Vector3 spawnValues;
    private Vector3 spawnPosition;
    public float spawnWait, spawnLeastWait, SpawnMostWait;
    public int startWait;
    private int cloudsNumber;
    #endregion

    #region Clouds
    public GameObject basicClouds;
    public float spawn;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpecialClouds());
        StartCoroutine(BasicClouds());
         
        basicClouds.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        for(int i = 0; i < specialClouds.Length; i++)
        {
            specialClouds[i].gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
         
    }

    // Update is called once per frame
    void Update()
    {
        spawnWait = Random.Range(spawnLeastWait, SpawnMostWait);
        
        spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), Random.Range(-spawnValues.y, spawnValues.y)
                , Random.Range(-spawnValues.z, spawnValues.z));
    }

    private IEnumerator SpecialClouds()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            cloudsNumber = Random.Range(0, 3);
            Instantiate(specialClouds[cloudsNumber], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            yield return new WaitForSeconds(spawnWait);
        }
    }

    private IEnumerator BasicClouds()
    {
        while (true)
        {
            Instantiate(basicClouds, spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            yield return new WaitForSeconds(spawn);
        }
    }
}
