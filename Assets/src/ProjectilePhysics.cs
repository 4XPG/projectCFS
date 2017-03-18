// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class ProjectilePhysics : MonoBehaviour {

// Componants
public Transform flyer;
public Rigidbody flyerRigidbody;


// Assorted control variables. These mostly handle realism settings, change as you see fit.
    public float accelerateConst = 5;             // Set these close to 0 to smooth out acceleration. Don't set it TO zero or you will have a division by zero error.
    public float decelerateConst = 0.065f; // I found this value gives semi-realistic deceleration, change as you see fit.
    
    /*  The ratio of MaxSpeed to Speed Const determines your true max speed. The formula is maxSpeed/SpeedConst = True Speed. 
        This way you wont have to scale your objects to make them seem like they are going fast or slow.
        MaxSpeed is what you will want to use for a GUI though.
    */ 
    public static float maxSpeed = 100;      
    public float speedConst = 50;
    private float objectdrag;
    public int throttleConst = 50;
    public float liftConst = 7.5f;                    // Another arbitrary constant, change it as you see fit.
    public float angleOfAttack = 15;         // Effective range: 0 <= angleOfAttack <= 20
    private float gravityConst= 9.8f;             // An arbitrary gravity constant, there is no particular reason it has to be 9.8f...
    public int levelFlightPercent = 25;
    public float maxDiveForce = 0.1f;
    public float noseDiveConst = 0.01f;
    public float minSmooth = 0.5f;
    public float maxSmooth = 500;
    public float maxControlSpeedPercent = 75;        // When your speed is withen the range defined by these two variables, your ship's rotation sensitivity fluxuates.
    public float minControlSpeedPercent = 25;        // If you reach the speed defined by either of these, your ship has reached it's max or min sensitivity.


// Rotation Variables, change these to give the effect of flying anything from a cargo plane to a fighter jet.
    public bool  lockRotation;     // If this is checked, it locks pitch roll and yaw constants to the var rotationConst.
    public int lockedRotationValue = 120;
    public float RollInput { get; private set; }
    public float PitchInput { get; private set; }
    public float YawInput { get; private set; }    
    public float ThrottleInput = 100;   
    public int pitchConst= 100;
    public int rollConst= 100;
    public int yawConst= 100;

// Airplane Aerodynamics - I strongly reccomend not touching these...
    private float nosePitch;
    private float trueSmooth;
    private float smoothRotation;
    private float truePitch;
    private float trueRoll;
    private float trueYaw;
    private float trueThrust;
    static float trueDrag;
    
// Misc. Variables
    static int altitude;

    
// HUD and Heading Variables. Use these to create your insturments.
    static float trueSpeed;
    static float attitude;
    static float incidence;
    static float bank;
    static float heading;


// Let the games begin!
void  Start (){
    trueDrag = 0;
    objectdrag = flyerRigidbody.drag;
    smoothRotation = minSmooth + 0.01f;
    if (lockRotation == true)
        {
        pitchConst = lockedRotationValue;
        rollConst = lockedRotationValue;
        yawConst = lockedRotationValue;
        Cursor.visible = false;
        }
}


void  Update (){

    // * * This section of code handles the plane's rotation.

    float pitch = PitchInput * pitchConst;
    float roll = RollInput * rollConst;
    float yaw = YawInput * yawConst;
    float throttle = ThrottleInput;
    
    // Smothing Rotations...    
    if ((smoothRotation > minSmooth)&&(smoothRotation < maxSmooth))
    {
        smoothRotation = Mathf.Lerp (smoothRotation, trueThrust, (maxSpeed-(maxSpeed/minControlSpeedPercent))* Time.deltaTime);
    }
    if (smoothRotation <= minSmooth)
    {
        smoothRotation = smoothRotation +0.01f;
    }
    if ((smoothRotation >= maxSmooth) &&(trueThrust < (maxSpeed*(minControlSpeedPercent/100))))
    {
        smoothRotation = smoothRotation -0.1f;
    }
    trueSmooth = Mathf.Lerp (trueSmooth, smoothRotation, 5* Time.deltaTime);
    truePitch = Mathf.Lerp (truePitch, pitch, trueSmooth * Time.deltaTime);
    trueRoll = Mathf.Lerp (trueRoll, roll, trueSmooth * Time.deltaTime);
    trueYaw = Mathf.Lerp (trueYaw, yaw, trueSmooth * Time.deltaTime);



    
// * * This next block handles the thrust and drag.
    

    if ( throttle/speedConst >= trueThrust)
    {
        trueThrust = Mathf.SmoothStep (trueThrust, throttle/speedConst, accelerateConst * Time.deltaTime);
    }
    if (throttle/speedConst < trueThrust)
    {
        trueThrust = Mathf.Lerp (trueThrust, throttle/speedConst, decelerateConst * Time.deltaTime);
    }   
    objectdrag = liftConst*((trueThrust)*(trueThrust));
    if (trueThrust <= (maxSpeed/levelFlightPercent))
    {
        
        nosePitch = Mathf.Lerp (nosePitch, maxDiveForce, noseDiveConst * Time.deltaTime);
    }
    else
    {
        
        nosePitch = Mathf.Lerp (nosePitch, 0, 2* noseDiveConst * Time.deltaTime);
    }
    
    trueSpeed = ((trueThrust/2)*maxSpeed);
    
// ** Additional Input

    // Airbrake
/*    if (Input.GetButton ("Airbrake"))
    {
        trueDrag = Mathf.Lerp (trueDrag, trueSpeed, raiseFlapRate * Time.deltaTime);        
        
    }
    
    if ((!Input.GetButton ("Airbrake"))&&(trueDrag !=0))
    {
        trueDrag = Mathf.Lerp (trueDrag, 0, lowerFlapRate * Time.deltaTime);
    }
    */
    
    // Afterburner
    /*if (Input.GetButton ("Afterburner"))
    {
        afterburnerConst = Mathf.Lerp (afterburnerConst, maxAfterburner, afterburnerAccelerate * Time.deltaTime);       
    }
    
    if ((!Input.GetButton ("Afterburner"))&&(afterburnerConst !=0))
    {
        afterburnerConst = Mathf.Lerp (afterburnerConst, 0, afterburnerDecelerate * Time.deltaTime);
    }*/
    
    
    // Adding nose dive when speed gets below a percent of your max speed   
    if ( ((trueSpeed - trueDrag)) <= (maxSpeed*levelFlightPercent/100))
    {
        noseDiveConst = Mathf.Lerp (noseDiveConst,maxDiveForce, (((trueSpeed - trueDrag)) - (maxSpeed * levelFlightPercent/100)) *5 *Time.deltaTime);
        flyer.Rotate(noseDiveConst,0,0,Space.World);        
    }
    
    
    // Calculating Flight Mechanics. Used mostly for the HUD.
/*    attitude = float.Parse(-((Vector3.Angle(Vector3.up, flyer.forward))-90));
    bank = float.Parse(((Vector3.Angle(Vector3.up, flyer.up))));   
    incidence = attitude + angleOfAttack;
    heading = float.Parse(flyer.eulerAngles.y);*/
    //Debug.Log ((((trueSpeed - trueDrag) + afterburnerConst) - (maxSpeed * levelFlightPercent/100)));
}   // End function Update( );


void  FixedUpdate (){
    if (trueThrust <= maxSpeed)
    {
        // Horizontal Force
        transform.Translate(0,0,((trueSpeed - trueDrag)/100));
    }
    flyerRigidbody.AddForce (0,(objectdrag - gravityConst),0);
    transform.Rotate (truePitch,-trueYaw,trueRoll);

}
}

