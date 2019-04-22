using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zom = NPC.Enemy;
using ald = NPC.Ally;


public class Taller : MonoBehaviour
{
    public Text numeroZombies;
    public  Text numeroAldeanos;
    public int numZombies;
    public int numAldeanos;
    public GameObject[] zom, ald;
    /// <summary>
    /// llama al metodo que realiza las instancias
    /// </summary>
    void Start()
    {
       new Creator();
    }

    /// <summary>
    /// Se asignan la cantidad a mostrar en los textos del canvas
    /// </summary>
    void Update()
    {
        zom = GameObject.FindGameObjectsWithTag("Zombie");
        ald = GameObject.FindGameObjectsWithTag("Villager");
        foreach (GameObject item in zom)
        {
            numZombies = zom.Length;
        }
        foreach (GameObject item in ald)
        {
            numAldeanos = ald.Length;
        }

        if (ald.Length == 0)
        {
            numeroAldeanos.text = 0.ToString();
        }
        else
        {
            numeroAldeanos.text = numAldeanos.ToString();
        }

        numeroZombies.text = numZombies.ToString();
    }
}
/// <summary>
/// Se generan todas las instancias del jeugo
/// </summary>
public class Creator
{
    public GameObject go;
    public readonly int minInstancias = Random.Range(5, 16);
    int unic = 0;
    const int MAX = 26;
    public Creator()
    {
        for (int i = 0; i < Random.Range(minInstancias, MAX); i++)
        {
            if (unic == 0)
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<Camera>();
                go.AddComponent<Hero>();
                unic += 1;
            }

            int selec = Random.Range(unic, 5);

            if (selec == 1)
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<ald.Villager>();
                go.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
            if (selec == 2)
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<zom.Zombie>();
                go.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
            if(selec == 3)
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<zom.Slime>();
                go.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
            if(selec == 4)
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<Box>();
                go.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
        }
    }
}
