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

            //LucenceEngine.ReadDocs("../1.txt", "../idx");

            LucenceEngine L = new LucenceEngine("../idx");

            string[] s = L.Search("madam hello");

            B.Text = "";

            for (int i = 0; i < s.Length; i += 1)
            {
                B.Text += s[i] + "\n";
            }
            
            
        }

    }
}
