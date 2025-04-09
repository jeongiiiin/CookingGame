using UnityEngine;
using UnityEngine.UI;

public class Define
{
    #region Scene
    public const string Main = "Main";
    public const string Stage = "Stage";
    public const string End = "End";
    #endregion

    #region Input
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";
    #endregion

    #region Animation
    // Player
    public readonly static int IsWalk = Animator.StringToHash("IsWalk");
    public readonly static int IsRun = Animator.StringToHash("IsRun");
    // TrashCan
    public readonly static int IsOpen = Animator.StringToHash("IsOpen");
    #endregion

    #region Tag
    public const string Player = "PLAYER";
    public const string Plate = "PLATE";
    public const string LettuceCabinet = "LETTUCECABINET";
    public const string TomatoCabinet = "TOMATOCABINET";
    public const string BeefCabinet = "BEEFCABINET";
    public const string GroundBeefCabinet = "GROUNDBEEFCABINET";
    public const string CheeseCabinet = "CHEESECABINET";
    public const string BurgerBun_TCabinet = "BURGERBUN_TCABINET";
    public const string BurgerBun_BCabinet = "BURGERBUN_BCABINET";
    public const string Cabinet = "CABINET";
    public const string Ingredient = "INGREDIENT";
    public const string ExitCabinet = "EXITCABINET";
    public const string CutCabinet = "CUTCABINET";
    public const string BakeCabinet = "BAKECABINET";
    #endregion

    // 정답 체크를 위한 재료 프리팹명
    #region Ingredient
    public const string Beef = "Beef_Cooked";
    public const string BurgerBun_B = "BurgerBun_Buttom";
    public const string BurgerBun_T = "BurgerBun_Top";
    public const string Cheese = "Cheese";
    public const string GroundBeef = "GroundBeef_Cooked";
    public const string Lettuce = "Lettuce";
    public const string Tomato = "Tomato_Slice";
    #endregion

    // 주문서 재료 아이콘 경로
    #region Path
    public const string BeefIconPath = "Image/Ingredient/Beef";
    public const string BurgerBun_BIconPath = "Image/Ingredient/BurgerBun_B";
    public const string BurgerBun_TIconPath = "Image/Ingredient/BurgerBun_T";
    public const string CheeseIconPath = "Image/Ingredient/Cheese";
    public const string GroundBeefIconPath = "Image/Ingredient/GroundBeef";
    public const string LettuceIconPath = "Image/Ingredient/Lettuce";
    public const string TomatoIconPath = "Image/Ingredient/Tomato";
    public const string XIconPath = "Image/Ingredient/X";
    #endregion

    // 자주 쓰는 단어
    #region Word
    public const string CenterPoint = "CenterPoint"; // 캐비넷 중심점
    public const string StackPosition = "StackPosition"; // 접시 중심점
    #endregion
}
