using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Carte firstSelectedCard;
    private Carte secondSelectedCard;
    private bool isChecking = false;

    [Header("Configuration des paires")]
    [Tooltip("Glisse ici tes sprites de face (ex: Lion, Singe, etc.)")]
    [SerializeField] private List<Sprite> frontSprites;

    private void Start()
    {
        InitializeCards();
    }

    private void InitializeCards()
    {
        // Récupère toutes les cartes présentes dans la scène
        Carte[] allCards = FindObjectsByType<Carte>();

        // Petite vérification pour s'assurer qu'on a assez de sprites pour faire des paires
        if (frontSprites == null || frontSprites.Count == 0)
        {
            Debug.LogError("Aucun sprite de face n'a été assigné dans le GameManager !");
            return;
        }

        // On s'assure d'avoir un nombre pair de cartes pour former des paires
        if (allCards.Length % 2 != 0)
        {
            Debug.LogWarning("Attention : Le nombre de cartes dans la scène est impair !");
        }

        // Pour assigner les paires par paires :
        // On associe chaque carte à un ID et un sprite.
        // Exemple simpliste : on distribue les sprites par paires (carte 0 et 1 = ID 0, carte 2 et 3 = ID 1, etc.)
        for (int i = 0; i < allCards.Length; i++)
        {
            int spriteIndex = (i / 2) % frontSprites.Count; // Fait correspondre 2 cartes au même sprite/ID
            int cardId = spriteIndex;

            Sprite assignedSprite = frontSprites[spriteIndex];

            // Appel de la fonction Init de chaque carte
            allCards[i].Init(cardId, assignedSprite, this);
        }
    }

    public void OnCardSelected(Carte selectedCard)
    {
        if (isChecking) return;

        if (firstSelectedCard == null)
        {
            firstSelectedCard = selectedCard;
        }
        else if (secondSelectedCard == null && selectedCard != firstSelectedCard)
        {
            secondSelectedCard = selectedCard;
            isChecking = true;

            StartCoroutine(CheckMatchRoutine());
        }
    }

    private IEnumerator CheckMatchRoutine()
    {
        yield return new WaitForSeconds(2f);

        if (firstSelectedCard.CardId == secondSelectedCard.CardId)
        {
            firstSelectedCard.SetMatched();
            secondSelectedCard.SetMatched();

            Destroy(firstSelectedCard.gameObject);
            Destroy(secondSelectedCard.gameObject);
        }
        else
        {
            firstSelectedCard.HideCard();
            secondSelectedCard.HideCard();
        }

        firstSelectedCard = null;
        secondSelectedCard = null;
        isChecking = false;
    }
}