/* Created By: Jared Fowler  Aug. 13, 2014
 * 
 * First attempt at making a trainer via visual studio. 
 * I've chosen C# b/c of its easy to use forum creator,
 * along with the asily imported DLL "kernel32.dll" which
 * will allow me to access other window's processes. This 
 * program will hopefully serve as a future template for 
 * other trainers.
 */

/*
 * TRAINER: Lands Of Lore Guardians Of Destiny V. 1.30
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*Extra Tools*/
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using System.Threading;


namespace Trainer
{
    public partial class Form1 : Form
    {
        /* Set permissions. This deals with what we will be able to do with 
         * the external process memory.  A list of permissions can be found
         * here: http://msdn.microsoft.com/en-us/library/windows/desktop/ms684880(v=vs.85).aspx
         */

        const int PROCESS_VM_WRITE      = 0x0020;
        const int PROCESS_VM_OPERATION  = 0x0008;
        const int PROCESS_WM_READ       = 0x0010;

        const UInt16 MAX_WRITE_ATTEMPTS = 5000;  //MAX TIMES WE WILL TRY WRITING TO MEMORY BEFORE GIVING UP (FAILSAFE FOR INFINITE LOOPS)


        /*Import needed library functions from kernel32 : Open, Write, Read*/
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        /*If the function succeeds, the return value specifies whether the key was pressed since the last call to GetAsyncKeyState, and whether 
         * the key is currently up or down. If the most significant bit is set, the key is down, and if the least significant bit is set, the key 
         * was pressed after the previous call to GetAsyncKeyState. However, you should not rely on this last behavior; for more information, see 
         * the Remarks. The return value is zero for the following cases:

         * The current desktop is not the active desktop
         * The foreground thread belongs to another process and the desktop does not allow the hook or the journal record.
         */
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern short GetAsyncKeyState(int vkey);


        /*VARIABLES*/
        Process process = null;     //Used to get process ID
        IntPtr processHandleRead;   //Read Handle
        IntPtr processHandleWrite;  //Write Handle

        //Base Pointers
        UInt32 basePointer1 = 0;
        UInt32 basePointer2 = 0;

        byte[] buffer = new byte[4]; //General Use Buffer (4 elements for basic 32-bit address)
        int bytesRead = 0;           //Keep track of number of bytes read from process
        int bytesWritten = 0;        //Keep track of number of bytes written to process memory

        //Threads
        Thread hotKeyThread      = null;
        Thread freezeValueThread = null;


        //#####################################################################################################################################
        //GAME DEPENDENT VARS AND CONFIGS -- MODIFY HERE#######################################################################################
        //#####################################################################################################################################
        const string ABOUT_TRAINER = "     Created By: JWF 2014\n" +
                                     "  yrolg4tseuq@sbcglobal.net\n" +
                                     "Lands of Lore Guardians V_1.30";
        
        /*********************************************************BASE CONFIGS****************************************************************/
        const String PROCESS_NAME       = "dosbox";
        const String PROCESS_NAME_ALT   = "lolg";

        /*********************************************************BASE POINTER****************************************************************/
        const UInt32 BASE_PTR_ADR   = 0x00B95DF0;
        const UInt32 BASE_PTR_ADR2  = 0x00795DF4;

        /****************************************** Put a list of the pointer offsets here: **************************************************/
        const UInt32 FIGHTER_XP     = 0x101D06CC;  //4 BYTE  ADR
        const UInt32 MAGIC_XP       = 0x101D06D0;  //4 BYTE  ADR

        const UInt32 HP_CURRENT     = 0x101D06C0;  //4 BYTE  ADR
        const UInt32 HP_MAX         = 0x101D06BC;  //4 BYTE  ADR   SHOULD BE SET TO 54272
        const UInt32 MANA_CURRENT   = 0x101D06B8;  //4 BYTE  ADR
        const UInt32 MANA_MAX       = 0x101D06B4;  //4 BYTE  ADR   SHOULD BE SET TO 102656

        const UInt32 BEAST_MAGIC    = 0x101D071F;  //1 BYTE ADR  0:ENABLED ; 1:DISABLED

        const UInt32 DUPLICATE      = 0x101D15C8;  //4 BYTE

        //const UInt32 SHIFT_COUNT    = 0x101D067C;  //8 BYTE ADR (1 BYTE BUT SWITCHES OFF)  - SET TO 0 TO MORPH  
        //const UInt32 TYPE_SHIFT_LAST= 0x101D071C;  //1 BYTE ADR (1 BYTE BUT SWITCHES OFF) - 0:BEAST ; 1:HUMAN ; 2:LIZARD
        //const UInt32 TYPE_SHIFT_NOW = 0x101D0720;  //1 BYTE ADR (1 BYTE BUT SWITCHES OFF) - 0:BEAST ; 1:HUMAN ; 2:LIZARD

