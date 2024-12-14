using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] private Text messageText;
    private PartyMember[] memberSlots;
    private List<Pokemon> pokemons;

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMember>();
        if (memberSlots == null || memberSlots.Length == 0)
        {
            Debug.LogError("No PartyMember components found as children of PartyScreen");
        }
    }

    public void SetPartyData(List<Pokemon> pokemons)
    {
        this.pokemons = pokemons;
        if (pokemons == null)
        {
            Debug.LogError("Pokemons list is null. Cannot set party data.");
            return;
        }

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < pokemons.Count)
            {
                if (memberSlots[i] != null)
                {
                    memberSlots[i].SetData(pokemons[i]);
                }
                else
                {
                    Debug.LogError($"PartyMember at index {i} is null.");
                }
            }
            else if (memberSlots[i] != null)
            {
                memberSlots[i].gameObject.SetActive(false);
            }
        }

        if (messageText != null)
        {
            messageText.text = "Choose a Pokemon";
        }
        else
        {
            Debug.LogError("MessageText is not assigned in PartyScreen");
        }
    }

    public void SetMessageText(string v)
    {
        messageText.text = v;
    }

    public void UpdateSelectedMember(int currentPokemon)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            if (i == currentPokemon)
            {
                memberSlots[i].SetSelected(true);
            }
            else
            {
                memberSlots[i].SetSelected(false);
            }
        }
    }
}
