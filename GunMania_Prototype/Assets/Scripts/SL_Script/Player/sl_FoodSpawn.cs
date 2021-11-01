using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class sl_FoodSpawn : MonoBehaviour
{
    [Header("Food Spawnpoint")]
    public List<GameObject> foodSpawnPoint;
    public List<GameObject> dishSpawnPoint;

    [Header("Prefabs")]
    public GameObject JPfoodPrefabs;
    public GameObject KRfoodPrefabs;
    public GameObject CNfoodPrefabs;
    public GameObject TWfoodPrefabs;

    public List<GameObject> JPdishPrefabs;
    public List<GameObject> KRdishPrefabs;
    public List<GameObject> CNdishPrefabs;
    public List<GameObject> TWdishPrefabs;

    [Header("Food Respawn Time")]
    public int sec;

    [Header("Dish Spawn Time //Time = time before countdown + start countdown and spawn")]
    public int dishsec;
    public int countdownTime;

    [Header("Dish Respawn Time")]
    public int dishrespawnSec;

    PhotonView view;

    private IEnumerator countdownCoro;
    private IEnumerator dishCoro;

    int count;
    bool spawn;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();

        //Instantiate(JPfoodPrefabs[Random.Range(0, JPfoodPrefabs.Count)], foodSpawnPoint[0].transform.position, Quaternion.identity);
        //Instantiate(KRfoodPrefabs[Random.Range(0, KRfoodPrefabs.Count)], foodSpawnPoint[1].transform.position, Quaternion.identity);
        //Instantiate(CNfoodPrefabs[Random.Range(0, CNfoodPrefabs.Count)], foodSpawnPoint[2].transform.position, Quaternion.identity);
        //Instantiate(TWfoodPrefabs[Random.Range(0, TWfoodPrefabs.Count)], foodSpawnPoint[3].transform.position, Quaternion.identity);

        JPfoodPrefabs.SetActive(true);
        KRfoodPrefabs.SetActive(true);
        CNfoodPrefabs.SetActive(true);
        TWfoodPrefabs.SetActive(true);

        //countdownCoro = DishCountdown(countdownTime);
        //StartCoroutine(countdownCoro);

    }
    // Update is called once per frame
    void Update()
    {
        //view.RPC("dishSpawnUpdate", RpcTarget.All);
        //view.RPC("spawnUpdate", RpcTarget.All);
        spawnUpdate();

        

        if (count < 1 && spawn == false)
        {
            if (count < 1)
            {
                dishSpawnUpdate();

                StartCoroutine(DishRespawn(dishrespawnSec));
                //view.RPC("DishRespawn", RpcTarget.All, dishrespawnSec);
                DishDespawn.canSpawn = false;
                count++;
            }
            if (count == 1)
            {
                spawn = false;
            }
        }
    }


    #region
    public void spawnUpdate()
    {
        int layerMask = 1 << 3;

        for (int i = 0; i < foodSpawnPoint.Count; i++)
        {
            Collider[] hitColliders = Physics.OverlapSphere(foodSpawnPoint[i].transform.position, 10, layerMask);
            if (hitColliders.Length == 0)
            {
                if (sl_P1PickUp.isPicked == true)
                {
                    StartCoroutine(sl_FoodActive(sec));
                    sl_P1PickUp.isPicked = false;
                }

                if (sl_P2PickUp.isPicked == true)
                {
                    StartCoroutine(sl_FoodActive(sec));

                    sl_P2PickUp.isPicked = false;
                }
            }
        }

    }

    //Mycode
    public IEnumerator sl_FoodActive(int secs)
    {
        if (JPfoodPrefabs.activeSelf == false)
        {
            yield return new WaitForSeconds(secs);
            JPfoodPrefabs.SetActive(true);
        }
        if (KRfoodPrefabs.activeSelf == false)
        {
            yield return new WaitForSeconds(secs);
            KRfoodPrefabs.SetActive(true);
        }
        if (CNfoodPrefabs.activeSelf == false)
        {
            yield return new WaitForSeconds(secs);
            CNfoodPrefabs.SetActive(true);
        }
        if (JPfoodPrefabs.activeSelf == false)
        {
            yield return new WaitForSeconds(secs);
            TWfoodPrefabs.SetActive(true);
        }

    }

    //public IEnumerator Spawn(int secs, int index)
    //{
    //    yield return new WaitForSeconds(secs);

    //    switch (index)
    //    {
    //        case 0:
    //            PhotonNetwork.Instantiate(JPfoodPrefabs[Random.Range(0, JPfoodPrefabs.Count)].name, foodSpawnPoint[0].transform.position, Quaternion.identity);
    //            break;
    //        case 1:
    //            PhotonNetwork.Instantiate(KRfoodPrefabs[Random.Range(0, KRfoodPrefabs.Count)].name, foodSpawnPoint[1].transform.position, Quaternion.identity);
    //            break;
    //        case 2:
    //            PhotonNetwork.Instantiate(CNfoodPrefabs[Random.Range(0, CNfoodPrefabs.Count)].name, foodSpawnPoint[2].transform.position, Quaternion.identity);
    //            break;
    //        case 3:
    //            PhotonNetwork.Instantiate(TWfoodPrefabs[Random.Range(0, TWfoodPrefabs.Count)].name, foodSpawnPoint[3].transform.position, Quaternion.identity);
    //            break;
    //        default:
    //            Debug.Log("unknown spawn point");
    //            break;
    //    }

    //    Debug.Log("spawn at " + index);
    //    Debug.Log("dish spawn");
    //}

    #endregion



    //Dish
    #region
    public void dishSpawnUpdate()
    {
        int layerMask = 1 << 6;

        for (int i = 0; i < dishSpawnPoint.Count; i++)
        {
            Collider[] hitColliders = Physics.OverlapSphere(dishSpawnPoint[i].transform.position, 10, layerMask);
            if (hitColliders.Length == 0)
            {
                if (sl_P1PickUp.isPickedDish == true)
                {
                    StartCoroutine(DishSpawn(dishrespawnSec));
                    sl_P1PickUp.isPickedDish = false;
                }
                if (sl_P2PickUp.isPickedDish == true)
                {
                    StartCoroutine(DishSpawn(dishrespawnSec));
                    sl_P2PickUp.isPickedDish = false;
                }
            }
        }



    }

    public IEnumerator DishCountdown(int countdownTime)
    {
        yield return new WaitForSeconds(countdownTime);

        dishCoro = DishSpawn(dishsec);
        StartCoroutine(dishCoro);
    }


    public IEnumerator DishSpawn(int dishsecs)
    {
        yield return new WaitForSeconds(dishsecs);
        int dishIndex;
        dishIndex = Random.Range(0, dishSpawnPoint.Count);

        if (dishIndex == 0)
        {
            //Japan dish spawn
            PhotonNetwork.Instantiate(JPdishPrefabs[Random.Range(0, JPdishPrefabs.Count)].name, dishSpawnPoint[0].transform.position, Quaternion.identity);
        }
        else if (dishIndex == 1)
        {
            //Korea dish
            PhotonNetwork.Instantiate(KRdishPrefabs[Random.Range(0, KRdishPrefabs.Count)].name, dishSpawnPoint[1].transform.position, Quaternion.identity);
        }
        else if (dishIndex == 2)
        {
            //China dish
            PhotonNetwork.Instantiate(CNdishPrefabs[Random.Range(0, CNdishPrefabs.Count)].name, dishSpawnPoint[2].transform.position, Quaternion.identity);
        }
        else if (dishIndex == 3)
        {
            //Taiwan dish
            PhotonNetwork.Instantiate(TWdishPrefabs[Random.Range(0, TWdishPrefabs.Count)].name, dishSpawnPoint[3].transform.position, Quaternion.identity);
        }

        Debug.Log("dish spawn");

    }


    public IEnumerator DishRespawn(int secs)
    {
        yield return new WaitForSeconds(secs);
        int dishIndex;
        dishIndex = Random.Range(0, dishSpawnPoint.Count);

        if (dishIndex == 0)
        {
            //Japan dish spawn
            PhotonNetwork.Instantiate(JPdishPrefabs[Random.Range(0, JPdishPrefabs.Count)].name, dishSpawnPoint[0].transform.position, Quaternion.identity);
            DishDespawn.canSpawn = false;
            spawn = false;
            count = 0;
        }
        else if (dishIndex == 1)
        {
            //Korea dish
            PhotonNetwork.Instantiate(KRdishPrefabs[Random.Range(0, KRdishPrefabs.Count)].name, dishSpawnPoint[1].transform.position, Quaternion.identity);
            DishDespawn.canSpawn = false;
            spawn = false;
            count = 0;

        }
        else if (dishIndex == 2)
        {
            //China dish
            PhotonNetwork.Instantiate(CNdishPrefabs[Random.Range(0, CNdishPrefabs.Count)].name, dishSpawnPoint[2].transform.position, Quaternion.identity);
            DishDespawn.canSpawn = false;
            spawn = false;
            count = 0;

        }
        else if (dishIndex == 3)
        {
            //Taiwan dish
            PhotonNetwork.Instantiate(TWdishPrefabs[Random.Range(0, TWdishPrefabs.Count)].name, dishSpawnPoint[3].transform.position, Quaternion.identity);
            DishDespawn.canSpawn = false;
            spawn = false;
            count = 0;

        }

        Debug.Log("dish respawn");

    }

    #endregion
}