        const UInt32 SHOW_SPELLS    = 0x101D17CF;  //1 BYTE ADR2  SHOULD BE SET TO 0 (HEAL + SPARK = 61 ; HEAL + SPARK + SUMMON = 46 ; ETC.)
        const UInt32 ALL_SPELLS     = 0x101D17F9;  //1 BYTE ADR2  SHOULD BE SET TO 6 (HEAL + SPARK = 2 ; HEAL + SPARK + SUMMON = 3 ; ETC.)
        const UInt32 ALL_MORPH      = 0x101D1ABD;  //1 BYTE ADR2  SHOULD BE SET TO 255

        const UInt32 SKILLS_1       = 0x101D16DC;  //4 BYTE  ADR2
        const UInt32 SKILLS_2       = 0x101D16E0;  //4 BYTE  ADR2

        const UInt32 ANCIENT_MAGIC  = 0x101D1C3F;  //1 BYTE ADR2


        //const UInt32 PARRY_ADD      = 0x101D1759;  //1 BYTE
        //const UInt32 BOW_ADD        = 0x101D1755;  //1 BYTE
        //const UInt32 ARMOR_ADD      = 0x101D16F0;  //1 BYTE

        byte[] dup = { 0x00, 0x00, 0x00, 0x00 };  //Duplicate byte array here b/c only want to read once

        /********************************************* HOT KEY ENABLE/DISABLE FREEZING ********************************************************/
        /* These guys set a flag for our freezing value thread.  The hot key itself will just activate one of these flags, and then the freezing
         * value thread will read that flag and do the appropriate functionality.*/
        bool hotKeyEnable1 = false;
        bool hotKeyEnable2 = false;
        bool hotKeyEnable3 = false;
        bool hotKeyEnable4 = false;

        //#####################################################################################################################################
        //END OF: GAME DEPENDENT VARS AND CONFIGS #############################################################################################
        //#####################################################################################################################################


        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //FUNCTIONS PERTAINING TO FORM$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        public Form1()
        {
            //Initialize Form Components
            InitializeComponent();

            //Open Process via process name (use [0] to get first process in returned list)
            try
            {
                process = Process.GetProcessesByName(PROCESS_NAME)[0];
                //If process found, change text on trainer
                this.label7.Text = string.Format("PROCESS {0} FOUND!", PROCESS_NAME);
                this.label7.ForeColor = Color.Lime;
            }
            catch (Exception e)
            {
                //Do Nothing - Try Alternate Process Name
            }

            if (process == null)
            {
                try
                {
                    process = Process.GetProcessesByName(PROCESS_NAME_ALT)[0];
                    //If process found, change text on trainer
                    this.label7.Text = string.Format("PROCESS {0} FOUND!", PROCESS_NAME_ALT);
                    this.label7.ForeColor = Color.Lime;
                }
                catch (Exception e)
                {
                    //Print Error Mesage and EXIT
                    System.Console.WriteLine("Error: Invalid Process Name");
                    Environment.Exit(1);
                }
            }


            //Get Read and Write Handles
            processHandleRead = OpenProcess(PROCESS_WM_READ, false, process.Id);
            processHandleWrite = OpenProcess(0x1F0FFF, false, process.Id);


            /*Use Base Pointer Address to get the current base pointer value. Note: Buffer will
             * be updated with information read from memory.  It will be read exactly as it is 
             * placed in memory, which means we will be reading in BIG INDIAN format.  Essentialy,
             * the address will be 'backwards'. */

            //BasePointer 1
            ReadProcessMemory((int)processHandleRead, (int)BASE_PTR_ADR, buffer, buffer.Length, ref bytesRead);
            basePointer1 = reverseByteAryToHex(buffer);

            //BasePointer 2  -- Not all games have 2 base pointers, this one does
            ReadProcessMemory((int)processHandleRead, (int)BASE_PTR_ADR2, buffer, buffer.Length, ref bytesRead);
            basePointer2 = reverseByteAryToHex(buffer);


            /*Set up and start hotkey thread*/
            hotKeyThread = new Thread(new ThreadStart(this.HandleHotKeys));
            hotKeyThread.Start();

            /*Set up and start Freezing Values Thread*/
            freezeValueThread = new Thread(new ThreadStart(this.HandleFreezingValues));
            freezeValueThread.Start();
        }

