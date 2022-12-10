using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace WindowsFormsApp1{
    public partial class Form1 : Form{

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(int hwnd, int howshow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(int hwnd);
        public static int myID=0;
        public Form1(){
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {}

        private void button1_Click(object sender, EventArgs e){
            if(textBox2.Text == "kkutu") {
                ///--and char_length(_id) < 4
                SendImage(textBox1.Text, "select * from kkutu_en where _id not like '{%} {%}' and _id like '" +
                    textBox3.Text
                    + "{%}' order by {(}char_length{(}_id{)} - char_length{(}replace{(}_id,'" +
                    textBox4.Text
                    +"', ''{)}{)}{)} desc{f5}");
            }else SendImage(textBox1.Text, textBox2.Text+"{Enter}");
            textBox2.Focus();
        }



        public void SendImage(string room, string imagePath){
            if (myID == 0) {
                myID = FindWindow(null, "Form1");
            }

            int j = FindWindow(null, room);
            Console.WriteLine(string.Format("Number in {1}: {0},", j,room));
            if (j == 0){
                label2.Text = "방을 찾을 수 없습니다 : " + room;
                return;
            }

            ShowWindowAsync(j, 1);
            Thread.Sleep(20);
            ShowWindowAsync(j, 1);
            SetForegroundWindow(j);

            Thread.Sleep(200);

            SendKeys.SendWait(Kor2Eng(imagePath));
            //SendKeys.Send("{ENTER}"); //카카오톡 화면에 복사를하고 엔터를 입력합니다.

            ShowWindowAsync(myID, 1);
            Thread.Sleep(20);
            ShowWindowAsync(myID, 1);
            SetForegroundWindow(myID);

        }

        public static string Kor2Eng(string kor){
            if (kor == null) throw new ArgumentNullException();
            if (kor.Length == 0) throw new ArgumentException();

            var sb = new System.Text.StringBuilder(kor.Length * 2);
            int ini, vow, und;

            int i = 0;
            do{
                if (!Split(kor[i], out ini, out vow, out und))
                    sb.Append(kor[i]);
                else{
                    if (ini != -1) sb.Append(Table[Array.IndexOf<string>(Table, IniS[ini]) + 1]);
                    if (vow != -1) sb.Append(Table[Array.IndexOf<string>(Table, VolS[vow]) + 1]);
                    if (und > 0) sb.Append(Table[Array.IndexOf<string>(Table, UndS[und]) + 1]);
                }
            } while (++i < kor.Length);

            return sb.ToString();
        }

        private static bool Split(char src, out int ini, out int vow, out int und){
            // 원래 초중종 나눔
            int charCode = Convert.ToInt32(src) - 44032;
            int i;

            if ((charCode < 0) || (charCode > 11171)){
                ini = vow = und = -1;

                if ((i = Array.IndexOf<char>(IniC, src)) != -1)
                    ini = i;
                else if ((i = Array.IndexOf<char>(VolC, src)) != -1)
                    vow = i;
                else if (src != '\0' && (i = Array.IndexOf<char>(UndC, src)) != -1)
                    und = i;
            }
            else{
                ini = charCode / 588;
                vow = (charCode % 588) / 28;
                und = (charCode % 588) % 28;
            }

            return ini != -1 || vow != -1 || und != -1;
        }

        private static readonly char[] IniC = { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
        private static readonly string[] IniS = { "ㄱ", "ㄲ", "ㄴ", "ㄷ", "ㄸ", "ㄹ", "ㅁ", "ㅂ", "ㅃ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅉ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ" };

        private static readonly char[] VolC = { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
        private static readonly string[] VolS = { "ㅏ", "ㅐ", "ㅑ", "ㅒ", "ㅓ", "ㅔ", "ㅕ", "ㅖ", "ㅗ", "ㅘ", "ㅙ", "ㅚ", "ㅛ", "ㅜ", "ㅝ", "ㅞ", "ㅟ", "ㅠ", "ㅡ", "ㅢ", "ㅣ" };

        private static readonly char[] UndC = { '\0', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
        private static readonly string[] UndS = { "", "ㄱ", "ㄲ", "ㄳ", "ㄴ", "ㄵ", "ㄶ", "ㄷ", "ㄹ", "ㄺ", "ㄻ", "ㄼ", "ㄽ", "ㄾ", "ㄿ", "ㅀ", "ㅁ", "ㅂ", "ㅄ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ" };

        private static readonly string[] Table ={
            "ㄱ", "r", "ㄲ", "R",  "ㄳ", "rt",
            "ㄴ", "s", "ㄵ", "sw", "ㄶ", "sg",
            "ㄷ", "e", "ㄸ", "E",
            "ㄹ", "f", "ㄺ", "fr", "ㄻ", "fa", "ㄼ", "fq", "ㄽ", "ft", "ㄾ", "fx", "ㄿ", "fv", "ㅀ", "fg",
            "ㅁ", "a",
            "ㅂ", "q", "ㅃ", "Q",  "ㅄ", "qt",
            "ㅅ", "t", "ㅆ", "T",
            "ㅇ", "d",
            "ㅈ", "w",
            "ㅉ", "W",
            "ㅊ", "c",
            "ㅋ", "z",
            "ㅌ", "x",
            "ㅍ", "v",
            "ㅎ", "g",
            "ㅏ", "k",
            "ㅐ", "o", "ㅒ", "O",
            "ㅑ", "i",
            "ㅓ", "j",
            "ㅔ", "p", "ㅖ", "P",
            "ㅕ", "u",
            "ㅗ", "h", "ㅘ", "hk", "ㅙ", "ho", "ㅚ", "hl",
            "ㅛ", "y",
            "ㅜ", "n", "ㅝ", "nj", "ㅞ", "np", "ㅟ", "nl",
            "ㅠ", "b",
            "ㅣ", "l",
            "ㅡ", "m", "ㅢ", "ml",
        };

        private void key_Down(object sender, KeyEventArgs e){
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}