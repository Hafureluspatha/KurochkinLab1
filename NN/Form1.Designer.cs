﻿namespace NN
{
    partial class Form1
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
            this.networkStructure = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // networkStructure
            // 
            this.networkStructure.Location = new System.Drawing.Point(127, 33);
            this.networkStructure.Name = "networkStructure";
            this.networkStructure.Size = new System.Drawing.Size(100, 20);
            this.networkStructure.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Структура сети";
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(565, 275);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 2;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Путь до исходных данных";
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(184, 82);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(307, 20);
            this.pathBox.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 332);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.start);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.networkStructure);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox networkStructure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox pathBox;
    }
}