        private void button1_Click(object sender, EventArgs e)  //About
        {
            System.Windows.Forms.MessageBox.Show(ABOUT_TRAINER);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //Stop extra threads
            hotKeyThread.Abort();
            freezeValueThread.Abort();

            //Call base funciton
            base.OnFormClosing(e);
        }


        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //END OF OTHER FUNCTIONS PERTAINING TO FORM$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$


        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //HELPER FUNCTIONS STARTS HERE$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        ///*Takes a reversed 4 byte array and converts it to a hex value (memory address)*/
        private UInt32 reverseByteAryToHex(byte[] ary)
        {
            UInt32 tempVal = 0;

            tempVal += (uint)(((int)buffer[3]) * 0x1000000);
            tempVal += (uint)(((int)buffer[2]) * 0x10000);
            tempVal += (uint)(((int)buffer[1]) * 0x100);
            tempVal += (uint)(((int)buffer[0]) * 0x1);

            return tempVal;
        }

        ///*Compares 2 byte arrays from element 0 to numElements - 1 .  Returns true if equal, false otherwise*/
        private bool compareByteArrays(byte[] ary1, byte[] ary2, UInt16 numElements)
        {
            for (int index = 0; index < numElements; index++)
            {
                if (ary1[index] != ary2[index])
                {
                    return false;
                }
            }

            return true;
        }

        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //END OF HELPER FUNCTIONS $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$


        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //HOTKEY CODE STARTS HERE $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        ///*This function will be called in a seperate thread. It will handle hot key presses*/
        public void HandleHotKeys()
        {
            //Allow up to 6 different hot keys
            short hotKeyResult0 = 0;
            short hotKeyResult1 = 0;
            short hotKeyResult2 = 0;
            short hotKeyResult3 = 0;
            short hotKeyResult4 = 0;
            short hotKeyResult5 = 0;

            //Infinite loop which will service hot key presses
            while(true)
            {
                //Find out if hotkey was pressed, if so, call appropriate function

                hotKeyResult0 = GetAsyncKeyState((int)(Keys.F5));
                hotKeyResult1 = GetAsyncKeyState((int)(Keys.F6));
                hotKeyResult2 = GetAsyncKeyState((int)(Keys.F7));
                hotKeyResult3 = GetAsyncKeyState((int)(Keys.F8));
                hotKeyResult4 = GetAsyncKeyState((int)(Keys.F9));
                hotKeyResult5 = GetAsyncKeyState((int)(Keys.F10));

                if      ((hotKeyResult0 & 0x01) == 0x01) { hotKey0Fun(); }
                else if ((hotKeyResult1 & 0x01) == 0x01) { hotKey1Fun(); }
                else if ((hotKeyResult2 & 0x01) == 0x01) { hotKey2Fun(); }
                else if ((hotKeyResult3 & 0x01) == 0x01) { hotKey3Fun(); }
                else if ((hotKeyResult4 & 0x01) == 0x01) { hotKey4Fun(); }
                else if ((hotKeyResult5 & 0x01) == 0x01) { hotKey5Fun(); }

                else { }

                //Let this thread sleep a while (GREATLY REDUCES CPU COST!!!)
                Thread.Sleep(10);
            }
            
        }

        private void hotKey0Fun()  //SUPER XP
        {
            //Prepare the needed addresses
            UInt32 fighterXP = basePointer1 + FIGHTER_XP;
            UInt32 magicXP = basePointer1 + MAGIC_XP;

            //Clear off any extra bytes from offset!
            fighterXP = (fighterXP & 0xFFFFFFFF);
            magicXP = (magicXP & 0xFFFFFFFF);

            //Prepare buffer to write to memory
            byte[] xpAry = { 0x00, 0xFF, 0xFF, 0xFF }; //Remember to put this in big indian format

            //Attempt to write to memory
            UInt16 failSafeExit = 0;

            while (failSafeExit < MAX_WRITE_ATTEMPTS)
            {
                //Write to memory
                WriteProcessMemory((int)processHandleWrite, (int)fighterXP, xpAry, xpAry.Length, ref bytesWritten);

                //Did the value get written?
                ReadProcessMemory((int)processHandleRead, (int)fighterXP, buffer, xpAry.Length, ref bytesRead);

                if (compareByteArrays(xpAry, buffer, 4))
                {
                    break;  //Successfully written
                }
                else
                {
                    failSafeExit++;  //Not successful, try again.
                }
            }

            System.Console.WriteLine("Failsafe Count Write Fighter XP: {0}", failSafeExit);
            failSafeExit = 0;

            while (failSafeExit < MAX_WRITE_ATTEMPTS)
            {
                //Write to memory
                WriteProcessMemory((int)processHandleWrite, (int)magicXP, xpAry, xpAry.Length, ref bytesWritten);

                //Did the value get written?
                ReadProcessMemory((int)processHandleRead, (int)magicXP, buffer, xpAry.Length, ref bytesRead);

                if (compareByteArrays(xpAry, buffer, 4))
                {
                    break;  //Successfully written
                }
                else
                {
                    failSafeExit++;  //Not successful, try again.
                }
            }

            System.Console.WriteLine("Failsafe Count Write Magic XP: {0}", failSafeExit);

        }

