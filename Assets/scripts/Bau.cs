using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bau : MonoBehaviour
{
    [SerializeField] GameObject particula;
    [SerializeField] private bool ehMagico;
    [SerializeField] private int numeroChave;
    [SerializeField] private List<GameObject> itens = new List<GameObject>();
    [SerializeField] private int ouro;
    // Start is called before the first frame update
    void Start()
    {
       if(ehMagico)
       {
        particula.SetActive(true);
        ouro = Random.Range(100, 400);
       } 
       else
       {
        particula.SetActive(false);
        ouro = Random.Range(10,100);
       }
    }
    private void DesativarParticula()
    {
       // particula.SetActive(false);
        particula.GetComponent<ParticleSystem>().Stop();
    }
    public int PegarOuro()
    {
        DesativarParticula();
        StartCoroutine(ZerarBau());
        return ouro;
    }

    IEnumerator ZerarBau()
    {
        yield return new WaitForSeconds(2.5f);
        ouro = 0;
    }
 
}
