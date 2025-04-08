using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class DestroyBricks : MonoBehaviour
{
    public Tilemap destructableTiles;
    //public AudioManager audioManager;
    [SerializeField] private GameObject breakFx;

    private void Start() {
        destructableTiles = GetComponent<Tilemap>();
        Debug.Log($"Tilemap: {destructableTiles.name}");
        //Vector3Int test = new Vector3Int(-9,2,0);
        //destructableTiles.SetTile(test, null);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball")) {
            foreach(ContactPoint2D hit in collision.contacts) {   
                Vector3 hitPosition = hit.point - hit.normal * 0.01f;

                Vector3 tileAnchorOffset = destructableTiles.tileAnchor; //gets the anchor point value
                hitPosition -= tileAnchorOffset; //calculates the offset of the tile

                Vector3Int cellPosition = destructableTiles.WorldToCell(hitPosition); //gets tile cell position
                
                //might change later, calculates cell positon basedo n anchor point for each brick corner
                Vector3Int cellA = new Vector3Int((int)(cellPosition.x + 0.5), (int)(cellPosition.y + 0.5)); 
                Vector3Int cellB = new Vector3Int((int)(cellPosition.x - 0.5), (int)(cellPosition.y - 0.5));
                Vector3Int cellC = new Vector3Int((int)(cellPosition.x + 0.5), (int)(cellPosition.y - 0.5)); 
                Vector3Int cellD = new Vector3Int((int)(cellPosition.x - 0.5), (int)(cellPosition.y + 0.5));  
                //audioManager.PlaySFX(audioManager.hitBlock);
                //Debug.Log($"Cell Position: {cellPosition} / Cell location: {cellLocation}. Cells A:{cellA} B:{cellB} C:{cellC} D:{cellD} Adjusted Hit point: {hitPosition}");

                if (destructableTiles.HasTile(cellA))
                {
                    DestroyCellAtPosition(cellA);
                    Debug.Log($"Collision with {collision.gameObject.name} at {cellA}");
                }
                else if (destructableTiles.HasTile(cellB))
                {
                    DestroyCellAtPosition(cellB);
                    Debug.Log($"Collision with {collision.gameObject.name} at {cellB}");
                }
                else if (destructableTiles.HasTile(cellC))
                {
                    DestroyCellAtPosition(cellC);
                    Debug.Log($"Collision with {collision.gameObject.name} at {cellC}");
                }
                else if (destructableTiles.HasTile(cellD))
                {
                    DestroyCellAtPosition(cellD);
                    Debug.Log($"Collision with {collision.gameObject.name} at {cellD}");
                }
                else 
                {
                    Debug.LogWarning("No tile at the calculated cell position!");
                }
            } 
        }      
    }

    //this function will destroy the cell at the calcualted Vector3Int position
    private void DestroyCellAtPosition(Vector3Int cell)
    {
        destructableTiles.SetTile(cell, null);

        Vector3 effectPosition = destructableTiles.CellToWorld(cell);
        GameObject fx = Instantiate(breakFx, effectPosition, Quaternion.identity);
        Destroy(fx, 2f);
    }
}
