using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ald = NPC.Ally;
using UnityEngine.SceneManagement;

namespace NPC
{
    namespace Enemy
    {
        public class Zombie : MonoBehaviour
        {

            Hero hero = new Hero();
            public InfoZombie infoZombie;
            bool infected = false;
            int rndColor;
            public string Gus;
            public float edad;
            public float tiempo;
            public bool mirar = false;
            public float porsuingSpeed;
            public Estado zombieEstado;
            public Vector3 direccion;
            float distanciaA;
            float distanciaH;
            public bool pursuingState = false;
            GameObject Target, heroe;
            GameObject[] aldeanos;

            // gustos del zombie
            public enum Gusto
            {
                Cabezas, Dedos, Lenguas, Higados, Muslos
            }

            //estados del zombie
            public enum Estado
            {
                Moving, Idle, Rotating, Pursuing
            }
            /// <summary>
            ///  asignamos la informacion proncipal del zombie
            /// </summary>

            void Start()
            {
                if (!infected)
                {
                    edad = (int)Random.Range(15, 101);
                    infoZombie = new InfoZombie();
                    rndColor = Random.Range(0, 3);
                    Rigidbody Zom;
                    Zom = this.gameObject.AddComponent<Rigidbody>();
                    Zom.constraints = RigidbodyConstraints.FreezeAll;
                    Zom.useGravity = false;
                    this.gameObject.name = "Zombie";
                }
                else
                {
                    edad = infoZombie.edad;
                    this.gameObject.name = infoZombie.nombre;
                }
                StartCoroutine(buscaAldeanos());
                porsuingSpeed = 10 / edad;
                this.gameObject.tag = "Zombie";
                Gusto gusto;
                gusto = (Gusto)Random.Range(0, 5);
                Gus = gusto.ToString();
                infoZombie.gusto = Gus;
                if (rndColor == 0)
                {
                    this.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                }
                if (rndColor == 1)
                {
                    this.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                }
                if (rndColor == 2)
                {
                    this.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
            }
            /// <summary>
            /// se realiza la busqueda de los aldeanos y heroe cercanos
            /// </summary>
            /// <returns></returns>
            IEnumerator buscaAldeanos()
            {
                heroe = GameObject.FindGameObjectWithTag("Hero");
                aldeanos = GameObject.FindGameObjectsWithTag("Villager");
                foreach (GameObject item in aldeanos)
                {
                    yield return new WaitForEndOfFrame();
                    ald.Villager componenteAldeano = item.GetComponent<ald.Villager>();
                    if (componenteAldeano != null)
                    {
                        distanciaH = Mathf.Sqrt(Mathf.Pow((heroe.transform.position.x - transform.position.x), 2) + Mathf.Pow((heroe.transform.position.y - transform.position.y), 2) + Mathf.Pow((heroe.transform.position.z - transform.position.z), 2));
                        distanciaA = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                        if (!pursuingState)
                        {

                            if (distanciaA < 5f)
                            {
                                zombieEstado = Estado.Pursuing;
                                Target = item;
                                pursuingState = true;
                            }
                            else if (distanciaH < 5f)
                            {
                                zombieEstado = Estado.Pursuing;
                                Target = heroe;
                                pursuingState = true;
                            }
                        }
                        if (distanciaA < 5f && distanciaH < 5f)
                        {
                            Target = item;
                        }
                    }
                }

                if (pursuingState)
                {
                    if (distanciaA > 5f && distanciaH > 5f)
                    {
                        pursuingState = false;
                    }
                }

                yield return new WaitForSeconds(0.1f);
                StartCoroutine(buscaAldeanos());
            }
            /// <summary>
            /// Cambia aleatoreamente los estados del zombie
            /// </summary>
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!pursuingState)
                {
                    if (tiempo >= 3)
                    {
                       zombieEstado =(Estado)Random.Range(0, 3);
                        mirar = true;
                        tiempo = 0;
                    }
                }
                switch (zombieEstado)
                {
                    case Estado.Idle:
                        break;

                    case Estado.Moving:
                        if (mirar)
                        {
                            this.gameObject.transform.Rotate(0, Random.Range(0, 361), 0);
                        }
                        this.gameObject.transform.Translate(0, 0, 0.05f);
                        mirar = false;
                        break;

                    case Estado.Rotating:
                        this.gameObject.transform.Rotate(0, Random.Range(1, 50), 0);
                        break;

                    case Estado.Pursuing:
                        direccion = Vector3.Normalize(Target.transform.position - transform.position);
                        transform.position += direccion * porsuingSpeed;
                        break;
                }
                if (hero.hp == 0)
                {
                    SceneManager.LoadScene(0);
                }
            }
            /// <summary>
            /// collision se encarga de la infeccion de los aldeanos y la de restarle vida al heroe
            /// </summary>
            private void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag == "Villager")
                {
                    collision.gameObject.AddComponent<Zombie>().infoZombie = collision.gameObject.GetComponent<ald.Villager>().infoAlde;
                    collision.gameObject.GetComponent<Zombie>().infected = true;
                    Destroy(collision.gameObject.GetComponent<NPC.Ally.Villager>());
                }

                if (collision.gameObject.tag == "Hero")
                {
                    hero.hp -= 1; 
                }
            }
            /// <summary>
            /// verifica si el heroe ha golpeado al zombie
            /// </summary>
            /// <param name="other"></param>
            private void OnTriggerEnter(Collider other)
            {
                if(other.gameObject.tag == "Weapon")
                {
                    gameObject.SetActive(false);
                    gameObject.tag = "Finish";
                }
            }
        }
    }
}


