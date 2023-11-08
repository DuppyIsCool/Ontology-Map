using UnityEngine;

namespace Mirror.Examples.AdditiveScenes
{
    public class SetPlayerColor : NetworkBehaviour
    {
        public override void OnStartServer()
        {
            color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        // Color32 packs to 4 bytes
        [SyncVar(hook = nameof(SetColor))]
        public Color32 color = Color.black;

        // Unity clones the material when GetComponent<Renderer>().material is called
        // Cache it here and destroy it in OnDestroy to prevent a memory leak
        Material headMaterial;
        public GameObject head;

        void SetColor(Color32 _, Color32 newColor)
        {
            if (headMaterial == null) headMaterial = head.GetComponentInChildren<Renderer>().material;
            headMaterial.color = newColor;
        }

        void OnDestroy()
        {
            Destroy(headMaterial);
        }
    }
}
