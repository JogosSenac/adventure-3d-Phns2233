using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chave : MonoBehaviour
{
    [SerializeField] private int numeroChave;
    [SerializeField] GameObject particula;
    
    public int PegarNumeroChave()
    {
        return numeroChave;
    }

    public GameObject CopiaDoObjeto()//no caso a chave
    {
        gameObject.SetActive(false);
        return gameObject;
    }
     private void DesativarParticulaChave()
    {
        // particula.SetActive(false);
        particula.GetComponent<ParticleSystem>().Stop();
    }
}
