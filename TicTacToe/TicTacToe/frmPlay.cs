using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace TicTacToe
{
    public partial class frmPlay : Form
    {
        Board board;
        int depth; 
        int playerScore = 0;
        int computerScore = 0;
        int computerPlayIndex = 0;
        bool isPlayerTurn;
        public List<Button> buttons;
        public frmPlay()
        {
            InitializeComponent();
        }
        private void frmPlay_Load(object sender, EventArgs e)
        {
            board = new Board();
            buttons = new List<Button>();
            buttons.Add(button1);
            buttons.Add(button2);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
            buttons.Add(button7);
            buttons.Add(button8);
            buttons.Add(button9);
            for (int i = 1; i <= 9; i++)
            {
                cbLevel.Items.Add(i);
            }
            cbLevel.SelectedIndex = 0;
            lbResult.Text = "";
            depth = (int)cbLevel.SelectedItem;
            SetFirstTurn(picOn.Visible);
            EndGameState();
            btnReset.Enabled = false;
        }

        

        private void gameLoop(int x, int y)
        {
            if (!board.isGameOver())
            {
                board.move(x * 3 + y);
                Minimax_AlphaBeta alphaBeta = new Minimax_AlphaBeta(depth);
                alphaBeta.alphaBetaPruning(board, Board.State.O, 0, int.MinValue, int.MaxValue);
                moveOnBoard(board);
                if (board.isGameOver())
                    checkTheWinner();
            }
            else
            {
                checkTheWinner();
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Image == null)
            {
                lbResult.Text = "";
                SetColor();
                string buttonNumber = button.Name.Substring(button.Name.Length - 1);
                int x = (Int32.Parse(buttonNumber) - 1) / 3;
                int y = (Int32.Parse(buttonNumber) - 1) % 3;
                button.Image = pbPlayer.Image;
                gameLoop(x, y);
            }
        }


        private void moveOnBoard(Board board)
        {
            List<int> oStates = board.oStates();
            for (int i = 0; i < oStates.Count; i++)
            {
                int index = oStates[i];
                if (buttons[index].Image == null)
                {
                    buttons[index].Image = pbAI.Image;
                }
            }
        }


        private void checkTheWinner()
        {
            if (board.getWinner() == Board.State.X)
            {
                lbResult.Text = "You Won!";
                playerScore++;
                lbPlayerScore.Text = playerScore.ToString();
                lbAiScore.Text = computerScore.ToString();
                EndGameState();
                btn_start.Text = "Restart";
            }
            else if (board.getWinner() == Board.State.O)
            {
                lbResult.Text = "You Lost!";
                computerScore++;
                lbPlayerScore.Text = playerScore.ToString();
                lbAiScore.Text = computerScore.ToString();
                EndGameState();
                btn_start.Text = "Restart";
            }
            else if (board.getMoveCount() == 9 && board.getWinner() == Board.State.BLANK)
            {
                lbResult.Text = "Tie!";
                EndGameState();
                btn_start.Text = "Restart";
            }
        }


        private void refreshBoard()
        {
            board.reset();
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Enabled = true;
                buttons[i].Image = null;
            }
            lbResult.Text = "";
        }

        private void cbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            depth = (int)cbLevel.SelectedItem;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (btn_start.Text == "Start" || btn_start.Text == "Restart")
            {
                refreshBoard();
                SetFirstTurn(picOn.Visible);
                EnableChangeTurn(false);
                cbLevel.Enabled = false;
                btnReset.Enabled = true;
                if (!isPlayerTurn)
                {
                    Thread.Sleep(500);
                    computerPlayIndex = board.playComputerTurn();
                    if (buttons[computerPlayIndex].Image == null)
                    {
                        buttons[computerPlayIndex].Image = pbAI.Image;
                    }
                }
                btn_start.Text = "Stop";
            }
            else if (btn_start.Text == "Stop")
            {
                EndGameState();
                SetFirstTurn(picOn.Visible);
                EnableChangeTurn(true);
                cbLevel.Enabled = true;
                btn_start.Text = "Restart";
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            playerScore = 0;
            computerScore = 0;
            lbPlayerScore.Text = playerScore.ToString();
            lbAiScore.Text = computerScore.ToString();
            refreshBoard();
            EndGameState();
            btn_start.Text = "Start";
        }
        private void ChangeTurn_Click(object sender, EventArgs e)
        {
            SetFirstTurn(!picOn.Visible);
        }

        private void EndGameState()
        {
            foreach (var button in buttons)
            {
                if (button.Image == null)
                {
                    button.Enabled = false;
                }
            }
            EnableChangeTurn(true);
            cbLevel.Enabled = true;
        }

        private void EnableChangeTurn(bool check)
        {
            if (check)
            {
                if (picOn.Visible)
                {
                    picOn.Enabled = true;
                }
                else
                {
                    picOff.Enabled = true;
                }
            }
            else
            {
                if (picOn.Visible)
                {
                    picOn.Enabled = false;
                }
                else
                {
                    picOff.Enabled = false;
                }
            }
        }
        public void SetFirstTurn(bool firstTurn)
        {
            isPlayerTurn = firstTurn;
            if (firstTurn)
            {
                picOn.Visible = true;
                picOn.Enabled = true;
                picOff.Visible = false;
                picOff.Enabled = false;
            }
            else
            {
                picOn.Visible = false;
                picOn.Enabled = false;
                picOff.Visible = true;
                picOff.Enabled = true;
            }
        }

        public void SetColor()
        {
            foreach (var button in buttons)
            {
                button.BackColor = Color.DarkGray;
            }
        }
    }
}
