using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToeGame
{
    public partial class Form1 : Form
    {
        private bool isPlayerXTurn; // true - ход игрока Х, false - ход игрока O
        private bool playWithBot;
        private Random random = new Random();
        private Color xColor;
        private Color oColor;

        public Form1()
        {
            InitializeComponent();
            resetButton.Text = "Играть снова";
            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            gamePanel.Visible = false;
            resetButton.Visible = false;
            mainMenuPanel.Visible = true;
        }

        private void StartGame(bool playWithBot)
        {
            this.playWithBot = playWithBot;
            mainMenuPanel.Visible = false;
            gamePanel.Visible = true;
            reset();
        }

        private void EnableButtons(bool enable)
        {
            foreach (var button in gamePanel.Controls.OfType<Button>())
            {
                if (button != resetButton)
                {
                    button.Enabled = enable && button.Text == "";
                }
            }
        }

        private void reset()
        {
            xColor = GetRandomPastelColor();
            oColor = GetRandomPastelColor();

            foreach (var button in gamePanel.Controls.OfType<Button>())
            {
                if (button != resetButton)
                {
                    button.Text = "";
                    button.BackColor = SystemColors.Control;
                    button.Enabled = true;
                }
            }
            isPlayerXTurn = true;
            resetButton.Visible = false;
        }

        private void CheckGameStatus()
        {
            if (CheckWin("X"))
            {
                MessageBox.Show("Выиграл игрок X");
                EnableButtons(false);
                resetButton.Visible = true;
            }
            else if (CheckWin("O"))
            {
                MessageBox.Show(playWithBot ? "Выиграл компьютер" : "Выиграл игрок O");
                EnableButtons(false);
                resetButton.Visible = true;
            }
            else if (IsBoardFull())
            {
                MessageBox.Show("Ничья");
                EnableButtons(false);
                resetButton.Visible = true;
            }
            else if (playWithBot && !isPlayerXTurn)
            {
                ComputerMove();
            }
        }

        private bool IsBoardFull()
        {
            return gamePanel.Controls.OfType<Button>().All(b => b.Text != "" || b == resetButton);
        }

        private bool CheckWin(string mark)
        {
            var buttons = gamePanel.Controls.OfType<Button>().Where(b => b != resetButton).ToArray();

            int[,] winPositions = new int[,] {
                {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Горизонтальные
                {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Вертикальные
                {0, 4, 8}, {2, 4, 6}             // Диагональные
            };

            for (int i = 0; i < winPositions.GetLength(0); i++)
            {
                if (buttons[winPositions[i, 0]].Text == mark &&
                    buttons[winPositions[i, 1]].Text == mark &&
                    buttons[winPositions[i, 2]].Text == mark)
                {
                    Color winColor = mark == "X" ? xColor : oColor;
                    buttons[winPositions[i, 0]].BackColor = winColor;
                    buttons[winPositions[i, 1]].BackColor = winColor;
                    buttons[winPositions[i, 2]].BackColor = winColor;
                    return true;
                }
            }

            return false;
        }

        private void ComputerMove()
        {
            var emptyButtons = gamePanel.Controls.OfType<Button>().Where(b => b.Text == "" && b != resetButton).ToList();
            if (emptyButtons.Count > 0)
            {
                int index = random.Next(emptyButtons.Count);
                emptyButtons[index].Text = "O";
                emptyButtons[index].Enabled = false;
                emptyButtons[index].BackColor = oColor;
                isPlayerXTurn = true;
                CheckGameStatus();
            }
        }

        private Color GetRandomPastelColor()
        {
            int r = (random.Next(256) + 255) / 2;
            int g = (random.Next(256) + 255) / 2;
            int b = (random.Next(256) + 255) / 2;
            return Color.FromArgb(r, g, b);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Text == "")
            {
                if (isPlayerXTurn)
                {
                    button.Text = "X";
                    button.BackColor = xColor;
                }
                else
                {
                    button.Text = "O";
                    button.BackColor = oColor;
                }
                button.Enabled = false;
                isPlayerXTurn = !isPlayerXTurn;
                CheckGameStatus();
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            StartGame(false);
        }

        private void playWithBotButton_Click(object sender, EventArgs e)
        {
            StartGame(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var button in gamePanel.Controls.OfType<Button>())
            {
                button.Click += new EventHandler(button_Click);
            }
        }
    }
}