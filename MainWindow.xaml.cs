using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectA
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            /** test */


            

            LucenceEngine.ReadDocs("resources/Docs.txt", "resources/idx");   

            LucenceEngine L = new LucenceEngine("resources/idx");

            string[] s = L.Search("madam is");

            B.Text = "";

            for (int i = 0; i < s.Length; i += 1)
            {
                B.Text += s[i] + "\n";
            }

            PipeEngine p = new PipeEngine("testpipe");
            p.StartEngine();

            B.Text += p.Request("Request Request\n");
        }

    }
}
