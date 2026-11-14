using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
public class JugadorMorgue : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 4.5f;
    public float gravedad = -9.81f;

    [Header("Cámara (Primera Persona)")]
    public Transform camaraJugador;
    public float sensibilidadMouse = 15f;

    private CharacterController controller;
    private Vector3 velocidadVertical;
    private float rotacionX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MoverJugador();
        RotarCamara();
    }

    private void MoverJugador()
    {
        float movX = 0f;
        float movZ = 0f;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) movZ += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) movZ -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) movX += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) movX -= 1f;
        }
#else
        movX = Input.GetAxis("Horizontal");
        movZ = Input.GetAxis("Vertical");
#endif

        Vector3 direccion = (transform.right * movX + transform.forward * movZ).normalized;
        controller.Move(direccion * velocidad * Time.deltaTime);

        if (controller.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }

        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    private void RotarCamara()
    {
        if (camaraJugador == null) return;

        float mouseX = 0f;
        float mouseY = 0f;

#if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            mouseX = delta.x * sensibilidadMouse * Time.deltaTime;
            mouseY = delta.y * sensibilidadMouse * Time.deltaTime;
        }
#else
        mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * 10f * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * 10f * Time.deltaTime;
#endif

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -85f, 85f);

        camaraJugador.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}