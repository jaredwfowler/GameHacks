using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lands_Of_Lore_1_Mega_Trainer
{

    public partial class LandsOfLoreMegaTrainer_Form : Form
    {
        public LandsOfLoreMegaTrainer_Form()
        {
            InitializeComponent();
        }

        private void sge_button_browse_Click(object sender, EventArgs e)
        {
            /*This button should enable the user to select a save game file,
             * which will then automatically be added to the File Path Text Box.*/

            //Create a File Dialog Object
            var FD = new System.Windows.Forms.OpenFileDialog();

            //Set behaviors of FD
            FD.Filter = ".DAT Game | *.DAT";
            FD.Title = "Select LOL .DAT Game File";

            //Bring up browse windows mode
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Set the string to the file path text box
                this.sge_textbox_filePath.Text = FD.FileName;

                //Load values into editor
                loadInitialValues();
            }
        }

        private void sge_button_reset_Click(object sender, EventArgs e)
        {
            //This button should clear all the fields within the save game editor (except for the chosen file)
            sge_textBox_crowns.Text = "";

            sge_comboBox_p1.Text = "";
            sge_textBox_p1_might.Text = "";
            sge_textBox_p1_armor.Text = "";
            sge_textBox_p1_fighterlvl.Text = "";
            sge_textBox_p1_roguelvl.Text = "";
            sge_textBox_p1_magelvl.Text = "";
            sge_textBox_p1_hp_mana.Text = "";

            sge_comboBox_p2.Text = "";
            sge_textBox_p2_might.Text = "";
            sge_textBox_p2_armor.Text = "";
            sge_textBox_p2_fighterlvl.Text = "";
            sge_textBox_p2_roguelvl.Text = "";
            sge_textBox_p2_magelvl.Text = "";
            sge_textBox_p2_hp_mana.Text = "";

            sge_comboBox_p3.Text = "";
            sge_textBox_p3_might.Text = "";
            sge_textBox_p3_armor.Text = "";
            sge_textBox_p3_fighterlvl.Text = "";
            sge_textBox_p3_roguelvl.Text = "";
            sge_textBox_p3_magelvl.Text = "";
            sge_textBox_p3_hp_mana.Text = "";

            sge_comboBox_location.Text = "";

            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            comboBox6.Text = "";
            comboBox7.Text = "";
            comboBox8.Text = "";
            comboBox9.Text = "";
            comboBox10.Text = "";
            comboBox11.Text = "";
            comboBox12.Text = "";
            comboBox13.Text = "";
            comboBox14.Text = "";
            comboBox15.Text = "";
            comboBox16.Text = "";
            comboBox17.Text = "";
            comboBox18.Text = "";
            comboBox19.Text = "";
            comboBox20.Text = "";
            comboBox21.Text = "";
            comboBox22.Text = "";
            comboBox23.Text = "";
            comboBox24.Text = "";
            comboBox25.Text = "";
            comboBox26.Text = "";
            comboBox27.Text = "";
            comboBox28.Text = "";
            comboBox29.Text = "";
            comboBox30.Text = "";
            comboBox31.Text = "";
            comboBox32.Text = "";
            comboBox33.Text = "";
            comboBox34.Text = "";
            comboBox35.Text = "";
            comboBox36.Text = "";
            comboBox37.Text = "";
            comboBox38.Text = "";
            comboBox39.Text = "";
            comboBox40.Text = "";
            comboBox41.Text = "";
            comboBox42.Text = "";
            comboBox43.Text = "";
            comboBox44.Text = "";
            comboBox45.Text = "";
            comboBox46.Text = "";
            comboBox47.Text = "";
            comboBox48.Text = "";
            if(String.Compare(sge_textbox_filePath.Text, "") != 0)
                loadInitialValues();
        }

        private void sge_button_save_Click(object sender, EventArgs e)
        {
            //Local Variables
            FileStream fd;
            byte[] buffer = new byte[50];

            //Are the fields valid?
            {
                int temp;
                if (!int.TryParse(sge_textBox_p1_might.Text, out temp) && String.Compare(sge_textBox_p1_might.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p2_might.Text, out temp) && String.Compare(sge_textBox_p2_might.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p3_might.Text, out temp) && String.Compare(sge_textBox_p3_might.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p1_armor.Text, out temp) && String.Compare(sge_textBox_p1_armor.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p2_armor.Text, out temp) && String.Compare(sge_textBox_p2_armor.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p3_armor.Text, out temp) && String.Compare(sge_textBox_p3_armor.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p1_fighterlvl.Text, out temp) && String.Compare(sge_textBox_p1_fighterlvl.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p2_fighterlvl.Text, out temp) && String.Compare(sge_textBox_p2_fighterlvl.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p3_fighterlvl.Text, out temp) && String.Compare(sge_textBox_p3_fighterlvl.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p1_magelvl.Text, out temp) && String.Compare(sge_textBox_p1_magelvl.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p2_magelvl.Text, out temp) && String.Compare(sge_textBox_p2_magelvl.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p3_magelvl.Text, out temp) && String.Compare(sge_textBox_p3_magelvl.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p1_roguelvl.Text, out temp) && String.Compare(sge_textBox_p1_roguelvl.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p2_roguelvl.Text, out temp) && String.Compare(sge_textBox_p2_roguelvl.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p3_roguelvl.Text, out temp) && String.Compare(sge_textBox_p3_roguelvl.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p1_hp_mana.Text, out temp) && String.Compare(sge_textBox_p1_hp_mana.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p2_hp_mana.Text, out temp) && String.Compare(sge_textBox_p2_hp_mana.Text, "") != 0 ||
                    !int.TryParse(sge_textBox_p3_hp_mana.Text, out temp) && String.Compare(sge_textBox_p3_hp_mana.Text, "") != 0
                    )
                {
                    System.Windows.Forms.MessageBox.Show("Non-numerical values in one or more fields...");
                    return;
                }

            }
            
            //Attempt to open the file
            if (String.Compare(sge_textbox_filePath.Text, "") == 0)
            {
                System.Windows.Forms.MessageBox.Show("No File Selected!");
                return;
            }
            try
            {
                fd = File.Open(sge_textbox_filePath.Text, FileMode.Open);
            }
            catch(Exception j)
            {
                System.Windows.Forms.MessageBox.Show("Failed to open file.");
                return;
            }

            //Attempt to verify that the file is the right type
            fd.Seek(FILE_START_SEQ_ADDRESS, SeekOrigin.Begin);
            fd.Read(buffer, 0, FILE_START_SEQ.Length);

            for (int i = 0; i < FILE_START_SEQ.Length; i++)
            {
                if (buffer[i] != FILE_START_SEQ[i])
                {
                    System.Windows.Forms.MessageBox.Show("Incorrect File Type");
                    return;
                }
            }

            //Crowns ***************************************************************
            if (String.Compare(sge_textBox_crowns.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_crowns.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(ADDR_CROWNS, SeekOrigin.Begin);
                fd.Write(buffer, 0, CROWNS_SIZE);
            }

            //Location *************************************************************
            if(String.Compare(sge_comboBox_location.Text, "") != 0)
            {
                //Write values to file
                fd.Seek(TEL_ADDR, SeekOrigin.Begin);
                fd.Write(LOCATIONS[sge_comboBox_location.SelectedIndex], 0, LOCATIONS[sge_comboBox_location.SelectedIndex].Length);
            }

            //Player SKILLS ********************************************************
            if (String.Compare(sge_textBox_p1_might.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_p1_might.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(P1_ADDR_MIGHT, SeekOrigin.Begin);
                fd.Write(buffer, 0, MIGHT_PROTECTION_SIZE);
            }
            if (String.Compare(sge_textBox_p2_might.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_p2_might.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(P2_ADDR_MIGHT, SeekOrigin.Begin);
                fd.Write(buffer, 0, MIGHT_PROTECTION_SIZE);
            }
            if (String.Compare(sge_textBox_p3_might.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_p3_might.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(P3_ADDR_MIGHT, SeekOrigin.Begin);
                fd.Write(buffer, 0, MIGHT_PROTECTION_SIZE);
            }
            if (String.Compare(sge_textBox_p1_armor.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_p1_armor.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(P1_ADDR_PROT, SeekOrigin.Begin);
                fd.Write(buffer, 0, MIGHT_PROTECTION_SIZE);
            }
            if (String.Compare(sge_textBox_p2_armor.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_p2_armor.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(P2_ADDR_PROT, SeekOrigin.Begin);
                fd.Write(buffer, 0, MIGHT_PROTECTION_SIZE);
            }
            if (String.Compare(sge_textBox_p3_armor.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_p3_armor.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(P3_ADDR_PROT, SeekOrigin.Begin);
                fd.Write(buffer, 0, MIGHT_PROTECTION_SIZE);
            }
            if (String.Compare(sge_textBox_p1_roguelvl.Text, "") != 0)
            {
                //Valid Value?
                byte value8 = (byte)Convert.ToInt32(sge_textBox_p1_roguelvl.Text);
                if (value8 > MAX_8 || value8 < 0)
                    value8 = (byte)MAX_8;

                //Prepare buffer
                buffer[0] = value8;

                //Write value to file
                fd.Seek(P1_ADDR_ROGUE, SeekOrigin.Begin);
                fd.Write(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);
            }
            if (String.Compare(sge_textBox_p2_roguelvl.Text, "") != 0)
            {
                //Valid Value?
                byte value8 = (byte)Convert.ToInt32(sge_textBox_p2_roguelvl.Text);
                if (value8 > MAX_8 || value8 < 0)
                    value8 = (byte)MAX_8;

                //Prepare buffer
                buffer[0] = value8;

                //Write value to file
                fd.Seek(P2_ADDR_ROGUE, SeekOrigin.Begin);
                fd.Write(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);
            }
            if (String.Compare(sge_textBox_p3_roguelvl.Text, "") != 0)
            {
                //Valid Value?
                byte value8 = (byte)Convert.ToInt32(sge_textBox_p3_roguelvl.Text);
                if (value8 > MAX_8 || value8 < 0)
                    value8 = (byte)MAX_8;

                //Prepare buffer
                buffer[0] = value8;

                //Write value to file
                fd.Seek(P3_ADDR_ROGUE, SeekOrigin.Begin);
                fd.Write(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);
            }
            if (String.Compare(sge_textBox_p1_magelvl.Text, "") != 0)
            {
                //Valid Value?
                byte value8 = (byte)Convert.ToInt32(sge_textBox_p1_magelvl.Text);
                if (value8 > MAX_8 || value8 < 0)
                    value8 = (byte)MAX_8;

                //Prepare buffer
                buffer[0] = value8;

                //Write value to file
                fd.Seek(P1_ADDR_MAGE, SeekOrigin.Begin);
                fd.Write(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);
            }
            if (String.Compare(sge_textBox_p2_magelvl.Text, "") != 0)
            {
                //Valid Value?
                byte value8 = (byte)Convert.ToInt32(sge_textBox_p2_magelvl.Text);
                if (value8 > MAX_8 || value8 < 0)
                    value8 = (byte)MAX_8;

                //Prepare buffer
                buffer[0] = value8;

                //Write value to file
                fd.Seek(P2_ADDR_MAGE, SeekOrigin.Begin);
                fd.Write(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);
            }
            if (String.Compare(sge_textBox_p3_magelvl.Text, "") != 0)
            {
                //Valid Value?
                byte value8 = (byte)Convert.ToInt32(sge_textBox_p3_magelvl.Text);
                if (value8 > MAX_8 || value8 < 0)
                    value8 = (byte)MAX_8;

                //Prepare buffer
                buffer[0] = value8;

                //Write value to file
                fd.Seek(P3_ADDR_MAGE, SeekOrigin.Begin);
                fd.Write(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);
            }
            if (String.Compare(sge_textBox_p1_fighterlvl.Text, "") != 0)
            {
                //Valid Value?
                byte value8 = (byte)Convert.ToInt32(sge_textBox_p1_fighterlvl.Text);
                if (value8 > MAX_8 || value8 < 0)
                    value8 = (byte)MAX_8;

                //Prepare buffer
                buffer[0] = value8;

                //Write value to file
                fd.Seek(P1_ADDR_FIGHT, SeekOrigin.Begin);
                fd.Write(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);
            }
            if (String.Compare(sge_textBox_p2_fighterlvl.Text, "") != 0)
            {
                //Valid Value?
                byte value8 = (byte)Convert.ToInt32(sge_textBox_p2_fighterlvl.Text);
                if (value8 > MAX_8 || value8 < 0)
                    value8 = (byte)MAX_8;

                //Prepare buffer
                buffer[0] = value8;

                //Write value to file
                fd.Seek(P2_ADDR_FIGHT, SeekOrigin.Begin);
                fd.Write(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);
            }
            if (String.Compare(sge_textBox_p3_fighterlvl.Text, "") != 0)
            {
                //Valid Value?
                byte value8 = (byte)Convert.ToInt32(sge_textBox_p3_fighterlvl.Text);
                if (value8 > MAX_8 || value8 < 0)
                    value8 = (byte)MAX_8;

                //Prepare buffer
                buffer[0] = value8;

                //Write value to file
                fd.Seek(P3_ADDR_FIGHT, SeekOrigin.Begin);
                fd.Write(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);
            }
            if (String.Compare(sge_textBox_p1_hp_mana.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_p1_hp_mana.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(P1_ADDR_HPCUR, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
                //Write value to file
                fd.Seek(P1_ADDR_HP, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
                //Write value to file
                fd.Seek(P1_ADDR_MPCUR, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
                //Write value to file
                fd.Seek(P1_ADDR_MP, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
            }
            if (String.Compare(sge_textBox_p2_hp_mana.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_p2_hp_mana.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(P2_ADDR_HPCUR, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
                //Write value to file
                fd.Seek(P2_ADDR_HP, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
                //Write value to file
                fd.Seek(P2_ADDR_MPCUR, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
                //Write value to file
                fd.Seek(P2_ADDR_MP, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
            }
            if (String.Compare(sge_textBox_p3_hp_mana.Text, "") != 0)
            {
                //Valid Value?
                Int16 value16 = (Int16)Convert.ToInt32(sge_textBox_p3_hp_mana.Text);
                if (value16 > MAX_16 || value16 < 0)
                    value16 = (Int16)MAX_16;

                //Prepare buffer
                buffer[0] = (byte)((value16 << 8) >> 8);
                buffer[1] = (byte)(value16 >> 8);

                //Write value to file
                fd.Seek(P3_ADDR_HPCUR, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
                //Write value to file
                fd.Seek(P3_ADDR_HP, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
                //Write value to file
                fd.Seek(P3_ADDR_MPCUR, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
                //Write value to file
                fd.Seek(P3_ADDR_MP, SeekOrigin.Begin);
                fd.Write(buffer, 0, HEALTH_MAGIC_SIZE);
            }

            //Character Selection ************************************************************************
            if (String.Compare(sge_comboBox_p1.Text, "") != 0)
            {
                byte[] ptr = CHARACTERS[sge_comboBox_p1.SelectedIndex];

                //Write value to file
                fd.Seek(CHAR_P1_ADDR, SeekOrigin.Begin);
                fd.Write(ptr, 0, ptr.Length);
            }
            if (String.Compare(sge_comboBox_p2.Text, "") != 0)
            {
                byte[] ptr = CHARACTERS[sge_comboBox_p2.SelectedIndex];

                //Write value to file
                fd.Seek(CHAR_P2_ADDR, SeekOrigin.Begin);
                fd.Write(ptr, 0, ptr.Length);
            }
            if (String.Compare(sge_comboBox_p3.Text, "") != 0)
            {
                byte[] ptr = CHARACTERS[sge_comboBox_p3.SelectedIndex];

                //Write value to file
                fd.Seek(CHAR_P3_ADDR, SeekOrigin.Begin);
                fd.Write(ptr, 0, ptr.Length);
            }

            fd.Close();

            //Inventory *****************************************************************
            UInt16 k = 1;
            if (String.Compare(comboBox1.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox1.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox2.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox2.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox3.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox3.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox4.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox4.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox5.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox5.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox6.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox6.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox7.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox7.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox8.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox8.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox9.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox9.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox10.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox10.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox11.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox11.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox12.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox12.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox13.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox13.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox14.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox14.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox15.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox15.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox16.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox16.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox17.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox17.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox18.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox18.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox19.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox19.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox20.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox20.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox21.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox21.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox22.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox22.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox23.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox23.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox24.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox24.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox25.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox25.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox26.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox26.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox27.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox27.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox28.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox28.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox29.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox29.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox30.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox30.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox31.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox31.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox32.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox32.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox33.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox33.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox34.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox34.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox35.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox35.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox36.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox36.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox37.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox37.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox38.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox38.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox39.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox39.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox40.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox40.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox41.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox41.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox42.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox42.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox43.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox43.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox44.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox44.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox45.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox45.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox46.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox46.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox47.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox47.SelectedIndex + 1)); k++;
            if (String.Compare(comboBox48.Text, "") != 0) saveInventoryId(k, (UInt16)(comboBox48.SelectedIndex + 1)); k++;



            System.Windows.Forms.MessageBox.Show("All Done!.");
        }

        void loadInitialValues()
        {
            FileStream fd;
            byte[] buffer = new byte[50];

            try
            {
                fd = File.Open(sge_textbox_filePath.Text, FileMode.Open);
            }
            catch (Exception j)
            {
                System.Windows.Forms.MessageBox.Show("Failed to open file.");
                return;
            }


            //Load Crowns
            {
                fd.Seek(ADDR_CROWNS, SeekOrigin.Begin);
                fd.Read(buffer, 0, CROWNS_SIZE);

                UInt16 value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_crowns.Text = value16.ToString();
            }

            //Load player skills
            {
                UInt16 value16;
                byte value8;

                fd.Seek(P1_ADDR_MIGHT, SeekOrigin.Begin);
                fd.Read(buffer, 0, MIGHT_PROTECTION_SIZE);

                value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_p1_might.Text = value16.ToString();

                fd.Seek(P2_ADDR_MIGHT, SeekOrigin.Begin);
                fd.Read(buffer, 0, MIGHT_PROTECTION_SIZE);

                value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_p2_might.Text = value16.ToString();

                fd.Seek(P3_ADDR_MIGHT, SeekOrigin.Begin);
                fd.Read(buffer, 0, MIGHT_PROTECTION_SIZE);

                value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_p3_might.Text = value16.ToString();

                fd.Seek(P1_ADDR_PROT, SeekOrigin.Begin);
                fd.Read(buffer, 0, MIGHT_PROTECTION_SIZE);

                value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_p1_armor.Text = value16.ToString();

                fd.Seek(P2_ADDR_PROT, SeekOrigin.Begin);
                fd.Read(buffer, 0, MIGHT_PROTECTION_SIZE);

                value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_p2_armor.Text = value16.ToString();

                fd.Seek(P3_ADDR_PROT, SeekOrigin.Begin);
                fd.Read(buffer, 0, MIGHT_PROTECTION_SIZE);

                value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_p3_armor.Text = value16.ToString();

                fd.Seek(P1_ADDR_HP, SeekOrigin.Begin);
                fd.Read(buffer, 0, HEALTH_MAGIC_SIZE);

                value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_p1_hp_mana.Text = value16.ToString();

                fd.Seek(P2_ADDR_HP, SeekOrigin.Begin);
                fd.Read(buffer, 0, HEALTH_MAGIC_SIZE);

                value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_p2_hp_mana.Text = value16.ToString();

                fd.Seek(P3_ADDR_HP, SeekOrigin.Begin);
                fd.Read(buffer, 0, HEALTH_MAGIC_SIZE);

                value16 = (UInt16)buffer[1];
                value16 = (UInt16)(((UInt16)(value16 << 8)) | (UInt16)buffer[0]);

                sge_textBox_p3_hp_mana.Text = value16.ToString();

                fd.Seek(P1_ADDR_ROGUE, SeekOrigin.Begin);
                fd.Read(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);

                value8 = buffer[0];

                sge_textBox_p1_roguelvl.Text = value8.ToString();

                fd.Seek(P2_ADDR_ROGUE, SeekOrigin.Begin);
                fd.Read(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);

                value8 = buffer[0];

                sge_textBox_p2_roguelvl.Text = value8.ToString();

                fd.Seek(P3_ADDR_ROGUE, SeekOrigin.Begin);
                fd.Read(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);

                value8 = buffer[0];

                sge_textBox_p3_roguelvl.Text = value8.ToString();

                fd.Seek(P1_ADDR_FIGHT, SeekOrigin.Begin);
                fd.Read(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);

                value8 = buffer[0];

                sge_textBox_p1_fighterlvl.Text = value8.ToString();

                fd.Seek(P2_ADDR_FIGHT, SeekOrigin.Begin);
                fd.Read(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);

                value8 = buffer[0];

                sge_textBox_p2_fighterlvl.Text = value8.ToString();

                fd.Seek(P3_ADDR_FIGHT, SeekOrigin.Begin);
                fd.Read(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);

                value8 = buffer[0];

                sge_textBox_p3_fighterlvl.Text = value8.ToString();

                fd.Seek(P1_ADDR_MAGE, SeekOrigin.Begin);
                fd.Read(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);

                value8 = buffer[0];

                sge_textBox_p1_magelvl.Text = value8.ToString();

                fd.Seek(P3_ADDR_MAGE, SeekOrigin.Begin);
                fd.Read(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);

                value8 = buffer[0];

                sge_textBox_p2_magelvl.Text = value8.ToString();

                fd.Seek(P3_ADDR_MAGE, SeekOrigin.Begin);
                fd.Read(buffer, 0, FIGHT_ROGUE_MAGE_SIZE);

                value8 = buffer[0];

                sge_textBox_p3_magelvl.Text = value8.ToString();

            }

            fd.Close();

            //Load inventory
            {
                UInt16 value16, i;

                i = 0;

                if ((value16 = getInventoryId(i)) != 0) comboBox1.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox2.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox3.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox4.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox5.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox6.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox7.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox8.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox9.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox10.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox11.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox12.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox13.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox14.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox15.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox16.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox17.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox18.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox19.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox20.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox21.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox22.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox23.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox24.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox25.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox26.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox27.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox28.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox29.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox30.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox31.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox32.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox33.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox34.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox35.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox36.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox37.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox38.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox39.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox40.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox41.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox42.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox43.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox44.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox45.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox46.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox47.SelectedIndex = value16 - 1; i++;
                if ((value16 = getInventoryId(i)) != 0) comboBox48.SelectedIndex = value16 - 1; 
            }
        }

        UInt16 getInventoryId(UInt16 offset)
        {
            FileStream fd;
            byte[] buffer = new byte[2];

            try
            {
                fd = File.Open(sge_textbox_filePath.Text, FileMode.Open);
            }
            catch (Exception k)
            {
                System.Windows.Forms.MessageBox.Show("Failed to open file.");
                return 0;
            }

            UInt16 invNum, invId;
            UInt32 i, j;

            i = (UInt32)INV_ADDR_LO_START + ((UInt32)offset * INV_LO_SIZE);
            j = (UInt32)INV_ADDR_HI_START + (UInt32)INV_HI_OFFSEt;


            fd.Seek(i, SeekOrigin.Begin);
            fd.Read(buffer, 0, INV_LO_SIZE);
            invNum = (UInt16)buffer[1];
            invNum = (UInt16)(((UInt16)(invNum << 8)) | (UInt16)buffer[0]);
            if (invNum == 0)
            {
                fd.Close();
                return 0;
            }
                
            fd.Seek(j + ((invNum - 1) * INV_ROW_SIZE), SeekOrigin.Begin);
            fd.Read(buffer, 0, INV_HI_SIZE);
            invId = (UInt16)buffer[1];
            invId = (UInt16)(((UInt16)(invId << 8)) | (UInt16)buffer[0]);

            fd.Close();
            return invId;
        }

        void saveInventoryId(UInt16 slot, UInt16 id)
        {
            FileStream fd;
            byte[] buffer = new byte[2];

            try
            {
                fd = File.Open(sge_textbox_filePath.Text, FileMode.Open);
            }
            catch (Exception k)
            {
                System.Windows.Forms.MessageBox.Show("Failed to open file.");
                return;
            }

            UInt16 invNum = (UInt16)((Int16)slot - 1);
            UInt16 invId = id;
            UInt32 i, j;

            i = (UInt32)INV_ADDR_LO_START + ((UInt32)invNum * INV_LO_SIZE);
            j = (UInt32)INV_ADDR_HI_START + (UInt32)INV_HI_OFFSEt + (UInt32)(invNum * INV_ROW_SIZE);

            fd.Seek(i, SeekOrigin.Begin);
            buffer[0] = (byte)slot;
            buffer[1] = (byte)(slot >> 8);
            fd.Write(buffer, 0, INV_LO_SIZE);

            fd.Seek(j, SeekOrigin.Begin);
            buffer[0] = (byte)id;
            buffer[1] = (byte)(id >> 8);
            fd.Write(buffer, 0, INV_HI_SIZE);

            fd.Close();
        }


    }
}
