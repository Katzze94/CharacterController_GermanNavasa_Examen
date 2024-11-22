using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
   private CharacterController _controller;

   private Transform _camera;

   private Animator _animator;
   
   
   
   
   
   //----------------------------Movimiento------------------

   [SerializeField] private float _movementSpeed = 10;

     private float _vertical;

    private float _horizontal;
    [SerializeField] private float _jumpHeight = 5;

     private float _turnSmoothVelocity;

     [SerializeField] private float _turnSmoothTime = 0.5f;


   //----------------------------Gravity---------------------¨
   [SerializeField] private float _gravity = -9.81f;

   [SerializeField] private Vector3 _playerGravity;



   //-----------------------------GorundSensor---------------

     [SerializeField] Transform _sensorPosition;

    [SerializeField] LayerMask _groundLayer;

    [SerializeField] float _sensorRadius =0.5f;

   
   
   
   //-----------------------------Funciones-----------------------


void Awake()
{
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main.transform;
}

void Update()
{
    _horizontal = Input.GetAxis("Horizontal");
    _vertical = Input.GetAxis("Vertical");



    Movement();


    if(Input.GetButtonDown("Jump") && IsGrounded()) 
    {
    Jump();
    }

    Gravity();
    
}




void Movement()
{

 Vector3 direction = new Vector3(_horizontal, 0, _vertical);  

        _animator.SetFloat("VelZ", direction.magnitude);
        _animator.SetFloat("VelX", 0);

        if(direction != Vector3.zero) 
        {
      
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y; 
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        Vector3  moveDirection = Quaternion.Euler(0, targetAngle, 0)*Vector3.forward;

        _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
       
        }

}

bool IsGrounded()
{
    return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
}

void Gravity()
{

    if(!IsGrounded())
    {
    _playerGravity.y += _gravity *Time.deltaTime;
    }
    else if(IsGrounded() && _playerGravity.y < 0)
    {

     _playerGravity.y = -1;

    _animator.SetBool("IsJumping", false);
    }

    _controller.Move(_playerGravity * Time.deltaTime);


}


void Jump()
{
   _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);

    _animator.SetBool("IsJumping", true);
}


void OnDrawGizmos() 

    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);
    }

   
}
