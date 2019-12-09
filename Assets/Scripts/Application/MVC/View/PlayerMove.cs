using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : View
{
    #region 常量

    const float grivaty = 9.8f;
    const float m_jumpValue = 5;
    const float m_moveSpeed = 13;

    const float m_SpeedAddDis = 200;
    const float m_SpeedAddRate = 0.5f;
    const float m_MaxSpeed = 40;

    #endregion

    #region 事件
    #endregion

    #region 字段

    float speed = 20;
    CharacterController m_cc;
    InputDirection m_inputDir = InputDirection.NULL;
    bool activeInput = false;
    Vector3 m_mousePos;
    int m_nowIndex = 1;
    int m_targetIndex = 1;
    float m_xDistance;
    float m_yDistance;

    bool m_IsSlide = false;
    float m_SlideTime;

    float m_SpeedAddCount;

    GameModel gm;

    //记录速度
    float m_Maskspeed;
    //增加速度的速率
    float m_AddRate = 10;
    bool m_IsHit = false;

    //item有关
    int m_DoubleTime = 1;
    int m_SkillTime;
    //双倍金币协程
    IEnumerator MultiplyCor;
    //吸铁石协程
    IEnumerator MagnetCor;
    //无敌状态协程
    IEnumerator InvinciblelCor;
    SphereCollider m_MagnetCollider;

    bool m_IsInvincible = false;

    //和射门有关
    GameObject m_Ball;
    GameObject m_Trail;
    IEnumerator GoalCor;
    bool m_IsGoal = false;
    //皮肤更新
    public SkinnedMeshRenderer skm;
    public MeshRenderer render;


    #endregion

    #region 属性
    public override string Name { get { return Consts.V_PlayerMove; } }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
            if (speed > m_MaxSpeed)
            {
                speed = m_MaxSpeed;
            }
        }
    }
    #endregion

    #region 方法


    #region 移动

    IEnumerator UpdateAction() {

        while (true)
        {
            if (!gm.IsPause && gm.IsPlay)
            {
                //更新ui
                UpdateDis();

                m_yDistance -= grivaty * Time.deltaTime;
                m_cc.Move((transform.forward * Speed + new Vector3(0, m_yDistance, 0)) * Time.deltaTime);
                MoveControl();
                UpdatePosition();
                UpdateSpeed();

                /// <summary>
                /// 这里加一个X限定，确保如果移动操作过快，在还没有移动到目标位置的时候，主角不会移出跑道
                /// </summary>
                if (transform.position.x >= 2)
                {
                    transform.position = new Vector3(2, transform.position.y, transform.position.z);

                    m_nowIndex = 2;
                }
                if (transform.position.x <= -2)
                {
                    transform.position = new Vector3(-2, transform.position.y, transform.position.z);

                    m_nowIndex = 0;
                }
                /// <summary>
                /// 这里加一个X限定，确保如果移动操作过快，在还没有移动到目标位置的时候，主角不会移出跑道
                /// </summary>

            }
            yield return 0;
        }
    }

    void UpdateDis()
    {
        DistanceArgs e = new DistanceArgs {
            distance = (int)transform.position.z
        };
        SendEvent(Consts.E_UpdataDis, e);
    }


    //获取输入
    void GetInputDirection()
    {
        //手势识别
        m_inputDir = InputDirection.NULL;
        if (Input.GetMouseButtonDown(0))
        {
            activeInput = true;
            m_mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && activeInput)
        {
            Vector3 Dir = Input.mousePosition - m_mousePos;
            if (Dir.magnitude > 20)
            {
                if (Mathf.Abs(Dir.x) > Mathf.Abs(Dir.y) && Dir.x > 0)
                {
                    m_inputDir = InputDirection.Right;
                }
                else if (Mathf.Abs(Dir.x) > Mathf.Abs(Dir.y) && Dir.x < 0)
                {
                    m_inputDir = InputDirection.Left;
                }
                else if (Mathf.Abs(Dir.x) < Mathf.Abs(Dir.y) && Dir.y > 0)
                {
                    m_inputDir = InputDirection.Up;
                }
                else if (Mathf.Abs(Dir.x) < Mathf.Abs(Dir.y) && Dir.y < 0)
                {
                    m_inputDir = InputDirection.Down;
                }
                activeInput = false;
            }
        }

        //键盘识别
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            m_inputDir = InputDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            m_inputDir = InputDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            m_inputDir = InputDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            m_inputDir = InputDirection.Right;
        }


        //print(m_inputDir);
    }

    //更新位置
    void UpdatePosition()
    {
        GetInputDirection();
        switch (m_inputDir)
        {
            case InputDirection.NULL:
                break;
            case InputDirection.Right:
                if (m_targetIndex < 2)
                {
                    m_targetIndex++;
                    m_xDistance = 2;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.sound.PlayEffect("Se_UI_Huadong");
                }
                break;
            case InputDirection.Left:
                if (m_targetIndex > 0)
                {
                    m_targetIndex--;
                    m_xDistance = -2;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.sound.PlayEffect("Se_UI_Huadong");
                }
                break;
            case InputDirection.Down:
                if (m_IsSlide == false)
                {
                    m_IsSlide = true;
                    m_SlideTime = 0.733f;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.sound.PlayEffect("Se_UI_Slide");
                }
                break;
            case InputDirection.Up:
                if (m_cc.isGrounded)
                {
                    Game.Instance.sound.PlayEffect("Se_UI_Jump");
                    m_yDistance = m_jumpValue;
                    SendMessage("AnimManager", m_inputDir);
                }
                break;
            default:
                break;
        }

    }

    //移动
    void MoveControl()
    {
        //左右移动
        if (m_targetIndex != m_nowIndex)
        {
            float move = Mathf.Lerp(0, m_xDistance, m_moveSpeed * Time.deltaTime);
            transform.position += new Vector3(move, 0, 0);
            m_xDistance -= move;
            if (Mathf.Abs(m_xDistance) < 0.05f)
            {
                m_xDistance = 0;
                m_nowIndex = m_targetIndex;

                switch (m_nowIndex)
                {
                    case 0:
                        transform.position = new Vector3(-2, transform.position.y, transform.position.z);
                        break;
                    case 1:
                        transform.position = new Vector3(0, transform.position.y, transform.position.z);
                        break;
                    case 2:
                        transform.position = new Vector3(2, transform.position.y, transform.position.z);
                        break;

                }
            }
        }

        if (m_IsSlide)
        {
            m_SlideTime -= Time.deltaTime;
            if (m_SlideTime < 0)
            {
                m_IsSlide = false;
                m_SlideTime = 0;
            }
        }
    }
    //更新速度
    void UpdateSpeed()
    {
        m_SpeedAddCount += Speed * Time.deltaTime;
        if (m_SpeedAddCount > m_SpeedAddDis)
        {
            m_SpeedAddCount = 0;
            Speed += m_SpeedAddRate;
        }
    }

    #endregion


    //减速
    public void HitObstacles()
    {
        if (m_IsHit)
            return;
        m_IsHit = true;
        m_Maskspeed = Speed;
        Speed = 0;
        StartCoroutine(DecreaseSpeed());
    }

    IEnumerator DecreaseSpeed()
    {
        while(Speed <= m_Maskspeed)
        {
            Speed += Time.deltaTime * m_AddRate;
            yield return 0;
        }
        m_IsHit = false;
    }

    //吃金币
    public void HitCoin()
    {
        //print("eat");
        CoinArgs e = new CoinArgs {
            coin = m_DoubleTime
        };
        SendEvent(Consts.E_UpdateCoin,e);
    }
    //
    public void HitItem(ItemKind item)
    {
        ItemArgs e = new ItemArgs
        {
            hitCount = 0,
            kind = item
        };
        SendEvent(Consts.E_HitItem, e);
        //switch (item)
        //{
        //    case ItemKind.ItemMagnet:
                
        //        break;
        //    case ItemKind.ItemMultiply:
        //        break;
        //    case ItemKind.ItemInvincible:
        //        break;
        //    default:
        //        break;
        //}
    }

    //双倍金币
    public void HitMultiply()
    {
        
      if(MultiplyCor!= null)
        {
            StopCoroutine(MultiplyCor);       
        }
        MultiplyCor = MultiplyCoroutine();
        StartCoroutine(MultiplyCor);
    }

    IEnumerator MultiplyCoroutine()
    {
        m_DoubleTime = 2;
        float timer = m_SkillTime;
        while(timer > 0)
        {
            if(gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
            }
            yield return 0;
        }
        //yield return new WaitForSeconds(m_SkillTime);
        m_DoubleTime = 1;
    }

    //吸铁石
    public void HitMagnet()
    {
        if (MagnetCor != null)
        {
            StopCoroutine(MagnetCor);
        }
        MagnetCor = MagnetCoroutine();
        StartCoroutine(MagnetCor);
     
    }

    IEnumerator MagnetCoroutine()
    {
        m_MagnetCollider.enabled = true;
        float timer = m_SkillTime;
        while (timer > 0)
        {
            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
            }
            yield return 0;
        }
        //yield return new WaitForSeconds(m_SkillTime);
        m_MagnetCollider.enabled = false;
    }

    //加时间
    public void HitAddTime()
    {
        //sendEvent 加时间
        //print("add time");
        SendEvent(Consts.E_HitAddTime);
    }

    //无敌状态
    public void HitInvincible()
    {
        
        if (InvinciblelCor != null)
        {
            StopCoroutine(InvinciblelCor);
        }
        InvinciblelCor = InvincibleCoroutine();
        StartCoroutine(InvinciblelCor);
    }

    IEnumerator InvincibleCoroutine()
    {
        m_IsInvincible = true;
        float timer = m_SkillTime;
        while (timer > 0)
        {
            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
            }
            yield return 0;
            //print(timer);
        }
        //yield return new WaitForSeconds(m_SkillTime);
        m_IsInvincible = false;
    }


    //*****************************************//
    //射门相关

     public void OnGoalClick()
    {
        if(GoalCor != null)
        {
            StopCoroutine(GoalCor);
        }
        SendMessage("MessagePlayShoot");
        m_Trail.SetActive(true);
        m_Ball.SetActive(false);
        GoalCor = MoveBall();
        StartCoroutine(GoalCor);
    }

    IEnumerator MoveBall()
    {
        while (true)
        {
            if (!gm.IsPause && gm.IsPlay)
            {
                m_Trail.transform.Translate(transform.forward * 40 * Time.deltaTime);
            }
            yield return 0;
        }
    }

    //球进了
    public void HitBallDoor()
    {
        //1.停止协程
        StopCoroutine(GoalCor);
        //2.归为
        m_Trail.transform.localPosition = new Vector3(0, 1.62f, 6.28f);
        m_Trail.SetActive(false);
        m_Ball.SetActive(true);
        //3.isgoal变为true
        m_IsGoal = true;

        //4.生成特效
        Game.Instance.objectPool.Spawn("FX_GOAL", m_Trail.gameObject.transform.parent);

        //5.播放音效
        Game.Instance.sound.PlayEffect("Se_UI_Goal");

        //6.发送事件UI goal + 1
        SendEvent(Consts.E_ShootGoal);

    }
    #endregion

    #region Unity回调

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Tag.smallFence)
        {
            if (m_IsInvincible)
                return;
            other.gameObject.SendMessage("HitPlayer",transform.position);
            HitObstacles();
            //2.声音
            Game.Instance.sound.PlayEffect("Se_UI_Hit");
        }   
        else if (other.gameObject.tag == Tag.bigFence)
        {
            if (m_IsInvincible)
                return;
            if (m_IsSlide)
                return;
            other.gameObject.SendMessage("HitPlayer", transform.position);
            //2.声音
            Game.Instance.sound.PlayEffect("Se_UI_Hit");
            HitObstacles();
        }
        else if (other.gameObject.tag == Tag.block) //游戏结束
        {
            //2.声音
            Game.Instance.sound.PlayEffect("Se_UI_End");
            other.gameObject.SendMessage("HitPlayer", transform.position);

            //sendEvent 游戏结束
            SendEvent(Consts.E_EndGame);
        }
        else if (other.gameObject.tag == Tag.smallBlock) //游戏结束
        {
            //2.声音
            Game.Instance.sound.PlayEffect("Se_UI_End");
            other.transform.parent.parent. SendMessage("HitPlayer", transform.position);

            //sendEvent 游戏结束
            SendEvent(Consts.E_EndGame);
        }
        else if (other.gameObject.tag == Tag.beforeTrigger) //汽车触发器
        {
            other.transform.parent.SendMessage("HitTrigger", SendMessageOptions.RequireReceiver);
        }
        else if(other.tag == Tag.beforeGoalTrigger)           
        {
            //准备射门
            //1.发消息给UIboard
            SendEvent(Consts.E_HitGoalTrigger);
            //2.显示加速特效
            Game.Instance.objectPool.Spawn("FX_JiaSu", m_Trail.gameObject.transform.parent);
        }
        else if(other.tag == Tag.goalkeeper)//撞到守门员
        {
            HitObstacles();
            //1.守门员飞走
            other.transform.parent.parent.parent.SendMessage("HitGoalKeeper", SendMessageOptions.RequireReceiver);
        }
        else if (other.tag == Tag.ballDoor)//撞到球门
        {
            if (m_IsGoal)
            {
                m_IsGoal = false;
                return;
            }
            //1.减速
            HitObstacles();

            //3.球网占到身上
            Game.Instance.objectPool.Spawn("Effect_QiuWang", m_Trail.gameObject.transform.parent);
            other.transform.parent.parent.SendMessage("HitDoor", m_nowIndex);
        }
    }

    private void Awake()
    {
        m_cc = GetComponent<CharacterController>();
        gm = GetModel<GameModel>();
        m_SkillTime = gm.SkillTime;

        //获取MagnetCollider
        m_MagnetCollider = GetComponentInChildren<SphereCollider>();
        m_MagnetCollider.enabled = false;

        //对射门进行赋值
        m_Ball = transform.Find("Ball").gameObject;
        m_Trail = GameObject.Find("trail").gameObject;
        m_Trail.gameObject.SetActive(false);

        //更新皮肤
        skm.material = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, gm.TakeOnCloth.ClothID).Material;
        render.material = Game.Instance.staticData.GetFootballInfo(gm.TakeOnFootball).Material;
    }

    private void Start()
    {
        StartCoroutine(UpdateAction());

    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    gm.IsPause = true;
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    gm.IsPause = false;
        //}


    }
    #endregion


    #region 事件回调
    public override void HandleEvent(string name, object data)
    {
        switch (name)
        {
            case Consts.E_ClickGoalButton:
                OnGoalClick();
                break;
        }
    }

    public override void RegisterAttentionEvent()
    {
        AttentionList.Add(Consts.E_ClickGoalButton);
    }
    #endregion
}


























