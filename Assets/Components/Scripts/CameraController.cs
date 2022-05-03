using UnityEngine;

public class CameraController : MonoBehaviour {
    
    [SerializeField] private GameObject character;
    
    [SerializeField] private float returnSpeed;
    [SerializeField] private float height;
    [SerializeField] private float rearDistance;
    
    private Vector3 currentPosition;

    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 300;
        
        transform.position = new Vector3(character.transform.position.x, character.transform.position.y + height, character.transform.position.z - rearDistance);
        transform.rotation = Quaternion.LookRotation(character.transform.position - transform.position);
    }
    
    private void Update() {
        currentPosition = new Vector3(character.transform.position.x, character.transform.position.y + height, character.transform.position.z - rearDistance);
        transform.position = Vector3.Lerp(transform.position, currentPosition, returnSpeed * Time.deltaTime);
    }
}