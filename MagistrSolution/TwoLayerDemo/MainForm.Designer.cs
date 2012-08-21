namespace TwoLayerDemo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.txtDelta = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtM = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtK = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtT0 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTr = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtT1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtC2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtC1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRo2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRo1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtLambda2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLambda1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtN = new System.Windows.Forms.TextBox();
            this.lblN = new System.Windows.Forms.Label();
            this.btnDo = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtTMax = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtTdelta = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 730);
            this.splitContainer1.SplitterDistance = 612;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.txtTdelta);
            this.splitContainer.Panel1.Controls.Add(this.label15);
            this.splitContainer.Panel1.Controls.Add(this.txtTMax);
            this.splitContainer.Panel1.Controls.Add(this.label13);
            this.splitContainer.Panel1.Controls.Add(this.button2);
            this.splitContainer.Panel1.Controls.Add(this.txtDelta);
            this.splitContainer.Panel1.Controls.Add(this.label3);
            this.splitContainer.Panel1.Controls.Add(this.button1);
            this.splitContainer.Panel1.Controls.Add(this.txtM);
            this.splitContainer.Panel1.Controls.Add(this.label2);
            this.splitContainer.Panel1.Controls.Add(this.txtK);
            this.splitContainer.Panel1.Controls.Add(this.label1);
            this.splitContainer.Panel1.Controls.Add(this.txtT0);
            this.splitContainer.Panel1.Controls.Add(this.label14);
            this.splitContainer.Panel1.Controls.Add(this.txtTr);
            this.splitContainer.Panel1.Controls.Add(this.label11);
            this.splitContainer.Panel1.Controls.Add(this.txtT1);
            this.splitContainer.Panel1.Controls.Add(this.label12);
            this.splitContainer.Panel1.Controls.Add(this.txtC2);
            this.splitContainer.Panel1.Controls.Add(this.label10);
            this.splitContainer.Panel1.Controls.Add(this.txtC1);
            this.splitContainer.Panel1.Controls.Add(this.label9);
            this.splitContainer.Panel1.Controls.Add(this.txtRo2);
            this.splitContainer.Panel1.Controls.Add(this.label8);
            this.splitContainer.Panel1.Controls.Add(this.txtRo1);
            this.splitContainer.Panel1.Controls.Add(this.label7);
            this.splitContainer.Panel1.Controls.Add(this.txtLambda2);
            this.splitContainer.Panel1.Controls.Add(this.label6);
            this.splitContainer.Panel1.Controls.Add(this.txtLambda1);
            this.splitContainer.Panel1.Controls.Add(this.label5);
            this.splitContainer.Panel1.Controls.Add(this.txtL);
            this.splitContainer.Panel1.Controls.Add(this.label4);
            this.splitContainer.Panel1.Controls.Add(this.txtN);
            this.splitContainer.Panel1.Controls.Add(this.lblN);
            this.splitContainer.Panel1.Controls.Add(this.btnDo);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer.Size = new System.Drawing.Size(1008, 612);
            this.splitContainer.SplitterDistance = 184;
            this.splitContainer.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(17, 576);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 23);
            this.button2.TabIndex = 61;
            this.button2.Text = "test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // txtDelta
            // 
            this.txtDelta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDelta.Location = new System.Drawing.Point(72, 413);
            this.txtDelta.Name = "txtDelta";
            this.txtDelta.Size = new System.Drawing.Size(100, 20);
            this.txtDelta.TabIndex = 60;
            this.txtDelta.Text = "0,001";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 416);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 59;
            this.label3.Text = "delta";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 546);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 23);
            this.button1.TabIndex = 58;
            this.button1.Text = "Go1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnRalgirithmClick);
            // 
            // txtM
            // 
            this.txtM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtM.Location = new System.Drawing.Point(72, 104);
            this.txtM.Name = "txtM";
            this.txtM.Size = new System.Drawing.Size(100, 20);
            this.txtM.TabIndex = 57;
            this.txtM.Text = "32";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "M";
            // 
            // txtK
            // 
            this.txtK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtK.Location = new System.Drawing.Point(72, 64);
            this.txtK.Name = "txtK";
            this.txtK.Size = new System.Drawing.Size(100, 20);
            this.txtK.TabIndex = 55;
            this.txtK.Text = "0,1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "K";
            // 
            // txtT0
            // 
            this.txtT0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtT0.Location = new System.Drawing.Point(72, 387);
            this.txtT0.Name = "txtT0";
            this.txtT0.Size = new System.Drawing.Size(100, 20);
            this.txtT0.TabIndex = 53;
            this.txtT0.Text = "10";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 390);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(20, 13);
            this.label14.TabIndex = 52;
            this.label14.Text = "T0";
            // 
            // txtTr
            // 
            this.txtTr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTr.Location = new System.Drawing.Point(72, 361);
            this.txtTr.Name = "txtTr";
            this.txtTr.Size = new System.Drawing.Size(100, 20);
            this.txtTr.TabIndex = 51;
            this.txtTr.Text = "50";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 364);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 13);
            this.label11.TabIndex = 50;
            this.label11.Text = "Tr";
            // 
            // txtT1
            // 
            this.txtT1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtT1.Location = new System.Drawing.Point(72, 335);
            this.txtT1.Name = "txtT1";
            this.txtT1.Size = new System.Drawing.Size(100, 20);
            this.txtT1.TabIndex = 49;
            this.txtT1.Text = "100";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 338);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(16, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "Tl";
            // 
            // txtC2
            // 
            this.txtC2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtC2.Location = new System.Drawing.Point(72, 297);
            this.txtC2.Name = "txtC2";
            this.txtC2.Size = new System.Drawing.Size(100, 20);
            this.txtC2.TabIndex = 47;
            this.txtC2.Text = "381";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 300);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 13);
            this.label10.TabIndex = 46;
            this.label10.Text = "c2";
            // 
            // txtC1
            // 
            this.txtC1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtC1.Location = new System.Drawing.Point(72, 193);
            this.txtC1.Name = "txtC1";
            this.txtC1.Size = new System.Drawing.Size(100, 20);
            this.txtC1.TabIndex = 45;
            this.txtC1.Text = "460";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 196);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 44;
            this.label9.Text = "c1";
            // 
            // txtRo2
            // 
            this.txtRo2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRo2.Location = new System.Drawing.Point(72, 271);
            this.txtRo2.Name = "txtRo2";
            this.txtRo2.Size = new System.Drawing.Size(100, 20);
            this.txtRo2.TabIndex = 43;
            this.txtRo2.Text = "8800";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 274);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 13);
            this.label8.TabIndex = 42;
            this.label8.Text = "ro2";
            // 
            // txtRo1
            // 
            this.txtRo1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRo1.Location = new System.Drawing.Point(72, 167);
            this.txtRo1.Name = "txtRo1";
            this.txtRo1.Size = new System.Drawing.Size(100, 20);
            this.txtRo1.TabIndex = 41;
            this.txtRo1.Text = "7800";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "ro1";
            // 
            // txtLambda2
            // 
            this.txtLambda2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLambda2.Location = new System.Drawing.Point(72, 245);
            this.txtLambda2.Name = "txtLambda2";
            this.txtLambda2.Size = new System.Drawing.Size(100, 20);
            this.txtLambda2.TabIndex = 39;
            this.txtLambda2.Text = "384";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "lambda2";
            // 
            // txtLambda1
            // 
            this.txtLambda1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLambda1.Location = new System.Drawing.Point(72, 141);
            this.txtLambda1.Name = "txtLambda1";
            this.txtLambda1.Size = new System.Drawing.Size(100, 20);
            this.txtLambda1.TabIndex = 37;
            this.txtLambda1.Text = "46";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "lambda1";
            // 
            // txtL
            // 
            this.txtL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtL.Location = new System.Drawing.Point(72, 38);
            this.txtL.Name = "txtL";
            this.txtL.Size = new System.Drawing.Size(100, 20);
            this.txtL.TabIndex = 35;
            this.txtL.Text = "0,3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "L";
            // 
            // txtN
            // 
            this.txtN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtN.Location = new System.Drawing.Point(72, 12);
            this.txtN.Name = "txtN";
            this.txtN.Size = new System.Drawing.Size(100, 20);
            this.txtN.TabIndex = 29;
            this.txtN.Text = "256";
            // 
            // lblN
            // 
            this.lblN.AutoSize = true;
            this.lblN.Location = new System.Drawing.Point(14, 15);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(15, 13);
            this.lblN.TabIndex = 28;
            this.lblN.Text = "N";
            // 
            // btnDo
            // 
            this.btnDo.Location = new System.Drawing.Point(17, 517);
            this.btnDo.Name = "btnDo";
            this.btnDo.Size = new System.Drawing.Size(155, 23);
            this.btnDo.TabIndex = 27;
            this.btnDo.Text = "Go";
            this.btnDo.UseVisualStyleBackColor = true;
            this.btnDo.Click += new System.EventHandler(this.btnDo_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1008, 114);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chart1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.chart2);
            this.splitContainer2.Size = new System.Drawing.Size(818, 610);
            this.splitContainer2.SplitterDistance = 413;
            this.splitContainer2.TabIndex = 0;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(413, 610);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(0, 0);
            this.chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart2.Series.Add(series2);
            this.chart2.Size = new System.Drawing.Size(401, 610);
            this.chart2.TabIndex = 2;
            this.chart2.Text = "chart1";
            // 
            // txtTMax
            // 
            this.txtTMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTMax.Location = new System.Drawing.Point(72, 454);
            this.txtTMax.Name = "txtTMax";
            this.txtTMax.Size = new System.Drawing.Size(100, 20);
            this.txtTMax.TabIndex = 63;
            this.txtTMax.Text = "600";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 457);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 13);
            this.label13.TabIndex = 62;
            this.label13.Text = "Tmax";
            // 
            // txtTdelta
            // 
            this.txtTdelta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTdelta.Location = new System.Drawing.Point(72, 480);
            this.txtTdelta.Name = "txtTdelta";
            this.txtTdelta.Size = new System.Drawing.Size(100, 20);
            this.txtTdelta.TabIndex = 65;
            this.txtTdelta.Text = "20";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(14, 483);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(37, 13);
            this.label15.TabIndex = 64;
            this.label15.Text = "Tdelta";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtDelta;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtT0;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtTr;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtT1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtC2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtC1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRo2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRo1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLambda2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLambda1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtN;
        private System.Windows.Forms.Label lblN;
        private System.Windows.Forms.Button btnDo;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.TextBox txtTdelta;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtTMax;
        private System.Windows.Forms.Label label13;


    }
}

