using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    
    public Image image;
    public Text countText;
    // Variabile per memorizzare il genitore dopo il trascinamento
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Item item;
    [HideInInspector] public int Count = 1;


    // RectTransform del Canvas
    private RectTransform canvasRectTransform;

    private void Start()
    {
        // Ottiene il RectTransform del Canvas
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        InitaliseItem(item);
    }

    public void InitaliseItem(Item newitem)
    {
        item = newitem;
        image.sprite = newitem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = Count.ToString();
        bool textActive = Count > 1 ;
        countText.gameObject.SetActive(textActive);
    }

    // Metodo chiamato quando inizia il trascinamento
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Disabilita il rilevamento dei clic sull'immagine
        image.raycastTarget = false;

        // Memorizza il genitore attuale
        parentAfterDrag = transform.parent;

        // Sposta l'oggetto fuori dal suo genitore
        transform.SetParent(transform.root);
    }

    // Metodo chiamato durante il trascinamento
    public void OnDrag(PointerEventData eventData)
    {
        // Aggiorna la posizione dell'oggetto in base alla posizione del puntatore
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        transform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Riabilita il rilevamento dei clic sull'immagine
        image.raycastTarget = true;

        // Riposiziona l'oggetto nel suo genitore originale
        transform.SetParent(parentAfterDrag);

        // Rileva se è stato rilasciato su un terreno
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Terreno"))
        {
            // Ottieni il terreno (potresti usare un sistema di tilemap per questa parte)
            TileBase tile = hit.collider.GetComponent<Tilemap>().GetTile(hit.collider.GetComponent<Tilemap>().WorldToCell(hit.point));

            // Controlla se l'oggetto trascinato è una carota
            if (item.type == ItemType.Tool && item.actionType == ActionType.Dig)
            {
                // Pianta l'ortaggio (es. cambia il tile del terreno)
                tile = item.tile; // Imposta il tile dell'ortaggio sulla tilemap
            }
        }
    }

        

   

}
