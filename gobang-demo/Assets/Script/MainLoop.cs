using UnityEngine;
using UnityEngine.UI;

public class MainLoop : MonoBehaviour {

    // 棋子的模板
    public GameObject WhitePrefab;
    public GameObject BlackPrefab;
    public Button HostingButton;
    public Button ButtonRepeat;
    public Button ButtonGiveUp;
    public Button WiriteButton;
    public Button BlackButton;
    public Button DoubleStartButton;
    public Text txttimer;
    public Text txthost;
    public Simpleobjectpool ChessPool;
    // 结果窗口
    public ResultWindow ResultWindow;
    //public mem Memoryitgrid;
    public int currentX;
    public int currentY;
    public int lastX;
    public int lastY;
    private bool FirstSetup;
    enum State
    {
        BlackGo, // 黑方(玩家)走
        WhiteGo, // 白方(电脑)走
        Over,    // 结束
    }


    // 当前状态
    State _state;

    // 棋盘显示
    Board _board;

    // 棋盘数据
    BoardModel _model;

    // 人工智能
    AI _ai;
    AI _aj;

    bool CanPlace(int gridX, int gridY)
    {
        // 如果这个地方可以下棋子
        return _model.Get(gridX, gridY) == ChessType.None;
    }

    // base 1
    bool PlaceChess(Cross cross, bool isblack)
    {
        if (cross == null)
            return false;

        // 创建棋子
        var newChess = GameObject.Instantiate<GameObject>(isblack ? BlackPrefab : WhitePrefab);
        newChess.transform.SetParent(cross.gameObject.transform, false);
        // 设置数据
        _model.Set(cross.GridX, cross.GridY, isblack ? ChessType.Black : ChessType.White);

        var ctype = isblack ? ChessType.Black : ChessType.White;

        var linkCount = _model.CheckLink(cross.GridX, cross.GridY, ctype);
        lastX = currentX;
        lastY = currentY;
        currentX = cross.GridX;
        currentY = cross.GridY;
        return linkCount >= BoardModel.WinChessCount;
    }

    public void Restart()
    {
        _state = State.BlackGo;
        _model = new BoardModel();

        _ai = new AI();
        _aj = new AI();
        _board.Reset();
        Time.timeScale = 1;
        deltime = 0;
        sdeltime = 0;
        WhoFirstflag = true;
        Hostingflag = false;
        txthost.text = string.Format("托管");
        FirstSetup = true;
    }

