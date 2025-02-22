using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player;
    private Animator animator; 
    private void Awake(){
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
    
    }

    private void OnEnable()
    {
        animator.Play("sleep");
        animator.SetBool("isSleep", true);
    }

    public void PlayerTurnBeginAnimation()
    {
        animator.SetBool("isSleep", false);
        animator.SetBool("isParry", false);
    }

    public void PlayerTurnEndAnimation()
    {
        if(player.defense.currentValue > 0){
            animator.SetBool("isSleep", false);
            animator.SetBool("isParry", true);
        }
        else{
            animator.SetBool("isSleep", true);
            animator.SetBool("isParry", false);
        }
    }
    public void OnPlayerCardEvent(object obj)
    {
        Card card = obj as Card;
        switch (card.cardData.cardType)
        {
            case CardType.Attack:
                animator.SetTrigger("attack");
                break;
            case CardType.Defence:
            case CardType.Ability:
            case CardType.Heal:
                animator.SetTrigger("skill");
                break;
        }
    }
    public void SetDeathAnimation()
    {
        animator.Play("death");
    }
}
