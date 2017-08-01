
var bomb : Transform;

function Update() {
    if(Input.GetKeyDown("space")) {
        bomb.parent = null;
        bomb.GetComponent.<Rigidbody>().isKinematic = false;
        bomb.GetComponent.<Rigidbody>().velocity = GetComponent.<Rigidbody>().velocity;
    }
}

function FixedUpdate () {
    GetComponent.<Rigidbody>().AddForce(Vector3.forward);
}
