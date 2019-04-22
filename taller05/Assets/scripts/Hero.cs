using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zom = NPC.Enemy;
using ald = NPC.Ally;

public class Hero : MonoBehaviour
{
    float distancia;
    float distanciaZ;
    public float time;
    public Text textoZombie;
    public Text textoAldeano;
    public Text hitPoints;
    GameObject[] aldeanos, zombie;
    InfoAlde infoAlde = new InfoAlde();
    InfoZombie infoZombie = new InfoZombie();
    Movement movement;
    Look look;
    public GameObject sword;
    public int hp;

    /// <summary>
    /// asignamos la informacion proncipal del heroe
    /// </summary>
    void Start()
    {
        sword = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        sword.transform.position = new Vector3(this.gameObject.transform.position.x + .313f, this.gameObject.transform.position.y - .224f, this.gameObject.transform.position.z + .232f);
        sword.transform.localScale = new Vector3(.3f, .7f, .3f);
        sword.transform.Rotate(90, 0, 0);
        sword.transform.SetParent(this.gameObject.transform);
        sword.GetComponent<Renderer>().material.color = Color.grey;
        sword.tag = "Weapon";
        sword.GetComponent<Collider>().isTrigger = true;
        Rigidbody hero = this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.tag = "Hero";
        this.gameObject.name = "Hero";
        hero.constraints = RigidbodyConstraints.FreezeAll;
        hero.useGravity = false;
        StartCoroutine(BuscaEntidades());
        movement = gameObject.AddComponent<Movement>();
        look = gameObject.AddComponent<Look>();
        textoZombie = GameObject.FindGameObjectWithTag("TextZombie").GetComponent<Text>();
        textoAldeano = GameObject.FindGameObjectWithTag("TextAlde").GetComponent<Text>();
        hp = 10;
    }

    /// <summary>
    /// contamos un tiempo
    /// </summary>
    public void Update()
    {
        time += Time.fixedDeltaTime;
        movement.Move();
        look.Arround();
        hitPoints.text = hp.ToString();
    }

    /// <summary>
    /// calculamos la distancia de los objetos con respecto al heroe para hacer la retroalimentacon por medio del canvas 
    /// </summary>
    /// <returns></returns>
    IEnumerator BuscaEntidades()
    {
        zombie = GameObject.FindGameObjectsWithTag("Zombie");
        aldeanos = GameObject.FindGameObjectsWithTag("Villager");

        // retroalimentacion para el aldeano
        foreach (GameObject item in aldeanos)
        {
            yield return new WaitForEndOfFrame();
           ald.Villager componenteAldeano = item.GetComponent<ald.Villager>();
            if (componenteAldeano != null)
            {
                distancia = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                if (distancia < 5f)
                {
                    time = 0;
                    infoAlde = item.GetComponent<ald.Villager>().infoAlde;
                    textoAldeano.text = "Hola, soy " + infoAlde.name + " y tengo " + infoAlde.edad.ToString() + " años";
                }
                if (time > 3)
                {
                    textoAldeano.text = " ";
                }
            }
        }

        // retroalimentacion para el zombie
        foreach (GameObject itemZ in zombie)
        {
            yield return new WaitForEndOfFrame();
            zom.Zombie componenteZombie = itemZ.GetComponent<zom.Zombie>();
            if (componenteZombie != null)
            {
                distanciaZ = Mathf.Sqrt(Mathf.Pow((itemZ.transform.position.x - transform.position.x), 2) + Mathf.Pow((itemZ.transform.position.y - transform.position.y), 2) + Mathf.Pow((itemZ.transform.position.z - transform.position.z), 2));
                if (distanciaZ < 5f)
                {
                    time = 0;
                    infoZombie = itemZ.GetComponent<zom.Zombie>().infoZombie;
                    textoZombie.text = "Waaaarrrr quiero comer " + infoZombie.gusto;
                }
                if (time > 3)
                {
                    textoZombie.text = " ";
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(BuscaEntidades());
    }
   
}

public class Movement : MonoBehaviour
{
    /// <summary>
    /// se toma el tranform del objeto que tiene el script
    /// </summary>
    Transform movableTransform;

    private void Awake()
    {
        movableTransform = transform;
    }
    /// <summary>
    /// se obtiene un flotante que determina la velocidad con la que se movera el objeto que tiene este script
    /// y se le asignan input del te clado con los que se activaran esta acciones
    /// </summary>
    /// <param name="speedChange"></param>
    public void Move()
    {
        Move move = new Move(Random.Range(0.09f, 0.19f));

        if (Input.GetKey(KeyCode.W))
        {
            movableTransform.Translate(0, 0, move.speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            movableTransform.Translate(0, 0, -move.speed);
        }

    }

}
public class Look : MonoBehaviour
{
    /// <summary>
    /// se toma el tranform del objeto que tiene el script
    /// </summary>
    Transform movableTransform;

    private void Awake()
    {
        movableTransform = transform;
    }

    /// <summary>
    /// se le asignan input del teclado con los que se activaran la accion de rotar la mira del personaje 
    /// </summary>

    public void Arround()
    {
        if (Input.GetKey(KeyCode.A))
        {
            movableTransform.Rotate(0, -1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movableTransform.Rotate(0, 1, 0, 0);
        }

    }
}
public class Move
{
    public readonly float speed;
    public Move(float veloz)
    {
        speed = veloz;
    }

}



