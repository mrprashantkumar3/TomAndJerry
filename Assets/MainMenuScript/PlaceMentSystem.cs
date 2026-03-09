using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlaceMentSystem : MonoBehaviour
{
   [SerializeField] GameObject mouseIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectDataBaseSO dataBase;
    [SerializeField] private int selectObjectIndex = -1;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private AudioClip correctPlacementclip, wrongPlacementClip;
    [SerializeField] private AudioClip deleteClip;
    [SerializeField] private AudioSource source;
    [Header("Placement Animation")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float bounceScale; // Overshoot scale
    [SerializeField] private Ease animationEase = Ease.OutBack; 
    [Header("Delete Animation")]
    [SerializeField] private float deleteDuration;
    private GridData floorData, furnitureData;
    
    [SerializeField] private PreviewSystem preview; 
    
    private List<GameObject> placeGameObjects = new();
    private Vector3Int lastdetectedPosition = Vector3Int.zero;
    private List<int> placeObjectRotation = new();
    
    

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
        DOTween.Init();
       
    }
    public void startPalacment(int ID)
    {
        StopPlacement();
       
        selectObjectIndex = dataBase.objectData.FindIndex(data => data.ID == ID);
        if(selectObjectIndex < 0)
        {
            Debug.LogError($"No ID found{ID}");
            return;
        }
        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(dataBase.objectData[selectObjectIndex].Prefab, 
        dataBase.objectData[selectObjectIndex].Size);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        inputManager.OnRotate += RotateObject;
    }
    
    private void RotateObject()
    {
        preview.RotatePreview();
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectorMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        
        bool placementValidity = CheckPlacementValidity(gridPosition, selectObjectIndex);
        if(placementValidity == false)
        {
            source.PlayOneShot(wrongPlacementClip);
            return;
        }
        source.PlayOneShot(correctPlacementclip);
        GameObject newObject  = Instantiate(dataBase.objectData[selectObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);

        int rotation = preview.GetCurrentRotation();
        newObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
        

        PlayPlacementAnimation(newObject);
        placeGameObjects.Add(newObject);
        placeObjectRotation.Add(rotation);

        GridData selectedData = dataBase.objectData[selectObjectIndex].ID <= 9 ? 
        floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, 
        dataBase.objectData[selectObjectIndex].Size,
        dataBase.objectData[selectObjectIndex].ID,
        placeGameObjects.Count - 1);
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }
     private void PlayPlacementAnimation(GameObject obj)
    {
        // Start scale 0 se
        Vector3 originalScale = obj.transform.localScale;
        
        // Start scale 0 se
        obj.transform.localScale = Vector3.zero;
        
        // Animate to ORIGINAL scale (not Vector3.one)
        obj.transform.DOScale(originalScale, animationDuration)
            .SetEase(animationEase);
        
        // Optional: Thoda Y axis par bhi bounce
        Vector3 originalPos = obj.transform.position;
        obj.transform.DOMoveY(originalPos.y + 0.3f, animationDuration * 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => 
            {
                obj.transform.DOMoveY(originalPos.y, animationDuration * 0.2f)
                    .SetEase(Ease.InQuad);
            });
        
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectObjectIndex)
    {
       GridData selectedData = dataBase.objectData[selectObjectIndex].ID <= 9 ? 
       floorData : furnitureData;

       return selectedData.CanPlaceObjectAt(gridPosition, dataBase.objectData[selectObjectIndex].Size);

    }

    private void StopPlacement()
    {
        selectObjectIndex = -1;
       // isRemoveMode = false;
        gridVisualization.SetActive(false);
        preview.StopShowingPreview();
        
        inputManager.OnClicked -= PlaceStructure;
        //inputManager.OnClicked -= RemoveStructure;
        inputManager.OnExit -= StopPlacement;
        inputManager.OnRotate -= RotateObject;
        lastdetectedPosition = Vector3Int.zero;
    }

    private void Update()
    {
        if(selectObjectIndex < 0)
        return;
        Vector3 mousePosition = inputManager.GetSelectorMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if(lastdetectedPosition != gridPosition)
        {
           
            bool placementValidity = CheckPlacementValidity(gridPosition, selectObjectIndex);
        
            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastdetectedPosition = gridPosition;
        }
            
       
    }
}
