using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt6_6
{
    public partial class Form1 : Form
    {
        //SPOLEČNÉ ATRIBUTY
        //"♦","♥", "♣","♠"
        public int pozice_balicku = 0; //nesmi byt vetsi nez balicek.length

        public string[] popisky = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        public int[] hodnoty = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        public string[] symboly = { "Srdce", "Kary", "Piky", "Listy" };
        public char[] znaky = { '♥','♦', '♠', '♣'};

        Card karta_na_stole1 = new Card("default", "default", 50, '♦');
        Card karta_na_stole2 = new Card("default", "default", 50, '♦');
        Card karta_na_stole3 = new Card("default", "default", 50, '♦');
        Card karta_na_stole4 = new Card("default", "default", 50, '♦');

        List<Card> balicek_karet;
        List<Card> herni_slot1;
        List<Card> herni_slot2;
        List<Card> herni_slot3;
        List<Card> herni_slot4;

        Boolean hrac_je_na_tahu;
        Boolean odlizano = false;
        Boolean konec = false;


        //ATRIBUTY HRÁČE
        Card karta_v_paklu;

        Card karta_v_ruce1;
        Card karta_v_ruce2;
        Card karta_v_ruce3;
        Card karta_v_ruce4;
        Card karta_v_ruce5;


        Card karta_rezerva1 = new Card("default", "default", 50, '♦');
        Card karta_rezerva2 = new Card("default", "default", 50, '♦');
        Card karta_rezerva3 = new Card("default", "default", 50, '♦');
        Card karta_rezerva4 = new Card("default", "default", 50, '♦');

       
        List<Card> karty_v_paklu;

        List<Card> karty_rezerva1;
        List<Card> karty_rezerva2;
        List<Card> karty_rezerva3;
        List<Card> karty_rezerva4;


        //ATRIBUTY POČÍTAČE

        Card karta_v_pakluPC;
        Card hrana_karta_z_ruky;

        Card karta_rezerva1PC;
        Card karta_rezerva2PC;
        Card karta_rezerva3PC;
        Card karta_rezerva4PC;


        List<Card> karty_v_pakluPC;

        List<Card> karty_v_rucePC;

        List<Card> karty_rezerva1PC;
        List<Card> karty_rezerva2PC;
        List<Card> karty_rezerva3PC;
        List<Card> karty_rezerva4PC;

        


        public Form1()
        {

            WelcomeForm Wf = new WelcomeForm();
            DialogResult dr = Wf.ShowDialog();
            InitializeComponent();
            //volitelny parametr pocet balicku
            pripravaStolu();
            zalozeniBalicku();
            zalozeniBalicku();


            int pocet_karet_paklu = Wf.pocet_karet;
            hrac_je_na_tahu = Wf.zacina_hrac;

            rozdaniKaretHraci(pocet_karet_paklu);
            rozdaniKaretPocitaci(pocet_karet_paklu);

         
            herniHlasatel.Text = "Velikost balicku je: " + balicek_karet.Count;
            if (!hrac_je_na_tahu)
            {
                tahPocitaceAsync();
            }

        }

        

        private void pripravaStolu()
        {
            balicek_karet = new List<Card>();
            herni_slot1 = new List<Card>();
            herni_slot2 = new List<Card>();
            herni_slot3 = new List<Card>();
            herni_slot4 = new List<Card>();
        }

        private void rozdaniKaretHraci(int pocet)
        {
            karty_rezerva1 = new List<Card>();
            karty_rezerva2 = new List<Card>();
            karty_rezerva3 = new List<Card>();
            karty_rezerva4 = new List<Card>();


            naplneniPaklu(pocet);
            novaKartaPakl();
        }

        private void rozdaniKaretPocitaci(int pocet)
        {
            karty_rezerva1PC = new List<Card>();
            karty_rezerva2PC = new List<Card>();
            karty_rezerva3PC = new List<Card>();
            karty_rezerva4PC = new List<Card>();
            karty_v_rucePC = new List<Card>();

            //zatim nejsou hotove
            naplneniPakluPC(pocet);
            novaKartaPaklPC();
            karta_rezerva1PC = new Card("default", "default", 50, '♦');
            karta_rezerva2PC = new Card("default", "default", 50, '♦');
            karta_rezerva3PC = new Card("default", "default", 50, '♦');
            karta_rezerva4PC = new Card("default", "default", 50, '♦');
        }



        /** ZALOZENI BALICKU
         * pridani do listu
         * zamichani v listu
         */
        private void zalozeniBalicku()
        {
            Card c;
            for (int i = 0; i < popisky.Length; i++)
            {
                for (int j = 0; j < symboly.Length; j++)
                {
                    c = new Card(popisky[i], symboly[j], hodnoty[i], znaky[j]);
                    balicek_karet.Add(c);
                }

            }
            
            balicek_karet = zamichat(balicek_karet);
            balicek_karet = zamichat(balicek_karet);
           
        }

        public List<Card> zamichat(List<Card> karty)
        {
            Random rand = new Random();
            return karty.OrderBy(_ => rand.Next()).ToList();
        }
         

        private void naplneniPaklu(int pocet)
        {
            karty_v_paklu = new List<Card>();
            for (int i = 0; i < pocet; i++)
            {
                karty_v_paklu.Add(balicek_karet.ElementAt(i));
                balicek_karet.Remove(balicek_karet.ElementAt(i));
            }
            labelPocetPaklu.Text = "Pocet:" + karty_v_paklu.Count;
        }
        public void novaKartaPakl()
        {
            if (karty_v_paklu.Count > 0)
            {

                karta_v_paklu = karty_v_paklu.ElementAt(0);
                if (jeCervena(karta_v_paklu))
                {
                    pakl.ForeColor = Color.Red;
                }
                else
                {
                    pakl.ForeColor = Color.Black;
                }
                pakl.Text = karta_v_paklu.getNazevKarty();
            }
            else
            {
                posledniKartaPakluAsync();
            }
            labelPocetPaklu.Text = "Pocet:" + karty_v_paklu.Count;


        }

        private void naplneniPakluPC(int pocet)
        {
            karty_v_pakluPC = new List<Card>();
            for (int i = 0; i < pocet; i++)
            {
                karty_v_pakluPC.Add(balicek_karet.ElementAt(i));
                balicek_karet.Remove(balicek_karet.ElementAt(i));
            }
            labelPocetPakluPC.Text = "Pocet:" + karty_v_pakluPC.Count;
        }

        public void novaKartaPaklPC()
        {
            if (karty_v_pakluPC.Count > 0)
            {

                karta_v_pakluPC = karty_v_pakluPC.ElementAt(0);
                if (jeCervena(karta_v_pakluPC))
                {
                    paklPC.ForeColor = Color.Red;
                }
                else
                {
                    paklPC.ForeColor = Color.Black;
                }
                paklPC.Text = karta_v_pakluPC.getNazevKarty();
            }
            else
            {
                posledniKartaPakluPCAsync();
            }
            labelPocetPakluPC.Text = "Pocet:" + karty_v_pakluPC.Count;


        }



        public async Task posledniKartaPakluAsync()
        {
            if (karty_v_paklu.Count < 1)
            {
                
                herniHlasatel.Text = "VYHRÁL JSI!!!";
                konec = true;
                pakl.Text = "👍";
                await Task.Delay(2500);
                this.Close();
                
            }
        }

        public async Task posledniKartaPakluPCAsync()
        {
            if (karty_v_pakluPC.Count < 1)
            {

                herniHlasatel.Text = "POČÍTAČ VYHRÁL!!!";
                konec = true;
                paklPC.Text = "👎";
                await Task.Delay(2500);
                this.Close();

            }
        }


        public Boolean jeCervena(Card karta)
        {
            return (karta.getZnak() == '♦' || karta.getZnak() == '♥');

        }

        

        private void pakl_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu && !konec)
            {
                if (karta_v_paklu.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_v_paklu.getPopis() == "A" && herniPole1.Text == "--"))
                {
                    //herniPole1.Text = karta_v_paklu.getNazevKarty();
                    //karta_na_stole1 = karta_v_paklu;
                    //karty_v_paklu.Remove(karty_v_paklu.ElementAt(0));
                    odehrajNaSlot1(karta_v_paklu);
                    karty_v_paklu.Remove(karty_v_paklu.ElementAt(0));
                    novaKartaPakl();


                }
                else if (karta_v_paklu.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_v_paklu.getPopis() == "A" && herniPole2.Text == "--"))
                {
                    //herniPole2.Text = karta_v_paklu.getNazevKarty();
                    //karta_na_stole2 = karta_v_paklu;
                    //karty_v_paklu.Remove(karty_v_paklu.ElementAt(0));
                    odehrajNaSlot2(karta_v_paklu);
                    karty_v_paklu.Remove(karty_v_paklu.ElementAt(0));
                    novaKartaPakl();

                }
                else if (karta_v_paklu.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_v_paklu.getPopis() == "A" && herniPole3.Text == "--"))
                {
                    //herniPole3.Text = karta_v_paklu.getNazevKarty();
                    //karta_na_stole3 = karta_v_paklu;
                    //karty_v_paklu.Remove(karty_v_paklu.ElementAt(0));

                    odehrajNaSlot3(karta_v_paklu);
                    karty_v_paklu.Remove(karty_v_paklu.ElementAt(0));
                    novaKartaPakl();

                }
                else if (karta_v_paklu.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_v_paklu.getPopis() == "A" && herniPole4.Text == "--"))
                {
                    //herniPole4.Text = karta_v_paklu.getNazevKarty();
                    // karta_na_stole4 = karta_v_paklu;
                    //karty_v_paklu.Remove(karty_v_paklu.ElementAt(0));

                    odehrajNaSlot4(karta_v_paklu);
                    karty_v_paklu.Remove(karty_v_paklu.ElementAt(0));
                    novaKartaPakl();

                }
                else
                {
                    herniHlasatel.Text = "Kartu z paklu nelze hrat";
                }
            }
            else
            {
                herniHlasatel.Text = "Nejsi na tahu";
            }
        }





        /** LIZANI KARET
         * 
         */
        private void Balicek_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu)
            {
                if (balicek_karet.Count > 0)
                {
                    
                    if (Ruka1.Text == "--")
                    {
                        karta_v_ruce1 = balicek_karet.ElementAt(0);
                        if (jeCervena(karta_v_ruce1))
                        {
                            Ruka1.ForeColor = Color.Red;

                        }
                        else
                        {
                            Ruka1.ForeColor = Color.Black;
                        }
                        Ruka1.Text = karta_v_ruce1.getNazevKarty();
                        balicek_karet.Remove(balicek_karet.ElementAt(0));
                        Ruka1.Enabled = true;


                    }
                    if (Ruka2.Text == "--")
                    {
                        karta_v_ruce2 = balicek_karet.ElementAt(0);
                        if (jeCervena(karta_v_ruce2))
                        {
                            Ruka2.ForeColor = Color.Red;

                        }
                        else
                        {
                            Ruka2.ForeColor = Color.Black;
                        }
                        Ruka2.Text = karta_v_ruce2.getNazevKarty();
                        balicek_karet.Remove(balicek_karet.ElementAt(0));
                        Ruka2.Enabled = true;
                    }
                    if (Ruka3.Text == "--")
                    {
                        karta_v_ruce3 = balicek_karet.ElementAt(0);
                        if (jeCervena(karta_v_ruce3))
                        {
                            Ruka3.ForeColor = Color.Red;

                        }
                        else
                        {
                            Ruka3.ForeColor = Color.Black;
                        }
                        Ruka3.Text = karta_v_ruce3.getNazevKarty();
                        balicek_karet.Remove(balicek_karet.ElementAt(0));
                        Ruka3.Enabled = true;
                    }
                    if (Ruka4.Text == "--")
                    {
                        karta_v_ruce4 = balicek_karet.ElementAt(0);
                        if (jeCervena(karta_v_ruce4))
                        {
                            Ruka4.ForeColor = Color.Red;

                        }
                        else
                        {
                            Ruka4.ForeColor = Color.Black;
                        }
                        Ruka4.Text = karta_v_ruce4.getNazevKarty();
                        balicek_karet.Remove(balicek_karet.ElementAt(0));
                        Ruka4.Enabled = true;
                    }
                    if (Ruka5.Text == "--")
                    {
                        karta_v_ruce5 = balicek_karet.ElementAt(0);
                        if (jeCervena(karta_v_ruce5))
                        {
                            Ruka5.ForeColor = Color.Red;

                        }
                        else
                        {
                            Ruka5.ForeColor = Color.Black;
                        }
                        Ruka5.Text = karta_v_ruce5.getNazevKarty();
                        balicek_karet.Remove(balicek_karet.ElementAt(0));
                        Ruka5.Enabled = true;
                    }
                    herniHlasatel.Text = "Velikost balicku je: " + balicek_karet.Count;

                    odlizano = true;
                    Balicek.Enabled = false;
                }

                else
                {
                    herniHlasatel.Text = "Nejsi na tahu: ";
                    herniHlasatel.Text = "V balicku došli karty, PROHRÁL JSI!!!: ";
                }

            }
            else
            {
                herniHlasatel.Text = "Nejsi na tahu: ";
            }
            

        }

        public void odehrajNaSlot1(Card karta_v_ruce)
        {
            herni_slot1.Add(karta_v_ruce);
            karta_na_stole1 = herni_slot1.Last();
            if (jeCervena(karta_na_stole1))
            {
                herniPole1.ForeColor = Color.Red;
            }
            else
            {
                herniPole1.ForeColor = Color.Black;
            }
            herniPole1.Text = karta_na_stole1.getNazevKarty();
            

        }
        public void odehrajNaSlot2(Card karta_v_ruce)
        {
            herni_slot2.Add(karta_v_ruce);
            karta_na_stole2 = herni_slot2.Last();
            if (jeCervena(karta_na_stole2))
            {
                herniPole2.ForeColor = Color.Red;
            }
            else
            {
                herniPole2.ForeColor = Color.Black;
            }
            herniPole2.Text = karta_na_stole2.getNazevKarty();
            

        }
        public void odehrajNaSlot3(Card karta_v_ruce)
        {
            herni_slot3.Add(karta_v_ruce);
            karta_na_stole3 = herni_slot3.Last();
            if (jeCervena(karta_na_stole3))
            {
                herniPole3.ForeColor = Color.Red;
            }
            else
            {
                herniPole3.ForeColor = Color.Black;
            }
            herniPole3.Text = karta_na_stole3.getNazevKarty();
            

        }
        public void odehrajNaSlot4(Card karta_v_ruce)
        {

            herni_slot4.Add(karta_v_ruce);
            karta_na_stole4 = herni_slot4.Last();
            if (jeCervena(karta_na_stole4))
            {
                herniPole4.ForeColor = Color.Red;
            }
            else
            {
                herniPole4.ForeColor = Color.Black;
            }
            herniPole4.Text = karta_na_stole4.getNazevKarty();
            

        }

        private void zalozitDo1(Card karta_v_ruce)
        {
            karty_rezerva1.Insert(0, karta_v_ruce);
            karta_rezerva1 = karty_rezerva1.ElementAt(0);
            if (jeCervena(karta_rezerva1))
            {
                rezerva1.ForeColor = Color.Red;
            }
            else
            {
                rezerva1.ForeColor = Color.Black;
            }
            rezerva1.Text = karta_rezerva1.getNazevKarty();
            rezerva1.Enabled = true;
        }

        private void zalozitDo2(Card karta_v_ruce)
        {
            karty_rezerva2.Insert(0, karta_v_ruce);
            karta_rezerva2 = karty_rezerva2.ElementAt(0);
            if (jeCervena(karta_rezerva2))
            {
                rezerva2.ForeColor = Color.Red;
            }
            else
            {
                rezerva2.ForeColor = Color.Black;
            }
            rezerva2.Text = karta_rezerva2.getNazevKarty();
            rezerva2.Enabled = true;
        }

        private void zalozitDo3(Card karta_v_ruce)
        {
            karty_rezerva3.Insert(0, karta_v_ruce);
            karta_rezerva3 = karty_rezerva3.ElementAt(0);
            if (jeCervena(karta_rezerva3))
            {
                rezerva3.ForeColor = Color.Red;
            }
            else
            {
                rezerva3.ForeColor = Color.Black;
            }
            rezerva3.Text = karta_rezerva3.getNazevKarty();
            rezerva3.Enabled = true;
        }

        private void zalozitDo4(Card karta_v_ruce)
        {
            karty_rezerva4.Insert(0, karta_v_ruce);
            karta_rezerva4 = karty_rezerva4.ElementAt(0);
            if (jeCervena(karta_rezerva4))
            {
                rezerva4.ForeColor = Color.Red;
            }
            else
            {
                rezerva4.ForeColor = Color.Black;
            }
            rezerva4.Text = karta_rezerva4.getNazevKarty();
            rezerva4.Enabled = true;
        }

        private void pridatKartuDoPaklu(Card karta_v_ruce)
        {
            karty_v_paklu = karty_v_paklu.Append(karta_v_ruce).ToList();
            labelPocetPaklu.Text = "Pocet:" + karty_v_paklu.Count;
            //herniHlasatel.Text = "Karta na posledni pozici je" + karty_v_paklu.ElementAt(karty_v_paklu.Count - 1).getNazevKarty();


        }


        private void Ruka1_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu) {
                if (radioButtonHerniPole.Checked)
                {
                    if (karta_v_ruce1.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_v_ruce1.getPopis() == "A" && herniPole1.Text == "--"))
                    {
                        odehrajNaSlot1(karta_v_ruce1);

                        Ruka1.Text = "--";
                        Ruka1.Enabled = false;

                    }
                    else if (karta_v_ruce1.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_v_ruce1.getPopis() == "A" && herniPole2.Text == "--"))
                    {
                        odehrajNaSlot2(karta_v_ruce1);

                        Ruka1.Text = "--";
                        Ruka1.Enabled = false;
                    }
                    else if (karta_v_ruce1.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_v_ruce1.getPopis() == "A" && herniPole3.Text == "--"))
                    {
                        odehrajNaSlot3(karta_v_ruce1);
                        Ruka1.Text = "--";
                        Ruka1.Enabled = false;

                    }
                    else if (karta_v_ruce1.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_v_ruce1.getPopis() == "A" && herniPole4.Text == "--"))
                    {
                        odehrajNaSlot4(karta_v_ruce1);
                        Ruka1.Text = "--";
                        Ruka1.Enabled = false;

                    }
                    else
                    {
                        herniHlasatel.Text = "Kartu nelze hrat";
                    }
                
            }


            else if (radioButtonZalozit.Checked)
            {
                if ((karta_rezerva1.getHodnota() == karta_v_ruce1.getHodnota() || rezerva1.Text == "--") && 
                    (karta_v_ruce1.getHodnota() != karta_rezerva2.getHodnota() && karta_v_ruce1.getHodnota() != karta_rezerva3.getHodnota()&&
                    karta_v_ruce1.getHodnota() != karta_rezerva4.getHodnota() ))
                {
                    zalozitDo1(karta_v_ruce1);
                    Ruka1.Text = "--";
                    Ruka1.Enabled = false;
                }
                else if ((karta_rezerva2.getHodnota() == karta_v_ruce1.getHodnota() || rezerva2.Text == "--") &&
                    (karta_v_ruce1.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce1.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce1.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo2(karta_v_ruce1);
                    Ruka1.Text = "--";
                    Ruka1.Enabled = false;
                }
                else if ((karta_rezerva3.getHodnota() == karta_v_ruce1.getHodnota() || rezerva3.Text == "--") &&
                    (karta_v_ruce1.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce1.getHodnota() != karta_rezerva2.getHodnota() &&
                    karta_v_ruce1.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo3(karta_v_ruce1);
                    Ruka1.Text = "--";
                    Ruka1.Enabled = false;
                }
                else if ((karta_rezerva4.getHodnota() == karta_v_ruce1.getHodnota() || rezerva4.Text == "--") &&
                    (karta_v_ruce1.getHodnota() != karta_rezerva2.getHodnota() && karta_v_ruce1.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce1.getHodnota() != karta_rezerva1.getHodnota()))
                {
                    zalozitDo4(karta_v_ruce1);
                    Ruka1.Text = "--";
                    Ruka1.Enabled = false;
                }
                else
                {
                    herniHlasatel.Text = "Můžeš zaloit pouze 4 stejné karty";
                }

            }
            else if (radioButtonDoPaklu.Checked)
            {
                pridatKartuDoPaklu(karta_v_ruce1);
                Ruka1.Text = "--";
                Ruka1.Enabled = false;
            }
            else
            {
                herniHlasatel.Text = "Není vybrán způsob odehrání karty";
            }
         }
        else
        {
            herniHlasatel.Text = "Nejsi na tahu";
        }
        }

        

        private void Ruka2_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu) { 
            if (radioButtonHerniPole.Checked)
            {
                if (karta_v_ruce2.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_v_ruce2.getPopis() == "A" && herniPole1.Text == "--"))
                {
                    odehrajNaSlot1(karta_v_ruce2);
                    Ruka2.Text = "--";
                    Ruka2.Enabled = false;

                }
                else if (karta_v_ruce2.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_v_ruce2.getPopis() == "A" && herniPole2.Text == "--"))
                {
                    odehrajNaSlot2(karta_v_ruce2);
                    Ruka2.Text = "--";
                    Ruka2.Enabled = false;
                }
                else if (karta_v_ruce2.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_v_ruce2.getPopis() == "A" && herniPole3.Text == "--"))
                {
                    odehrajNaSlot3(karta_v_ruce2);
                    Ruka2.Text = "--";
                    Ruka2.Enabled = false;
                }
                else if (karta_v_ruce2.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_v_ruce2.getPopis() == "A" && herniPole4.Text == "--"))
                {
                    odehrajNaSlot4(karta_v_ruce2);
                    Ruka2.Text = "--";
                    Ruka2.Enabled = false;
                }
                else
                {
                    herniHlasatel.Text = "Kartu nelze hrat";
                }
            }


            else if (radioButtonZalozit.Checked)
            {
                if ( (karta_rezerva1.getHodnota() == karta_v_ruce2.getHodnota() || rezerva1.Text == "--") &&
                    (karta_v_ruce2.getHodnota() != karta_rezerva2.getHodnota() && karta_v_ruce2.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce2.getHodnota() != karta_rezerva4.getHodnota()) )
                {
                    zalozitDo1(karta_v_ruce2);
                    Ruka2.Text = "--";
                    Ruka2.Enabled = false;
                }
                else if ((karta_rezerva2.getHodnota() == karta_v_ruce2.getHodnota() || rezerva2.Text == "--") &&
                    (karta_v_ruce2.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce2.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce2.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo2(karta_v_ruce2);
                    Ruka2.Text = "--";
                    Ruka2.Enabled = false;
                }
                else if ( (karta_rezerva3.getHodnota() == karta_v_ruce2.getHodnota() || rezerva3.Text == "--") &&
                    (karta_v_ruce2.getHodnota() != karta_rezerva2.getHodnota() && karta_v_ruce2.getHodnota() != karta_rezerva1.getHodnota() &&
                    karta_v_ruce2.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo3(karta_v_ruce2);
                    Ruka2.Text = "--";
                    Ruka2.Enabled = false;
                }
                else if ( (karta_rezerva4.getHodnota() == karta_v_ruce2.getHodnota() || rezerva4.Text == "--") &&
                    (karta_v_ruce2.getHodnota() != karta_rezerva2.getHodnota() && karta_v_ruce2.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce2.getHodnota() != karta_rezerva1.getHodnota()))
                {
                    zalozitDo4(karta_v_ruce2);
                    Ruka2.Text = "--";
                    Ruka2.Enabled = false;
                }
                else
                {
                    herniHlasatel.Text = "Můžeš zaloit pouze 4 stejné karty";
                }

            }
            else if (radioButtonDoPaklu.Checked)
            {
                pridatKartuDoPaklu(karta_v_ruce2);
                Ruka2.Text = "--";
                Ruka2.Enabled = false;
            }
            else
            {
                herniHlasatel.Text = "Není vybrán způsob odehrání karty";
            }

        }
        else
        {
            herniHlasatel.Text = "Nejsi na tahu";
        }
        }

        private void Ruka3_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu) {
            if (radioButtonHerniPole.Checked)
            {
                if (karta_v_ruce3.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_v_ruce3.getPopis() == "A" && herniPole1.Text == "--"))
                {
                    odehrajNaSlot1(karta_v_ruce3);

                    Ruka3.Text = "--";
                    Ruka3.Enabled = false;
                }
                else if (karta_v_ruce3.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_v_ruce3.getPopis() == "A" && herniPole2.Text == "--"))
                {
                    odehrajNaSlot2(karta_v_ruce3);
                    Ruka3.Text = "--";
                    Ruka3.Enabled = false;
                }
                else if (karta_v_ruce3.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_v_ruce3.getPopis() == "A" && herniPole3.Text == "--"))
                {
                    odehrajNaSlot3(karta_v_ruce3);
                    Ruka3.Text = "--";
                    Ruka3.Enabled = false;
                }
                else if (karta_v_ruce3.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_v_ruce3.getPopis() == "A" && herniPole4.Text == "--"))
                {
                    odehrajNaSlot4(karta_v_ruce3);
                    Ruka3.Text = "--";
                    Ruka3.Enabled = false;
                }
                else
                {
                    herniHlasatel.Text = "Kartu nelze hrat";
                }
            }


            else if (radioButtonZalozit.Checked)
            {
                if ( (karta_rezerva1.getHodnota() == karta_v_ruce3.getHodnota() || rezerva1.Text == "--") &&
                    (karta_v_ruce3.getHodnota() != karta_rezerva2.getHodnota() && karta_v_ruce3.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce3.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo1(karta_v_ruce3);
                    Ruka3.Text = "--";
                    Ruka3.Enabled = false;
                }
                else if ( (karta_rezerva2.getHodnota() == karta_v_ruce3.getHodnota() || rezerva2.Text == "--") &&
                    (karta_v_ruce3.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce3.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce3.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo2(karta_v_ruce3);
                    Ruka3.Text = "--";
                    Ruka3.Enabled = false;
                }
                else if ( (karta_rezerva3.getHodnota() == karta_v_ruce3.getHodnota() || rezerva3.Text == "--") &&
                    (karta_v_ruce3.getHodnota() != karta_rezerva2.getHodnota() && karta_v_ruce3.getHodnota() != karta_rezerva1.getHodnota() &&
                    karta_v_ruce3.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo3(karta_v_ruce3);
                    Ruka3.Text = "--";
                    Ruka3.Enabled = false;
                }
                else if ( (karta_rezerva4.getHodnota() == karta_v_ruce3.getHodnota() || rezerva4.Text == "--") &&
                    (karta_v_ruce3.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce3.getHodnota() != karta_rezerva2.getHodnota() &&
                    karta_v_ruce3.getHodnota() != karta_rezerva3.getHodnota()))
                {
                    zalozitDo4(karta_v_ruce3);
                    Ruka3.Text = "--";
                    Ruka3.Enabled = false;
                }

                else
                {
                    herniHlasatel.Text = "Můžeš zaloit pouze 4 stejné karty";
                }

            }
            else if (radioButtonDoPaklu.Checked)
            {
                pridatKartuDoPaklu(karta_v_ruce3);
                Ruka3.Text = "--";
                Ruka3.Enabled = false;
            }
            else
            {
                herniHlasatel.Text = "Není vybrán způsob odehrání karty";
            }
        }
        else
        {
            herniHlasatel.Text = "Nejsi na tahu";
        }
        }

        private void Ruka4_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu) {
            if (radioButtonHerniPole.Checked)
            {
                if (karta_v_ruce4.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_v_ruce4.getPopis() == "A" && herniPole1.Text == "--"))
                {
                    odehrajNaSlot1(karta_v_ruce4);
                    Ruka4.Text = "--";
                    Ruka4.Enabled = false;
                }
                else if (karta_v_ruce4.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_v_ruce4.getPopis() == "A" && herniPole2.Text == "--"))
                {
                    odehrajNaSlot2(karta_v_ruce4);
                    Ruka4.Text = "--";
                    Ruka4.Enabled = false;
                }
                else if (karta_v_ruce4.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_v_ruce4.getPopis() == "A" && herniPole3.Text == "--"))
                {
                    odehrajNaSlot3(karta_v_ruce4);
                    Ruka4.Text = "--";
                    Ruka4.Enabled = false;
                }
                else if (karta_v_ruce4.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_v_ruce4.getPopis() == "A" && herniPole4.Text == "--"))
                {
                    odehrajNaSlot4(karta_v_ruce4);
                    Ruka4.Text = "--";
                    Ruka4.Enabled = false;
                }
                else
                {
                    herniHlasatel.Text = "Kartu nelze hrat";
                }
            }


            else if (radioButtonZalozit.Checked)
            {
                if ( (karta_rezerva1.getHodnota() == karta_v_ruce4.getHodnota() || rezerva1.Text == "--") &&
                    (karta_v_ruce4.getHodnota() != karta_rezerva2.getHodnota() && karta_v_ruce4.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce4.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo1(karta_v_ruce4);
                    Ruka4.Text = "--";
                    Ruka4.Enabled = false;
                }
                else if ( (karta_rezerva2.getHodnota() == karta_v_ruce4.getHodnota() || rezerva2.Text == "--")  &&
                    (karta_v_ruce4.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce4.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce4.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo2(karta_v_ruce4);
                    Ruka4.Text = "--";
                    Ruka4.Enabled = false;
                }
                else if ( (karta_rezerva3.getHodnota() == karta_v_ruce4.getHodnota() || rezerva3.Text == "--")  &&
                    (karta_v_ruce4.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce4.getHodnota() != karta_rezerva2.getHodnota() &&
                    karta_v_ruce4.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo3(karta_v_ruce4);
                    Ruka4.Text = "--";
                    Ruka4.Enabled = false;
                }
                else if ( (karta_rezerva4.getHodnota() == karta_v_ruce4.getHodnota() || rezerva4.Text == "--")  &&
                    (karta_v_ruce4.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce4.getHodnota() != karta_rezerva2.getHodnota() &&
                    karta_v_ruce4.getHodnota() != karta_rezerva3.getHodnota()))
                {
                    zalozitDo4(karta_v_ruce4);
                    Ruka4.Text = "--";
                    Ruka4.Enabled = false;
                }

                else
                {
                    herniHlasatel.Text = "Můžeš zaloit pouze 4 stejné karty";
                }

            }
            else if (radioButtonDoPaklu.Checked)
            {
                pridatKartuDoPaklu(karta_v_ruce4);
                Ruka4.Text = "--";
                Ruka4.Enabled = false;
            }
            else
            {
                herniHlasatel.Text = "Není vybrán způsob odehrání karty";
            }
        }
        else
        {
            herniHlasatel.Text = "Nejsi na tahu";
        }
        }

        private void Ruka5_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu) {
            if (radioButtonHerniPole.Checked)
            {
                if (karta_v_ruce5.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_v_ruce5.getPopis() == "A" && herniPole1.Text == "--"))
                {
                    odehrajNaSlot1(karta_v_ruce5);
                    Ruka5.Text = "--";
                    Ruka5.Enabled = false;
                }
                else if (karta_v_ruce5.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_v_ruce5.getPopis() == "A" && herniPole2.Text == "--"))
                {
                    odehrajNaSlot2(karta_v_ruce5);
                    Ruka5.Text = "--";
                    Ruka5.Enabled = false;
                }
                else if (karta_v_ruce5.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_v_ruce5.getPopis() == "A" && herniPole3.Text == "--"))
                {
                    odehrajNaSlot3(karta_v_ruce5);
                    Ruka5.Text = "--";
                    Ruka5.Enabled = false;
                }
                else if (karta_v_ruce5.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_v_ruce5.getPopis() == "A" && herniPole4.Text == "--"))
                {
                    odehrajNaSlot4(karta_v_ruce5);
                    Ruka5.Text = "--";
                    Ruka5.Enabled = false;
                }
                else
                {
                    herniHlasatel.Text = "Kartu nelze hrat";
                }
            }


            else if (radioButtonZalozit.Checked)
            {
                if ( (karta_rezerva1.getHodnota() == karta_v_ruce5.getHodnota() || rezerva1.Text == "--")  &&
                    (karta_v_ruce5.getHodnota() != karta_rezerva2.getHodnota() && karta_v_ruce5.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce5.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo1(karta_v_ruce5);
                    Ruka5.Text = "--";
                    Ruka5.Enabled = false;
                }
                else if( (karta_rezerva2.getHodnota() == karta_v_ruce5.getHodnota() || rezerva2.Text == "--")  &&
                    (karta_v_ruce5.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce5.getHodnota() != karta_rezerva3.getHodnota() &&
                    karta_v_ruce5.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo2(karta_v_ruce5);
                    Ruka5.Text = "--";
                    Ruka5.Enabled = false;
                }
                else if ( (karta_rezerva3.getHodnota() == karta_v_ruce5.getHodnota() || rezerva3.Text == "--") &&
                    (karta_v_ruce5.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce5.getHodnota() != karta_rezerva2.getHodnota() &&
                    karta_v_ruce5.getHodnota() != karta_rezerva4.getHodnota()))
                {
                    zalozitDo3(karta_v_ruce5);
                    Ruka5.Text = "--";
                    Ruka5.Enabled = false;
                }
                else if ((karta_rezerva4.getHodnota() == karta_v_ruce5.getHodnota() || rezerva4.Text == "--") &&
                    (karta_v_ruce5.getHodnota() != karta_rezerva1.getHodnota() && karta_v_ruce5.getHodnota() != karta_rezerva2.getHodnota() &&
                    karta_v_ruce5.getHodnota() != karta_rezerva3.getHodnota()))
                {
                    zalozitDo4(karta_v_ruce5);
                    Ruka5.Text = "--";
                    Ruka5.Enabled = false;
                }

                else
                {
                    herniHlasatel.Text = "Můžeš zaloit pouze 4 stejné karty";
                }

            }
            else if (radioButtonDoPaklu.Checked)
            {
                pridatKartuDoPaklu(karta_v_ruce5);
                Ruka5.Text = "--";
                Ruka5.Enabled = false;
            }
            else
            {
                herniHlasatel.Text = "Není vybrán způsob odehrání karty";
            }
        }
        else
        {
                herniHlasatel.Text = "Nejsi na tahu";
        }
        }

        private void setRezerva1()
        {
            if (karty_rezerva1.Count -1 > 0)
            {
                karty_rezerva1.Remove(karta_rezerva1);
                karta_rezerva1 = karty_rezerva1.ElementAt(0);
                if (jeCervena(karta_rezerva1))
                {
                    rezerva1.ForeColor = Color.Red;
                }
                else
                {
                    rezerva1.ForeColor = Color.Black;
                }
                rezerva1.Text = karta_rezerva1.getNazevKarty();
            }
            else
            {
                karty_rezerva1.Clear();
                karta_rezerva1 = new Card("default", "default", 50, '♦');

                rezerva1.Text = "--";
                rezerva1.Enabled = false;

            }
        }

        private void setRezerva2()
        {
            if (karty_rezerva2.Count-1 > 0)
            {
                karty_rezerva2.Remove(karta_rezerva2);
                karta_rezerva2 = karty_rezerva2.ElementAt(0);
                if (jeCervena(karta_rezerva2))
                {
                    rezerva2.ForeColor = Color.Red;
                }
                else
                {
                    rezerva2.ForeColor = Color.Black;
                }
                rezerva2.Text = karta_rezerva2.getNazevKarty();
            }
            else
            {
                karty_rezerva2.Clear();
                karta_rezerva2 = new Card("default", "default", 50, '♦');
                rezerva2.Text = "--";
                rezerva2.Enabled = false;

            }
        }

        private void setRezerva3()
        {
            if (karty_rezerva3.Count - 1 > 0)
            {
                karty_rezerva3.Remove(karta_rezerva3);
                karta_rezerva3 = karty_rezerva3.ElementAt(0);
                if (jeCervena(karta_rezerva3))
                {
                    rezerva3.ForeColor = Color.Red;
                }
                else
                {
                    rezerva3.ForeColor = Color.Black;
                }
                rezerva3.Text = karta_rezerva3.getNazevKarty();
            }
            else
            {
                karty_rezerva3.Clear();
                karta_rezerva3 = new Card("default", "default", 50, '♦');
                rezerva3.Text = "--";
                rezerva3.Enabled = false;

            }
        }

        private void setRezerva4()
        {
            if (karty_rezerva4.Count - 1 > 0)
            {
                karty_rezerva4.Remove(karta_rezerva4);
                karta_rezerva4 = karty_rezerva4.ElementAt(0);
                if (jeCervena(karta_rezerva4))
                {
                    rezerva4.ForeColor = Color.Red;
                }
                else
                {
                    rezerva4.ForeColor = Color.Black;
                }
                rezerva4.Text = karta_rezerva4.getNazevKarty();
            }
            else
            {
                karty_rezerva4.Clear();
                karta_rezerva4 = new Card("default", "default", 50, '♦');
                rezerva4.Text = "--";
                rezerva4.Enabled = false;

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            

        }

        private void rezerva1_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu) {
            if (karta_rezerva1.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_rezerva1.getPopis() == "A" && herniPole1.Text == "--"))
            {
                odehrajNaSlot1(karta_rezerva1);
                setRezerva1();


            }
            else if (karta_rezerva1.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_rezerva1.getPopis() == "A" && herniPole2.Text == "--"))
            {
                odehrajNaSlot2(karta_rezerva1);
                setRezerva1();
            }
            else if (karta_rezerva1.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_rezerva1.getPopis() == "A" && herniPole3.Text == "--"))
            {
                odehrajNaSlot3(karta_rezerva1);
                setRezerva1();
            }
            else if (karta_rezerva1.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_rezerva1.getPopis() == "A" && herniPole4.Text == "--"))
            {
                odehrajNaSlot4(karta_rezerva1);
                setRezerva1();
            }
            else
            {
                herniHlasatel.Text = "Kartu nelze hrat";
            }
            }
        else
        {
            herniHlasatel.Text = "Nejsi na tahu";
        }
        }

        private void rezerva2_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu) { 
            if (karta_rezerva2.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_rezerva2.getPopis() == "A" && herniPole1.Text == "--"))
            {
                odehrajNaSlot1(karta_rezerva2);
                setRezerva2();

            }
            else if (karta_rezerva2.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_rezerva2.getPopis() == "A" && herniPole2.Text == "--"))
            {
                odehrajNaSlot2(karta_rezerva2);
                setRezerva2();
            }
            else if (karta_rezerva2.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_rezerva2.getPopis() == "A" && herniPole3.Text == "--"))
            {
                odehrajNaSlot3(karta_rezerva2);
                setRezerva2();
            }
            else if (karta_rezerva2.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_rezerva2.getPopis() == "A" && herniPole4.Text == "--"))
            {
                odehrajNaSlot4(karta_rezerva2);
                setRezerva2();
            }
            else
            {
                herniHlasatel.Text = "Kartu nelze hrat";
            }

         }
        else
        {
            herniHlasatel.Text = "Nejsi na tahu";
        }
        }

        private void rezerva3_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu) { 
            if (karta_rezerva3.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_rezerva3.getPopis() == "A" && herniPole1.Text == "--"))
            {
                odehrajNaSlot1(karta_rezerva3);
                setRezerva3();

            }
            else if (karta_rezerva3.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_rezerva3.getPopis() == "A" && herniPole2.Text == "--"))
            {
                odehrajNaSlot2(karta_rezerva3);
                setRezerva3();
            }
            else if (karta_rezerva3.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_rezerva3.getPopis() == "A" && herniPole3.Text == "--"))
            {
                odehrajNaSlot3(karta_rezerva3);
                setRezerva3();
            }
            else if (karta_rezerva3.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_rezerva3.getPopis() == "A" && herniPole4.Text == "--"))
            {
                odehrajNaSlot4(karta_rezerva3);
                setRezerva3();
            }
            else
            {
                herniHlasatel.Text = "Kartu nelze hrat";
            }
        }
        else
        {
            herniHlasatel.Text = "Nejsi na tahu";
        }
        }

        private void rezerva4_Click(object sender, EventArgs e)
        {
            if (hrac_je_na_tahu) {
            if (karta_rezerva4.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_rezerva4.getPopis() == "A" && herniPole1.Text == "--"))
            {
                odehrajNaSlot1(karta_rezerva4);
                setRezerva4();

            }
            else if (karta_rezerva4.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_rezerva4.getPopis() == "A" && herniPole2.Text == "--"))
            {
                odehrajNaSlot2(karta_rezerva4);
                setRezerva4();
            }
            else if (karta_rezerva4.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_rezerva4.getPopis() == "A" && herniPole3.Text == "--"))
            {
                odehrajNaSlot3(karta_rezerva4);
                setRezerva4();
            }
            else if (karta_rezerva4.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_rezerva4.getPopis() == "A" && herniPole4.Text == "--"))
            {
                odehrajNaSlot4(karta_rezerva4);
                setRezerva4();
            }
            else
            {
                herniHlasatel.Text = "Kartu nelze hrat";
            }
        }
        else
        {
            herniHlasatel.Text = "Nejsi na tahu";
        }
        }

        

        private void herniPole1_Click(object sender, EventArgs e)
        {
            if(herni_slot1.Last().getHodnota() >= 13)
            {
                for(int i=0; i<herni_slot1.Count; i++)
                {
                    balicek_karet.Add(herni_slot1.ElementAt(i));
                }
                balicek_karet = zamichat(balicek_karet);
                herni_slot1.Clear();
                karta_na_stole1 = new Card("default", "default", 50, '♦'); ;
                herniPole1.Text = "--";
                herniHlasatel.Text = "Velikost balicku je: " + balicek_karet.Count;
            }
            else
            {
                herniHlasatel.Text = "Herní slot ještě není plný";
            }

                
        }

        private void herniPole2_Click(object sender, EventArgs e)
        {
            if (herni_slot2.Last().getHodnota() >= 13)
            {
                for (int i = 0; i < herni_slot2.Count; i++)
                {
                    balicek_karet.Add(herni_slot2.ElementAt(i));
                }
                balicek_karet = zamichat(balicek_karet);
                herni_slot2.Clear();
                karta_na_stole2 = new Card("default", "default", 50, '♦'); ;
                herniPole2.Text = "--";
                herniHlasatel.Text = "Velikost balicku je: " + balicek_karet.Count;
            }
            else
            {
                herniHlasatel.Text = "Herní slot ještě není plný";
            }
        }

        private void herniPole3_Click(object sender, EventArgs e)
        {
            if (herni_slot3.Last().getHodnota() >= 13)
            {
                for (int i = 0; i < herni_slot3.Count; i++)
                {
                    balicek_karet.Add(herni_slot3.ElementAt(i));
                }
                balicek_karet = zamichat(balicek_karet);
                herni_slot3.Clear();
                karta_na_stole3 = new Card("default", "default", 50, '♦'); ;
                herniPole3.Text = "--";
                herniHlasatel.Text = "Velikost balicku je: " + balicek_karet.Count;
            }
            else
            {
                herniHlasatel.Text = "Herní slot ještě není plný";
            }
        }

        private void herniPole4_Click(object sender, EventArgs e)
        {
            if (herni_slot4.Last().getHodnota() >= 13)
            {
                for (int i = 0; i < herni_slot4.Count; i++)
                {
                    balicek_karet.Add(herni_slot4.ElementAt(i));
                }
                balicek_karet = zamichat(balicek_karet);
                herni_slot4.Clear();
                karta_na_stole4 = new Card("default", "default", 50, '♦'); ;
                herniPole4.Text = "--";
                herniHlasatel.Text = "Velikost balicku je: " + balicek_karet.Count;
            }
            else
            {
                herniHlasatel.Text = "Herní slot ještě není plný";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonKonecTahu_Click(object sender, EventArgs e)
        {
            if ((Ruka1.Text == "--" || Ruka2.Text == "--" || Ruka3.Text == "--" || Ruka4.Text == "--" || Ruka5.Text == "--" ) && odlizano)
            {
                //nastaveni vsech tlacitek na false
                hrac_je_na_tahu = false;
                
                if (!hrac_je_na_tahu)
                {
                    tahPocitaceAsync();
                }
            }
            else
            {
                herniHlasatel.Text = "Na konci tahu nesmíš mít v ruce 5 karet";
            }
        }

        /**
         * CELÁ LOGIKA POČÍTAČE
         * 
         */

        public async Task tahPocitaceAsync()
        {
            int faze_hry = 0;
            //hrana_karta_z_ruky = null;
                while (!hrac_je_na_tahu) {
                    //liznoutKarty();
                    switch (faze_hry)
                    {
                        case 0:
                           
                                liznoutKarty();
                                Debug.WriteLine("PC je ve fazi 0");
                                textBoxPocitace.Text = "Počítač líže karty";
                                await Task.Delay(1500);
                                faze_hry = 1;

                                break;
                           
                        case 1:
                           
                                Debug.WriteLine("PC je ve fazi 1");
                                Debug.WriteLine("Karta v paklu pc je: ");
                                if (lze_hrat_z_paklu()) //vraci cislo podle herniho slotu
                                {       
                                    
                                    //hrat_z_paklu(lze_hrat_z_paklu());
                                    
                                    await Task.Delay(1500);
                                    faze_hry = 1;
                                }
                                else { faze_hry = 2; }
                                break;
                            
                        case 2:
                            
                                Debug.WriteLine("PC je ve fazi 2");
                                if (lze_hrat_z_ruky())
                                {

                                    for (int i = 0; i < karty_v_rucePC.Count; i++)
                                    {
                                        Debug.WriteLine( (i+1)+". karta: "+karty_v_rucePC.ElementAt(i).getNazevKarty());
                                    }
                                    //hrat_z_ruky(lze_hrat_z_ruky());
                                    
                                    await Task.Delay(1500);
                                    faze_hry = 1;
                                }
                                else
                                {
                                    faze_hry = 3;
                                }
                                break;
                            
                        case 3:
                            
                                Debug.WriteLine("PC je ve fazi 3");
                                if (lze_hrat_z_rezervy())
                                {
                                    textBoxPocitace.Text = "Počítač hraje kartu z rezervy ";
                                    await Task.Delay(1500);
                                    
                                    faze_hry = 1;
                                }
                                else if (!je_plna_ruka())
                                {

                                    faze_hry = 6;
                                }
                                else
                                {
                                    faze_hry = 4;
                                }
                                break;
                            
                        case 4:
                            
                                Debug.WriteLine("PC je ve fazi 4");
                                if (lze_zalozit())
                                {
                                    Debug.WriteLine("PC zaklada kartu");
                                    textBoxPocitace.Text = "Počítač zakládá kartu ";
                                    await Task.Delay(1500);
                                    faze_hry = 6; //kontrola
                                }
                                else
                                {
                                    faze_hry = 5;
                                }

                                break;
                            

                        case 5:
                            
                                Debug.WriteLine("PC je ve fazi 5");
                                pridat_kartu_do_pakluPC(karty_v_rucePC.Last());
                                Debug.WriteLine("PC dava do paklu kartu ");
                                textBoxPocitace.Text = "Počítač přidává do paklu ";
                                await Task.Delay(1500);
                                faze_hry = 6;
                                break;
                            

                        case 6:
                            
                                Debug.WriteLine("PC je ve fazi 6");
                                Debug.WriteLine("Pocet karet v ruce pc je: "+karty_v_rucePC.Count);
                                konec_tahuPC();
                                Debug.WriteLine("Počítač ukončuje svůj tah");
                                //hrac_je_na_tahu = true;
                                textBoxPocitace.Text = "Počítač ukončuje svůj tah";
                                await Task.Delay(1500);
                                break;
                            
                    }
                } 
        }

        private bool je_plna_ruka()
        {
            if (karty_v_rucePC.Count >= 5)
            {
                return true;
            }
            else { return false; }
        }

        private void liznoutKarty()
        {
            int i = 0;
            Card vrchni_karta;
            while (karty_v_rucePC.Count < 5)
            {
                vrchni_karta = balicek_karet.ElementAt(i);
                karty_v_rucePC.Add(vrchni_karta);
                balicek_karet.Remove(vrchni_karta);
                i++;
            }
            Ruka1PC.Text = "?";
            Ruka2PC.Text = "?";
            Ruka3PC.Text = "?";
            Ruka4PC.Text = "?";
            Ruka5PC.Text = "?";
        }


    private Boolean lze_hrat_z_paklu()
    {
            Boolean end = false;
            if (!konec)
            {
                if (karta_v_pakluPC.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_v_pakluPC.getPopis() == "A" && herniPole1.Text == "--"))
                {
                    Debug.WriteLine("PC hraje z paklu kartu " + karta_v_pakluPC.getNazevKarty());
                    textBoxPocitace.Text = "Počítač hraje z paklu kartu " + karta_v_pakluPC.getNazevKarty();

                    odehrajNaSlot1(karta_v_pakluPC);
                    karty_v_pakluPC.Remove(karta_v_pakluPC);
                    karta_v_pakluPC = null;
                    novaKartaPaklPC();
                    end = true;
                }
                else if (karta_v_pakluPC.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_v_pakluPC.getPopis() == "A" && herniPole2.Text == "--"))
                {
                    Debug.WriteLine("PC hraje z paklu kartu " + karta_v_pakluPC.getNazevKarty());
                    textBoxPocitace.Text = "Počítač hraje z paklu kartu " + karta_v_pakluPC.getNazevKarty();

                    odehrajNaSlot2(karta_v_pakluPC);
                    karty_v_pakluPC.Remove(karta_v_pakluPC);
                    karta_v_pakluPC = null;
                    novaKartaPaklPC();
                    end = true;
                }

                else if (karta_v_pakluPC.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_v_pakluPC.getPopis() == "A" && herniPole3.Text == "--"))
                {
                    Debug.WriteLine("PC hraje z paklu kartu " + karta_v_pakluPC.getNazevKarty());
                    textBoxPocitace.Text = "Počítač hraje z paklu kartu " + karta_v_pakluPC.getNazevKarty();

                    odehrajNaSlot3(karta_v_pakluPC);
                    karty_v_pakluPC.Remove(karta_v_pakluPC);
                    karta_v_pakluPC = null;
                    novaKartaPaklPC();
                    end = true;
                }
                else if (karta_v_pakluPC.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_v_pakluPC.getPopis() == "A" && herniPole4.Text == "--"))
                {
                    Debug.WriteLine("PC hraje z paklu kartu " + karta_v_pakluPC.getNazevKarty());
                    textBoxPocitace.Text = "Počítač hraje z paklu kartu " + karta_v_pakluPC.getNazevKarty();

                    odehrajNaSlot4(karta_v_pakluPC);
                    karty_v_pakluPC.Remove(karta_v_pakluPC);
                    karta_v_pakluPC = null;
                    novaKartaPaklPC();
                    end = true;
                }
            }
            return end;
    }
    /*
    public void hrat_z_paklu(int kam)
    {
        if(kam == 1)
            {
            odehrajNaSlot1(karta_v_pakluPC);
            karty_v_pakluPC.Remove(karty_v_pakluPC.ElementAt(0));
            novaKartaPaklPC();
            }
        else if (kam == 2)
            {
                odehrajNaSlot2(karta_v_pakluPC);
                karty_v_pakluPC.Remove(karty_v_pakluPC.ElementAt(0));
                novaKartaPaklPC();
            }
         else if (kam == 3)
            {
                odehrajNaSlot3(karta_v_pakluPC);
                karty_v_pakluPC.Remove(karty_v_pakluPC.ElementAt(0));
                novaKartaPaklPC();
            }
         else if (kam == 4)
            {
                odehrajNaSlot4(karta_v_pakluPC);
                karty_v_pakluPC.Remove(karty_v_pakluPC.ElementAt(0));
                novaKartaPaklPC();
            }
        }
        */

    private Boolean lze_hrat_z_ruky()
    {
        Boolean end = false;
        
            

        for(int i=0; i<karty_v_rucePC.Count && !end; i++)
            {
                
                if( (karty_v_rucePC.ElementAt(i).getHodnota() == karta_na_stole1.getHodnota()+1) ||
                    (karty_v_rucePC.ElementAt(i).getPopis() == "A" && herniPole1.Text == "--"))
                {

                    hrana_karta_z_ruky = karty_v_rucePC.ElementAt(i);
                    Debug.WriteLine("PC hraje z ruky kartu " + hrana_karta_z_ruky.getNazevKarty());
                    textBoxPocitace.Text = "Počítač hraje z ruky kartu " + hrana_karta_z_ruky.getNazevKarty();

                    odehrajNaSlot1(hrana_karta_z_ruky);
                    karty_v_rucePC.Remove(hrana_karta_z_ruky);
                    //hodnota = 1;
                    end = true;
                    
                }
                else if ((karty_v_rucePC.ElementAt(i).getHodnota() == karta_na_stole2.getHodnota() + 1) ||
                    (karty_v_rucePC.ElementAt(i).getPopis() == "A" && herniPole2.Text == "--"))
                {
                    hrana_karta_z_ruky = karty_v_rucePC.ElementAt(i);
                    Debug.WriteLine("PC hraje z ruky kartu " + hrana_karta_z_ruky.getNazevKarty());
                    textBoxPocitace.Text = "Počítač hraje z ruky kartu " + hrana_karta_z_ruky.getNazevKarty();

                    odehrajNaSlot2(hrana_karta_z_ruky);
                    karty_v_rucePC.Remove(hrana_karta_z_ruky);
                    //hodnota = 2;
                    end = true;

                }
                else if ((karty_v_rucePC.ElementAt(i).getHodnota() == karta_na_stole3.getHodnota() + 1) ||
                    (karty_v_rucePC.ElementAt(i).getPopis() == "A" && herniPole3.Text == "--"))
                {
                    

                    hrana_karta_z_ruky = karty_v_rucePC.ElementAt(i);
                    Debug.WriteLine("PC hraje z ruky kartu " + hrana_karta_z_ruky.getNazevKarty());
                    textBoxPocitace.Text = "Počítač hraje z ruky kartu " + hrana_karta_z_ruky.getNazevKarty();

                    odehrajNaSlot3(hrana_karta_z_ruky);
                    karty_v_rucePC.Remove(hrana_karta_z_ruky);
                    //hodnota = 3;
                    end = true;

                }
                else if ((karty_v_rucePC.ElementAt(i).getHodnota() == karta_na_stole4.getHodnota() + 1) ||
                    (karty_v_rucePC.ElementAt(i).getPopis() == "A" && herniPole4.Text == "--"))
                {

                    hrana_karta_z_ruky = karty_v_rucePC.ElementAt(i);
                    Debug.WriteLine("PC hraje z ruky kartu " + hrana_karta_z_ruky.getNazevKarty());
                    textBoxPocitace.Text = "Počítač hraje z ruky kartu " + hrana_karta_z_ruky.getNazevKarty();

                    odehrajNaSlot4(hrana_karta_z_ruky);
                    karty_v_rucePC.Remove(hrana_karta_z_ruky);
                    //hodnota = 4;
                    end = true;

                }

            }
            return end;
    }


        public Boolean lze_hrat_z_rezervy() {

            Boolean hodnota =false;
            //karta z rezervy 1 
            if (karta_rezerva1PC.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_rezerva1PC.getPopis() == "A" && herniPole1.Text == "--"))
            {
                odehrajNaSlot1(karta_rezerva1PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva1PC.getNazevKarty());
                setRezerva1PC();
                hodnota = true;

            }
            else if (karta_rezerva1PC.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_rezerva1PC.getPopis() == "A" && herniPole2.Text == "--"))
            {
                odehrajNaSlot2(karta_rezerva1PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva1PC.getNazevKarty());
                setRezerva1PC();
                hodnota = true;
            }
            else if (karta_rezerva1PC.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_rezerva1PC.getPopis() == "A" && herniPole3.Text == "--"))
            {
                odehrajNaSlot3(karta_rezerva1PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva1PC.getNazevKarty());
                setRezerva1PC();
                hodnota = true;
            }
            else if (karta_rezerva1PC.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_rezerva1PC.getPopis() == "A" && herniPole4.Text == "--"))
            {
                odehrajNaSlot4(karta_rezerva1PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva1PC.getNazevKarty());
                setRezerva1PC();
                hodnota = true;
            }

            //karta z rezervy 2
            else if (karta_rezerva2PC.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_rezerva2PC.getPopis() == "A" && herniPole1.Text == "--"))
            {
                odehrajNaSlot1(karta_rezerva2PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva2PC.getNazevKarty());
                setRezerva2PC();
                hodnota = true;

            }
            else if (karta_rezerva2PC.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_rezerva2PC.getPopis() == "A" && herniPole2.Text == "--"))
            {
                odehrajNaSlot2(karta_rezerva2PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva2PC.getNazevKarty());
                setRezerva2PC();
                hodnota = true;
            }
            else if (karta_rezerva2PC.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_rezerva2PC.getPopis() == "A" && herniPole3.Text == "--"))
            {
                odehrajNaSlot3(karta_rezerva2PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva2PC.getNazevKarty());
                setRezerva2PC();
                hodnota = true;
            }
            else if (karta_rezerva2PC.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_rezerva2PC.getPopis() == "A" && herniPole4.Text == "--"))
            {
                odehrajNaSlot4(karta_rezerva2PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva2PC.getNazevKarty());
                setRezerva2PC();
                hodnota = true;
            }

            //karta z rezervy 3
            else if (karta_rezerva3PC.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_rezerva3PC.getPopis() == "A" && herniPole1.Text == "--"))
            {
                odehrajNaSlot1(karta_rezerva3PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva3PC.getNazevKarty());
                setRezerva3PC();
                hodnota = true;
            }
            else if (karta_rezerva3PC.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_rezerva3PC.getPopis() == "A" && herniPole2.Text == "--"))
            {
                odehrajNaSlot2(karta_rezerva3PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva3PC.getNazevKarty());
                setRezerva3PC();
                hodnota = true;
            }
            else if (karta_rezerva3PC.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_rezerva3PC.getPopis() == "A" && herniPole3.Text == "--"))
            {
                odehrajNaSlot3(karta_rezerva3PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva3PC.getNazevKarty());
                setRezerva3PC();
                hodnota = true;
            }
            else if (karta_rezerva3PC.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_rezerva3PC.getPopis() == "A" && herniPole4.Text == "--"))
            {
                odehrajNaSlot4(karta_rezerva3PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva3PC.getNazevKarty());
                setRezerva3PC();
                hodnota = true;

            }
            //karta z rezervy 4 
            else if (karta_rezerva4PC.getHodnota() == karta_na_stole1.getHodnota() + 1 || (karta_rezerva4PC.getPopis() == "A" && herniPole1.Text == "--"))
            {
                odehrajNaSlot1(karta_rezerva4PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva4PC.getNazevKarty());
                setRezerva4PC();
                hodnota = true;

            }
            else if (karta_rezerva4PC.getHodnota() == karta_na_stole2.getHodnota() + 1 || (karta_rezerva4PC.getPopis() == "A" && herniPole2.Text == "--"))
            {
                odehrajNaSlot2(karta_rezerva4PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva4PC.getNazevKarty());
                setRezerva4PC();
                hodnota = true;
            }
            else if (karta_rezerva4PC.getHodnota() == karta_na_stole3.getHodnota() + 1 || (karta_rezerva4PC.getPopis() == "A" && herniPole3.Text == "--"))
            {
                odehrajNaSlot3(karta_rezerva4PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva4PC.getNazevKarty());
                setRezerva4PC();
                hodnota = true;
            }
            else if (karta_rezerva4PC.getHodnota() == karta_na_stole4.getHodnota() + 1 || (karta_rezerva4PC.getPopis() == "A" && herniPole4.Text == "--"))
            {
                odehrajNaSlot4(karta_rezerva4PC);
                Debug.WriteLine("PC hraje z rezervy kartu " + karta_rezerva4PC.getNazevKarty());
                setRezerva4PC();
                hodnota = true;
            }


            return hodnota;
        }

        

        private void setRezerva1PC()
        {
            if (karty_rezerva1PC.Count - 1 > 0)
            {
                karty_rezerva1PC.Remove(karta_rezerva1PC);
                karta_rezerva1PC = karty_rezerva1PC.Last();
                if (jeCervena(karta_rezerva1PC))
                {
                    rezerva1PC.ForeColor = Color.Red;
                }
                else
                {
                    rezerva1PC.ForeColor = Color.Black;
                }
                rezerva1PC.Text = karta_rezerva1PC.getNazevKarty();
            }
            else
            {
                karty_rezerva1PC.Clear();
                karta_rezerva1PC = new Card("default", "default", 50, '♦');

                rezerva1PC.Text = "--";

            }
        }

        private void setRezerva2PC()
        {
            if (karty_rezerva2PC.Count - 1 > 0)
            {
                karty_rezerva2PC.Remove(karta_rezerva2PC);
                karta_rezerva2PC = karty_rezerva2PC.Last();
                if (jeCervena(karta_rezerva2PC))
                {
                    rezerva2PC.ForeColor = Color.Red;
                }
                else
                {
                    rezerva2PC.ForeColor = Color.Black;
                }
                rezerva2PC.Text = karta_rezerva2PC.getNazevKarty();
            }
            else
            {
                karty_rezerva2PC.Clear();
                karta_rezerva2PC = new Card("default", "default", 50, '♦');

                rezerva2PC.Text = "--";

            }
        }

        private void setRezerva3PC()
        {
            if (karty_rezerva3PC.Count - 1 > 0)
            {
                karty_rezerva3PC.Remove(karta_rezerva3PC);
                karta_rezerva3PC = karty_rezerva3PC.Last();
                if (jeCervena(karta_rezerva3PC))
                {
                    rezerva3PC.ForeColor = Color.Red;
                }
                else
                {
                    rezerva3PC.ForeColor = Color.Black;
                }
                rezerva3PC.Text = karta_rezerva3PC.getNazevKarty();
            }
            else
            {
                karty_rezerva3PC.Clear();
                karta_rezerva3PC = new Card("default", "default", 50, '♦');

                rezerva3PC.Text = "--";

            }
        }

        private void setRezerva4PC()
        {
            if (karty_rezerva4PC.Count - 1 > 0)
            {
                karty_rezerva4PC.Remove(karta_rezerva4PC);
                karta_rezerva4PC = karty_rezerva4PC.Last();
                if (jeCervena(karta_rezerva4PC))
                {
                    rezerva4PC.ForeColor = Color.Red;
                }
                else
                {
                    rezerva4PC.ForeColor = Color.Black;
                }
                rezerva4PC.Text = karta_rezerva4PC.getNazevKarty();
            }
            else
            {
                karty_rezerva4PC.Clear();
                karta_rezerva4PC = new Card("default", "default", 50, '♦');

                rezerva4PC.Text = "--";

            }
        }
        // 162x if

        private Boolean lze_zalozit()
        {
            Boolean lze = false;
            Card karta_v_ruce1PC = karty_v_rucePC.ElementAt(0);
            Card karta_v_ruce2PC = karty_v_rucePC.ElementAt(1);
            Card karta_v_ruce3PC = karty_v_rucePC.ElementAt(2);
            Card karta_v_ruce4PC = karty_v_rucePC.ElementAt(3);
            Card karta_v_ruce5PC = karty_v_rucePC.ElementAt(4);
            

            //1.karta v ruce
            if ((karta_rezerva1PC.getHodnota() == karta_v_ruce1PC.getHodnota() || rezerva1PC.Text == "--") &&
                    (karta_v_ruce1PC.getHodnota() != karta_rezerva2PC.getHodnota() && karta_v_ruce1PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                    karta_v_ruce1PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo1PC(karta_v_ruce1PC);
                karty_v_rucePC.Remove(karta_v_ruce1PC);
                Ruka1PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva2PC.getHodnota() == karta_v_ruce1PC.getHodnota() || rezerva2PC.Text == "--") &&
                (karta_v_ruce1PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce1PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                karta_v_ruce1PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo2PC(karta_v_ruce1PC);
                karty_v_rucePC.Remove(karta_v_ruce1PC);
                Ruka1PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva3PC.getHodnota() == karta_v_ruce1PC.getHodnota() || rezerva3PC.Text == "--") &&
                (karta_v_ruce1PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce1PC.getHodnota() != karta_rezerva2PC.getHodnota() &&
                karta_v_ruce1PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo3PC(karta_v_ruce1PC);
                karty_v_rucePC.Remove(karta_v_ruce1PC);
                Ruka1PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva4PC.getHodnota() == karta_v_ruce1PC.getHodnota() || rezerva4PC.Text == "--") &&
                (karta_v_ruce1PC.getHodnota() != karta_rezerva2PC.getHodnota() && karta_v_ruce1PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                karta_v_ruce1PC.getHodnota() != karta_rezerva1PC.getHodnota()))
            {
                zalozitDo4PC(karta_v_ruce1PC);
                karty_v_rucePC.Remove(karta_v_ruce1PC);
                Ruka1PC.Text = "--";
                lze = true;

            }


            //2.karta v ruce
            else if ((karta_rezerva1PC.getHodnota() == karta_v_ruce2PC.getHodnota() || rezerva1PC.Text == "--") &&
                    (karta_v_ruce2PC.getHodnota() != karta_rezerva2PC.getHodnota() && karta_v_ruce2PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                    karta_v_ruce2PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo1PC(karta_v_ruce2PC);
                karty_v_rucePC.Remove(karta_v_ruce2PC);
                Ruka2PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva2PC.getHodnota() == karta_v_ruce2PC.getHodnota() || rezerva2PC.Text == "--") &&
                (karta_v_ruce2PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce2PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                karta_v_ruce2PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo2PC(karta_v_ruce2PC);
                karty_v_rucePC.Remove(karta_v_ruce2PC);
                Ruka2PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva3PC.getHodnota() == karta_v_ruce2PC.getHodnota() || rezerva3PC.Text == "--") &&
                (karta_v_ruce2PC.getHodnota() != karta_rezerva2PC.getHodnota() && karta_v_ruce2PC.getHodnota() != karta_rezerva1PC.getHodnota() &&
                karta_v_ruce2PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo3PC(karta_v_ruce2PC);
                karty_v_rucePC.Remove(karta_v_ruce2PC);
                Ruka2PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva4PC.getHodnota() == karta_v_ruce2PC.getHodnota() || rezerva4PC.Text == "--") &&
                (karta_v_ruce2PC.getHodnota() != karta_rezerva2PC.getHodnota() && karta_v_ruce2PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                karta_v_ruce2PC.getHodnota() != karta_rezerva1PC.getHodnota()))
            {
                zalozitDo4PC(karta_v_ruce2PC);
                karty_v_rucePC.Remove(karta_v_ruce2PC);
                Ruka2PC.Text = "--";
                lze = true;

            }

            //3.karta v ruce
            else if ((karta_rezerva1PC.getHodnota() == karta_v_ruce3PC.getHodnota() || rezerva1PC.Text == "--") &&
                    (karta_v_ruce3PC.getHodnota() != karta_rezerva2PC.getHodnota() && karta_v_ruce3PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                    karta_v_ruce3PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo1PC(karta_v_ruce3PC);
                karty_v_rucePC.Remove(karta_v_ruce3PC);
                Ruka3PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva2PC.getHodnota() == karta_v_ruce3PC.getHodnota() || rezerva2PC.Text == "--") &&
                (karta_v_ruce3PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce3PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                karta_v_ruce3PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo2PC(karta_v_ruce3PC);
                karty_v_rucePC.Remove(karta_v_ruce3PC);
                Ruka3PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva3PC.getHodnota() == karta_v_ruce3PC.getHodnota() || rezerva3PC.Text == "--") &&
                (karta_v_ruce3PC.getHodnota() != karta_rezerva2PC.getHodnota() && karta_v_ruce3PC.getHodnota() != karta_rezerva1PC.getHodnota() &&
                karta_v_ruce3PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo3PC(karta_v_ruce3PC);
                karty_v_rucePC.Remove(karta_v_ruce3PC);
                Ruka3PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva4PC.getHodnota() == karta_v_ruce3PC.getHodnota() || rezerva4PC.Text == "--") &&
                (karta_v_ruce3PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce3PC.getHodnota() != karta_rezerva2PC.getHodnota() &&
                karta_v_ruce3PC.getHodnota() != karta_rezerva3PC.getHodnota()))
            {
                zalozitDo4PC(karta_v_ruce3PC);
                karty_v_rucePC.Remove(karta_v_ruce3PC);
                Ruka3PC.Text = "--";
                lze = true;

            }

            //4.karta v ruce 
            else if ((karta_rezerva1PC.getHodnota() == karta_v_ruce4PC.getHodnota() || rezerva1PC.Text == "--") &&
                    (karta_v_ruce4PC.getHodnota() != karta_rezerva2PC.getHodnota() && karta_v_ruce4PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                    karta_v_ruce4PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo1PC(karta_v_ruce4PC);
                karty_v_rucePC.Remove(karta_v_ruce4PC);
                Ruka4PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva2PC.getHodnota() == karta_v_ruce4PC.getHodnota() || rezerva2PC.Text == "--") &&
                (karta_v_ruce4PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce4PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                karta_v_ruce4PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo2PC(karta_v_ruce4PC);
                karty_v_rucePC.Remove(karta_v_ruce4PC);
                Ruka4PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva3PC.getHodnota() == karta_v_ruce4PC.getHodnota() || rezerva3PC.Text == "--") &&
                (karta_v_ruce4PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce4PC.getHodnota() != karta_rezerva2PC.getHodnota() &&
                karta_v_ruce4PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo3PC(karta_v_ruce4PC);
                karty_v_rucePC.Remove(karta_v_ruce4PC);
                Ruka4PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva4PC.getHodnota() == karta_v_ruce4PC.getHodnota() || rezerva4PC.Text == "--") &&
                (karta_v_ruce4PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce4PC.getHodnota() != karta_rezerva2PC.getHodnota() &&
                karta_v_ruce4PC.getHodnota() != karta_rezerva3PC.getHodnota()))
            {
                zalozitDo4PC(karta_v_ruce4PC);
                karty_v_rucePC.Remove(karta_v_ruce4PC);
                Ruka4PC.Text = "--";
                lze = true;

            }

            //5.karta v ruce
            else if ((karta_rezerva1PC.getHodnota() == karta_v_ruce5PC.getHodnota() || rezerva1PC.Text == "--") &&
                    (karta_v_ruce5PC.getHodnota() != karta_rezerva2PC.getHodnota() && karta_v_ruce5PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                    karta_v_ruce5PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo1PC(karta_v_ruce5PC);
                karty_v_rucePC.Remove(karta_v_ruce5PC);
                Ruka5PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva2PC.getHodnota() == karta_v_ruce5PC.getHodnota() || rezerva2PC.Text == "--") &&
                (karta_v_ruce5PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce5PC.getHodnota() != karta_rezerva3PC.getHodnota() &&
                karta_v_ruce5PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo2PC(karta_v_ruce5PC);
                karty_v_rucePC.Remove(karta_v_ruce5PC);
                Ruka5PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva3PC.getHodnota() == karta_v_ruce5PC.getHodnota() || rezerva3PC.Text == "--") &&
                (karta_v_ruce5PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce5PC.getHodnota() != karta_rezerva2PC.getHodnota() &&
                karta_v_ruce5PC.getHodnota() != karta_rezerva4PC.getHodnota()))
            {
                zalozitDo3PC(karta_v_ruce5PC);
                karty_v_rucePC.Remove(karta_v_ruce5PC);
                Ruka5PC.Text = "--";
                lze = true;

            }
            else if ((karta_rezerva4PC.getHodnota() == karta_v_ruce5PC.getHodnota() || rezerva4PC.Text == "--") &&
                (karta_v_ruce5PC.getHodnota() != karta_rezerva1PC.getHodnota() && karta_v_ruce5PC.getHodnota() != karta_rezerva2PC.getHodnota() &&
                karta_v_ruce5PC.getHodnota() != karta_rezerva3PC.getHodnota()))
            {
                zalozitDo4PC(karta_v_ruce5PC);
                karty_v_rucePC.Remove(karta_v_ruce5PC);
                Ruka5PC.Text = "--";
                lze = true;

            }
            return lze;

        }

        private void zalozitDo1PC(Card karta_v_ruce)
        {
            karty_rezerva1PC.Add(karta_v_ruce);
            karta_rezerva1PC = karty_rezerva1PC.Last();
            if (jeCervena(karta_rezerva1PC))
            {
                rezerva1PC.ForeColor = Color.Red;
            }
            else
            {
                rezerva1PC.ForeColor = Color.Black;
            }
            rezerva1PC.Text = karta_rezerva1PC.getNazevKarty();
            
        }

        private void zalozitDo2PC(Card karta_v_ruce)
        {
            karty_rezerva2PC.Add(karta_v_ruce);
            karta_rezerva2PC = karty_rezerva2PC.Last();
            if (jeCervena(karta_rezerva2PC))
            {
                rezerva2PC.ForeColor = Color.Red;
            }
            else
            {
                rezerva2PC.ForeColor = Color.Black;
            }
            rezerva2PC.Text = karta_rezerva2PC.getNazevKarty();
        }

        private void zalozitDo3PC(Card karta_v_ruce)
        {
            karty_rezerva3PC.Add(karta_v_ruce);
            karta_rezerva3PC = karty_rezerva3PC.Last();
            if (jeCervena(karta_rezerva3PC))
            {
                rezerva3PC.ForeColor = Color.Red;
            }
            else
            {
                rezerva3PC.ForeColor = Color.Black;
            }
            rezerva3PC.Text = karta_rezerva3PC.getNazevKarty();
        }

        private void zalozitDo4PC(Card karta_v_ruce)
        {
            karty_rezerva4PC.Add(karta_v_ruce);
            karta_rezerva4PC = karty_rezerva4PC.Last();
            if (jeCervena(karta_rezerva4PC))
            {
                rezerva4PC.ForeColor = Color.Red;
            }
            else
            {
                rezerva4PC.ForeColor = Color.Black;
            }
            rezerva4PC.Text = karta_rezerva4PC.getNazevKarty();
        }

        private void pridat_kartu_do_pakluPC(Card karta_z_ruky)
        {
            karty_v_pakluPC = karty_v_pakluPC.Append(karta_z_ruky).ToList();
            karty_v_rucePC.Remove(karta_z_ruky);
            labelPocetPakluPC.Text = "Pocet:" + karty_v_pakluPC.Count;
        }

        private void konec_tahuPC()
        {
            if (!je_plna_ruka())
            {
                Debug.WriteLine("PC ma min nez 5 karet a muze ukoncit tah");
                hrac_je_na_tahu = true;
                odlizano = false;
                Balicek.Enabled = true;
                herniHlasatel.Text = "HRAJEŠ!";

            }
        }

        private void radioButtonZalozit_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void paklPC_Click(object sender, EventArgs e)
        {

        }
    }
}
