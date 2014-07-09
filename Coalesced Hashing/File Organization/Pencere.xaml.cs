using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics; //Sonradan Eklenen
using System.Windows.Threading; //Sonradan Eklenen

namespace File_Organization
{
    public struct Tablo
    {
        public short veri;
        public sbyte link;
    }

    public partial class Pencere
    {
        private const string hatamesajı = "Error, Try Again";
        private const string floatkontrol = "#0.00000";

        private readonly TextBox[] sayı_textbox;
        private readonly TextBlock[] veri_textblock;
        private readonly TextBlock[] link_textblock;

        private readonly Tablo[] tablo = new Tablo[10];
        private readonly short[] sayı = new short[10];

        private readonly Stopwatch kronometre = new Stopwatch();
        private readonly DispatcherTimer zamanlayıcı = new DispatcherTimer();
        private readonly Random random = new Random();

        private ushort limit = 900;
        private byte adet = 9;

        public Pencere()
        {
            InitializeComponent();

            Geliştirici_Label.Content = "Asil Arslan\n111180006\nGazi Universty\nComputer Engineering \nDepartment\n\nIDE: Visual Studio 2012\nPlatform: C#, WPF\nLicense: GPLv3+";
            sayı_textbox = new TextBox[9] { Sayı1_TextBox, Sayı2_TextBox, Sayı3_TextBox, Sayı4_TextBox, Sayı5_TextBox, Sayı6_TextBox, Sayı7_TextBox, Sayı8_TextBox, Sayı9_TextBox };
            veri_textblock = new TextBlock[10] { Veri1_TextBlock, Veri2_TextBlock, Veri3_TextBlock, Veri4_TextBlock, Veri5_TextBlock, Veri6_TextBlock, Veri7_TextBlock, Veri8_TextBlock, Veri9_TextBlock, Veri10_TextBlock };
            link_textblock = new TextBlock[10] { Link1_TextBlock, Link2_TextBlock, Link3_TextBlock, Link4_TextBlock, Link5_TextBlock, Link6_TextBlock, Link7_TextBlock, Link8_TextBlock, Link9_TextBlock, Link10_TextBlock };
            zamanlayıcı.Interval = new TimeSpan(0, 0, 0, 2);
            zamanlayıcı.Tick += Zamanlayıcı;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Tablo_Oluştur_Button_Click(this, e);
        }

        private void Tablo_Oluştur_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adet = Convert.ToByte(Adet_TextBox.Text);
                limit = Convert.ToUInt16(Limit_TextBox.Text);
                if (limit == 0 || limit > 900 || adet > 9)
                {
                    throw new OverflowException();
                }
            }
            catch
            {
                HataGöster(); //Harici Fonksiyon
                return;
            }

            for (byte i = 0; i < adet; i++)
            {
                sayı[i] = (short)(random.Next() % limit);
                sayı_textbox[i].Text = sayı[i].ToString();
                sayı_textbox[i].IsEnabled = true;
            }
            for (byte i = adet; i < 9; i++)
            {
                sayı[i] = -1; //Boş Değer
                sayı_textbox[i].Text = "Space";
                sayı_textbox[i].IsEnabled = false;
            }

