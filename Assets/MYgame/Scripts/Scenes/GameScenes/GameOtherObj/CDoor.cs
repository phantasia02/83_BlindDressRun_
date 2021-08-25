using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CDoor : MonoBehaviour
{

    [SerializeField] protected AnimationCurve m_MyAnimationCurve;
    [SerializeField] protected Renderer m_DoorRenderer = null;
    [SerializeField] protected Renderer m_DoorCurtainRenderer = null;
    [SerializeField] protected Renderer m_AccessoriesRenderer = null;
    [SerializeField] protected GameObject m_ShowAccessories = null;


    [SerializeField] protected CGGameSceneData.EDoorType m_MyDoorType;
    public CGGameSceneData.EDoorType DoorType { get { return m_MyDoorType; } }
    [SerializeField] protected CGGameSceneData.EPlayAccessoriesType m_MyPlayAccessoriesType;
    public CGGameSceneData.EPlayAccessoriesType PlayAccessoriesType { get { return m_MyPlayAccessoriesType; } }
    [SerializeField] protected Transform m_TargetPos = null;
    public Transform TargetPos { get { return m_TargetPos; } }

    protected CDoorGroup m_MyDoorGroup = null;
    protected CAnimationCallback m_MyCurtainAnimationCallback = null;
    protected Animator m_MyAnimator = null;


    private void Awake()
    {
        m_MyDoorGroup = this.GetComponentInParent<CDoorGroup>();
        m_MyAnimator = this.GetComponentInChildren<Animator>(true);

        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;

        m_DoorRenderer.material         = lTempCGGameSceneData.m_AllDoorMat[(int)m_MyDoorType];
        m_DoorCurtainRenderer.material  = lTempCGGameSceneData.m_AllDoorCurtainMat[(int)m_MyDoorType];
        m_AccessoriesRenderer.material  = lTempCGGameSceneData.m_AllPlayAccessoriesMat[(int)m_MyPlayAccessoriesType];


        GameObject lTempGameObject = GameObject.Instantiate(m_ShowAccessories, this.transform);
        Transform lTempTransform = lTempGameObject.transform;
        Vector3 lTempV3 = this.transform.position;
        lTempV3.y += 2.5f;
        lTempGameObject.transform.position = lTempV3;

        Tween lTempTween = lTempTransform.DORotate(new Vector3(0.0f, 360.0f, 0.0f), 2.0f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
        lTempTween.SetLoops(-1, LoopType.Restart);
        lTempTween = lTempTransform.DOLocalMoveY( 3.0f, 3.0f).SetEase(m_MyAnimationCurve);
        lTempTween.SetLoops(-1, LoopType.Restart);

    }

    public void ShowAccessories(bool show){m_AccessoriesRenderer.gameObject.SetActive(show);}

    // Start is called before the first frame update
    void Start()
    {
        m_MyCurtainAnimationCallback = this.GetComponentInChildren<CAnimationCallback>();
        m_MyCurtainAnimationCallback.m_KeyFramMessageCallBack = (int index) =>
        {

        };
    }

    public void PlayAnimation()
    {
        m_MyAnimator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
