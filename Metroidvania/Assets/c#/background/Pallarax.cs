using UnityEngine;
using Cinemachine;

public class Pallarax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float parallaxFactor;
    }

    public ParallaxLayer[] parallaxLayers;
    public float smoothing = 1f;

    private Vector3 previousCameraPosition;
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        previousCameraPosition = virtualCamera.transform.position;
    }

    void Update()
    {
        Vector3 deltaMovement = virtualCamera.transform.position - previousCameraPosition;

        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            float parallaxSpeed = parallaxLayers[i].parallaxFactor;
            Vector3 parallaxMovement = new Vector3(deltaMovement.x * parallaxSpeed, deltaMovement.y * parallaxSpeed, 0);
            Vector3 targetPosition = parallaxLayers[i].layerTransform.position + parallaxMovement;

            parallaxLayers[i].layerTransform.position = Vector3.Lerp(
                parallaxLayers[i].layerTransform.position,
                targetPosition,
                smoothing * Time.deltaTime
            );
        }

        previousCameraPosition = virtualCamera.transform.position;
    }
}
