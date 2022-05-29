using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrydayProject
{
    public partial class Emulight : Form
    {
        public Emulight()
        {
            InitializeComponent();
        }
        bool TopLightLampState = true;  // true = work, false = not work
        bool SideLightLampState = true;
        bool BackLightLampState = true;

        public int switchLightTop(bool State)
        {
            if (CheckTopLamp())
                if (State)  // switch on
                {
                    if (panel2.BackColor == Color.Yellow) return 1; // light is already on
                    else panel2.BackColor = Color.Yellow;
                }
                else
                {
                    if (panel2.BackColor == Color.WhiteSmoke) return 2; // light is already off
                    else panel2.BackColor = Color.WhiteSmoke;
                }
                else return 3; // lamp broken and can't switch on
            return 0;
        }
        public int switchLightBottom(bool State)
        {
            if (CheckBottomLamp())
                if (State)
                {
                    if (panel1.BackColor == Color.Yellow) return 1; // light is already on
                    else panel1.BackColor = Color.Yellow;
                }
                else
                {
                    if (panel1.BackColor == Color.WhiteSmoke) return 2; // light is already off
                    else panel1.BackColor = Color.WhiteSmoke;
                }
            else  return 3; // lamp broken and can't switch on
            return 0;
        }
        public int switchLightSide(bool State)
        {
            if (CheckBottomLamp())
                if (State)
                {
                    if (panel3.BackColor == Color.Yellow) return 1; // light is already on
                    else panel3.BackColor = panel4.BackColor = panel5.BackColor = panel6.BackColor = panel7.BackColor = panel8.BackColor = Color.Yellow;
                }
                else
                {
                    if (panel3.BackColor == Color.WhiteSmoke) return 2; // light is already off
                    else panel3.BackColor = panel4.BackColor = panel5.BackColor = panel6.BackColor = panel7.BackColor = panel8.BackColor = Color.WhiteSmoke;
                }
            else return 3; // lamp broken and can't switch on
            return 0;
        }

        public bool CheckTopLamp() {
            return TopLightLampState;
        }
        public bool CheckSideLamp() {
            return SideLightLampState;
        }
        public bool CheckBottomLamp() {
            return BackLightLampState;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            switchLightTop(true);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            switchLightTop(false);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            switchLightSide(true);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            switchLightSide(false);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            switchLightBottom(true);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            switchLightBottom(false);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!TopLightLampState) panel2.BackColor = Color.Red;    
            if (!SideLightLampState) panel3.BackColor = panel4.BackColor = panel5.BackColor = panel6.BackColor = panel7.BackColor = panel8.BackColor = Color.Red;
            if (!BackLightLampState) panel1.BackColor = Color.Red;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TopLightLampState = false;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            SideLightLampState = false;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            BackLightLampState = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            TopLightLampState = true;
            panel2.BackColor = Color.WhiteSmoke;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SideLightLampState = true;
            panel3.BackColor = panel4.BackColor = panel5.BackColor = panel6.BackColor = panel7.BackColor = panel8.BackColor = Color.WhiteSmoke;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            BackLightLampState = true;
            panel1.BackColor = Color.WhiteSmoke;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
