using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using ControlIni;

namespace Justech
{
    public partial class FrmMotion : Form
    {
        GClsMethod myMethod = new GClsMethod();//新建保存修改参数值的类实例       
        public int axisID;//当前轴号     
        public double pulseAxis;
        public double mannualSpeed;
        Ini iniMontion = new Ini();//专用
        public FrmMotion()
        {
            InitializeComponent();
        }
        private void FrmMontion_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            axisID = 0;
            foreach (TabPage page in tabControl1.TabPages)//combox的item选择0
            {
                foreach (Control ctl in page.Controls)
                {
                    if (ctl is ComboBox)
                    {
                        ((ComboBox)ctl).SelectedIndex = 0;
                    }
                }
            }
            iniMontion.filePath = CAMiClsVariable.montionIniPath;
            txtCCD1GapX.Text = iniMontion.ReadIni("间距", "CCD1间距X");
           
            txtCCD1GapY.Text = iniMontion.ReadIni("间距", "CCD1间距Y");
            
            txtCCD2GapX1.Text = iniMontion.ReadIni("间距", "CCD2间距X1");
            txtCCD2GapX2.Text = iniMontion.ReadIni("间距", "CCD2间距X2");
            txtCCD2GapY.Text = iniMontion.ReadIni("间距", "CCD1间距Y");
            txtCCD1TeachPosX.Text = iniMontion.ReadIni("CCD1-X轴", "初始位置");
            txtCCD1TeachPosY.Text = iniMontion.ReadIni("CCD1-Y轴", "初始位置");
            txtCCD2TeachPosX.Text = iniMontion.ReadIni("CCD2-X轴", "初始位置");
            txtCCD2TeachPosY.Text = iniMontion.ReadIni("CCD2-Y轴", "初始位置");
            txtCCD2TeachPosZ.Text = iniMontion.ReadIni("CCD2-Z轴", "初始位置");
            txtCCD1SpeedX.Text = iniMontion.ReadIni("速度", "CCD1-X速度");
            txtCCD1SpeedY.Text = iniMontion.ReadIni("速度", "CCD1-Y速度");
            txtCCD2SpeedX.Text = iniMontion.ReadIni("速度", "CCD2-X速度");
            txtCCD2SpeedY.Text = iniMontion.ReadIni("速度", "CCD2-Y速度");
            txtCCD2SpeedZ.Text = iniMontion.ReadIni("速度", "CCD2-Z速度");
            txtCCD1TaccX.Text = iniMontion.ReadIni("速度", "CCD1-X加减速");
            txtCCD1TaccY.Text = iniMontion.ReadIni("速度", "CCD1-Y加减速");
            txtCCD2TaccX.Text = iniMontion.ReadIni("速度", "CCD2-X加减速");
            txtCCD2TaccY.Text = iniMontion.ReadIni("速度", "CCD2-Y加减速");
            txtCCD2TaccZ.Text = iniMontion.ReadIni("速度", "CCD2-Z加减速");
        }

        #region 界面加载
        //private void btnServoOn_Click(object sender, EventArgs e)//激磁
        //{            
        //    int axisServoOnStatus;
        //    MC1104.jmc_mc1104_get_servo(axisID, out axisServoOnStatus);//获取轴的激磁状态
        //    if (axisServoOnStatus == 0)
        //    {               
        //        ClsMontion.ServoOn(axisID, 1);
        //    }
        //    else
        //    {                
        //        ClsMontion.ServoOn(axisID, 0);
        //    }
        //}
        private void lblServo_Click(object sender, EventArgs e)
        {
            int axisServoOnStatus;
            MC1104.jmc_mc1104_get_servo(axisID, out axisServoOnStatus);//获取轴的激磁状态
            if (axisServoOnStatus == 0)
            {
                GClsMontion.ServoOn(axisID, 1);
            }
            else
            {
                GClsMontion.ServoOn(axisID, 0);
            }
        }