    public void HostingService()    //托管服务
    {
        if (!Hostingflag)
        {
            Hostingflag = true;
            txthost.text = string.Format("取消托管");
            txthost.color = Color.blue;
        }
        else
        {
            Hostingflag = false;
            txthost.text = string.Format("托管");
            // txthost.color = Color.blue;
        }
    }
    public void RepeatService()     // 悔棋触发
    {
        Transform childchese = this.transform.GetChild(currentX * 15 + currentY).GetChild(0);
        Destroy(childchese.gameObject);
        Transform childchesee = this.transform.GetChild(lastX * 15 + lastY).GetChild(0);
        Destroy(childchesee.gameObject);
        _model.Set(currentX, currentY, ChessType.None);
        _model.Set(lastX, lastY, ChessType.None);

    }
    public void GiveUpService()    //放弃
    {
        _state = State.Over;
        ShowResult(ChessType.White);
    }
    public void OnClick(Cross cross)
    {
        if (WhoFirstflag)
        {
            if (!Hostingflag)
            {
                if (_state == State.BlackGo)
                {
                    // 不能在已经放置过的棋子上放置
                    /* if (CanPlace(cross.GridX, cross.GridY))
                     {
                         _lastPlayerX = cross.GridX;
                         _lastPlayerY = cross.GridY;

                         if (PlaceChess(cross, true))
                         {
                             // 已经胜利
                             _state = State.Over;
                             ShowResult(ChessType.Black);
                         }
                         else
                         {
                             // 换电脑走
                             _state = State.WhiteGo;
                         }
                     }*/
                    HunmanDoBlack(cross);
                }
            }
            if (DoubleStartflag)
            {
                if (_state == State.WhiteGo) // return;
                {
                    // 不能在已经放置过的棋子上放置
                    /* if (CanPlace(cross.GridX, cross.GridY))
                     {
                         _lastPlayerX = cross.GridX;
                         _lastPlayerY = cross.GridY;

                         if (PlaceChess(cross, false))
                         {
                             // 已经胜利
                             _state = State.Over;
                             ShowResult(ChessType.White);
                         }
                         else
                         {
                             // 换电脑走
                             _state = State.BlackGo;
                         }
                     }*/
                    HunmanDoWhite(cross);
                }
            }
        }
        else
        {
            if (!Hostingflag)
            {
                 if (_state == State.WhiteGo) // return;
                {
                    HunmanDoWhite(cross);
                }
            }
            if (DoubleStartflag)
            {
                if (_state == State.BlackGo)
                {
                    HunmanDoBlack(cross);
                }
            }
        }
    }
    public bool Hostingflag;
    void Start()
    {
        _board = GetComponent<Board>();
        Hostingflag = false;
        deltime = 1;
        Restart();
        HostingButton.onClick.AddListener(HostingService);
        ButtonGiveUp.onClick.AddListener(GiveUpService);
        ButtonRepeat.onClick.AddListener(RepeatService);
         WiriteButton.onClick.AddListener(WiriteFirstFunction);
         BlackButton.onClick.AddListener(BlackFirstFunction);
        DoubleStartButton.onClick.AddListener(DoubleStartFunction);
        /* Memoryitgrid.lastX =0;
         Memoryitgrid.lastY = 0;
         Memoryitgrid.currentX = 0;
         Memoryitgrid.currentY =0;*/
        DoubleStartflag = false;

        currentX = 0;
        currentY = 0;
        lastX = 0;
        lastY = 0;
    }
    public bool WhoFirstflag;
    public void WiriteFirstFunction()
    {
        WhoFirstflag = false;
    }
    public void BlackFirstFunction()
    {
        WhoFirstflag = true;
    }
    public bool DoubleStartflag;
    public void DoubleStartFunction()
    {
        if (!DoubleStartflag)
        {
            DoubleStartflag = true;
        }
        else
        {
            DoubleStartflag = false;
        }

    }

    int _lastPlayerX, _lastPlayerY;

    void ShowResult(ChessType winside)
    {
        ResultWindow.gameObject.SetActive(true);
        ResultWindow.Show(winside);
        Time.timeScale = 0;

    }

