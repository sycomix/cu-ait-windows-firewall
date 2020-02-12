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
using AIT_LABO_006.LIB;
using NetFwTypeLib;

namespace AIT_LABO_006.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // NIET VERGETEN : referentie leggen via browse naar C:\windows\system32\FirewallAPI.dll
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //WinFireWall.BlockIp("1.2.3.4", "_test_block_1234");
            winAddRule venster = new winAddRule();
            venster.ShowDialog();
        }

        private void LeesRules(object sender, RoutedEventArgs e)
        {
            NET_FW_RULE_DIRECTION_ richting;
            if ((bool)rdbInboundRules.IsChecked) richting = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            else richting = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            VulRules(richting);
        }
        private void VulRules(NET_FW_RULE_DIRECTION_ richting)
        {
            lstRules.Items.Clear();
            ListBoxItem itm;
            foreach (INetFwRule regel in WinFireWall.GetAllRules(richting))
            {
                itm = new ListBoxItem();
                itm.Content = regel.Name;
                itm.Tag = regel;
                lstRules.Items.Add(itm);
            }
        }
        private void ClearDetails()
        {
            txtFirewallRuleName.Text = "";
            lblBeschrijving.Content = "";
            chkProfielDomein.IsChecked = false;
            chkProfielOpenbaar.IsChecked = false;
            chkProfielPrive.IsChecked = false;
            chkIsActief.IsChecked = false;
            chkActionAllow.IsChecked = false;
            chkActionAllow.Background = Brushes.White;
            chkActionBlock.IsChecked = false;
            chkActionBlock.Background = Brushes.White;
            txtProgramma.Text = "";
            txtLokaalAdres.Text = "";
            txtExternAdres.Text = "";
            txtLokalePoorten.Text = "";
            txtExternePoorten.Text = "";
            txtProtocol.Text = "";
        }
        private void LstRules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearDetails();
            if (lstRules.SelectedItem == null) return;

            ListBoxItem itm =(ListBoxItem)lstRules.SelectedItem;
            INetFwRule regel = (INetFwRule)itm.Tag;

            txtFirewallRuleName.Text = regel.Name;
            lblBeschrijving.Content = regel.Description;

            //int d = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN;  //1
            //int p = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE; //2
            //int pu = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC;  //4
            //int a = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL; //2147483647
            //int x = (int)regel.Profiles;
            if (regel.Profiles == (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL)
            {
                chkProfielDomein.IsChecked = true;
                chkProfielOpenbaar.IsChecked = true;
                chkProfielPrive.IsChecked = true;
            }
            if (regel.Profiles == 4 || regel.Profiles == 6 || regel.Profiles == 7)
                chkProfielOpenbaar.IsChecked = true;
            if (regel.Profiles == 2 || regel.Profiles == 3 || regel.Profiles == 6 || regel.Profiles == 7)
                chkProfielPrive.IsChecked = true;
            if (regel.Profiles == 1 || regel.Profiles == 3 || regel.Profiles == 5 || regel.Profiles == 7)
                chkProfielOpenbaar.IsChecked = true;

            chkIsActief.IsChecked = regel.Enabled;

            if (regel.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
            {
                chkActionAllow.IsChecked = true;
                chkActionAllow.Background = Brushes.ForestGreen;
            }
            else if (regel.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
            {
                chkActionBlock.IsChecked = true;
                chkActionBlock.Background = Brushes.Tomato;
            }
            //else if (regel.Action == NET_FW_ACTION_.NET_FW_ACTION_MAX)

            if (regel.ApplicationName != null)
                txtProgramma.Text = regel.ApplicationName;
            else
                txtProgramma.Text = "<alle programma's>";
            string x = regel.RemoteAddresses;
            if (regel.LocalAddresses == "*")
                txtLokaalAdres.Text = "<elk willekeurig adres>";
            else
                txtLokaalAdres.Text = regel.LocalAddresses;
            if (regel.RemoteAddresses == "*")
                txtExternAdres.Text = "<elk willekeurig adres>";
            else
                txtExternAdres.Text = regel.RemoteAddresses;

            if (regel.LocalPorts == "*")
                txtLokalePoorten.Text = "<elke willekeurige poort>";
            else
                txtLokalePoorten.Text = regel.LocalPorts;
            if (regel.RemotePorts == "*")
                txtExternePoorten.Text = "<elke willekeurige poort>";
            else
                txtExternePoorten.Text = regel.RemotePorts;

            txtProtocol.Text = WinFireWall.getProtocolAsString(regel.Protocol);

        }
    }
}
