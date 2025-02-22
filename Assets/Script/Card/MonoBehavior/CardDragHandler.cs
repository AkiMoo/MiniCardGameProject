using UnityEngine.EventSystems;
using UnityEngine;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arrowPrefab;
    private GameObject currentArrow;

    private Card currentCard;
    private bool canMove;
    private bool canExecute;

    private CharacterBase targetCharacter;
    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }
    //使用完被回收，置为false，防止卡组洗切后一开始就是excute状态
    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!currentCard.isAvailiable)
        {
            return;
        }
        canMove = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(!currentCard.isAvailiable)
        {
            return;
        }
        //因为我前面的所有卡牌都是canmove，所以根本不会进入到else
        if(canMove)
        {
            currentCard.isAnimating = true;
            //10这里的意思是摄像机在世界坐标系中的初始位置是-10，将鼠标变成屏幕平面
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            currentCard.transform.position = worldPos;
            //判断卡是不是要执行了，可是这里1f是1的浮点数
            canExecute = worldPos.y > 0;
        }
        else
        {
            if(eventData.pointerEnter == null)
            {
                return;
            }

            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                canExecute = true;
                Debug.Log("touch enemy");
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            canExecute = false; 
            targetCharacter = null;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(!currentCard.isAvailiable)
        {
            return;
        }
        if(canExecute)
        {
            if(currentCard.cardData.cardType==CardType.Attack)
            {
                targetCharacter = GameObject.FindWithTag("Enemy").GetComponent<CharacterBase>();
            }
            currentCard.ExecuteEffect(currentCard.player, targetCharacter);
        }
        else
        {
            currentCard.ResetCardTransform();
            currentCard.isAnimating = false;
        }
    }
}
