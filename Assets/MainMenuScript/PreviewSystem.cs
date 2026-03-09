using System;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
   [SerializeField] private float previewYOffset = 0.06f;
   [SerializeField] private GameObject cellIndicator;
   private GameObject previewObject;
   [SerializeField] private Material previewMaterialPrefab;
   private Material previewMaterialInstance;
   private Renderer cellIndicatiorRenderer;
   private int currentRotation = 0;


    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatiorRenderer = cellIndicator.GetComponentInChildren<Renderer>();

    }
    public void StartShowingPlacementPreview(GameObject prefabe, Vector2Int size)
    {
        previewObject = Instantiate(prefabe);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
        currentRotation = 0;
        previewObject.transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation
        cellIndicator.transform.rotation = Quaternion.Euler(0, 0, 0); // Cursor bhi reset
  
    }

    private void PrepareCursor(Vector2Int size)
    {
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatiorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
       
       foreach(Renderer renderer in renderers)
       {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
       }
    }
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if(previewObject != null)
            Destroy(previewObject);
        currentRotation = 0;
    }
    public void UpdatePosition(Vector3 position, bool validity)
    {
        if(previewObject != null)
        {
            movePreview(position);
            ApplyFeedback(validity);
        }
        
        MoveCursor(position);
        
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        
        c.a = 0.50f;
       
        cellIndicatiorRenderer.material.color = c;
        previewMaterialInstance.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void movePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);

    }
    public void RotatePreview()
    {
        if (previewObject == null)
            return;

        currentRotation = (currentRotation + 90) % 360;
        previewObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        
        // Cursor ko bhi rotate karo agar size different hai
        cellIndicator.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }

    public int GetCurrentRotation()
    {
        return currentRotation;
    }

}