        private void hotKey1Fun()  //ALL SPELLS
        {
            
            //Prepare the needed addresses
            UInt32 allSpells      = basePointer2 + ALL_SPELLS;
            UInt32 allSpellsShow  = basePointer2 + SHOW_SPELLS;
            UInt32 allMorphLevels = basePointer2 + ALL_MORPH;

            //Clear off any extra bytes from offset!
            allSpells      = (allSpells & 0xFFFFFFFF);
            allSpellsShow  = (allSpellsShow & 0xFFFFFFFF);
            allMorphLevels = (allMorphLevels & 0xFFFFFFFF);

            //Prepare buffer to write to memory
            byte[] magicAry  = {0x06}; //AllSpells
            byte[] magicAry2 = {0x00}; //ShowAllSpells
            byte[] magicAry3 = {0xFF}; //AllMorph

            //Attempt to write to memory
            UInt16 failSafeExit = 0;

            while (failSafeExit < MAX_WRITE_ATTEMPTS)
            {
                //Write to memory
                WriteProcessMemory((int)processHandleWrite, (int)allSpells, magicAry, magicAry.Length, ref bytesWritten);

                //Did the value get written?
                ReadProcessMemory((int)processHandleRead, (int)allSpells, buffer, magicAry.Length, ref bytesRead);

                if (compareByteArrays(magicAry, buffer, 1))
                {
                    break;  //Successfully written
                }
                else
                {
                    failSafeExit++;  //Not successful, try again.
                }
            }

            System.Console.WriteLine("Failsafe Count Write All Spells: {0}", failSafeExit);
            failSafeExit = 0;

            while (failSafeExit < MAX_WRITE_ATTEMPTS)
            {
                //Write to memory
                WriteProcessMemory((int)processHandleWrite, (int)allSpellsShow, magicAry2, magicAry2.Length, ref bytesWritten);

                //Did the value get written?
                ReadProcessMemory((int)processHandleRead, (int)allSpellsShow, buffer, magicAry2.Length, ref bytesRead);

                if (compareByteArrays(magicAry2, buffer, 1))
                {
                    break;  //Successfully written
                }
                else
                {
                    failSafeExit++;  //Not successful, try again.
                }
            }

            System.Console.WriteLine("Failsafe Count Write Show All Spells: {0}", failSafeExit);
            failSafeExit = 0;

            while (failSafeExit < MAX_WRITE_ATTEMPTS)
            {
                //Write to memory
                WriteProcessMemory((int)processHandleWrite, (int)allMorphLevels, magicAry3, magicAry3.Length, ref bytesWritten);

                //Did the value get written?
                ReadProcessMemory((int)processHandleRead, (int)allMorphLevels, buffer, magicAry3.Length, ref bytesRead);

                if (compareByteArrays(magicAry3, buffer, 1))
                {
                    break;  //Successfully written
                }
                else
                {
                    failSafeExit++;  //Not successful, try again.
                }
            }

            System.Console.WriteLine("Failsafe Count Write All Morph Levels: {0}", failSafeExit);

        }

        private void hotKey2Fun()  //SUPER LUTHER
        {
            if (hotKeyEnable1 == false)
                hotKeyEnable1 = true;
            else
                hotKeyEnable1 = false;

            //Code handled in Frozen Value Code Section
        }

        private void hotKey3Fun()  //FREEZE HP/MANA
        {
            if (hotKeyEnable2 == false)
                hotKeyEnable2 = true;
            else
                hotKeyEnable2 = false;

            //Code handled in Frozen Value Code Section
        }

