using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] private Text messageText;
    private PartyMember[] _partyMembers;

    public void Init()
    {
        _partyMembers = GetComponentsInChildren<PartyMember>();
        if (_partyMembers == null || _partyMembers.Length == 0)
        {
            Debug.LogError("No PartyMember components found as children of PartyScreen");
        }
    }

    public void SetPartyData(List<Pokemon> pokemons)
    {
        if (pokemons == null)
        {
            Debug.LogError("Pokemons list is null. Cannot set party data.");
            return;
        }

        for (int i = 0; i < _partyMembers.Length; i++)
        {
            if (i < pokemons.Count)
            {
                if (_partyMembers[i] != null)
                {
                    _partyMembers[i].SetData(pokemons[i]);
                    _partyMembers[i].gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogError($"PartyMember at index {i} is null.");
                }
            }
            else if (_partyMembers[i] != null)
            {
                _partyMembers[i].gameObject.SetActive(false);
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
}
