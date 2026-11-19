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

### Ejercicio 2

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

### Ejercicio 3

3. Opcional: Verificar si está en el suelo utilizando un Raycast. Para ello, lanzar el rayo desde los pies del jugador hasta el suelo, y si hay impacto con un objeto en la capa suelo en un rango muy corto, se considera que el jugador está en el suelo.

**Resultado:**

```csharp
    void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        if (isJumping && isGrounded) Jump();

        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
    }

    private bool CheckGrounded()
    {
        Vector2 origin;

        if (groundCheck != null)
        {
            origin = groundCheck.position;
        }
        else if (playerCollider != null)
        {
            origin = new Vector2(
                playerCollider.bounds.center.x,
                playerCollider.bounds.min.y
            );
        }
        else
        {
            origin = transform.position;
        }

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        return hit.collider != null;
    }
```

![Salto](Media/Salto.gif)

## Plataformas

1. Salto a una plataforma: Esta mecánica se consigue con objetos físicos puesto que hay que implementar un salto, sin embargo, en este caso es necesario detectar la colisión con la plataforma y convertir el sistema de referencia del jugador en hijo de la plataforma. De esta forma conseguiremos que el jugador se mueva sobre/con ella. Del mismo modo una vez que el jugador no esté sobre la plataforma, debemos desvincularla de ella. Esto lo manejaremos cuando se produzca un evento. Desarrollar la versión para plataforma estática y plataforma en movimiento.

```csharp
void OnCollisionExit2D(Collision2D collision)
```

**Resultado:**

```csharp
public class PlatformAttachment2D : MonoBehaviour
{
    [SerializeField] private string platformTag = "Platform";
    [SerializeField] private bool requireTopContact = true;
    [SerializeField, Min(0f)] private float minimumUpwardNormal = 0.5f;

    private Transform currentPlatform;
    private Transform originalParent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryAttachToPlatform(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (currentPlatform == null) TryAttachToPlatform(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == currentPlatform) DetachFromPlatform();
    }

    private void OnDisable()
    {
        DetachFromPlatform();
    }

    private void TryAttachToPlatform(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(platformTag))       return;
        if (requireTopContact && !IsContactFromAbove(collision)) return;
        if (currentPlatform == null) originalParent = transform.parent;

        Transform platform = collision.transform;

        if (currentPlatform != platform)
        {
            transform.SetParent(platform, true);
            currentPlatform = platform;
        }
    }

    private bool IsContactFromAbove(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;
        foreach (ContactPoint2D contact in contacts)
        {
            if (Vector2.Dot(contact.normal, Vector2.up) >= minimumUpwardNormal)
            {
                return true;
            }
        }

        return false;
    }

    private void DetachFromPlatform()
    {
        if (currentPlatform == null) return;

        transform.SetParent(originalParent, true);
        currentPlatform = null;
        originalParent = null;
    }
}
```

![Plataformas](Media/Plataformas.gif)

## Colisiones con Capas

1. Manejar colisiones con elementos de una capa determinada: Podemos utilizar las capas para que el efecto de las colisiones sólo se tenga en cuenta cuando se pertenece a una determinada capa, o se descarten.

```csharp
void OnCollisionExit2D(Collision2D collision) {
    if (collision.gameObject.layer != LayerMask.NameToLayer("NoCollis")) {
        //Lógica para los elementos que si colisionan.
    }
}
```

**Resultado:**

```csharp
public class EnemyCollision2D : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask ignoredLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldHandleCollision(collision))
        {
            Debug.Log("Colisión con un enemigo: " + collision.gameObject.name);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (ShouldHandleCollision(collision))
        {
            Debug.Log("Fin de la colisión con un enemigo: " + collision.gameObject.name);
        }
    }

    private bool ShouldHandleCollision(Collision2D collision)
    {
        if (ignoredLayer >= 0 && collision.gameObject.layer == ignoredLayer)
        {
            return false;
        }

        int collisionLayer = 1 << collision.gameObject.layer;
        return (enemyLayer.value & collisionLayer) != 0;
    }
}
```

![Colisiones con Capas](Media/Colisiones.gif)