        private void hotKey4Fun()  //BEAST CAN USE MAGIC
        {
            UInt32 beastMagic = basePointer1 + BEAST_MAGIC;
            beastMagic = (beastMagic & 0xFFFFFFFF);

            byte[] beastM = { 0x00 };

            UInt16 failSafeExit = 0;

            while (failSafeExit < MAX_WRITE_ATTEMPTS)
            {
                //Write to memory
                WriteProcessMemory((int)processHandleWrite, (int)beastMagic, beastM, 1, ref bytesWritten);

                //Did the value get written?
                ReadProcessMemory((int)processHandleRead, (int)beastMagic, buffer, 1, ref bytesRead);

                if (compareByteArrays(beastM, buffer, 1))
                {
                    break;  //Successfully written
                }
                else
                {
                    failSafeExit++;  //Not successful, try again.
                }
            }

            System.Console.WriteLine("Failsafe Count Write Beast Magic: {0}", failSafeExit);
        }

        private void hotKey5Fun()
        {
#if false
            if (hotKeyEnable4 == false)
            {
                UInt32 duplicate = basePointer1 + DUPLICATE;
                duplicate = (duplicate & 0xFFFFFFFF);
                ReadProcessMemory((int)processHandleRead, (int)duplicate, dup, 4, ref bytesRead);
                hotKeyEnable4 = true;
            }
            else
                hotKeyEnable4 = false;

            //Code handled in Frozen Value Code Section
#endif
        }

        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //END OF HOTKEY CODE$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$


        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //FROZEN VALUES CODE$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        /*If we desire to freeze values, we need to, in a seperate thread, loop around and reset the values.  Place functions
         * here that we desire to call depending on flag statuses */

        public void HandleFreezingValues()
        {
            /*Initializations*/

            //Super Luther (hotKeyEnable1) 
            UInt32 skills1 = basePointer2 + SKILLS_1;
            UInt32 skills2 = basePointer2 + SKILLS_2;
            UInt32 ancient = basePointer2 + ANCIENT_MAGIC;

            skills1 = (skills1 & 0xFFFFFFFF);
            skills2 = (skills2 & 0xFFFFFFFF);
            ancient = (ancient & 0xFFFFFFFF);

            byte[] skillAry = { 0xFF, 0x00, 0x00, 0xFF };
            byte[] ancientAry = { 0x09 };
            //End Super Luther Init.


            //HP/MANA FREEZE (hotKeyEnable2)
            UInt32 healthMax = basePointer1 + HP_MAX;
            UInt32 health    = basePointer1 + HP_CURRENT;
            UInt32 manaMax   = basePointer1 + MANA_MAX;
            UInt32 mana      = basePointer1 + MANA_CURRENT;

            healthMax = (healthMax & 0xFFFFFFFF);
            health    = (health & 0xFFFFFFFF);
            manaMax   = (manaMax & 0xFFFFFFFF);
            mana      = (mana & 0xFFFFFFFF);

            byte[] hp = { 0x00, 0xD4, 0x00, 0x00 };
            byte[] mp = { 0x00, 0x91, 0x01, 0x00 };
            //End HP/MANA freeze


            //DUPLICATE
            UInt32 duplicate = basePointer1 + DUPLICATE;

            duplicate = (duplicate & 0xFFFFFFFF);
            //End Duplicate


            
            /*Frozen Thread Loop*/
            while (true)
            {
                if(hotKeyEnable1) //Super Luther
                {
                    /*SKILLS*/
                    WriteProcessMemory((int)processHandleWrite, (int)skills1, skillAry, skillAry.Length, ref bytesWritten);
                    WriteProcessMemory((int)processHandleWrite, (int)skills2, skillAry, 1, ref bytesWritten);

                    /*ANCIENT MAGIC*/
                    WriteProcessMemory((int)processHandleWrite, (int)ancient, ancientAry, ancientAry.Length, ref bytesWritten);
                }

                if(hotKeyEnable2) //Freeze Health and Mana
                {
                    WriteProcessMemory((int)processHandleWrite, (int)healthMax, hp, 4, ref bytesWritten);
                    WriteProcessMemory((int)processHandleWrite, (int)health, hp, 4, ref bytesWritten);
                    WriteProcessMemory((int)processHandleWrite, (int)mana, mp, 4, ref bytesWritten);
                    WriteProcessMemory((int)processHandleWrite, (int)manaMax, mp, 4, ref bytesWritten);
                }

                if (hotKeyEnable4)  //Duplicate Inventory Slot 1
                {
                    WriteProcessMemory((int)processHandleWrite, (int)duplicate, dup, 4, ref bytesWritten);
                }



                //Rest the thread
                Thread.Sleep(100);

            }
        }

        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //END OF FROZEN VALUES CODE$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

    }
}
