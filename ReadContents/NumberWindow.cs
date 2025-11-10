using System;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;

namespace ReadContents
{
    public partial class NumberWindow : Form
    {
        public NumberWindow()
        {
            InitializeComponent();
            initializeWindow();
            createButtons();
            createPictbox();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        CommonConst CONST_NUM = new CommonConst();
        private Button[] buttons;
        private PictureBox[] pictureBoxes;
        private System.Media.SoundPlayer player = null;

        private void initializeWindow()
        {
            //サイズや配置の初期設定
            int vIndex = CONST_NUM.BTN_HEIGHT + CONST_NUM.BTN_SPACE;
            int hIndex = CONST_NUM.BTN_WIDTH + CONST_NUM.BTN_SPACE;
            this.Height = vIndex * 3 + CONST_NUM.PICT_HEIGHT * 2 + CONST_NUM.BTN_SPACE * 8;
            this.Width = hIndex * 4 + CONST_NUM.BTN_SPACE * 4;
        }

        private void createButtons()
        {
            int vIndex = CONST_NUM.BTN_HEIGHT + CONST_NUM.BTN_SPACE;
            int hIndex = CONST_NUM.BTN_WIDTH + CONST_NUM.BTN_SPACE;

            //0～9までの10数の初期化
            this.buttons = new Button[10];

            for (int i = 0; i < buttons.Length; i++)
            {
                this.buttons[i] = new Button();

                this.buttons[i].Name = "bt" + i.ToString();
                this.buttons[i].Text = i.ToString();
                this.buttons[i].Height = CONST_NUM.BTN_HEIGHT;
                this.buttons[i].Width = CONST_NUM.BTN_WIDTH;
                this.buttons[i].Font = new Font(this.buttons[i].Font.OriginalFontName, CONST_NUM.TXT_SIZE);

                //0～9ボタンの配置
                if (i == 0)
                {
                    //0は左上に表示
                    this.buttons[i].Top = CONST_NUM.BTN_SPACE;
                    this.buttons[i].Left = CONST_NUM.BTN_SPACE;
                }
                else
                {
                    //1～9は3×3の9マス表示
                    this.buttons[i].Top = CONST_NUM.BTN_SPACE + (((i - 1) / 3) * vIndex);
                    this.buttons[i].Left = CONST_NUM.BTN_SPACE + hIndex + (((i - 1) % 3) * hIndex);
                }

                this.Controls.Add(this.buttons[i]);
                this.buttons[i].Click += new System.EventHandler(btnClick);
            }
        }

        private void createPictbox()
        {
            int boxLocationX = CONST_NUM.BTN_SPACE * 4;
            int boxLocationY = CONST_NUM.BTN_SPACE + (CONST_NUM.BTN_HEIGHT+ CONST_NUM.BTN_SPACE) * 3;

            //1～9個の画像格納領域の初期化
            this.pictureBoxes = new PictureBox[9];

            for (int i = 0; i < pictureBoxes.Length; i++)
            {
                this.pictureBoxes[i] = new PictureBox();
                this.pictureBoxes[i].Name = "pict" + (i + 1).ToString();
                this.pictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
                this.pictureBoxes[i].Height = CONST_NUM.PICT_HEIGHT;
                this.pictureBoxes[i].Width = CONST_NUM.PICT_WIDTH;
                this.pictureBoxes[i].Top = boxLocationY + (i / 5) * CONST_NUM.PICT_HEIGHT;
                this.pictureBoxes[i].Left = boxLocationX + (i % 5) * CONST_NUM.PICT_WIDTH;
                this.Controls.Add(this.pictureBoxes[i]);
            }
        }

        private void btnClick(object sender, System.EventArgs e)
        {
            //引数のボタンオブジェクトをセット
            Button btn = (Button)sender;

            //メディアフォルダを取得
            string currentDirectory = Directory.GetCurrentDirectory();
            string parentDirectory = Path.GetDirectoryName(currentDirectory);
            parentDirectory = Path.GetDirectoryName(parentDirectory);
            string mediaDirectory = Path.Combine(parentDirectory, "number");

            //画像メディアを取得
            string imageMediaPath = Path.Combine(mediaDirectory, "image.jpg");
            setImage(imageMediaPath, int.Parse(btn.Text));

            //音声メディアのパスを取得
            string voiceFileName = btn.Text + ".wav";
            string voiceMediaPath = Path.Combine(mediaDirectory, voiceFileName);

            player = new System.Media.SoundPlayer(voiceMediaPath);
            if (player != null)
            {
                player.Play();
            }
        }

        private void setImage(string imagePath, int idx)
        {
            //画像を読み込んでpictureBoxに表示
            try
            {
                if (System.IO.File.Exists(imagePath))
                {
                    for (int i = 0; i < 9; i++)
                    {
                        //Image.FromFileメソッドを使用
                        string pictboxName = "pict" + (i + 1).ToString();
                        foreach (Control control in this.Controls)
                        {
                            if (control is PictureBox && control.Name == pictboxName)
                            {
                                PictureBox pictureBox = (PictureBox)control;

                                if (idx > 0 && i < idx)
                                {
                                    pictureBox.Image = Image.FromFile(imagePath);
                                }
                                else
                                {
                                    pictureBox.Image = null;
                                }

                                break;
                            }
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("画像の読み込み中にエラーが発生しました: " + ex.Message);
            }
        }
    }
}
