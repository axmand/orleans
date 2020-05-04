using System;
using System.Windows.Forms;
using Engine.NLP.Utils;

namespace Engine.NLP.Forms
{
    public partial class NLPConfigForm : Form
    {
        public NLPConfigForm()
        {
            InitializeComponent();
            InitialConfigValue();
        }

        private void InitialConfigValue()
        {
            //word vector
            GloVe_File_textBox.Text = NLPConfiguration.GloVeEmbeddingString;
            //stanford nlp
            StanfordNLP_Server_Url_textBox.Text = NLPConfiguration.CoreNLPAddress;
            StanfordNLP_Server_Port_textBox.Text = NLPConfiguration.CoreNLPPort;
            //baidu AI
            BaiduAI_APIKey_textBox.Text = NLPConfiguration.BaiduAIAPIKey;
            BaiduAI_AppId_textBox.Text = NLPConfiguration.BaiduAIAppId;
            BaiduAI_Secret_Key_textBox.Text = NLPConfiguration.BaiduAISecretKey;
        }

        /// <summary>
        /// 设置glove model file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GloVe_File_button_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog opg = new OpenFileDialog())
                if (opg.ShowDialog() == DialogResult.OK)
                    GloVe_File_textBox.Text = opg.FileName;
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            NLPConfiguration.GloVeEmbeddingString = GloVe_File_textBox.Text;
            NLPConfiguration.CoreNLPAddress = StanfordNLP_Server_Url_textBox.Text;
            NLPConfiguration.CoreNLPPort = StanfordNLP_Server_Port_textBox.Text;
            NLPConfiguration.BaiduAIAPIKey = BaiduAI_APIKey_textBox.Text;
            NLPConfiguration.BaiduAIAppId = BaiduAI_AppId_textBox.Text;
            NLPConfiguration.BaiduAISecretKey = BaiduAI_Secret_Key_textBox.Text;
            Close();
        }
    }
}
