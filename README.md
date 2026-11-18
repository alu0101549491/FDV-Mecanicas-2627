# FDV-Mecanicas-2627

## Salto

### Ejercicio 1

1. Recupera el Rigidbody2D del objeto y aplica una fuerza vertical al objeto cuando se pulse la tecla que hayas configurado para el salto en el Input. Manager:

```csharp
if ( Input.GetButtonDown("Jump")) {
// Aplica una fuerza vertical hacia arriba para realizar el salto.
        rb2D.AddForce(transform.up * thrust);
	
    }
```

**Resultado:**

```csharp
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }
    }
    
    void FixedUpdate()
    {
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * thrust, ForceMode2D.Impulse);
            jumpRequested = false;
        }
    }
```

2. En este apartado se debe agregar el código que maneja que sólo se salte si el jugador está en el suelo. Para ello se deben agregar una etiqueta al suelo, de forma que nos permita identificarlo y mediante una variable que actuará de flag, activar o desactivar el salto.

```csharp
 if (Input.GetButton("Jump") && !isJumping)
{
    //Saltar
    ................
    //Desactivar la bandera.
    isJumping = true;
}
private void OnCollisionEnter2D(Collision2D other)
{
    //Si el jugador colisiona con un objeto con la etiqueta suelo
    {
        //Activar la bandera para que vuelva a saltar
        isJumping = false;
        //Anular la velocidad remanente que tuviera
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
    }
}
```

**Resultado:**

```csharp
    void FixedUpdate()
    {
        if (isJumping && isGrounded) Jump();

        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * thrust, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            isJumping = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }
```