            adet = Convert.ToByte(Adet_TextBox.Text);
            TabloSıfırla(); //Harici Fonksiyon
            for (byte i = 0; i < adet; i++)
            {
                Ekle(sayı[i]); //Harici Fonksiyon
            }
            PerformansÖlç(); //Harici Fonksiyon
        }

        private void PerformansÖlç()
        {
            float temp = 0;
            kronometre.Start();
            for (byte i = 0; i < adet; i++)
            {
                temp += Ara(sayı[i], false);
            }
            kronometre.Stop();
            OkumaPerformansZaman_TextBlock.Text = "Average " + ((kronometre.ElapsedTicks * 1000.0) / Stopwatch.Frequency * adet).ToString(floatkontrol) + " ms";
            OkumaPerformansProbe_TextBlock.Text = "Average " + (temp / adet).ToString(floatkontrol) + " Probe";
            kronometre.Reset();
        }

        private void HataGöster()
        {
            Bilgi_TextBlock.Text = hatamesajı;
            zamanlayıcı.Start();
        }

        private void Zamanlayıcı(object sender, EventArgs e)
        {
            Bilgi_TextBlock.Text = "Box of Info";
            zamanlayıcı.Stop();
        }

        private void TabloSıfırla()
        {
            for (byte i = 0; i < 10; i++)
            {
                tablo[i].veri = -1; //Boş Değer
                veri_textblock[i].Text = "Space";
                tablo[i].link = -1; //Boş Değer
                link_textblock[i].Text = "Space";
            }
        }

        private void Ekle_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (byte i = 0; i < Convert.ToByte(Adet_TextBox.Text); i++)
                {
                    sayı[i] = Convert.ToInt16(sayı_textbox[i].Text);
                    if (sayı[i] < 0 || sayı[i] > limit)
                    {
                        throw new OverflowException();
                    }
                }
                sayı[adet] = Convert.ToInt16(VeriSon_TextBox.Text);
                if (sayı[adet] < 0 || sayı[adet] > limit)
                {
                    throw new OverflowException();
                }
            }
            catch
            {
                HataGöster(); //Harici Fonksiyon
                return;
            }

            adet++;
            Ekle(sayı[adet - 1]); //Harici Fonksiyon
            PerformansÖlç(); //Harici Fonksiyon
        }

        private void Ekle(short ekleneceksayı)
        {
            /*Algoritma Kontrol*/
            if (LISCH_Button.IsChecked == true)
            {
                LISCH_Ekle(ekleneceksayı); //Harici Fonksiyon
            }
            else if (EISCH_Button.IsChecked == true)
            {
                EISCH_Ekle(ekleneceksayı); //Harici Fonksiyon
            }
            else if (LICH_Button.IsChecked == true)
            {
                LICH_Ekle(ekleneceksayı); //Harici Fonksiyon
            }
            else if (EICH_Button.IsChecked == true)
            {
                EICH_Ekle(ekleneceksayı); //Harici Fonksiyon
            }
            else if (BEISCH_Button.IsChecked == true)
            {
                BEISCH_Ekle(ekleneceksayı); //Harici Fonksiyon
            }
            else if (RLISCH_Button.IsChecked == true)
            {
                RLISCH_Ekle(ekleneceksayı); //Harici Fonksiyon
            }

            Ara_Button.IsEnabled = true;
        }

        private void LISCH_Ekle(short ekleneceksayı)
        {
            sbyte id = (sbyte)(ekleneceksayı % 10);
            if (tablo[id].veri == -1)
            {
                tablo[id].veri = ekleneceksayı;
                veri_textblock[id].Text = ekleneceksayı.ToString();
            }
            else
            {
                sbyte son_id = 9;
                while (tablo[son_id].veri != -1)
                {
                    son_id--;
                }
                while (tablo[id].link != -1)
                {
                    id = tablo[id].link;
                }

                tablo[son_id].veri = ekleneceksayı;
                veri_textblock[son_id].Text = ekleneceksayı.ToString();

                tablo[id].link = son_id;
                link_textblock[id].Text = son_id.ToString();
            }
        }

        private void EISCH_Ekle(short ekleneceksayı)
        {
            sbyte id = (sbyte)(ekleneceksayı % 10);
            if (tablo[id].veri == -1)
            {
                tablo[id].veri = ekleneceksayı;
                veri_textblock[id].Text = ekleneceksayı.ToString();
            }
            else
            {
                sbyte son_id = id;
                while (tablo[son_id].link != -1)
                {
                    son_id = tablo[son_id].link;
                }
                while (tablo[son_id].veri != -1)
                {
                    son_id = (sbyte)((son_id + 9) % 10); //Dairesel Bir Üst ID
                }

                tablo[son_id].veri = ekleneceksayı;
                veri_textblock[son_id].Text = ekleneceksayı.ToString();

                if (tablo[id].link != -1)
                {
                    tablo[son_id].link = tablo[id].link;
                    link_textblock[son_id].Text = tablo[id].link.ToString();
                }
                tablo[id].link = son_id;
                link_textblock[id].Text = son_id.ToString();
            }
        }

        private void LICH_Ekle(short ekleneceksayı)
        {
            sbyte id = (sbyte)(ekleneceksayı % (9 - LICH_Overflow_ComboBox.SelectedIndex));
            if (tablo[id].veri == -1)
            {
                tablo[id].veri = ekleneceksayı;
                veri_textblock[id].Text = ekleneceksayı.ToString();
            }
            else
            {
                sbyte son_id = 9;
                while (tablo[son_id].veri != -1)
                {
                    son_id--;
                }
                while (tablo[id].link != -1)
                {
                    id = tablo[id].link;
                }

                tablo[son_id].veri = ekleneceksayı;
                veri_textblock[son_id].Text = ekleneceksayı.ToString();

                tablo[id].link = son_id;
                link_textblock[id].Text = son_id.ToString();
            }
        }

        private void EICH_Ekle(short ekleneceksayı)
        {
            sbyte id = (sbyte)(ekleneceksayı % (9 - EICH_Overflow_ComboBox.SelectedIndex));
            if (tablo[id].veri == -1)
            {
                tablo[id].veri = ekleneceksayı;
                veri_textblock[id].Text = ekleneceksayı.ToString();
            }
            else
            {
                sbyte son_id = id;
                while (tablo[son_id].link != -1)
                {
                    son_id = tablo[son_id].link;
                }
                while (tablo[son_id].veri != -1)
                {
                    son_id = (sbyte)((son_id + 9) % 10); //Dairesel Bir Üst ID
                }

                tablo[son_id].veri = ekleneceksayı;
                veri_textblock[son_id].Text = ekleneceksayı.ToString();

                if (tablo[id].link != -1)
                {
                    tablo[son_id].link = tablo[id].link;
                    link_textblock[son_id].Text = tablo[id].link.ToString();
                }
                tablo[id].link = son_id;
                link_textblock[id].Text = son_id.ToString();
            }
        }

        private void BEISCH_Ekle(short ekleneceksayı)
        {
            sbyte id = (sbyte)(ekleneceksayı % 10);
            if (tablo[id].veri == -1)
            {
                tablo[id].veri = ekleneceksayı;
                veri_textblock[id].Text = ekleneceksayı.ToString();
            }
            else
            {
                bool temp = true;
                sbyte alt = -1;
                sbyte üst = 9;
                sbyte son_id = üst;
                while (tablo[son_id].veri != -1)
                {
                    if (temp)
                    {
                        son_id = ++alt;
                    }
                    else
                    {
                        son_id = --üst;
                    }
                    temp = !temp;
                }

                tablo[son_id].veri = ekleneceksayı;
                veri_textblock[son_id].Text = ekleneceksayı.ToString();

                if (tablo[id].link != -1)
                {
                    tablo[son_id].link = tablo[id].link;
                    link_textblock[son_id].Text = tablo[id].link.ToString();
                }
                tablo[id].link = son_id;
                link_textblock[id].Text = son_id.ToString();
            }
        }

        private void RLISCH_Ekle(short ekleneceksayı)
        {
            sbyte id = (sbyte)(ekleneceksayı % 10);
            if (tablo[id].veri == -1)
            {
                tablo[id].veri = ekleneceksayı;
                veri_textblock[id].Text = ekleneceksayı.ToString();
            }
            else
            {
                sbyte son_id = (sbyte)(random.Next(10));
                while (tablo[son_id].veri != -1)
                {
                    son_id = (sbyte)(random.Next(10));
                }
                while (tablo[id].link != -1)
                {
                    id = tablo[id].link;
                }

                tablo[son_id].veri = ekleneceksayı;
                veri_textblock[son_id].Text = ekleneceksayı.ToString();

                tablo[id].link = son_id;
                link_textblock[id].Text = son_id.ToString();
            }
        }

        private void Ara_Button_Click(object sender, RoutedEventArgs e)
        {
            ushort istenenveri;
            try
            {
                istenenveri = Convert.ToUInt16(VeriSon_TextBox.Text);
            }
            catch
            {
                HataGöster(); //Harici Fonksiyon
                return;
            }
            Ara((short)istenenveri, true); //Harici Fonksiyon
        }

        private byte Ara(short istenenveri, bool gerçek)
        {
            byte probe = 1;
            sbyte id = (sbyte)(istenenveri % 10);
            while (true)
            {
                if (tablo[id].veri == istenenveri)
                {
                    if (gerçek)
                    {
                        Bilgi_TextBlock.Text = "Found, Index: " + id;
                        zamanlayıcı.Start();
                    }
                    break;
                }
                else
                {
                    id = tablo[id].link;
                    probe++;
                }

                if (id == -1)
                {
                    if (gerçek)
                    {
                        Bilgi_TextBlock.Text = "Results not found.";
                        zamanlayıcı.Start();
                    }
                    break;
                }
            }
            return probe;
        }

        private void Yenile_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adet = Convert.ToByte(Adet_TextBox.Text);
                for (byte i = 0; i < adet; i++)
                {
                    sayı[i] = Convert.ToInt16(sayı_textbox[i].Text);
                    if (sayı[i] < 0 || sayı[i] > limit)
                    {
                        throw new OverflowException();
                    }
                }
            }
            catch
            {
                HataGöster(); //Harici Fonksiyon
                return;
            }

            TabloSıfırla(); //Harici Fonksiyon
            for (byte i = 0; i < adet; i++)
            {
                Ekle(sayı[i]); //Harici Fonksiyon
            }
            PerformansÖlç(); //Harici Fonksiyon
        }
    }
}