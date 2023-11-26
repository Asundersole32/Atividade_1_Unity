using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{

    bool jump = false;

    bool canDash = true;

    bool isDashing = false;

    bool isGrounded = false;
    
    public float dashSpeedRight = 24f;

    public float dashSpeedLeft = -24f;

    public float speed = 0.3f;

    public float jumpHeight = 7f;

    private float dashTime = 0.2f;

    private float dashCooldown = 1f;

    private TrailRenderer tr;

    private Rigidbody2D rb;

    void Start()
    {
        tr = GetComponent<TrailRenderer>(); //Função que chama o componente do Trail Renderer, que pode ser aplicado em todo código

        rb = GetComponent<Rigidbody2D>(); //Função que chama o componente do Rigid Body 2D, que pode ser aplicado em todo código
    }

    void Update()
    {

        if(isDashing) 
        
        {

            return; //Nenhum comando será executado durante o Dash

        }

        if (Input.GetButtonDown("Fire2") && canDash)
        
        {

            StartCoroutine(Dash(dashSpeedRight)); //Se clicar com o botão direito do mouse, o dash será para direita    

        }

        if (Input.GetButtonDown("Fire1") && canDash)
        
        {

            StartCoroutine(Dash(dashSpeedLeft)); //Se clicar com o botão esquerdo do mouse, o dash será para esquerda

        }
       
    }

    private void FixedUpdate()
    {

        if (Input.GetKey("right"))
        
        {

            Debug.Log("Direita!");

            rb.AddForce(transform.right * speed, ForceMode2D.Impulse); //Se aperdar seta direita, se movimenta para direita

        }
        
        else if (Input.GetKey("left"))
        
        {
            
            Debug.Log("Esquerda!");

            rb.AddForce(transform.right * -speed, ForceMode2D.Impulse); //Se não, se apertar a seta esquerda, vai para esquerda

        }

        if (Input.GetKey("up"))
        
        {

            if (jump)
            {

                Debug.Log("Pulou!");

                jump = false; //Disabilita a opção de pular durante o pulo

                isGrounded = false; //Indica se está no chão ou não

                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse); //Se apertar a seta pra cima, pula

            }
            
        }

        if (jump)
        {
        
            if (Input.anyKey == false) //Se não há nenhuma tecla ou botão presionado e não está pulando, transforma a velocidade na horizontal em 0
            {

                var velocity = rb.velocity;

                velocity.x = 0.0f; 

                rb.velocity = velocity;

            }

        }

    }

    void OnCollisionEnter2D(Collision2D col) //Caso colida com algo, o pulo é habilitado e é indicado que o boneco está no chão(ou na parede)
    {
        
        isGrounded = true;

        jump = true;

    }

    private IEnumerator Dash(float dashSpeedDirection) //Função responsavel pelo dash
    {

        isDashing = true; //Indica que esta no dash

        canDash = false; //Desabilita dar dash enquanto está no dash

        float originalGravity = rb.gravityScale;

        rb.gravityScale = 0f; //desabilita a gravidade no dash

        rb.velocity = new Vector2(transform.localScale.x * dashSpeedDirection, 0f);
        
        tr.emitting = true; //habilita o traçado do dash

        yield return new WaitForSeconds(dashTime); // espera o tempo do dash acabar

        rb.velocity = new Vector2(0, 0);

        tr.emitting = false; //desabilita o traçado do dash

        rb.gravityScale = originalGravity; //retorna a gravidade

        isDashing = false;

        Debug.Log("Dash Carregando!");

        yield return new WaitForSeconds(dashCooldown); //dá um tempo de 1 segundo para outro dash

        Debug.Log("Dash Pronto!");

        canDash = true; //Habilita o dash

    }

}
