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
        static bool realTimeSearch = false;
        private SearchEngine S;
        public MainWindow()
        {
            InitializeComponent();

            Task T = new Task(()=>MessageBox.Show("正在初始化，完成后请关闭此窗口..."));
            T.Start();
            /** test */
            if (System.IO.File.Exists("DoNotUsePython"))
            {
                SearchEngine.UsingPipeEngine = false;
                UsingWordnet.IsChecked = false;
                UsingWordnet.IsEnabled = false;
            }
            try
            {
                S = new SearchEngine();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message,"Error");
                SearchEngine.UsingPipeEngine = false;
                UsingWordnet.IsChecked = false;
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (realTimeSearch && TextInput.Text!="")
            {
                try
                {
                    string query = TextInput.Text;
                    string[] result = S.Search(query);
                    TextShowing.Text = ".\n";
                    for(int i=0;i<result.Length;++i)
                    {
                        TextShowing.Text += result[i] + "\n.\n";
                    }
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.Message,"Error");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextInput.Text != "")
            {
                try
                {
                    string query = TextInput.Text;
                    string[] result = S.Search(query);
                    TextShowing.Text = ".\n";
                    for (int i = 0; i < result.Length; ++i)
                    {
                        TextShowing.Text += result[i] + "\n.\n";
                    }
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.Message, "Error");
                }
            }
        }

        private void RealTimeSearchChecked(object sender, RoutedEventArgs e)
        {
            realTimeSearch = true;
        }

        private void RealTimeSearchUnchecked(object sender, RoutedEventArgs e)
        {
            realTimeSearch = false;
        }

        private void UsingWordNetChecked(object sender, RoutedEventArgs e)
        {
            SearchEngine.UsingPipeEngine = true;
        }

        private void UsingWordNetUnchecked(object sender, RoutedEventArgs e)
        {
            SearchEngine.UsingPipeEngine = false;
        }
    }
}
