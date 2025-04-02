using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public Transform[] backgrounds; // Array der verschiedenen Ebenen
    public float[] parallaxScales; // Bestimmt, wie stark jede Ebene scrollt
    public float smoothing = 1f; // Glättung des Effekts (Muss noch ordentlich implementiert werden)

    private Vector3 previousCamPos;

    void Start()
    {
        previousCamPos = Camera.main.transform.position;

        if (parallaxScales.Length != backgrounds.Length)
        {
            parallaxScales = new float[backgrounds.Length];
            for (int i = 0; i < parallaxScales.Length; i++)
            {
                parallaxScales[i] = backgrounds[i].position.z * -1;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - Camera.main.transform.position.x) * parallaxScales[i];
            float targetPosX = backgrounds[i].position.x + parallax;

            Vector3 targetPos = new Vector3(targetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, targetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = Camera.main.transform.position;
    }
}