    // Update is called once per frame
    private int deltime = 1;
    private float sdeltime = 0;
    void Update()
    {
        sdeltime += Time.deltaTime;
        if (sdeltime >= deltime)
        {
            deltime++;
            txttimer.text = string.Format("{0:d2}:{1:d2}", deltime / 60, deltime % 60);
            if (WhoFirstflag)
            {
                if (!DoubleStartflag)
                {
                    if (!Hostingflag)
                    {
                        /* switch (_state)
                         {
                             // 白方(电脑)走
                             case State.WhiteGo:
                                 {
                                     // 计算电脑下的位置
                                     int gridX, gridY;
                                     _ai.ComputerDo(_lastPlayerX, _lastPlayerY, out gridX, out gridY);

                                     if (PlaceChess(_board.GetCross(gridX, gridY), false))
                                     {
                                         // 电脑胜利
                                         _state = State.Over;
                                         ShowResult(ChessType.White);
                                     }
                                     else
                                     {
                                         // 换玩家走
                                         _state = State.BlackGo;
                                         _lastPlayerX = gridX;
                                         _lastPlayerY = gridY;
                                     }

                                 }
                                 break;

                         }*/
                        ComputerDoWhite(_state);

                    }
                    else
                    {
                        switch (_state)
                        {
                            // 白方(电脑)走
                            case State.WhiteGo:
                                {
                                    // 计算电脑下的位置
                                    /* int gridX, gridY;
                                      _ai.ComputerDo(_lastPlayerX, _lastPlayerY, out gridX, out gridY);

                                      if (PlaceChess(_board.GetCross(gridX, gridY), false))
                                      {
                                          // 电脑胜利
                                          _state = State.Over;
                                          ShowResult(ChessType.White);
                                      }
                                      else
                                      {
                                          // 换玩家走
                                          _state = State.BlackGo;
                                          _lastPlayerX = gridX;
                                          _lastPlayerY = gridY;
                                      }*/
                                    ComputerDoWhite(_state);
                                }
                                break;
                            case State.BlackGo:
                                {
                                    // 计算电脑下的位置
                                    /* int gridX, gridY;
                                      _ai.ComputerDo(_lastPlayerX, _lastPlayerY, out gridX, out gridY);

                                      if (PlaceChess(_board.GetCross(gridX, gridY), true))
                                      {
                                          // 电脑胜利
                                          _state = State.Over;
                                          ShowResult(ChessType.Black);
                                      }
                                      else
                                      {
                                          // 换玩家走
                                          _state = State.WhiteGo;
                                          _lastPlayerX = gridX;
                                          _lastPlayerY = gridY;
                                      }*/
                                    ComputerDoBlack(_state);
                                }
                                break;
                        }


                    }
                }
            }
            else
            {
                if (!DoubleStartflag)
                {
                    if (!Hostingflag)
                    {
                        ComputerDoBlack(_state);
                    }
                    else
                    {
                        switch (_state)
                        {
                            // 白方(电脑)走
                            case State.WhiteGo:
                                {
                                    ComputerDoWhite(_state);
                                }
                                break;
                            case State.BlackGo:
                                {
                                    ComputerDoBlack(_state);
                                }
                                break;
                        }


                    }
                }
            }
        }
    }
    private void ComputerDoWhite(State state)
    {
        if (state == State.WhiteGo)
        {
            int gridX, gridY;
            _ai.setPlayerPiece(_lastPlayerX, _lastPlayerY);
            _ai.ComputerDo(_lastPlayerX, _lastPlayerY, out gridX, out gridY);

            if (PlaceChess(_board.GetCross(gridX, gridY), false))
            {
                // 电脑胜利
                _state = State.Over;
                ShowResult(ChessType.White);
            }
            else
            {
                // 换玩家走
                _state = State.BlackGo;
                _lastPlayerX = gridX;
                _lastPlayerY = gridY;
            }
        }
    }
    private void ComputerDoBlack(State state)
    {
        if (!FirstSetup)
        {
            _aj.setPlayerPiece(_lastPlayerX, _lastPlayerY);
        }
        FirstSetup = false;
        if (state == State.BlackGo) 
        {
            int gridX, gridY;
            _aj.ComputerDo(_lastPlayerX, _lastPlayerY, out gridX, out gridY);

            if (PlaceChess(_board.GetCross(gridX, gridY), true))
            {
                // 电脑胜利
                _state = State.Over;
                ShowResult(ChessType.Black);
            }
            else
            {
                // 换玩家走
                _state = State.WhiteGo;
                _lastPlayerX = gridX;
                _lastPlayerY = gridY;
            }
        }
    }
    private void HunmanDoWhite(Cross cross)
    {

        if (CanPlace(cross.GridX, cross.GridY))
        {
            _lastPlayerX = cross.GridX;
            _lastPlayerY = cross.GridY;
           
            if (PlaceChess(cross, false))
            {
                // 已经胜利
                _state = State.Over;
                ShowResult(ChessType.White);
            }
            else
            {
                // 换电脑走
                _state = State.BlackGo;
            }
        }
    }
    private void HunmanDoBlack(Cross cross)
    {
        if ((!Hostingflag) && (!FirstSetup))
        {
            _aj.setPlayerPiece(_lastPlayerX, _lastPlayerY);
        }
        FirstSetup = false;
        if (CanPlace(cross.GridX, cross.GridY))
        {
            _lastPlayerX = cross.GridX;
            _lastPlayerY = cross.GridY;
            _aj.CalcScore(_lastPlayerX, _lastPlayerY);
            if (PlaceChess(cross, true))
            {
                // 已经胜利
                _state = State.Over;
                ShowResult(ChessType.Black);
            }
            else
            {
                // 换电脑走
                _state = State.WhiteGo;
            }
        }
    }
}
