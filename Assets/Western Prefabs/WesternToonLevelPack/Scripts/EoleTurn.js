function Update() {
    transform.Rotate((-Vector3.left *15) * Time.deltaTime, Space.Self);
}