using UnityEngine;
using UnityEngine.InputSystem;

public class Carte : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite cardBackSprite;   // Image générique de dos
    [SerializeField] private Sprite cardFrontSprite;  // Image de l'animal (Lion, Singe, Éléphant)

    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    [SerializeField] private int cardId; // Identifiant pour comparer les paires (ex: 1 pour lion, 2 pour singe...)
    private bool isRevealed = false;
    private bool isMatched = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int id, Sprite frontSprite, GameManager manager)
    {
        cardId = id;
        cardFrontSprite = frontSprite;
        gameManager = manager;

        // Au départ, on montre le dos de la carte
        spriteRenderer.sprite = cardBackSprite;
    }

    private void Update()
    {
        // Si la carte est déjà trouvée ou révélée, on ne clique plus dessus
        if (isMatched || isRevealed) return;

        // Détection unifiée (clic souris ou toucher via le nouveau Input System)
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 inputPosition = Pointer.current.position.ReadValue();

            // Convertit la position écran en coordonnées du monde 2D
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            // Vérifie si le clic/toucher touche cette carte précisément
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log($"Carte {cardId} sélectionnée !");
                RevealCard();
            }
        }
    }



    public void RevealCard()
    {
        if (isRevealed || isMatched) return;

        isRevealed = true;
        spriteRenderer.sprite = cardFrontSprite;

        // Informe le gestionnaire qu'on a cliqué sur cette carte
        gameManager.OnCardSelected(this);
    }

    public void HideCard()
    {
        isRevealed = false;
        spriteRenderer.sprite = cardBackSprite;
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public int CardId => cardId;
}