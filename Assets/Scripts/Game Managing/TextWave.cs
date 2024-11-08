using UnityEngine;
using TMPro;

public class TextWaveEffect : MonoBehaviour
{
    public float waveSpeed = 5.0f;     // Speed of the wave
    public float waveHeight = 10.0f;   // Height of the bobbing motion

    private TMP_Text textComponent;
    private Vector3[] vertices;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        textComponent.ForceMeshUpdate(); // Ensures the text mesh is updated

        TMP_TextInfo textInfo = textComponent.textInfo;

        // Loop over each character and apply a bobbing effect
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            // Only animate visible characters
            if (!charInfo.isVisible)
                continue;

            // Get the index of the material and vertex for the character
            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            // Get the vertices of the character from the mesh
            vertices = textInfo.meshInfo[materialIndex].vertices;

            // Calculate offset for wave effect based on time and character position
            float offsetY = Mathf.Sin((Time.time * waveSpeed) + (i * 0.5f)) * waveHeight;

            // Apply the wave offset to each of the four vertices of the character
            vertices[vertexIndex + 0].y += offsetY;
            vertices[vertexIndex + 1].y += offsetY;
            vertices[vertexIndex + 2].y += offsetY;
            vertices[vertexIndex + 3].y += offsetY;
        }

        // Update the mesh with the new vertex positions
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
