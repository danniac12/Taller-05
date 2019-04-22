using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ald = NPC.Ally;

namespace NPC
{
    namespace Enemy
    {
        public class Slime: MonoBehaviour
        {
            Hero hero = new Hero();
            public float edad = 0;
            public float tiempo;
            public float porsuingSpeed;
            public Estado zombieEstado;
            public Vector3 direccion;
            float distanciaA;
            float distanciaH;
            public bool pursuingState = false;
            GameObject Target, heroe;
            GameObject[] aldeanos;

            public enum Estado
            {
                Moving, Idle, Rotating, Pursuing
            }

            // Start is called before the first frame update
            void Start()
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                gameObject.name = "Glob";
                gameObject.tag = "Glob";
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                StartCoroutine(buscaAldeanos());
                porsuingSpeed = 0.02f;
            }
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
            /// y verifica si la vida del heroe es igual a 0 
            /// </summary>
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!pursuingState)
                {
                    if (tiempo >= 3)
                    {
                        zombieEstado = (Estado)Random.Range(0, 3);
                        tiempo = 0;
                    }
                }
                switch (zombieEstado)
                {
                    case Estado.Idle:
                        break;

                    case Estado.Moving:
                        this.gameObject.transform.Translate(0, 0, 0.05f);
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
            /// verifica la collision con heroe y aldeano
            /// </summary>
            /// <param name="collision"></param>
            private void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag == "Villager")
                {
                    collision.gameObject.SetActive(false);
                }

                if (collision.gameObject.tag == "Hero")
                {
                   
                    hero.hp = hero.hp - 1;
                 
                }
            }
            /// <summary>
            /// verifica si a sido golpeado por el arma del heroe
            /// </summary>
            /// <param name="other"></param>
            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.tag == "Weapon")
                {
                    gameObject.SetActive(false);
                    gameObject.tag = "Finish";
                }
            }
        }
    }
}
