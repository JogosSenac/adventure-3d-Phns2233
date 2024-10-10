using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool estaVivo;
    [SerializeField] private int forcaPulo;
    [SerializeField] private float velocidade;
    [SerializeField] private bool temChave;
    private bool estaJump;
    private int ouro;
    private int vida;
    private Rigidbody rb;
    private Vector3 angeleRotation;
    [SerializeField] private bool pegando;
    [SerializeField] private bool podePegar;
    //inventario
    [SerializeField] private List<GameObject> inventario = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        angeleRotation = new Vector3(0, 90, 0);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        temChave = false;
        pegando = false;
    }

    // Update is called once per frame
    void Update()
    {
        TurnAround();

        if (Input.GetKeyDown(KeyCode.E) && podePegar)//siguinifica ou ||
        {
            animator.SetTrigger("pega");
            pegando = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("walk", true);
            animator.SetBool("tras", false);
            Walk();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("tras", true);
            animator.SetBool("walk", false);
            Walk();
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("tras", false);
        }

        if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("walk", false);
            animator.SetBool("tras", false);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("walk", true);
        }


        //pulo
        if (Input.GetKeyDown(KeyCode.Space) && !estaJump)
        {
            animator.SetTrigger("jump");
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("attackDois");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("block");
        }

        if (!estaVivo)
        {
            animator.SetTrigger("estaVivo");
            estaVivo = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("attack3");
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("correndo", true);
            Walk(3);
        }
        else
        {
            animator.SetBool("correndo", false);
        }
    }

    private void Walk(float velo = 1)
    {
        if ((velo == 1))
        {
            velo = velocidade;
        }
        float fowardInput = Input.GetAxis("Vertical");
        //transform.position += new Vector3(0, 0, moveV * velocidade * Time.deltaTime);
        Vector3 moveDirection = transform.forward * fowardInput;
        Vector3 moveForward = rb.position + moveDirection * velo * Time.deltaTime;
        rb.MovePosition(moveForward);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
        estaJump = true;
        animator.SetBool("estaNoChao", false);
    }
    private void TurnAround()
    {
        float sideInpot = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(angeleRotation * sideInpot * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estaJump = false;
            animator.SetBool("estaNoChao", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.tag);


        if (Input.GetKeyDown(KeyCode.E))//siguinifica ou ||
        {
            animator.SetTrigger("pega");
            pegando = true;
        }

        if (other.gameObject.CompareTag("chave") && pegando)
        {
            inventario.Add(Instantiate(other.gameObject.GetComponent<Chave>().CopiaDoObjeto()));
            int numero = other.gameObject.GetComponent<Chave>().PegarNumeroChave();
            Debug.LogFormat($"chave numero: {numero} inserida no inventario");
            Destroy(other.gameObject);
            podePegar = false;
            pegando = false;
        }
        if (other.gameObject.CompareTag("porta") && pegando && temChave)
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("abrindo");
            temChave = false;
        }

        if (other.gameObject.CompareTag("bau") && pegando)
        {
            if (VerificaChave(other.gameObject.GetComponent<Bau>().PegarNumeroFechadura()))
            {
                other.gameObject.GetComponent<Animator>().SetTrigger("temChave");
            }

            else
            {
                Debug.Log("você não tem a chave");
            }
        }
    }

    private bool VerificaChave(int chave)
    {
        foreach (GameObject item in inventario)
        {
            if (item.gameObject.CompareTag("chave"))
            {
                if (item.gameObject.GetComponent<Chave>().PegarNumeroChave() == chave)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void PegarConteudoBau(GameObject bau)
    {
        Bau bauTesouro = bau.GetComponent<Bau>();
        ouro = bauTesouro.PegarOuro();
        if (bauTesouro.AcesarConteudoBau() != null)
        {
            foreach (GameObject item in bauTesouro.AcesarConteudoBau())
            {
                inventario.Add(item);
            }
        }
    }
}