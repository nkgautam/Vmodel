using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace vmodel
{
   
    public partial class Form1 : Form
    {
        Graphics graphics;
        Pen pen = new Pen(Color.Black);
        Pen penBlue = new Pen(Color.Blue,2.5f);
        Pen penCurve = new Pen(Color.Green,2.0f);
        Pen gridPen = new Pen(Color.LightGray);
        Pen red = new Pen(Color.Red, 2.0f);
        Pen redCircle = new Pen(Color.Red);
        Bitmap bmp01, bmp;
        SolidBrush brsh;
        Font fnt;
        double[] nuggets;
        double[] range;
        double[] sill;
        int count = 0;
        double limit;
        float x_mm, y_mm;
        int index = 0;
        int[] nst;
        float[] aa;
        float[] cc;
        public Form1()
        {
            
            InitializeComponent();
            comboBoxNst.SelectedIndex = 0;
            
        }
        private void drawchart()
        {

            bmp01 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp01;
            GC.SuppressFinalize(bmp01);

            bmp = new Bitmap(pictureBox1.Image);
            graphics = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;

            graphics.Clear(Color.White);
            brsh = new SolidBrush(Color.Black);
            fnt = new Font("Arial", 8, FontStyle.Bold);
            GC.SuppressFinalize(graphics);

            //Y Axis labels
            for (int i = 0; i < 108; i = i + 6)
            {
                double text = i;
                float y = 329 - (i * 3) - 12;
                graphics.DrawString(String.Format("{0,4:0.00}", text / 100), fnt, brsh, 0, y);
                
                graphics.DrawLine(gridPen, 23, y, 573, y);
            }

            //X Axis lables
            for (int i = 0; i <= 130; i = i + 10)
            {
                if (i != 0)
                {
                    float x = (i * 4) + 23;
                    graphics.DrawString(String.Format("{0}", i), fnt, brsh, x, 318);
                   
                    graphics.DrawLine(gridPen, x, 318, x, 0);
                }
                

            }
            graphics.DrawLine(pen, 23, 317, 623, 317);
            graphics.DrawLine(pen, 23, 317, 23, 0);
            int cursurmax = 0;
            for (int i = 1; i < count; i++)
            {
                float x1 = (float)(range[i] * 4) + 23;
                float y1 = 329 - ((float)nuggets[i] * 100 * 3) - 12;
                int j = i + 1;
                float x2, y2;
                if (j < count)
                {
                    x2 = ((float)range[j] * 4) + 23;
                    y2 = 329 - ((float)nuggets[j] * 100 * 3) - 12;
                    graphics.DrawLine(penCurve, x1, y1, x2, y2);
                   // graphics.DrawLine(penCurve, x1, y1, x1 + 3, y1);
                    if (range[i] == limit)
                    {
                        cursurmax = i;
                        //break;
                    }
                }
           }

           int  nuggetCursor = (int) (317 - nuggets[1] * 100 * 3);
           graphics.DrawLine(red, 23, nuggetCursor, 38, nuggetCursor);

           int rangeCircleY, rangeCircleX;
           Rectangle Rect;

           for (int i = 0; i < nst[0]; i++)
           {
                   rangeCircleY = (int)(317 - cc[i] * 100 * 3);
                   rangeCircleX = (int)(aa[i] * 4 + 23);
                   Rect = new Rectangle(rangeCircleX - 5, rangeCircleY - 5, 3,3);
                   graphics.DrawRectangle(penBlue, Rect);
    

           }

           int rangeCircleY0 = (int)(317 - 0.99 * 100 * 3);
           int rangeCircleX0 = (int)(aa[0] * 4 + 23);
           Rectangle Rect0 = new Rectangle(rangeCircleX0 - 5, rangeCircleY0 - 5, 10, 10);
           graphics.DrawEllipse(red, Rect0); 

                   
       }
   
        private void setrot(double ang1, double ang2, double ang3, double anis1, double anis2, int ind, int MAXROT, ref double[, ,] rotmat)
        {
            const double DEG2RAD = 3.14159265 / 180.0;
            const double EPSLON = 1.0e-20;
            double afac1, afac2, sina, sinb, sint, cosa, cosb, cost;
            double alpha, beta, theta;

            if ((ang1 >= 0.0) && (ang1 < 270.0))
            {
                alpha = (90.0 - ang1) * DEG2RAD;
            }
            else
            {
                alpha = (450.0 - ang1) * DEG2RAD;
            }

            beta = -1.0 * ang2 * DEG2RAD;
            theta = ang3 * DEG2RAD;

            sina  = Convert.ToDouble(Math.Sin(alpha));
            sinb  = Convert.ToDouble(Math.Sin(beta));
            sint  = Convert.ToDouble(Math.Sin(theta));
            cosa  = Convert.ToDouble(Math.Cos(alpha));
            cosb  = Convert.ToDouble(Math.Cos(beta));
            cost = Convert.ToDouble(Math.Cos(theta));

            afac1 = 1.0 / (Convert.ToDouble(Math.Max(anis1,EPSLON)));
            afac2 = 1.0 / (Convert.ToDouble(Math.Max(anis2,EPSLON)));
     
            rotmat[ind-1,0,0] =       (cosb * cosa);
            rotmat[ind-1,0,1] =       (cosb * sina);
            rotmat[ind-1,0,2] =       (-sinb);
            rotmat[ind-1,1,0] = afac1*((-cost*sina) + (sint*sinb*cosa));
            rotmat[ind-1,1,1] = afac1*((cost*cosa) + (sint*sinb*sina));
            rotmat[ind-1,1,2] = afac1*( sint * cosb);
            rotmat[ind-1,2,0] = afac2*((sint*sina) + (cost*sinb*cosa));
            rotmat[ind-1,2,1] = afac2*((-sint*cosa) + (cost*sinb*sina));
            rotmat[ind-1, 2, 2] = afac2 * (cost * cosb);

            //setrot done
        }
        private double sqdist(double x1,double y1,double z1,double x2,double y2,double z2,int ind,int MAXROT,double[, ,]rotmat)
        {
            double sdist=0.0;
            double cont, dx, dy, dz; //real*8

            dx = x1 - x2;
            dy = y1 - y2;
            dz = z1 - z2;

            for (int i = 1; i <= 3; i++)
            {
                cont = (rotmat[ind-1, i-1, 0] * dx) + (rotmat[ind-1, i-1, 1] * dy) + (rotmat[ind-1, i-1, 2] * dz);
                sdist = sdist + cont * cont;
            }
            
            return sdist;
        }
        private void cova3(double x1, double y1, double z1, double x2, double y2, double z2, int ivarg, int[] nst, int MAXNST, float[] c0, int[] it, float[] cc, float[] aa, int irot, int MAXROT, double[, ,] rotmat, ref double cmax, ref double cova)
        {
            const double PI = 3.14159265, PMX = 999.0;
            const double EPSLON = 1.0e-10;
            double hsqd;
            int istart = 1 + (ivarg-1)*MAXNST;
            cmax   = c0[ivarg-1];

            for (int i = 1; i <= nst[ivarg-1]; i++)
            {
                int ist = istart + i - 1;
                if (it[ist-1] == 4)
                    cmax = cmax + PMX;
                else
                    cmax = cmax + cc[ist-1];
            }
            //waiting for sqdist from client 
            hsqd = sqdist(x1,y1,z1,x2,y2,z2,irot,MAXROT,rotmat);
            float fhsqd = (float)hsqd;

            //if(real(hsqd).lt.EPSLON) then
            if (fhsqd < EPSLON)
            {
                cova = cmax;
                return;
            }
            
            cova = 0.0;

            //do is=1,nst(ivarg)
            for (int i = 1; i <= nst[ivarg -1]; i++)
            {
                int ist = istart + i - 1;
                //if(ist.ne.1) then
                if (ist != 1)
                {
                    int ir = Math.Min((irot + i - 1), MAXROT);
                    hsqd = sqdist(x1, y1, z1, x2, y2, z2, ir, MAXROT, rotmat);
                }
                double h = Math.Sqrt(hsqd);
                if (it[ist-1] == 1)
                {
                    double hr = h / aa[ist-1];
                    if (hr < 1.0)
                        cova = cova + cc[ist-1] * (1.0 - hr * (1.5 - .5 * hr * hr));
                    else if (it[ist-1] == 2)
                        cova = cova + cc[ist-1] * Math.Exp(-3.0 * h / aa[ist-1]);
                    else if (it[ist-1] == 3)
                        cova = cova + cc[ist-1] * Math.Exp(-(3.0 * h / aa[ist-1]) * (3.0 * h / aa[ist-1]));
                    else if (it[ist-1] == 4)
                        cova = cova + cmax - cc[ist-1] * (h * aa[ist-1]);
                    else if (it[ist-1] == 5)
                    {
                        double d = 10.0 * aa[ist - 1];
                        cova = cova + cc[ist - 1] * Math.Exp(-3.0 * h / d) * Math.Cos(h / aa[ist - 1] * PI);
                        cova = cova + cc[ist - 1] * Math.Cos(h / aa[ist - 1] * PI);
                    
                    }
                }

            }

      

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            const int MAXNST = 5;
            const int MDIR = 10;
            const double DEG2RAD = 3.14159265/180.0;
            const int MAXROT = MAXNST+1;
            const double EPSLON = 1.0e-20;
            const double VERSION = 2.000;

            double[, ,] rotmat = new double[MAXROT, 3, 3];
            //double rotmat[MAXROT, 3, 3];// = new double[MAXROT, 3, 3];

            aa = new float[MAXNST];
            float[] c0 = new float[1];
            //double c0 = 0.0;
            cc = new float[MAXNST];

            //double[] aa = new double[MAXNST];
            //double[] c0 = new double[1];
            ////double c0 = 0.0;
            //double[] cc = new double[MAXNST];
            float[] ang1 = new float[MAXNST];
            float[] ang2 = new float[MAXNST];
            float[] ang3 = new float[MAXNST];
            float[] anis1 = new float[MAXNST];
            float[] anis2 = new float[MAXNST];
            double[] xoff = new double[MDIR];
            double[] yoff = new double[MDIR];
            double[] zoff = new double[MDIR];

            double maxcov = 0.0;
            double cmax = 0.0;
            nst = new int[1];
            int[] it = new int[MAXNST];
            int ndir = Convert.ToInt16(textBoxDirection.Text);
            int nlags = Convert.ToInt16(textBoxLags.Text);
            double  azm = Convert.ToDouble(textBoxAzim.Text);
            double dip = Convert.ToDouble(textBoxDip.Text);
            double xlag = Convert.ToDouble(textBoxLagDis.Text);
            nuggets = new double[nlags+2];
            range = new double[nlags+2];
            sill = new double[nlags + 2];

            for (int i = 1; i <= ndir ; i++)
            {
                xoff[i - 1] =  ((Math.Sin(DEG2RAD * azm)) * (Math.Cos(DEG2RAD * dip)) * xlag);
                yoff[i - 1] = ((Math.Cos(DEG2RAD * azm)) * (Math.Cos(DEG2RAD * dip)) * xlag);
                zoff[i - 1] = ((Math.Sin(DEG2RAD * dip)) * xlag);
            }

            //nst[0] = Convert.ToInt16(textBoxStructure.Text);
            nst[0] = comboBoxNst.SelectedIndex +1;
            c0[0] = (float)Convert.ToDouble(textBoxNugget.Text);
            it[0] = Convert.ToInt16(textBoxit.Text);
            cc[0] = (float)Convert.ToDouble(textBoxCC.Text);
            ang1[0] = (float)Convert.ToDouble(textBoxAng1.Text);
            ang2[0] = (float)Convert.ToDouble(textBoxAng2.Text);
            ang3[0] = (float)Convert.ToDouble(textBoxAng3.Text);
            aa[0] = (float)Convert.ToDouble(textBoxMaxRange.Text);

            it[1] = Convert.ToInt16(textBoxIt1.Text);
            cc[1] = (float)Convert.ToDouble(textBoxCC1.Text);
            ang1[1] = (float)Convert.ToDouble(textBoxAng11.Text);
            ang2[1] = (float)Convert.ToDouble(textBoxAng21.Text);
            ang3[1] = (float)Convert.ToDouble(textBoxAng31.Text);
            aa[1] = (float)Convert.ToDouble(textBoxMaxRange1.Text);

            it[2] = Convert.ToInt16(textBoxIt1.Text);
            cc[2] = (float)Convert.ToDouble(textBoxCC2.Text);
            ang1[2] = (float)Convert.ToDouble(textBoxAng12.Text);
            ang2[2] = (float)Convert.ToDouble(textBoxAng22.Text);
            ang3[2] = (float)Convert.ToDouble(textBoxAng32.Text);
            aa[2] = (float)Convert.ToDouble(textBoxMaxRange2.Text);

            limit = Convert.ToDouble(String.Format("{0,4:.00}", aa[0])); ;
            count = 0; 
            if (nst[0] <= 0)
            {
                MessageBox.Show("nst must be at least 1");
                return;
            }

            for (int i = 1; i <= nst[0]; i++)
            {
                anis1[i-1] = (float)(Convert.ToDouble(textBoxMinRange.Text) / Math.Max(Convert.ToDouble(textBoxMaxRange.Text), EPSLON));
                anis2[i-1] = (float)(Convert.ToDouble(textBoxVert.Text) / Math.Max(Convert.ToDouble(textBoxMaxRange.Text), EPSLON));
                if (Convert.ToInt16(textBoxit.Text) == 4)
                {
                    MessageBox.Show("Invalid power variogram");
                    return;
                }
            }

            //do is=1,nst(1)
            for(int i = 1; i<= nst[0];i++)
            {
                setrot(ang1[i-1],ang2[i-1],ang3[i-1],anis1[i-1],anis2[i-1],i,MAXROT,ref rotmat);
            }
            cova3(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1, nst, MAXNST, c0, it, cc, aa, 1, MAXROT, rotmat, ref cmax, ref maxcov);
            double xx,yy,zz,cov,gam,ro,h,x1,y1,z1;
            cov = 0.0;
            StreamWriter outfile = new StreamWriter("vmodelt.txt");
            string sb = null;
            for (int id = 1; id <= ndir; id++)
            {
                xx  = -xoff[id-1];
                yy  = -yoff[id-1];
                zz = -zoff[id-1];

                for (int il = 1; il <= nlags+1; il++)
                {
                    xx = xx + xoff[id-1];
                    yy = yy + yoff[id-1];
                    zz = zz + zoff[id-1];
                                       
                    cova3(0.0, 0.0, 0.0, xx, yy, zz, 1, nst, MAXNST, c0, it, cc, aa, 1, MAXROT, rotmat, ref cmax, ref cov);
                    gam = maxcov - cov;
                    ro = cov / maxcov;
                    h   = Math.Sqrt(Math.Max(((xx*xx)+(yy*yy)+(zz*zz)),0.0));
                    //sb = string.Format("{0,3} {1,8:.00000} {2,8:.00000} {3,8:.00000} {4,8:.00000} {5,8:.00000}", il.ToString(), h.ToString(), gam.ToString(), ndir.ToString(), cov.ToString(), ro.ToString());
                    sb = string.Format("{0,2} {1,8:.000} {2,8:.00000} {3,8} {4,8:.00000} {5,8:.00000}", il, h, gam, ndir, cov, ro);
                    range[count] = Convert.ToDouble(String.Format("{0,4:.00}",h));
                    nuggets[count] = Convert.ToDouble(String.Format("{0,4:.00}", gam));
                    sill[count] = Convert.ToDouble(String.Format("{0,4:.00}", cov));
                    count++;
                    outfile.WriteLine(sb);
                    //write(lout,101) il,h,gam,ndir,cov,ro
                    if(il==1)
                    {
                        x1 = xx + 0.0001*xoff[id-1];
                        y1 = yy + 0.0001*yoff[id-1];
                        z1 = zz + 0.0001*zoff[id-1];
                        cova3(0.0,0.0,0.0,x1,y1,z1,1,nst,MAXNST,c0,it,cc,aa,1,MAXROT,rotmat,ref cmax, ref cov);
                        gam = maxcov - cov;
                        ro  = cov/maxcov;
                        h = Math.Sqrt(Math.Max(((xx * xx) + (yy * yy) + (zz * zz)), 0.0));
                        sb = string.Format("{0,2} {1,8:.000} {2,8:.00000} {3,8} {4,8:.00000} {5,8:.00000}", il, h, gam, ndir, cov, ro);
                        range[count] = Convert.ToDouble(String.Format("{0,4:.00}", h));
                        nuggets[count] = Convert.ToDouble(String.Format("{0,4:.00}", gam));
                        sill[count] = Convert.ToDouble(String.Format("{0,4:.00}", cov));
                        count++;
                        //write(lout,101) il,h,gam,ndir,cov,ro
                        outfile.WriteLine(sb);
                    }
                  
                }
            }
            outfile.Close();
            //MessageBox.Show("vmodelt.txt file created in application folder");
            this.pictureBox1.Enabled = true;
            drawchart();
            //pictureBox1.mo
        }
        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            x_mm = e.X;
            y_mm = e.Y;
               
            //button1_Click(sender,e);
            bmp01 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp01;
            //GC.SuppressFinalize(bmp01);

            bmp = new Bitmap(pictureBox1.Image);
            graphics = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;

            graphics.Clear(Color.White);
            brsh = new SolidBrush(Color.Black);
            fnt = new Font("Arial", 8, FontStyle.Bold);
            //GC.SuppressFinalize(graphics);
            //GC.SuppressFinalize(pictureBox1);
            //GC.SuppressFinalize(brsh);
            //GC.SuppressFinalize(fnt);
            //GC.SuppressFinalize(bmp);
            //Y Axis labels
            for (int i = 0; i < 108; i = i + 6)
            {
                double text = i;
                float y = 329 - (i * 3) - 12;
                graphics.DrawString(String.Format("{0,4:0.00}", text / 100), fnt, brsh, 0, y);
                //graphics.DrawLine(pen, 10, y, 23, y);
                graphics.DrawLine(gridPen, 23, y, 573, y);
            }

            //X Axis lables
            for (int i = 0; i <= 130; i = i + 10)
            {
                //double text = i;
                if (i != 0)
                {
                    float x = (i * 4) + 23;
                    graphics.DrawString(String.Format("{0}", i), fnt, brsh, x, 318);
                    //graphics.DrawLine(pen, x, 329, x, 318);
                    graphics.DrawLine(gridPen, x, 318, x, 0);
                }
                

            }
            graphics.DrawLine(pen, 23, 317, 623, 317);
            graphics.DrawLine(pen, 23, 317, 23, 0);
            int cursurmax = 0;
            for (int i = 1; i < count; i++)
            {
                float x1 = (float)(range[i] * 4) + 23;
                float y1 = 329 - ((float)nuggets[i] * 100 * 3) - 12;
                int j = i + 1;
                float x2, y2;
                if (j < count)
                {
                    x2 = ((float)range[j] * 4) + 23;
                    y2 = 329 - ((float)nuggets[j] * 100 * 3) - 12;
                    graphics.DrawLine(penCurve, x1, y1, x2, y2);
                    //graphics.DrawLine(penCurve, x1, y1, x1 + 3, y1);
                    if (range[i] >= limit)
                    {
                        cursurmax = i;
                       // break;
                    }
                }
            }
            

            //if ((x_mm >= range[1] * 5 + 23) && (x_mm <= range[cursurmax] * 5 + 23))
            //{
            //    //if ((y_mm >= 329 - (nuggets[1] * 100 * 3) - 12) && (y_mm <= 329 - (nuggets[cursurmax] * 100 * 3))) 
            //    //{
            //    graphics.DrawLine(red, x_mm, 0, x_mm, 317);
            //    double value = (x_mm - 23) / 5;

            //    String str2 = String.Format("{0,4:00.00}", (int)value);
            //    if (str2 == "00.00")
            //        index = 1;
            //    for (int i = 1; i < count; i++)
            //    {
            //        String str1 = String.Format("{0,4:00.00}", (int)range[i]); // ignore 1st range index
            //        if (str1 == str2)
            //        {
            //            index = i;
            //            break;
            //        }
            //    }
            //    this.textBox1.Text = nuggets[index].ToString();
            //    this.textBox2.Text = range[index].ToString();
            //    // graphics.DrawEllipse(red,

            //    //graphics.DrawLine(red, 0, 0, y_mm, y_mm+10);
            //    //double value1 = (x_mm - 23) / 5;
            //    //double value2 = 
            //    //graphics.DrawEllipse(red,(float)value1,

            //    //String str2 = String.Format("{0,4:00.00}", (int)value);
            //    //if (str2 == "00.00")
            //    //    index = 1;
            //    //for (int i = 1; i < count; i++)
            //    //{
            //    //    String str1 = String.Format("{0,4:00.00}", (int)range[i]); // ignore 1st range index
            //    //    if (str1 == str2)
            //    //    {
            //    //        index = i;
            //    //        break;
            //    //    }
            //    //}
            //    //this.textBox1.Text = nuggets[index].ToString();
            //    //this.textBox2.Text = range[index].ToString();

            //    //}


            //}
            //int rangeCircleY0 = (int)(317 - nuggets[cursurmax - 1] * 100 * 3);
            //int rangeCircleX0 = (int)(range[cursurmax - 1] * 4 + 23);
            //Rectangle Rect0 = new Rectangle(rangeCircleX0 - 5, rangeCircleY0 - 5, 7, 7);
            //graphics.DrawEllipse(redCircle, Rect0);

            if ((y_mm <= 329 - nuggets[1] * 100 * 3 - 12) && (y_mm >= 329 - nuggets[cursurmax] * 100 * 3 - 12))
            {
             
                float value = (float) ((317 - y_mm) / 300);
                //this.textBox1.Text = String.Format("{0,4:00.00}", (int) value);
                String str2 = String.Format("{0,4:00.00}", value);
                if (str2 == "00.00")
                    index = 1;
                for (int i = 1; i < count; i++)
                {
                    String str1 = String.Format("{0,4:00.00}", (float)nuggets[i]); // ignore 1st range index
                    if (str1 == str2)
                    {
                        index = i;
                        int rangeCircleX = (int)(range[i] * 4 + 23);
                        Rectangle Rect = new Rectangle(rangeCircleX-5 ,(int) y_mm -5, 10, 10);
                        graphics.DrawEllipse(red, Rect);
                        graphics.DrawLine(red, 23, y_mm, 38, y_mm);
                        
                        this.textBox1.Text = nuggets[i].ToString();
                        this.textBox2.Text = range[i].ToString();
                        break;
                    }
                }
            }
            else
            {
                int  nuggetCursor = (int) (317 - nuggets[1] * 100 * 3);
                graphics.DrawLine(red, 23, nuggetCursor, 38, nuggetCursor);
            }

            int rangeCircleY1, rangeCircleX1;
            Rectangle Rect1;

            for (int i = 0; i < nst[0]; i++)
            {
                rangeCircleY1 = (int)(317 - cc[i] * 100 * 3);
                rangeCircleX1 = (int)(aa[i] * 4 + 23);
                Rect1 = new Rectangle(rangeCircleX1 - 5, rangeCircleY1 - 5, 3,3);
                graphics.DrawRectangle(penBlue, Rect1);


            }
        }

        private void comboBoxNst_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxNst.SelectedIndex)
            {
                case 0:
                    textBoxIt1.Enabled = false;
                    textBoxIt2.Enabled = false;
                    textBoxCC1.Enabled = false;
                    textBoxCC2.Enabled = false;
                    textBoxAng11.Enabled = false;
                    textBoxAng21.Enabled = false;
                    textBoxAng31.Enabled = false;
                    textBoxAng12.Enabled = false;
                    textBoxAng22.Enabled = false;
                    textBoxAng32.Enabled = false;
                    textBoxMaxRange1.Enabled = false;
                    textBoxMinRange1.Enabled = false;
                    textBoxVert1.Enabled = false;
                    textBoxMaxRange2.Enabled = false;
                    textBoxMinRange2.Enabled = false;
                    textBoxVert2.Enabled = false;
                    break;
                case 1:
                    textBoxIt1.Enabled = true;
                    textBoxIt2.Enabled = false;
                    textBoxCC1.Enabled = true;
                    textBoxCC2.Enabled = false;
                    textBoxAng11.Enabled = true;
                    textBoxAng21.Enabled = true;
                    textBoxAng31.Enabled = true;
                    textBoxAng12.Enabled = false;
                    textBoxAng22.Enabled = false;
                    textBoxAng32.Enabled = false;
                    textBoxMaxRange1.Enabled = true;
                    textBoxMinRange1.Enabled = true;
                    textBoxVert1.Enabled = true;
                    textBoxMaxRange2.Enabled = false;
                    textBoxMinRange2.Enabled = false;
                    textBoxVert2.Enabled = false;
                    break;
                case 2:
                    textBoxIt1.Enabled = true;
                    textBoxIt2.Enabled = true;
                    textBoxCC1.Enabled = true;
                    textBoxCC2.Enabled = true;
                    textBoxAng11.Enabled = true;
                    textBoxAng21.Enabled = true;
                    textBoxAng31.Enabled = true;
                    textBoxAng12.Enabled = true;
                    textBoxAng22.Enabled = true;
                    textBoxAng32.Enabled = true;
                    textBoxMaxRange1.Enabled = true;
                    textBoxMinRange1.Enabled = true;
                    textBoxVert1.Enabled = true;
                    textBoxMaxRange2.Enabled = true;
                    textBoxMinRange2.Enabled = true;
                    textBoxVert2.Enabled = true;
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "txt files (*.var)|*.var";
            fd.FilterIndex = 2;
            fd.RestoreDirectory = true;
            if (DialogResult.OK == fd.ShowDialog())
            {
                if ((myStream = fd.OpenFile()) != null)
                {
                    
                    using (StreamWriter sw = new StreamWriter( myStream))
                    {
                       
                        sw.WriteLine(String.Format("{0} {1}",textBoxDirection.Text,textBoxLags.Text));
                        sw.WriteLine(String.Format("{0} {1} {2}", textBoxAzim.Text, textBoxDip.Text,textBoxLagDis.Text));
                        sw.WriteLine(String.Format("{0} {1} ", comboBoxNst.SelectedIndex +1, textBoxNugget.Text));
                        sw.WriteLine(String.Format("{0} {1} {2} {3} {4}", textBoxit.Text, textBoxCC.Text, textBoxAng1.Text, textBoxAng2.Text, textBoxAng3.Text));
                        sw.WriteLine(String.Format("{0} {1} {2}", textBoxMaxRange.Text, textBoxMinRange.Text, textBoxVert.Text));
                        if (comboBoxNst.SelectedIndex == 1)
                        {
                            sw.WriteLine(String.Format("{0} {1} {2} {3} {4}", textBoxIt1.Text, textBoxCC1.Text, textBoxAng11.Text, textBoxAng21.Text, textBoxAng31.Text));
                            sw.WriteLine(String.Format("{0} {1} {2}", textBoxMaxRange1.Text, textBoxMinRange1.Text, textBoxVert1.Text));

                        }
                        if (comboBoxNst.SelectedIndex == 2)
                        {
                            sw.WriteLine(String.Format("{0} {1} {2} {3} {4}", textBoxIt2.Text, textBoxCC2.Text, textBoxAng12.Text, textBoxAng22.Text, textBoxAng32.Text));
                            sw.WriteLine(String.Format("{0} {1} {2}", textBoxMaxRange2.Text, textBoxMinRange2.Text, textBoxVert2.Text));

                        }
                    }
                    myStream.Close();
                }
                this.Text = "VMODEL-" + fd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.var)|*.var";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (StreamReader sr = new StreamReader(myStream))
                        {
                            String line = sr.ReadLine();
                            //StringBuilder sb = new StringBuilder(line);
                            string[] split = line.Split(new Char[] { ' ' });
                            textBoxDirection.Text = split[0];
                            textBoxLags.Text = split[1];

                            line = null;
                            line = sr.ReadLine();
                            split = line.Split(new Char[] { ' ' });
                            textBoxAzim.Text = split[0];
                            textBoxDip.Text = split[1];
                            textBoxLagDis.Text = split[2];

                            line = null;
                            line = sr.ReadLine();
                            split = line.Split(new Char[] { ' ' });
                            switch (split[0])
                            {
                                case "1":
                                    comboBoxNst.SelectedIndex = 0;
                                    break;
                                case "2":
                                    comboBoxNst.SelectedIndex = 1;
                                    break;
                                case "3":
                                    comboBoxNst.SelectedIndex = 2;
                                    break;
                            }
                            textBoxNugget.Text = split[1];

                            line = null;
                            line = sr.ReadLine();
                            split = line.Split(new Char[] { ' ' });
                            textBoxit.Text = split[0];
                            textBoxCC.Text = split[1];
                            textBoxAng1.Text = split[2];
                            textBoxAng2.Text = split[3];
                            textBoxAng3.Text = split[4];

                            line = null;
                            line = sr.ReadLine();
                            split = line.Split(new Char[] { ' ' });
                            textBoxMaxRange.Text = split[0];
                            textBoxMinRange.Text = split[1];
                            textBoxVert.Text = split[2];

                            if (comboBoxNst.SelectedIndex == 1)
                            {
                                line = null;
                                line = sr.ReadLine();
                                split = line.Split(new Char[] { ' ' });
                                textBoxIt1.Text = split[0];
                                textBoxCC1.Text = split[1];
                                textBoxAng11.Text = split[2];
                                textBoxAng21.Text = split[3];
                                textBoxAng31.Text = split[4];

                                line = null;
                                line = sr.ReadLine();
                                split = line.Split(new Char[] { ' ' });
                                textBoxMaxRange1.Text = split[0];
                                textBoxMinRange1.Text = split[1];
                                textBoxVert1.Text = split[2];

                            }

                            if (comboBoxNst.SelectedIndex == 2)
                            {
                                line = null;
                                line = sr.ReadLine();
                                split = line.Split(new Char[] { ' ' });
                                textBoxIt2.Text = split[0];
                                textBoxCC2.Text = split[1];
                                textBoxAng12.Text = split[2];
                                textBoxAng22.Text = split[3];
                                textBoxAng32.Text = split[4];

                                line = null;
                                line = sr.ReadLine();
                                split = line.Split(new Char[] { ' ' });
                                textBoxMaxRange2.Text = split[0];
                                textBoxMinRange2.Text = split[1];
                                textBoxVert2.Text = split[2];

                            }

                        }
                       // this.Name = "heelo";
                        this.Text = "VMODEL-"+openFileDialog1.FileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. " );
                }
            } 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Beta Release \nContact: narendra.gautam1@gmail.com");
        }

    }
}