        private void cobAxisNo_SelectedIndexChanged(object sender, EventArgs e)//轴号改变
        {
            switch (cobAxisNo.SelectedIndex)
            {
                case 0:
                    axisID = 0;
                    pulseAxis = CAMiClsVariable.pulseAxis0;
                    break;
                case 1:
                    axisID = 1;
                    pulseAxis = CAMiClsVariable.pulseAxis1;
                    break;
                case 2:
                    axisID = 2;
                    pulseAxis = CAMiClsVariable.pulseAxis2;
                    break;
                case 3:
                    axisID = 3;
                    pulseAxis = CAMiClsVariable.pulseAxis3;
                    break;
                case 4:
                    axisID = 4;
                    pulseAxis = CAMiClsVariable.pulseAxis4;
                    break;
            }
        }
        private void txtSpeed_TextChanged(object sender, EventArgs e)//速度变化
        {
            tbSpeed.Value = Convert.ToInt32(txtSpeed.Text.Trim());
        }
        private void tbSpeed_ValueChanged(object sender, EventArgs e)//速度变化
        {
            txtSpeed.Text = tbSpeed.Value.ToString();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //读各轴位置
            double positionAxis = 0;//各轴反馈的脉冲值
            double axisRatio = 0;//惯量比
            int tagAxis;//得到轴号            
            foreach (var textbox in groupCurrentPos.Controls.OfType<TextBox>())
            {
                tagAxis = Convert.ToInt32(textbox.Tag);
                switch (tagAxis)
                {
                    case 0:
                        axisRatio = CAMiClsVariable.pulseAxis0;
                        break;
                    case 1:
                        axisRatio = CAMiClsVariable.pulseAxis1;
                        break;
                    case 2:
                        axisRatio = CAMiClsVariable.pulseAxis2;
                        break;
                    case 3:
                        axisRatio = CAMiClsVariable.pulseAxis3;
                        break;
                    case 4:
                        axisRatio = CAMiClsVariable.pulseAxis4;
                        break;
                }
                mannualSpeed = Convert.ToDouble(txtSpeed.Text.Trim()) / pulseAxis;
                //MC1104.jmc_mc1104_get_position(tagAxis, out positionAxis);                
                MC1104.jmc_mc1104_get_command(tagAxis, out positionAxis);//-
                textbox.Text = (positionAxis * axisRatio).ToString("f2");//*(-1)
            }
            switch (tabControl1.SelectedTab.Name)
            {
                // bit0:RDY ; bit1:ALM ; bit2:PEL ; bit3:MEL ; bit4:ORG ; bit5:DIR ; bit6:EMG ; bit7:
                //bit8:ERC ; bit9:EZ ; bit10:CLR ; bit11:SD ; bit12: ; bit13:INP ; bit14:SVON      
                case "tabMontion":
                    int montionIoStatus;//运动轴的IO状态
                    MC1104.jmc_mc1104_get_io_status(axisID, out montionIoStatus);
                    foreach (var lbl in grbMontionIo.Controls.OfType<Label>())
                    {
                        lbl.BackColor = ((montionIoStatus >> Convert.ToInt32(lbl.Tag)) & 1) == 1 ? Color.LawnGreen : Color.DimGray;   //根据轴上IO获取的状态，改变label的背景颜色
                        if (Convert.ToInt32(lbl.Tag) == 1 & lbl.BackColor == Color.LawnGreen)
                        {
                            lbl.BackColor = Color.DimGray;
                        }
                    }
                    break;
                case "tabMontionIO1":
                    #region 第一张卡IO读
                    ////读IO状态
                    //CARD DI                   
                    bool firstCardDiStatus;
                    int firstIndex;
                    foreach (var label in grbFirstCardDI.Controls.OfType<Label>())
                    {
                        firstIndex = Convert.ToInt32(label.Tag);
                        firstCardDiStatus = GClsMontion.ReadCardInputBit(0, firstIndex) != 0;
                        label.BackColor = firstCardDiStatus ? Color.LawnGreen : Color.DimGray;
                    }
                    //CARD D0                    
                    bool firstCardDoStatus;
                    foreach (var label in grbFirstCardDO.Controls.OfType<Label>())
                    {
                        firstIndex = Convert.ToInt32(label.Tag);
                        firstCardDoStatus = GClsMontion.ReadCardOutputBit(0, firstIndex) != 0;
                        label.BackColor = firstCardDoStatus ? Color.LawnGreen : Color.DimGray;
                    }
                    //General Input                   
                    bool firstExtendInputStatus;
                    foreach (var label in grbFirstExtendDi.Controls.OfType<Label>())
                    {
                        firstIndex = Convert.ToInt32(label.Tag);
                        firstExtendInputStatus = GClsMontion.ReadCardExtendInputBit(0, firstIndex) != 0;
                        label.BackColor = firstExtendInputStatus ? Color.LawnGreen : Color.DimGray;
                    }
                    //General Output                   
                    bool firstExtendOutputStatus;
                    foreach (var label in grbFirstExtendDo.Controls.OfType<Label>())
                    {
                        firstIndex = Convert.ToInt32(label.Tag);
                        firstExtendOutputStatus = GClsMontion.ReadCardExtendOutputBit(0, firstIndex) != 0;
                        label.BackColor = firstExtendOutputStatus ? Color.LawnGreen : Color.DimGray;
                    }
                    #endregion
                    break;
                case "tabMontionIO2":
                    #region 第二张卡IO读
                    ////读IO状态
                    //CARD DI                   
                    bool secondCardDiStatus;
                    int secondIndex;
                    foreach (var label in grbSecondCardDI.Controls.OfType<Label>())
                    {
                        secondIndex = Convert.ToInt32(label.Tag);
                        secondCardDiStatus = GClsMontion.ReadCardInputBit(1, secondIndex) != 0;
                        label.BackColor = secondCardDiStatus ? Color.LawnGreen : Color.DimGray;
                    }
                    //CARD D0                    
                    bool secondCardDoStatus;
                    foreach (var label in grbSecondCardDO.Controls.OfType<Label>())
                    {
                        secondIndex = Convert.ToInt32(label.Tag);
                        secondCardDoStatus = GClsMontion.ReadCardOutputBit(1, secondIndex) != 0;
                        label.BackColor = secondCardDoStatus ? Color.LawnGreen : Color.DimGray;
                    }
                    //General Input                   
                    bool secondExtendInputStatus;
                    foreach (var label in grbSecondExtendDi.Controls.OfType<Label>())
                    {
                        secondIndex = Convert.ToInt32(label.Tag);
                        secondExtendInputStatus = GClsMontion.ReadCardExtendInputBit(1, secondIndex) != 0;
                        label.BackColor = secondExtendInputStatus ? Color.LawnGreen : Color.DimGray;
                    }
                    //General Output                   
                    bool secondExtendOutputStatus;
                    foreach (var label in grbSecondExtendDo.Controls.OfType<Label>())
                    {
                        secondIndex = Convert.ToInt32(label.Tag);
                        secondExtendOutputStatus = GClsMontion.ReadCardExtendOutputBit(1, secondIndex) != 0;
                        label.BackColor = secondExtendOutputStatus ? Color.LawnGreen : Color.DimGray;
                    }
                    break;
                    #endregion
                case "tabExtendIO7432":
                    #region 7432卡或APE IO读
                    int index7432;
                    bool status7432;
                    uint value7432;
                    if (CAMiClsVariable.strIOCard == "7432")
                    {
                        //刷新DI端口界面
                        DASK.DI_ReadPort((ushort)CAMiClsVariable.cardRegId, 0, out value7432);
                        foreach (var label in grbCard7432Di.Controls.OfType<Label>())
                        {
                            if (label.BorderStyle == BorderStyle.FixedSingle)
                            {
                                index7432 = Convert.ToInt32(label.Tag);
                                status7432 = (value7432 & (1 << index7432)) != 0;
                                label.BackColor = status7432 ? System.Drawing.Color.LawnGreen : System.Drawing.Color.DimGray;
                            }
                        }
                        //刷新DO界面
                        DASK.DO_ReadPort((ushort)CAMiClsVariable.cardRegId, 0, out value7432);
                        foreach (var label in grbCard7432Do.Controls.OfType<Label>())
                        {
                            index7432 = Convert.ToInt32(label.Tag);
                            status7432 = (value7432 & (1 << index7432)) != 0;
                            label.BackColor = status7432 ? System.Drawing.Color.LawnGreen : System.Drawing.Color.DimGray;
                        }
                    }
                    else//APE IO卡
                    {
                        //刷新DI端口界面
                        foreach (var label in grbCard7432Di.Controls.OfType<Label>())
                        {
                            index7432 = Convert.ToInt32(label.Tag);
                            status7432 = GClsMontion.ReadAPEIOCardInputBit(0, index7432) != 0;
                            label.BackColor = status7432 ? Color.LawnGreen : Color.DimGray;
                        }
                        //刷新DO界面
                        foreach (var label in grbCard7432Do.Controls.OfType<Label>())
                        {
                            index7432 = Convert.ToInt32(label.Tag);
                            status7432 = GClsMontion.ReadAPEIOCardOutputBit(0, index7432) != 0;
                            label.BackColor = status7432 ? Color.LawnGreen : Color.DimGray;
                        }
                    }
                    break;
                    #endregion
            }
            Thread.Sleep(10);
            Application.DoEvents();
        }
        #endregion
        #region 输出点位 Set or Reset
        private void lblFirstCard4DO_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            int index = Convert.ToInt32(label.Tag);
            int cardDoCurrentValue = GClsMontion.ReadCardOutputBit(0, index);
            if (cardDoCurrentValue == 1)
            {
                GClsMontion.WriteCardOutputBit(0, index, 0);
            }
            else
            {
                GClsMontion.WriteCardOutputBit(0, index, 1);
            }
        }

        private void lblFirstExtendDO_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            int indexDo = Convert.ToInt32(label.Tag);
            int currentCardStatus = GClsMontion.ReadCardExtendOutputBit(0, indexDo);//得到当前输出点的状态
            if (currentCardStatus == 1)//当前输出点状态为ON
            {
                GClsMontion.WriteCardExtendOutputBit(0, indexDo, 0);// 设置 输出点  Reset
            }
            else
            {
                GClsMontion.WriteCardExtendOutputBit(0, indexDo, 1);// 设置 输出点   Set 
            }
        }

        private void lblSecondCard4DO_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            int index = Convert.ToInt32(label.Tag);
            int cardDoCurrentValue = GClsMontion.ReadCardOutputBit(1, index);
            if (cardDoCurrentValue == 1)
            {
                GClsMontion.WriteCardOutputBit(1, index, 0);
            }
            else
            {
                GClsMontion.WriteCardOutputBit(1, index, 1);
            }
        }

        private void lblSecondExtendDO_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            int indexDo = Convert.ToInt32(label.Tag);
            int currentCardStatus = GClsMontion.ReadCardExtendOutputBit(1, indexDo);//得到当前输出点的状态
            if (currentCardStatus == 1)//当前输出点状态为ON
            {
                GClsMontion.WriteCardExtendOutputBit(1, indexDo, 0);// 设置 输出点  Reset
            }
            else
            {
                GClsMontion.WriteCardExtendOutputBit(1, indexDo, 1);// 设置 输出点   Set 
            }
        }
        private void lblIO7432CardDO_Click(object sender, EventArgs e)//7432
        {
            Label label = (Label)sender;
            if (CAMiClsVariable.strIOCard == "7432")
            {
                ushort index = Convert.ToUInt16(label.Tag);
                ushort value;
                DASK.DO_ReadLine((ushort)CAMiClsVariable.cardRegId, 0, index, out value);
                value = value > 0 ? (ushort)0 : (ushort)1;    //取反
                DASK.DO_WriteLine((ushort)CAMiClsVariable.cardRegId, 0, index, value);
            }
            else//APE IO卡
            {
                int indexDo = Convert.ToInt32(label.Tag);
                int currentAPEIOCardStatus = GClsMontion.ReadAPEIOCardOutputBit(0, indexDo);//得到当前输出点的状态
                if (currentAPEIOCardStatus == 1)//当前输出点状态为ON
                {
                    GClsMontion.WriteAPEIOCardOutputBit(0, indexDo, 0);// 设置 输出点  Reset
                }
                else
                {
                    GClsMontion.WriteAPEIOCardOutputBit(0, indexDo, 1);// 设置 输出点   Set 
                }
            }
        }
        #endregion
        #region 运动控制部分
        private void btnCW_MouseDown(object sender, MouseEventArgs e)//CW JOG动
        {
            GClsMontion.ContinueMove(axisID, mannualSpeed, Convert.ToDouble(txtAccelerate.Text.Trim()));
        }
        private void btnCW_MouseUp(object sender, MouseEventArgs e)//停止jog动
        {
            GClsMontion.StopAxis(axisID, Convert.ToDouble(txtAccelerate.Text.Trim()));
        }
        private void btnCCW_MouseDown(object sender, MouseEventArgs e)//CCW jog动
        {
            GClsMontion.ContinueMove(axisID, -mannualSpeed, Convert.ToDouble(txtAccelerate.Text.Trim()));
        }
        private void btnCCW_MouseUp(object sender, MouseEventArgs e)//停止jog动
        {
            GClsMontion.StopAxis(axisID, Convert.ToDouble(txtAccelerate.Text.Trim()));
        }
        private void btnAbsoluteMove_Click(object sender, EventArgs e)//绝对移动
        {
            DialogResult result = MessageBox.Show("请确定是否绝对运动？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                double acc = Convert.ToDouble(txtAccelerate.Text);
                double maxSpeed = mannualSpeed;
                double distance = Convert.ToDouble(txtAbsoluteDis.Text) / pulseAxis;
                GClsMontion.AbsoluteMove(axisID, distance, maxSpeed, acc);
                GClsMontion.WaitMotorStop(axisID);
                MessageBox.Show("绝对运动完成！");
            }
        }
        private void btnRelativeMove_Click(object sender, EventArgs e)//相对移动
        {
            DialogResult result = MessageBox.Show("请确定是否相对运动？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                double acc = Convert.ToDouble(txtAccelerate.Text);
                double maxSpeed = mannualSpeed;
                double distance = Convert.ToDouble(txtRelativeDis.Text) / pulseAxis;
                GClsMontion.RelativeMove(axisID, distance, maxSpeed, acc);
                GClsMontion.WaitMotorStop(axisID);
                MessageBox.Show("相对运动完成！");
            }
        }
        private void btnAxisGoHome_Click(object sender, EventArgs e)//回原点
        {
            DialogResult result = MessageBox.Show("请确定是否可以回原点？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                double acc = Convert.ToDouble(txtAccelerate.Text);
                double maxSpeed = mannualSpeed;
                GClsMontion.GoHome(axisID, maxSpeed, acc);
                GClsMontion.GoHomeJudge(axisID);
                int ioORGStatus;//运动轴的IO状态
                MC1104.jmc_mc1104_get_io_status(axisID, out ioORGStatus);
                if ((ioORGStatus & 16) == 16)
                {
                    MessageBox.Show("轴" + axisID + "回原点成功！");
                }
                else
                {
                    MessageBox.Show("轴" + axisID + "回零失败！");
                }
            }
        }
        private void btnStopAxis_Click(object sender, EventArgs e)//停止轴动
        {
            GClsMontion.StopAxis(axisID, Convert.ToDouble(txtAccelerate.Text.Trim()));
        }
        #endregion
        #region   GoTo                                         */
        private void AxisGoHome_Click(object sender, EventArgs e)//各轴回原点
        {
            Button button = (Button)sender;
            int index = Convert.ToInt32(button.Tag);
            DialogResult result = MessageBox.Show("请确定是否可以 go home？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                double acc = 0.2;
                double maxSpeed = 5000;
                GClsMontion.GoHome(index, maxSpeed, acc);
                int ioORGStatus;//运动轴的IO状态
                MC1104.jmc_mc1104_get_io_status(index, out ioORGStatus);
                if ((ioORGStatus & 16) == 16)
                {
                    MessageBox.Show("轴" + index + "回原点成功！");
                }
                else
                {
                    MessageBox.Show("轴" + index + "回零失败！");
                }
            }
        }
        double speedCCD1XGoto = 10000, speedCCD1YGoto = 10000, speedCCD2XGoto = 10000, speedCCD2YGoto = 10000, speedCCD2ZGoto = 10000;
        double TaccCCD1X = 0.2, TaccCCD1Y = 0.2, TaccCCD2X = 0.2, TaccCCD2Y = 0.2, TaccCCD2Z = 0.2;
        private void btnCCD1XGoTo_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否运动到指定位置？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD1PosX.Text == "初始位置")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "初始位置")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置1")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置1")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置2")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置2")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置3")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置3")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置4")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置4")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置5")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置5")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置6")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置6")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置7")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置7")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置8")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置8")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置9")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置9")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置10")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置10")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置11")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置11")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置12")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置12")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置13")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置13")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置14")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置14")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置15")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置15")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置16")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置16")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置17")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置17")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置18")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置18")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置19")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置19")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置20")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置20")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置21")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置21")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置22")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置22")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置23")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置23")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
                else if (cobCCD1PosX.Text == "拍照位置24")
                {
                    GClsMontion.AbsoluteMove(0, Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置24")) / CAMiClsVariable.pulseAxis0, speedCCD1XGoto, TaccCCD1X);
                    GClsMontion.WaitMotorStop(0);
                }
            }
        }

        private void btnCCD1YGoTo_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否运动到指定位置？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD1PosY.Text == "初始位置")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "初始位置")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置1")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置1")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置2")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置2")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置3")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置3")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置4")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置4")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置5")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置5")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置6")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置6")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置7")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置7")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置8")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置8")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置9")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置9")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置10")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置10")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置11")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置11")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置12")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置12")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置13")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置13")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置14")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置14")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置15")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置15")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置16")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置16")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置17")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置17")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置18")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置18")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置19")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置19")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置20")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置20")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置21")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置21")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置22")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置22")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置23")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置23")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
                else if (cobCCD1PosY.Text == "拍照位置24")
                {
                    GClsMontion.AbsoluteMove(1, Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置24")) / CAMiClsVariable.pulseAxis1, speedCCD1YGoto, TaccCCD1Y);
                    GClsMontion.WaitMotorStop(1);
                }
            }
        }

        private void btnCCD2XGoTo_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否运动到指定位置？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD2PosX.Text == "初始位置")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "初始位置")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置1")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置1")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置2")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置2")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置3")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置3")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置4")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置4")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置5")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置5")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置6")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置6")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置7")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置7")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置8")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置8")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置9")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置9")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置10")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置10")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置11")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置11")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
                else if (cobCCD2PosX.Text == "拍照位置12")
                {
                    GClsMontion.AbsoluteMove(2, Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置12")) / CAMiClsVariable.pulseAxis2, speedCCD2XGoto, TaccCCD2X);
                    GClsMontion.WaitMotorStop(2);
                }
            }
        }

        private void btnCCD2YGoTo_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否运动到指定位置并且Z轴在安全高度？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD2PosY.Text == "初始位置")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "初始位置")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "抓料位置")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "抓料位置")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置1")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置1")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置2")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置2")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置3")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置3")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置4")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置4")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置5")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置5")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置6")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置6")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置7")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置7")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置8")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置8")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置9")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置9")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置10")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置10")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置11")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置11")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "拍照位置12")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置12")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
                else if (cobCCD2PosY.Text == "抛料位置")
                {
                    GClsMontion.AbsoluteMove(3, Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "抛料位置")) / CAMiClsVariable.pulseAxis3, speedCCD2YGoto, TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                }
            }
        }

        private void btnCCD2ZGoTo_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否运动到指定位置？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD2PosZ.Text == "初始位置")
                {
                    GClsMontion.AbsoluteMove(4, Convert.ToDouble(iniMontion.ReadIni("CCD3-Z轴", "初始位置")) / CAMiClsVariable.pulseAxis4, speedCCD2ZGoto, TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                }
                else if (cobCCD2PosZ.Text == "抓料位置")
                {
                    GClsMontion.AbsoluteMove(4, Convert.ToDouble(iniMontion.ReadIni("CCD2-Z轴", "抓料位置")) / CAMiClsVariable.pulseAxis4, speedCCD2ZGoto, TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                }
                else if (cobCCD2PosZ.Text == "拍照位置")
                {
                    GClsMontion.AbsoluteMove(4, Convert.ToDouble(iniMontion.ReadIni("CCD2-Z轴", "拍照位置")) / CAMiClsVariable.pulseAxis4, speedCCD2ZGoto, TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                }
                else if (cobCCD2PosZ.Text == "抛料位置")
                {
                    GClsMontion.AbsoluteMove(4, Convert.ToDouble(iniMontion.ReadIni("CCD2-Z轴", "抛料位置")) / CAMiClsVariable.pulseAxis4, speedCCD2ZGoto, TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                }
            }
        }
        #endregion
        #region   示教                                         */
        private void btnCCD1XTeach_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否示教？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD1PosX.Text == "初始位置")
                {
                    myMethod.RecordParaModification("CCD1-X轴初始位置", iniMontion.ReadIni("CCD1-X轴", "初始位置"), txtCCD1XCurrPos.Text);
                    iniMontion.WriteIni("CCD1-X轴", "初始位置", txtCCD1XCurrPos.Text);
                    CAMiClsVariable.CCD1InitialPosX = Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "初始位置")) / CAMiClsVariable.pulseAxis0;
                }
                else if (cobCCD1PosX.Text == "拍照位置1")
                {
                    myMethod.RecordParaModification("CCD1-X轴拍照位置1", iniMontion.ReadIni("CCD1-X轴", "拍照位置1"), txtCCD1XCurrPos.Text);
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置1", txtCCD1XCurrPos.Text);
                    CAMiClsVariable.CCD1PosXValue[0] = Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置1")) / CAMiClsVariable.pulseAxis0;
                    CAMiClsVariable.CCD1PosXValue[2] = CAMiClsVariable.CCD1PosXValue[0] - CAMiClsVariable.CCD1GapX;
                    CAMiClsVariable.CCD1PosXValue[4] = CAMiClsVariable.CCD1PosXValue[0] - 2 * CAMiClsVariable.CCD1GapX;
                    CAMiClsVariable.CCD1PosXValue[6] = CAMiClsVariable.CCD1PosXValue[0] - 3 * CAMiClsVariable.CCD1GapX;

                    CAMiClsVariable.CCD1PosXValue[8] = CAMiClsVariable.CCD1PosXValue[6];
                    CAMiClsVariable.CCD1PosXValue[10] = CAMiClsVariable.CCD1PosXValue[4];
                    CAMiClsVariable.CCD1PosXValue[12] = CAMiClsVariable.CCD1PosXValue[2];
                    CAMiClsVariable.CCD1PosXValue[14] = CAMiClsVariable.CCD1PosXValue[0];

                    CAMiClsVariable.CCD1PosXValue[16] = CAMiClsVariable.CCD1PosXValue[0];
                    CAMiClsVariable.CCD1PosXValue[18] = CAMiClsVariable.CCD1PosXValue[2];
                    CAMiClsVariable.CCD1PosXValue[20] = CAMiClsVariable.CCD1PosXValue[4];
                    CAMiClsVariable.CCD1PosXValue[22] = CAMiClsVariable.CCD1PosXValue[6];
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置3", (CAMiClsVariable.CCD1PosXValue[2] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置5", (CAMiClsVariable.CCD1PosXValue[4] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置7", (CAMiClsVariable.CCD1PosXValue[6] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置9", (CAMiClsVariable.CCD1PosXValue[8] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置11", (CAMiClsVariable.CCD1PosXValue[10] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置13", (CAMiClsVariable.CCD1PosXValue[12] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置15", (CAMiClsVariable.CCD1PosXValue[14] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置17", (CAMiClsVariable.CCD1PosXValue[16] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置19", (CAMiClsVariable.CCD1PosXValue[18] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置21", (CAMiClsVariable.CCD1PosXValue[20] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置23", (CAMiClsVariable.CCD1PosXValue[22] * CAMiClsVariable.pulseAxis0).ToString());
                }
                else if (cobCCD1PosX.Text == "拍照位置2")
                {
                    myMethod.RecordParaModification("CCD1-X轴拍照位置2", iniMontion.ReadIni("CCD1-X轴", "拍照位置2"), txtCCD1XCurrPos.Text);
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置2", txtCCD1XCurrPos.Text);
                    CAMiClsVariable.CCD1PosXValue[1] = Convert.ToDouble(iniMontion.ReadIni("CCD1-X轴", "拍照位置2")) / CAMiClsVariable.pulseAxis0;
                    CAMiClsVariable.CCD1PosXValue[3] = CAMiClsVariable.CCD1PosXValue[1] - CAMiClsVariable.CCD1GapX;
                    CAMiClsVariable.CCD1PosXValue[5] = CAMiClsVariable.CCD1PosXValue[1] - 2 * CAMiClsVariable.CCD1GapX;
                    CAMiClsVariable.CCD1PosXValue[7] = CAMiClsVariable.CCD1PosXValue[1] - 3 * CAMiClsVariable.CCD1GapX;
                    CAMiClsVariable.CCD1PosXValue[9] = CAMiClsVariable.CCD1PosXValue[7];
                    CAMiClsVariable.CCD1PosXValue[11] = CAMiClsVariable.CCD1PosXValue[5];
                    CAMiClsVariable.CCD1PosXValue[13] = CAMiClsVariable.CCD1PosXValue[3];
                    CAMiClsVariable.CCD1PosXValue[15] = CAMiClsVariable.CCD1PosXValue[1];
                    CAMiClsVariable.CCD1PosXValue[17] = CAMiClsVariable.CCD1PosXValue[1];
                    CAMiClsVariable.CCD1PosXValue[19] = CAMiClsVariable.CCD1PosXValue[3];
                    CAMiClsVariable.CCD1PosXValue[21] = CAMiClsVariable.CCD1PosXValue[5];
                    CAMiClsVariable.CCD1PosXValue[23] = CAMiClsVariable.CCD1PosXValue[7];
                
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置4", (CAMiClsVariable.CCD1PosXValue[3] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置6", (CAMiClsVariable.CCD1PosXValue[5] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置8", (CAMiClsVariable.CCD1PosXValue[7] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置10", (CAMiClsVariable.CCD1PosXValue[9] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置12", (CAMiClsVariable.CCD1PosXValue[11] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置14", (CAMiClsVariable.CCD1PosXValue[13] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置16", (CAMiClsVariable.CCD1PosXValue[15] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置18", (CAMiClsVariable.CCD1PosXValue[17] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置20", (CAMiClsVariable.CCD1PosXValue[19] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置22", (CAMiClsVariable.CCD1PosXValue[21] * CAMiClsVariable.pulseAxis0).ToString());
                    iniMontion.WriteIni("CCD1-X轴", "拍照位置24", (CAMiClsVariable.CCD1PosXValue[23] * CAMiClsVariable.pulseAxis0).ToString());
                }
                else
                {
                    MessageBox.Show("如要更改其它点位位置，请更新拍照位置1即可。");
                }
            }
        }

        private void btnCCD1YTeach_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否示教？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD1PosY.Text == "初始位置")
                {
                    myMethod.RecordParaModification("CCD1-Y轴初始位置", iniMontion.ReadIni("CCD1-Y轴", "初始位置"), txtCCD1YCurrPos.Text);
                    iniMontion.WriteIni("CCD1-Y轴", "初始位置", txtCCD1YCurrPos.Text);
                    CAMiClsVariable.CCD1InitialPosY = Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "初始位置")) / CAMiClsVariable.pulseAxis1;
                }

                else if (cobCCD1PosY.Text == "拍照位置1")
                {
                    myMethod.RecordParaModification("CCD1-Y轴拍照位置1", iniMontion.ReadIni("CCD1-Y轴", "拍照位置1"), txtCCD1YCurrPos.Text);
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置1", txtCCD1YCurrPos.Text);
                    CAMiClsVariable.CCD1PosYValue[0] = Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置1")) / CAMiClsVariable.pulseAxis1;
                    CAMiClsVariable.CCD1PosYValue[2] = CAMiClsVariable.CCD1PosYValue[0];
                    CAMiClsVariable.CCD1PosYValue[4] = CAMiClsVariable.CCD1PosYValue[0];
                    CAMiClsVariable.CCD1PosYValue[6] = CAMiClsVariable.CCD1PosYValue[0];
                    CAMiClsVariable.CCD1PosYValue[8] = CAMiClsVariable.CCD1PosYValue[0] - CAMiClsVariable.CCD1GapY;
                    CAMiClsVariable.CCD1PosYValue[10] = CAMiClsVariable.CCD1PosYValue[0] - CAMiClsVariable.CCD1GapY;
                    CAMiClsVariable.CCD1PosYValue[12] = CAMiClsVariable.CCD1PosYValue[0] - CAMiClsVariable.CCD1GapY;
                    CAMiClsVariable.CCD1PosYValue[14] = CAMiClsVariable.CCD1PosYValue[0] - CAMiClsVariable.CCD1GapY;
                    CAMiClsVariable.CCD1PosYValue[16] = CAMiClsVariable.CCD1PosYValue[0] - 2 * CAMiClsVariable.CCD1GapY;
                    CAMiClsVariable.CCD1PosYValue[18] = CAMiClsVariable.CCD1PosYValue[0] - 2 * CAMiClsVariable.CCD1GapY;
                    CAMiClsVariable.CCD1PosYValue[20] = CAMiClsVariable.CCD1PosYValue[0] - 2 * CAMiClsVariable.CCD1GapY;
                    CAMiClsVariable.CCD1PosYValue[22] = CAMiClsVariable.CCD1PosYValue[0] - 2 * CAMiClsVariable.CCD1GapY;
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置3", (CAMiClsVariable.CCD1PosYValue[2] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置5", (CAMiClsVariable.CCD1PosYValue[4] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置7", (CAMiClsVariable.CCD1PosYValue[6] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置9", (CAMiClsVariable.CCD1PosYValue[8] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置11", (CAMiClsVariable.CCD1PosYValue[10] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置13", (CAMiClsVariable.CCD1PosYValue[12] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置15", (CAMiClsVariable.CCD1PosYValue[14] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置17", (CAMiClsVariable.CCD1PosYValue[16] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置19", (CAMiClsVariable.CCD1PosYValue[18] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置21", (CAMiClsVariable.CCD1PosYValue[20] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置23", (CAMiClsVariable.CCD1PosYValue[22] * CAMiClsVariable.pulseAxis1).ToString());
                }

                else if (cobCCD1PosY.Text == "拍照位置2")
                {
                    myMethod.RecordParaModification("CCD1-Y轴拍照位置2", iniMontion.ReadIni("CCD1-Y轴", "拍照位置2"), txtCCD1YCurrPos.Text);
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置2", txtCCD1YCurrPos.Text);
                    CAMiClsVariable.CCD1PosYValue[1] = Convert.ToDouble(iniMontion.ReadIni("CCD1-Y轴", "拍照位置2")) / CAMiClsVariable.pulseAxis1;
                    CAMiClsVariable.CCD1PosYValue[3] = CAMiClsVariable.CCD1PosYValue[1];
                    CAMiClsVariable.CCD1PosYValue[5] = CAMiClsVariable.CCD1PosYValue[1];
                    CAMiClsVariable.CCD1PosYValue[7] = CAMiClsVariable.CCD1PosYValue[1];
                    CAMiClsVariable.CCD1PosYValue[9] = CAMiClsVariable.CCD1PosYValue[1] - CAMiClsVariable.CCD1GapY ;
                    CAMiClsVariable.CCD1PosYValue[11] = CAMiClsVariable.CCD1PosYValue[1] - CAMiClsVariable.CCD1GapY ;
                    CAMiClsVariable.CCD1PosYValue[13] = CAMiClsVariable.CCD1PosYValue[1] - CAMiClsVariable.CCD1GapY ;
                    CAMiClsVariable.CCD1PosYValue[15] = CAMiClsVariable.CCD1PosYValue[1] - CAMiClsVariable.CCD1GapY ;
                    CAMiClsVariable.CCD1PosYValue[17] = CAMiClsVariable.CCD1PosYValue[1] - 2 * CAMiClsVariable.CCD1GapY ;
                    CAMiClsVariable.CCD1PosYValue[19] = CAMiClsVariable.CCD1PosYValue[1] - 2 * CAMiClsVariable.CCD1GapY ;
                    CAMiClsVariable.CCD1PosYValue[21] = CAMiClsVariable.CCD1PosYValue[1] - 2 * CAMiClsVariable.CCD1GapY ;
                    CAMiClsVariable.CCD1PosYValue[23] = CAMiClsVariable.CCD1PosYValue[1] - 2 * CAMiClsVariable.CCD1GapY ;

                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置4", (CAMiClsVariable.CCD1PosYValue[3] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置6", (CAMiClsVariable.CCD1PosYValue[5] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置8", (CAMiClsVariable.CCD1PosYValue[7] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置10", (CAMiClsVariable.CCD1PosYValue[9] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置12", (CAMiClsVariable.CCD1PosYValue[11] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置14", (CAMiClsVariable.CCD1PosYValue[13] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置16", (CAMiClsVariable.CCD1PosYValue[15] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置18", (CAMiClsVariable.CCD1PosYValue[17] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置20", (CAMiClsVariable.CCD1PosYValue[19] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置22", (CAMiClsVariable.CCD1PosYValue[21] * CAMiClsVariable.pulseAxis1).ToString());
                    iniMontion.WriteIni("CCD1-Y轴", "拍照位置24", (CAMiClsVariable.CCD1PosYValue[23] * CAMiClsVariable.pulseAxis1).ToString());
                }
                else
                {
                    MessageBox.Show("如要更改其它点位位置，请更新拍照位置1即可。");
                }
            }
        }
        private void btnCCD2XTeach_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否示教？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD2PosX.Text == "初始位置")
                {
                    myMethod.RecordParaModification("CCD2-X轴初始位置", iniMontion.ReadIni("CCD2-X轴", "初始位置"), txtCCD2XCurrPos.Text);
                    iniMontion.WriteIni("CCD2-X轴", "初始位置", txtCCD2XCurrPos.Text);
                    CAMiClsVariable.CCD2InitialPosX = Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "初始位置")) / CAMiClsVariable.pulseAxis2;
                }
                else if (cobCCD2PosX.Text == "拍照位置1")
                {
                    myMethod.RecordParaModification("CCD2-X轴拍照位置1", iniMontion.ReadIni("CCD2-X轴", "拍照位置1"), txtCCD2XCurrPos.Text);
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置1", txtCCD2XCurrPos.Text);
                    CAMiClsVariable.CCD2PosXValue[0] = Convert.ToDouble(iniMontion.ReadIni("CCD2-X轴", "拍照位置1")) / CAMiClsVariable.pulseAxis2;
                    CAMiClsVariable.CCD2PosXValue[1] = CAMiClsVariable.CCD2PosXValue[0] - CAMiClsVariable.CCD2GapX1;
                    CAMiClsVariable.CCD2PosXValue[2] = CAMiClsVariable.CCD2PosXValue[0] - CAMiClsVariable.CCD2GapX2;
                    CAMiClsVariable.CCD2PosXValue[3] = CAMiClsVariable.CCD2PosXValue[0] - CAMiClsVariable.CCD2GapX1 - CAMiClsVariable.CCD2GapX2;
                    CAMiClsVariable.CCD2PosXValue[4] = CAMiClsVariable.CCD2PosXValue[0];
                    CAMiClsVariable.CCD2PosXValue[5] = CAMiClsVariable.CCD2PosXValue[1];
                    CAMiClsVariable.CCD2PosXValue[6] = CAMiClsVariable.CCD2PosXValue[2];
                    CAMiClsVariable.CCD2PosXValue[7] = CAMiClsVariable.CCD2PosXValue[3];
                    CAMiClsVariable.CCD2PosXValue[8] = CAMiClsVariable.CCD2PosXValue[0];
                    CAMiClsVariable.CCD2PosXValue[9] = CAMiClsVariable.CCD2PosXValue[1];
                    CAMiClsVariable.CCD2PosXValue[10] = CAMiClsVariable.CCD2PosXValue[2];
                    CAMiClsVariable.CCD2PosXValue[11] = CAMiClsVariable.CCD2PosXValue[3];
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置2", (CAMiClsVariable.CCD2PosXValue[1] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置3", (CAMiClsVariable.CCD2PosXValue[2] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置4", (CAMiClsVariable.CCD2PosXValue[3] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置5", (CAMiClsVariable.CCD2PosXValue[4] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置6", (CAMiClsVariable.CCD2PosXValue[5] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置7", (CAMiClsVariable.CCD2PosXValue[6] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置8", (CAMiClsVariable.CCD2PosXValue[7] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置9", (CAMiClsVariable.CCD2PosXValue[8] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置10", (CAMiClsVariable.CCD2PosXValue[9] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置11", (CAMiClsVariable.CCD2PosXValue[10] * CAMiClsVariable.pulseAxis2).ToString());
                    iniMontion.WriteIni("CCD2-X轴", "拍照位置12", (CAMiClsVariable.CCD2PosXValue[11] * CAMiClsVariable.pulseAxis2).ToString());
                }
                else
                {
                    MessageBox.Show("如要更改其它点位位置，请更新拍照位置1即可。");
                }
            }
        }

        private void btnCCD2YTeach_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否示教？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD2PosY.Text == "初始位置")
                {
                    myMethod.RecordParaModification("CCD2-Y轴初始位置", iniMontion.ReadIni("CCD2-Y轴", "初始位置"), txtCCD2YCurrPos.Text);
                    iniMontion.WriteIni("CCD2-Y轴", "初始位置", txtCCD2YCurrPos.Text);
                    CAMiClsVariable.CCD2InitialPosY = Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "初始位置")) / CAMiClsVariable.pulseAxis3;
                }
                else if (cobCCD2PosY.Text == "抓料位置")
                {
                    myMethod.RecordParaModification("CCD2-Y轴抓料位置", iniMontion.ReadIni("CCD2-Y轴", "抓料位置"), txtCCD2YCurrPos.Text);
                    iniMontion.WriteIni("CCD2-Y轴", "抓料位置", txtCCD2YCurrPos.Text);
                    CAMiClsVariable.CCD2PickPosY = Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "抓料位置")) / CAMiClsVariable.pulseAxis3;
                }
                else if (cobCCD2PosY.Text == "抛料位置")
                {
                    myMethod.RecordParaModification("CCD2-Y轴抛料位置", iniMontion.ReadIni("CCD2-Y轴", "抛料位置"), txtCCD2YCurrPos.Text);
                    iniMontion.WriteIni("CCD2-Y轴", "抛料位置", txtCCD2YCurrPos.Text);
                    CAMiClsVariable.CCD2flingPosY = Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "抛料位置")) / CAMiClsVariable.pulseAxis3;
                    CAMiClsVariable.CCD2TossingPosYValue[0] = CAMiClsVariable.CCD2flingPosY;
                    CAMiClsVariable.CCD2TossingPosYValue[1] = CAMiClsVariable.CCD2flingPosY + 2 * CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2TossingPosYValue[2] = CAMiClsVariable.CCD2flingPosY + CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2TossingPosYValue[3] = CAMiClsVariable.CCD2flingPosY;
                    CAMiClsVariable.CCD2TossingPosYValue[4] = CAMiClsVariable.CCD2flingPosY - CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2TossingPosYValue[5] = CAMiClsVariable.CCD2flingPosY - 2 * CAMiClsVariable.CCD2GapY;
                    iniMontion.WriteIni("CCD2-Y轴", "抛料位置0", (CAMiClsVariable.CCD2TossingPosYValue[0] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "抛料位置1", (CAMiClsVariable.CCD2TossingPosYValue[1] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "抛料位置2", (CAMiClsVariable.CCD2TossingPosYValue[2] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "抛料位置3", (CAMiClsVariable.CCD2TossingPosYValue[3] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "抛料位置4", (CAMiClsVariable.CCD2TossingPosYValue[4] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "抛料位置5", (CAMiClsVariable.CCD2TossingPosYValue[5] * CAMiClsVariable.pulseAxis3).ToString());
                }
                else if (cobCCD2PosY.Text == "拍照位置1")
                {
                    myMethod.RecordParaModification("CCD2-Y轴拍照位置1", iniMontion.ReadIni("CCD2-Y轴", "拍照位置1"), txtCCD2YCurrPos.Text);
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置1", txtCCD2YCurrPos.Text);
                    CAMiClsVariable.CCD2PosYValue[0] = Convert.ToDouble(iniMontion.ReadIni("CCD2-Y轴", "拍照位置1")) / CAMiClsVariable.pulseAxis3;
                    CAMiClsVariable.CCD2PosYValue[1] = CAMiClsVariable.CCD2PosYValue[0];
                    CAMiClsVariable.CCD2PosYValue[2] = CAMiClsVariable.CCD2PosYValue[0];
                    CAMiClsVariable.CCD2PosYValue[3] = CAMiClsVariable.CCD2PosYValue[0];
                    CAMiClsVariable.CCD2PosYValue[4] = CAMiClsVariable.CCD2PosYValue[0] + CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2PosYValue[5] = CAMiClsVariable.CCD2PosYValue[0] + CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2PosYValue[6] = CAMiClsVariable.CCD2PosYValue[0] + CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2PosYValue[7] = CAMiClsVariable.CCD2PosYValue[0] + CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2PosYValue[8] = CAMiClsVariable.CCD2PosYValue[0] + 2 * CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2PosYValue[9] = CAMiClsVariable.CCD2PosYValue[0] + 2 * CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2PosYValue[10] = CAMiClsVariable.CCD2PosYValue[0] + 2 * CAMiClsVariable.CCD2GapY;
                    CAMiClsVariable.CCD2PosYValue[11] = CAMiClsVariable.CCD2PosYValue[0] + 2 * CAMiClsVariable.CCD2GapY;
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置2", (CAMiClsVariable.CCD2PosYValue[1] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置3", (CAMiClsVariable.CCD2PosYValue[2] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置4", (CAMiClsVariable.CCD2PosYValue[3] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置5", (CAMiClsVariable.CCD2PosYValue[4] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置6", (CAMiClsVariable.CCD2PosYValue[5] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置7", (CAMiClsVariable.CCD2PosYValue[6] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置8", (CAMiClsVariable.CCD2PosYValue[7] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置9", (CAMiClsVariable.CCD2PosYValue[8] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置10", (CAMiClsVariable.CCD2PosYValue[9] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置11", (CAMiClsVariable.CCD2PosYValue[10] * CAMiClsVariable.pulseAxis3).ToString());
                    iniMontion.WriteIni("CCD2-Y轴", "拍照位置12", (CAMiClsVariable.CCD2PosYValue[11] * CAMiClsVariable.pulseAxis3).ToString());
                }
                else
                {
                    MessageBox.Show("如要更改其它点位位置，请更新拍照位置1即可。");
                }
            }
        }

        private void btnCCD2ZTeach_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否示教？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (cobCCD2PosZ.Text == "初始位置")
                {
                    myMethod.RecordParaModification("CCD2-Z轴初始位置", iniMontion.ReadIni("CCD2-Z轴", "初始位置"), txtCCD2ZCurrPos.Text);
                    iniMontion.WriteIni("CCD2-Z轴", "初始位置", txtCCD2ZCurrPos.Text);
                    CAMiClsVariable.CCD2InitialPosZ = Convert.ToDouble(iniMontion.ReadIni("CCD2-Z轴", "初始位置")) / CAMiClsVariable.pulseAxis4;
                }
                else if (cobCCD2PosZ.Text == "抓料位置")
                {
                    myMethod.RecordParaModification("CCD2-Z轴抓料位置", iniMontion.ReadIni("CCD2-Z轴", "抓料位置"), txtCCD2ZCurrPos.Text);
                    iniMontion.WriteIni("CCD2-Z轴", "抓料位置", txtCCD2ZCurrPos.Text);
                    CAMiClsVariable.CCD2PickPosZ = Convert.ToDouble(iniMontion.ReadIni("CCD2-Z轴", "抓料位置")) / CAMiClsVariable.pulseAxis4;
                }
                else if (cobCCD2PosZ.Text == "拍照位置")
                {
                    myMethod.RecordParaModification("CCD2-Z轴拍照位置", iniMontion.ReadIni("CCD2-Z轴", "拍照位置"), txtCCD2ZCurrPos.Text);
                    iniMontion.WriteIni("CCD2-Z轴", "拍照位置", txtCCD2ZCurrPos.Text);
                    CAMiClsVariable.CCD2TakePhotoPosZ = Convert.ToDouble(iniMontion.ReadIni("CCD2-Z轴", "拍照位置")) / CAMiClsVariable.pulseAxis4;
                }
                else if (cobCCD2PosZ.Text == "抛料位置")
                {
                    myMethod.RecordParaModification("CCD2-Z轴抛料位置", iniMontion.ReadIni("CCD2-Z轴", "抛料位置"), txtCCD2ZCurrPos.Text);
                    iniMontion.WriteIni("CCD2-Z轴", "抛料位置", txtCCD2ZCurrPos.Text);
                    CAMiClsVariable.CCD2flingPosZ = Convert.ToDouble(iniMontion.ReadIni("CCD2-Z轴", "抛料位置")) / CAMiClsVariable.pulseAxis4;
                }
            }
        }
        #endregion
        #region 示教点位更新
        private void cobCCD1PosX_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniMontion.filePath = CAMiClsVariable.montionIniPath;
            ComboBox cb = (ComboBox)sender;
            txtCCD1TeachPosX.Text = iniMontion.ReadIni("CCD1-X轴", cb.Text);
        }

        private void cobCCD1PosY_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniMontion.filePath = CAMiClsVariable.montionIniPath;
            ComboBox cb = (ComboBox)sender;
            txtCCD1TeachPosY.Text = iniMontion.ReadIni("CCD1-Y轴", cb.Text);
        }

        private void cobCCD2PosX_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniMontion.filePath = CAMiClsVariable.montionIniPath;
            ComboBox cb = (ComboBox)sender;
            txtCCD2TeachPosX.Text = iniMontion.ReadIni("CCD2-X轴", cb.Text);
        }

        private void cobCCD2PosY_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniMontion.filePath = CAMiClsVariable.montionIniPath;
            ComboBox cb = (ComboBox)sender;
            txtCCD2TeachPosY.Text = iniMontion.ReadIni("CCD2-Y轴", cb.Text);
        }

        private void cobCCD2PosZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniMontion.filePath = CAMiClsVariable.montionIniPath;
            ComboBox cb = (ComboBox)sender;
            txtCCD2TeachPosZ.Text = iniMontion.ReadIni("CCD2-Z轴", cb.Text);
        }
        #endregion
        #region 间距和速度更改判断
        private void btnGapChange_Click(object sender, EventArgs e)//间距更改
        {
            DialogResult result = MessageBox.Show("请确定是否更改间距值？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (txtCCD1GapX.Text.Trim() != iniMontion.ReadIni("间距", "CCD1间距X"))
                {
                    myMethod.RecordParaModification("CCD1间距X", iniMontion.ReadIni("间距", "CCD1间距X"), txtCCD1GapX.Text.Trim());
                    iniMontion.WriteIni("间距", "CCD1间距X", txtCCD1GapX.Text.Trim());
                    CAMiClsVariable.CCD1GapX = Convert.ToDouble(iniMontion.ReadIni("间距", "CCD1间距X")) / CAMiClsVariable.pulseAxis0;
                }


                //if (txtCCD1GapX2.Text.Trim() != iniMontion.ReadIni("间距", "CCD1间距X2"))
                //{
                //    myMethod.RecordParaModification("CCD1间距X2", iniMontion.ReadIni("间距", "CCD1间距X2"), txtCCD1GapX.Text.Trim());
                //    iniMontion.WriteIni("间距", "CCD1间距X2", txtCCD1GapX2.Text.Trim());
                //    CAMiClsVariable.CCD1GapX = Convert.ToDouble(iniMontion.ReadIni("间距", "CCD1间距X2")) / CAMiClsVariable.pulseAxis0;
                //}

                if (txtCCD1GapY.Text.Trim() != iniMontion.ReadIni("间距", "CCD1间距Y"))
                {
                    myMethod.RecordParaModification("CCD1间距Y", iniMontion.ReadIni("间距", "CCD1间距Y"), txtCCD1GapY.Text.Trim());
                    iniMontion.WriteIni("间距", "CCD1间距Y", txtCCD1GapY.Text.Trim());
                    CAMiClsVariable.CCD1GapY = Convert.ToDouble(iniMontion.ReadIni("间距", "CCD1间距Y")) / CAMiClsVariable.pulseAxis1;
                }
                //if (txtCCD1GapY2.Text.Trim() != iniMontion.ReadIni("间距", "CCD1间距Y"))
                //{
                //    myMethod.RecordParaModification("CCD1间距Y2", iniMontion.ReadIni("间距", "CCD1间距Y2"), txtCCD1GapY.Text.Trim());
                //    iniMontion.WriteIni("间距", "CCD1间距Y2", txtCCD1GapY2.Text.Trim());
                //    CAMiClsVariable.CCD1GapY2 = Convert.ToDouble(iniMontion.ReadIni("间距", "CCD1间距Y2")) / CAMiClsVariable.pulseAxis1;
                //}
                if (txtCCD2GapY.Text.Trim() != iniMontion.ReadIni("间距", "CCD2间距Y"))
                {
                    myMethod.RecordParaModification("CCD2间距Y", iniMontion.ReadIni("间距", "CCD2间距Y"), txtCCD2GapY.Text.Trim());
                    iniMontion.WriteIni("间距", "CCD2间距Y", txtCCD2GapY.Text.Trim());
                    CAMiClsVariable.CCD2GapY = Convert.ToDouble(iniMontion.ReadIni("间距", "CCD2间距Y")) / CAMiClsVariable.pulseAxis3;
                }
                if (txtCCD2GapX1.Text.Trim() != iniMontion.ReadIni("间距", "CCD2间距X1"))
                {
                    myMethod.RecordParaModification("CCD2间距X1", iniMontion.ReadIni("间距", "CCD2间距X1"), txtCCD2GapX1.Text.Trim());
                    iniMontion.WriteIni("间距", "CCD2间距X1", txtCCD2GapX1.Text.Trim());
                    CAMiClsVariable.CCD2GapX1 = Convert.ToDouble(iniMontion.ReadIni("间距", "CCD2间距X1")) / CAMiClsVariable.pulseAxis2;
                }
                if (txtCCD2GapX2.Text.Trim() != iniMontion.ReadIni("间距", "CCD2间距Y2"))
                {
                    myMethod.RecordParaModification("CCD2间距X2", iniMontion.ReadIni("间距", "CCD2间距X2"), txtCCD2GapX2.Text.Trim());
                    iniMontion.WriteIni("间距", "CCD2间距X2", txtCCD2GapX2.Text.Trim());
                    CAMiClsVariable.CCD2GapX2 = Convert.ToDouble(iniMontion.ReadIni("间距", "CCD2间距X2")) / CAMiClsVariable.pulseAxis2;
                }
            }
        }

        private void btnSpeedChange_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否更改速度值？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniMontion.filePath = CAMiClsVariable.montionIniPath;
                if (txtCCD1SpeedX.Text.Trim() != iniMontion.ReadIni("速度", "CCD1-X速度"))
                {
                    myMethod.RecordParaModification("CCD1-X速度", iniMontion.ReadIni("速度", "CCD1-X速度"), txtCCD1SpeedX.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD1-X速度", txtCCD1SpeedX.Text.Trim());
                    CAMiClsVariable.speedCCD1X = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD1-X速度")) / CAMiClsVariable.pulseAxis0;
                }
                if (txtCCD1SpeedY.Text.Trim() != iniMontion.ReadIni("速度", "CCD1-Y速度"))
                {
                    myMethod.RecordParaModification("CCD1-Y速度", iniMontion.ReadIni("速度", "CCD1-Y速度"), txtCCD1SpeedY.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD1-Y速度", txtCCD1SpeedY.Text.Trim());
                    CAMiClsVariable.speedCCD1Y = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD1-Y速度")) / CAMiClsVariable.pulseAxis1;
                }
                if (txtCCD2SpeedX.Text.Trim() != iniMontion.ReadIni("速度", "CCD2-X速度"))
                {
                    myMethod.RecordParaModification("CCD2-X速度", iniMontion.ReadIni("速度", "CCD2-X速度"), txtCCD2SpeedX.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD2-X速度", txtCCD2SpeedX.Text.Trim());
                    CAMiClsVariable.speedCCD2X = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD2-X速度")) / CAMiClsVariable.pulseAxis2;
                }
                if (txtCCD2SpeedY.Text.Trim() != iniMontion.ReadIni("速度", "CCD2-Y速度"))
                {
                    myMethod.RecordParaModification("CCD2-Y速度", iniMontion.ReadIni("速度", "CCD2-Y速度"), txtCCD2SpeedY.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD2-Y速度", txtCCD2SpeedY.Text.Trim());
                    CAMiClsVariable.speedCCD2Y = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD2-Y速度")) / CAMiClsVariable.pulseAxis3;
                }
                if (txtCCD2SpeedZ.Text.Trim() != iniMontion.ReadIni("速度", "CCD2-Z速度"))
                {
                    myMethod.RecordParaModification("CCD2-Z速度", iniMontion.ReadIni("速度", "CCD2-Z速度"), txtCCD2SpeedZ.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD2-Z速度", txtCCD2SpeedZ.Text.Trim());
                    CAMiClsVariable.speedCCD2Z = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD2-Z速度")) / CAMiClsVariable.pulseAxis4;
                }

                if (txtCCD1TaccX.Text.Trim() != iniMontion.ReadIni("速度", "CCD1-X加减速"))
                {
                    myMethod.RecordParaModification("CCD1-X加减速", iniMontion.ReadIni("速度", "CCD1-X加减速"), txtCCD1TaccX.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD1-X加减速", txtCCD1TaccX.Text.Trim());
                    CAMiClsVariable.TaccCCD1X = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD1-X加减速"));
                }
                if (txtCCD1TaccY.Text.Trim() != iniMontion.ReadIni("速度", "CCD1-Y加减速"))
                {
                    myMethod.RecordParaModification("CCD1-Y加减速", iniMontion.ReadIni("速度", "CCD1-Y加减速"), txtCCD1TaccY.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD1-Y加减速", txtCCD1TaccY.Text.Trim());
                    CAMiClsVariable.TaccCCD1Y = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD1-Y加减速"));
                }
                if (txtCCD2TaccX.Text.Trim() != iniMontion.ReadIni("速度", "CCD2-X加减速"))
                {
                    myMethod.RecordParaModification("CCD2-X加减速", iniMontion.ReadIni("速度", "CCD2-X加减速"), txtCCD2TaccX.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD2-X加减速", txtCCD2TaccX.Text.Trim());
                    CAMiClsVariable.TaccCCD2X = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD2-X加减速"));
                }
                if (txtCCD2TaccY.Text.Trim() != iniMontion.ReadIni("速度", "CCD2-Y加减速"))
                {
                    myMethod.RecordParaModification("CCD2-Y加减速", iniMontion.ReadIni("速度", "CCD2-Y加减速"), txtCCD2TaccY.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD2-Y加减速", txtCCD2TaccY.Text.Trim());
                    CAMiClsVariable.TaccCCD2Y = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD2-Y加减速"));
                }
                if (txtCCD2TaccZ.Text.Trim() != iniMontion.ReadIni("速度", "CCD2-Z加减速"))
                {
                    myMethod.RecordParaModification("CCD2-Z加减速", iniMontion.ReadIni("速度", "CCD2-Z加减速"), txtCCD2TaccZ.Text.Trim());
                    iniMontion.WriteIni("速度", "CCD2-Z加减速", txtCCD2TaccZ.Text.Trim());
                    CAMiClsVariable.TaccCCD2Z = Convert.ToDouble(iniMontion.ReadIni("速度", "CCD2-Z加减速"));
                }

            }
        }
        #endregion

        private void lblTrayButton_Click(object sender, EventArgs e)//Tray上料按钮
        {
            DialogResult result = MessageBox.Show("请确定是否将NG Tray上料？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                CAMiClsMethod.NGTrayLoad();
            }
        }

   
    }
}
