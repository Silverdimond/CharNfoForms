using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Unicode;
using System.Windows.Forms;

namespace CharNfoForms
{
    public partial class CharNfoForm : Form
    {
        public CharNfoForm()
        {
            InitializeComponent();
            if (File.Exists("config.json"))
            {
                using Stream stream = File.OpenRead("config.json");
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
                reader.Dispose();
                checkBox1.Checked = JsonSerializer.Deserialize<Config>(content).StayOnTop;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Explain();
        }

        private void Explain()
        {
            richTextBox1.Text = string.Empty;
            foreach (char character in textBox1.Text)
            {
                richTextBox1.Text += GetCodePointInfo(character) + Environment.NewLine;
            }
        }

        private static string GetCodePointInfo(int codePoint)
        {
            UnicodeCharInfo charInfo = UnicodeInfo.GetCharInfo(codePoint);
            return UnicodeInfo.GetDisplayText(charInfo) + '\t' + "U+" + codePoint.ToString("X4") + '\t' + (charInfo.Name ?? charInfo.OldName) + charInfo.Category;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = checkBox1.Checked;
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Explain();
                e.Handled = true;
            }
        }

        private void CharNfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            using StreamWriter streamWriter = new StreamWriter("config.json");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            streamWriter.Write(JsonSerializer.Serialize(new Config { StayOnTop = checkBox1.Checked }, options));
        }
    }
}