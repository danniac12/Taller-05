using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    /// <summary>
    /// le damos las propiedades la objeto que lo diferencian
    /// </summary>
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        transform.localScale= new Vector3 (.3f, .3f,.3f);
    }
    /// <summary>
    /// verificamos si el heroe ha recogido el objeto
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hero")
        {
            Hero hero = new Hero();
            hero.hp = hero.hp + 3;
            gameObject.SetActive(false);
        }
    }
}
