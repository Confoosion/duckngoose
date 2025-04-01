using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POVSwapping : MonoBehaviour
{
    public static POVSwapping Swapping;
    public List<GameObject> characters;
    private int characterIndex = 0;

    void Awake()
    {
        Swapping = this;
    }

    public void AddCharacterToSwap(GameObject character)
    {
        characters.Add(character);
    }

    public void RemoveCharacterToSwap(GameObject character)
    {
        characters.Remove(character);
    }

    public virtual void SwapCharacter()
    {
        // Get current character to swap from
        GameObject currChar = characters[characterIndex];

        // Get next character to swap to
        characterIndex = (characterIndex + 1) % characters.Count;
        GameObject nextChar = characters[characterIndex];

        // Get character positions
        Vector2 currentCharPos = (Vector2)currChar.transform.position;

        Debug.Log("Current Character Position: " + currentCharPos);
        Debug.Log("INDEX: " + characterIndex);

        Vector2 nextCharPos = currentCharPos + new Vector2(0f, (1f - 2f * characterIndex) * 75f);

        nextChar.transform.position = nextCharPos;

        DeactivateCharacter(currChar);
        ActivateCharacter(nextChar);
    }

    void ActivateCharacter(GameObject character)
    {
        foreach(Transform child in character.transform)
        {
            child.gameObject.SetActive(true);
        }
        character.SetActive(true);

        Debug.Log("Now swapped to: " + character);
    }

    void DeactivateCharacter(GameObject character)
    {
        foreach(Transform child in character.transform)
        {
            child.gameObject.SetActive(false);
        }
        character.SetActive(false);

        Debug.Log("Swapping away from: " + character);
    }
}
