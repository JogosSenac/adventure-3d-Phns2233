using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool estaVivo;
    [SerializeField] private int forcaPulo;
    [SerializeField] private float velocidade;
    private bool estaJump;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if(Input.GetKey(KeyCode.D))
        {
            walkLado();
        }
        else if(Input.GetKey(KeyCode.A))
        {
            walkLado();
        }
 if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("walk", false);
            animator.SetBool("tras", false);
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

        if (Input.GetKeyDown(KeyCode.E))
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("pega");
        }
    }

    private void Walk(float velo = 1)
    {
        if ((velo == 1))
        {
            velo = velocidade;
        }
        float moveV = Input.GetAxis("Vertical");
        transform.position += new Vector3(0, 0, moveV * velocidade * Time.deltaTime);
        
    }
    private void walkLado()
    {
        float moveH = Input.GetAxis("Horizontal");
        transform.position += new Vector3(moveH * velocidade * Time.deltaTime,0, 0);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
        estaJump = true;
        animator.SetBool("estaNoChao", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estaJump = false;
            animator.SetBool("estaNoChao", true);
        }
    }
}
